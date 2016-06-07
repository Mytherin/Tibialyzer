
# Copyright 2016 Mark Raasveldt
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#   http://www.apache.org/licenses/LICENSE-2.0
# 
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
import xml.etree.ElementTree
import sqlite3

database_file = 'database.db'
#database_file = 'E:\\Github Projects\\Tibialyzer\\Tibialyzer\\Database\\database.db'
skins_xmlfile = "Extra Information/skins.xml"
huntdirections_xmlfile = "Extra Information/huntdirections.xml"
huntrequirements = "Extra Information/huntrequirements.xml"
questinstructions = "Extra Information/questinstructions.xml"

def getCreatureID(name):
    global c
    name = name.strip().lower()
    c.execute('SELECT id FROM Creatures WHERE LOWER(name)=? OR LOWER(title)=?', (name, name))
    return c.fetchall()[0][0]

def getHuntID(name):
    global c
    name = name.strip().lower()
    c.execute('SELECT id FROM HuntingPlaces WHERE LOWER(name)=?', (name,))
    print(name)
    return c.fetchall()[0][0]

def getQuestID(name):
    global c
    name = name.strip().lower()
    if name == "ferumbras' ascendant quest": name = 'ferumbras\' ascension quest'
    c.execute('SELECT id FROM Quests WHERE LOWER(name)=? OR LOWER(title)=?', (name, name))
    print(name)
    return c.fetchall()[0][0]

def getItemID(name):
    global c
    name = name.strip().lower()
    c.execute('SELECT id FROM Items WHERE LOWER(name)=? OR LOWER(title)=?', (name, name))
    results = c.fetchall()
    if len(results) == 0:
        print(name)
    return results[0][0]


conn = sqlite3.connect(database_file)
c = conn.cursor()
# skins
c.execute('DROP TABLE IF EXISTS Skins');
c.execute('CREATE TABLE Skins(creatureid INTEGER, skinitemid INTEGER, knifeitemid INTEGER, percentage FLOAT)')
root = xml.etree.ElementTree.parse(skins_xmlfile).getroot()
for child in root.getchildren():
    creatureid = getCreatureID(child.find('Name').text)
    knifeid = getItemID(child.find('Knife').text)
    for skininfo in child.find('Items').getchildren():
        skinid = getItemID(skininfo.find('Name').text)
        percentage = float(skininfo.find('Percentage').text)
        c.execute('INSERT INTO Skins(creatureid, skinitemid, knifeitemid, percentage) VALUES (?,?,?,?)', (creatureid, skinid, knifeid, percentage))

# hunt requirements
c.execute('DROP TABLE IF EXISTS HuntRequirements')
c.execute('CREATE TABLE HuntRequirements(huntingplaceid INTEGER, questid INTEGER, requirementtext STRING)')
root = xml.etree.ElementTree.parse(huntrequirements).getroot()
for child in root.getchildren():
    huntingplaceid = getHuntID(child.find('Name').text)
    for requirement in child.find('Requirements').getchildren():
        questid = getQuestID(requirement.find('Quest').text)
        reqtext = requirement.find('RequirementText').text
        c.execute('INSERT INTO HuntRequirements(huntingplaceid, questid, requirementtext) VALUES (?,?,?)', (huntingplaceid, questid, reqtext))

from coordinates import convert_x, convert_y
# hunt directions
c.execute('DROP TABLE IF EXISTS HuntDirections')
c.execute('CREATE TABLE HuntDirections(huntingplaceid INTEGER, beginx INTEGER, beginy INTEGER, beginz INTEGER, endx INTEGER, endy INTEGER, endz INTEGER, description STRING, ordering INTEGER, settings STRING)')
root = xml.etree.ElementTree.parse(huntdirections_xmlfile).getroot()
for child in root.getchildren():
    huntingplaceid = getHuntID(child.find('Name').text)
    ordering = 0
    for direction in child.find('Directions').getchildren():
        settings = ""
        if 'SamePage' in direction.attrib and direction.attrib['SamePage'] == 'True':
            ordering -= 1
        begin = direction.find('BeginCoordinate').text.split(',')
        end = direction.find('EndCoordinate').text.split(',')
        if direction.find("BeginImage") != None:
            settings = settings + "@Marking=" + str(convert_x(begin[0])) + "," + str(convert_y(begin[1])) + "," + str(int(begin[2])) + "@MarkIcon=" + direction.find("BeginImage").text
        if direction.find("EndImage") != None:
            settings = settings + "@Marking=" + str(convert_x(end[0])) + "," + str(convert_y(end[1])) + "," + str(int(end[2])) + "@MarkIcon=" + direction.find("EndImage").text
        description = direction.find('Text').text
        c.execute('INSERT INTO HuntDirections(huntingplaceid, beginx, beginy, beginz, endx, endy, endz, description, ordering, settings) VALUES (?,?,?,?,?,?,?,?,?,?)', 
            (huntingplaceid, convert_x(begin[0]), convert_y(begin[1]), int(begin[2]), convert_x(end[0]), convert_y(end[1]), int(end[2]), description, ordering, settings))
        ordering += 1

def addChildren(parentNode, missionName):
    ordering = 0
    for instruction in parentNode.findall('Instruction'):
        if 'SamePage' in instruction.attrib and instruction.attrib['SamePage'] == 'True':
            ordering -= 1
        beginx = None
        beginy = None
        beginz = None
        settings = ""
        if instruction.find('Markings') != None:
            averagex, averagey, averagez = (0, 0, None)
            count = 0
            for marking in instruction.find('Markings'):
                coordinate = marking.find('MarkLocation').text
                begin = coordinate.split(',')
                beginx = convert_x(begin[0])
                beginy = convert_y(begin[1])
                beginz = int(begin[2])

                settings = settings + "@Marking=" + str(beginx) + "," + str(beginy) + "," + str(beginz)
                averagex += beginx
                averagey += beginy
                if averagez == None:
                    averagez = beginz
                elif averagez != beginz:
                    raise Exception("Markings on different floors.")
                count += 1
                if marking.find("MarkIcon") != None:
                    settings = settings + "@MarkIcon=" + marking.find('MarkIcon').text
                if marking.find("MarkSize") != None:
                    settings = settings + "@MarkSize=" + str(int(marking.find('MarkSize').text))
            beginx = averagex / count
            beginy = averagey / count
            beginz = averagez
        if instruction.find("WalkableColor") != None:
            settings = settings + "@WalkableColor=" + instruction.find("WalkableColor").text

        if instruction.find('BeginCoordinate') != None:
            begin = instruction.find('BeginCoordinate').text.split(',')
            beginx = convert_x(begin[0])
            beginy = convert_y(begin[1])
            beginz = int(begin[2])
            if instruction.find("BeginImage") != None:
                settings = settings + "@Marking=" + str(beginx) + "," + str(beginy) + "," + str(beginz) + "@MarkIcon=" + instruction.find("BeginImage").text
        if instruction.find('EndCoordinate') == None:
            endx = None
            endy = None
            endz = 90
            try: endz = int(instruction.find('RectangleSize').text)
            except: pass
        else: 
            end = instruction.find('EndCoordinate').text.split(',')
            endx = convert_x(end[0])
            endy = convert_y(end[1])
            endz = int(end[2])
            if instruction.find("EndImage") != None:
                settings = settings + "@Marking=" + str(endx) + "," + str(endy) + "," + str(endz) + "@MarkIcon=" + instruction.find("EndImage").text
        description = instruction.find('Text').text
        c.execute('INSERT INTO QuestInstructions(questid, beginx, beginy, beginz, endx, endy, endz, description, ordering, missionname,settings) VALUES (?,?,?,?,?,?,?,?,?,?,?)', 
            (questid, convert_x(begin[0]), convert_y(begin[1]), int(begin[2]), endx, endy, endz, description, ordering, missionName,settings[1:] + "@" if len(settings) > 0 else None))
        ordering += 1
# quest information
c.execute('DROP TABLE IF EXISTS QuestAdditionalRequirements')
c.execute('DROP TABLE IF EXISTS QuestItemRequirements')
c.execute('DROP TABLE IF EXISTS QuestInstructions')
c.execute('CREATE TABLE QuestItemRequirements(questid INTEGER, itemid INTEGER, count INTEGER)')
c.execute('CREATE TABLE QuestAdditionalRequirements(questid INTEGER, requirementtext STRING)')
c.execute('CREATE TABLE QuestInstructions(questid INTEGER, beginx INTEGER, beginy INTEGER, beginz INTEGER, endx INTEGER, endy INTEGER, endz INTEGER, description STRING, ordering INTEGER, missionname STRING, settings STRING)')
root = xml.etree.ElementTree.parse(questinstructions).getroot()
for child in root.getchildren():
    questid = getQuestID(child.find('Name').text)
    requirementNode = child.find('Requirements')
    if requirementNode != None:
        itemsNode = requirementNode.find('Items')
        if itemsNode != None:
            for item in itemsNode.getchildren():
                _count = 1
                if 'Count' in item.attrib:
                    _count = int(item.attrib['Count'])
                itemid = getItemID(item.text)
                c.execute('INSERT INTO QuestItemRequirements(questid,itemid,count) VALUES (?,?,?)', (questid, itemid, _count))
        additionalNode = requirementNode.find('AdditionalRequirements')
        if additionalNode != None:
            for req in additionalNode.getchildren():
                import re
                match = re.search(r'\[([^]]+)\]', req.text)
                if match != None:
                    qname = match.groups()[0].strip().lower()
                    linkedqid = getQuestID(qname)
                c.execute('INSERT INTO QuestAdditionalRequirements(questid, requirementtext) VALUES (?,?)', (questid, req.text))
    instructionsNode = child.find('Instructions')
    if instructionsNode != None:
        addChildren(instructionsNode, None)
        for missionNode in instructionsNode.findall('Mission'):
            missionName = None
            if 'Name' in missionNode.attrib:
                missionName = missionNode.attrib['Name']
            addChildren(missionNode, missionName)


conn.commit()










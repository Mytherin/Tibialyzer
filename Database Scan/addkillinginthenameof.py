
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
cities_xmlfile = "Extra Information/killinginthenameof.xml"

from coordinates import convert_x, convert_y

conn = sqlite3.connect(database_file)
c = conn.cursor()


# cities
c.execute('DROP TABLE IF EXISTS TaskGroups');
c.execute('DROP TABLE IF EXISTS Tasks');
c.execute('DROP TABLE IF EXISTS TaskCreatures');
c.execute('DROP TABLE IF EXISTS TaskHunts');
c.execute('CREATE TABLE TaskGroups(id INTEGER PRIMARY KEY AUTOINCREMENT, name STRING)')
c.execute('CREATE TABLE Tasks(id INTEGER PRIMARY KEY AUTOINCREMENT, name STRING, groupid INTEGER, count INTEGER, taskpoints INTEGER, bossid INTEGER, bossx INTEGER, bossy INTEGER, bossz INTEGER)')
c.execute('CREATE TABLE TaskCreatures(taskid INTEGER, creatureid INTEGER)')
c.execute('CREATE TABLE TaskHunts(taskid INTEGER, huntingplaceid INTEGER)')
root = xml.etree.ElementTree.parse(cities_xmlfile).getroot()
for child in root.getchildren():
    taskbracketname = child.find('Name').text
    c.execute('INSERT INTO TaskGroups(name) VALUES (?)', (taskbracketname,))
    bracketid = c.lastrowid
    tasks = child.find('Tasks')
    for task in tasks.getchildren():
        name = None
        creatures = task.find('Creature').text
        if task.find('Name') != None:
            name = task.find('Name').text
        else:
            name = creatures.split(';')[0] + " Task"
        count = int(task.find('Count').text)
        taskpoints =  int(task.find('Points').text) if task.find('Points') != None else None
        boss = task.find('Boss').text.strip().lower() if task.find('Boss') != None else None
        bossid = None
        if boss != None:
            c.execute('SELECT id FROM Creatures WHERE LOWER(name)=? OR LOWER(title)=?', (boss, boss))
            bossid = c.fetchall()[0][0]
        uloc = task.find('BossLocation').text.split(',') if task.find('BossLocation') != None else (None, None, None)
        if uloc[0] != None:
            uloc[0] = convert_x(uloc[0])
            uloc[1] = convert_y(uloc[1])
            uloc[2] = int(uloc[2])
        c.execute('INSERT INTO Tasks(name,groupid, count, taskpoints, bossid, bossx, bossy, bossz) VALUES (?,?,?,?,?,?,?,?)', (name,bracketid, count, taskpoints, bossid, uloc[0], uloc[1], uloc[2]))
        taskid = c.lastrowid
        for creature in creatures.split(';'):
            creature = creature.strip().lower()
            print(creature)
            c.execute('SELECT id FROM Creatures WHERE LOWER(name)=? OR LOWER(title)=?', (creature, creature))
            creatureid = c.fetchall()[0][0]
            c.execute('INSERT INTO TaskCreatures(taskid, creatureid) VALUES (?,?)', (taskid, creatureid))
        hunts = task.find('Hunts')
        for huntchild in hunts.getchildren():
            huntname = huntchild.text.replace("_", " ").lower().strip()
            c.execute('SELECT id FROM HuntingPlaces WHERE LOWER(name)=?', (huntname,))
            results = c.fetchall()
            if len(results) <= 0:
                print(huntname, "not found")
                exit()
            c.execute('INSERT INTO TaskHunts(taskid, huntingplaceid) VALUES (?,?)', (taskid, results[0][0]))

conn.commit()






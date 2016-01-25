
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


import math
import re
import urllib.request
import sqlite3
import time

from imageoperations import crop_image, gif_is_animated, convert_to_png
from urlhelpers import getImage
from parseattribs import parseLoot
from format import formatTitle

numberRegex = re.compile('([0-9]+[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*)')
imageRegex = re.compile('<a href="([^"]*)"[ \t\n]*class="image image-thumbnail"')
imageRegex2 = re.compile('src="([^"]*vignette[^"]*)"')

def getBoolean(attributes, attrib, default=False):
    if attrib not in attributes:
        return default
    return attributes[attrib].strip().lower() == 'yes'

def getInteger(attributes, attrib):
    if attrib not in attributes:
        return None
    match = numberRegex.search(attributes[attrib])
    if match != None: 
        return int(match.groups()[0].replace(',','').replace('.',''))
    return None


def getMaxInteger(attributes, attrib):
    if attrib not in attributes:
        return None
    maxint = None
    for integers in numberRegex.findall(attributes[attrib]):
        val = int(integers.replace(',','').replace('.',''))
        if maxint == None or val > maxint:
            maxint = val
    return maxint

itemmap = dict()
itemmap['sais'] = 'sai'
itemmap['gold coins'] = 'gold coin'
itemmap['platinum coins'] = 'platinum coin'
itemmap['rusty armor'] = 'rusty armor'
itemmap['clusters of solace'] = 'cluster of solace'
itemmap['the lethal lissy\'s shirt'] = 'lethal lissy\'s shirt'
itemmap['music sheet'] = 'music sheet'
itemmap['picture'] = 'picture'
itemmap['part of a jester doll'] = 'part of a jester doll'

lootListRegex = re.compile('<table class="loot_list sortable">')
wikiURLRegex = re.compile('<td><a href="/wiki/([^"]+)"')
questionMarkRegex = re.compile('([^?]*)')
lootChanceRegex = re.compile('([0-9]+(?:[.][0-9]+)?)[%]')
abilityRegex = re.compile('<[^>]*>')
lootCountRegex = re.compile('<td class="loot_list_no_border">([0-9]+)[-]([0-9]+)</td>')

def filterItemName(item_name):
    item_name = item_name.lstrip('/wiki/').replace('_', ' ').replace('%27', '\'').replace('%C3%B1', 'n').lower()
    item_name = re.sub(r' \([^(]*\)', '', item_name).strip() #remove parenthesis
    match = questionMarkRegex.search(item_name)
    item_name = match.groups()[0]
    if item_name in itemmap: item_name = itemmap[item_name]
    return item_name


passList = ['Liberty Bay Fortress', 'Kraknaknork\'s Dimension', 'Snow White Room', 'Glooth']

def parseCreature(title, attributes, c, creaturedrops, getURL):
    if title in passList:
        return False
    name = title
    if 'actualname' in attributes and len(attributes['actualname']) > 0:
        name = formatTitle(attributes['actualname'])
    elif 'name' in attributes and len(attributes['name']) > 0:
        name = formatTitle(attributes['name'])
    hp = getInteger(attributes, 'hp')
    exp = None
    if 'exp' in attributes:
        try: exp = int(attributes['exp'])
        except: pass
    summon = None
    if 'summon' in attributes:
        try: summon = int(attributes['summon'])
        except: pass
    illusionable = getBoolean(attributes,'illusionable')
    pushable = getBoolean(attributes,'pushable')
    pushes = getBoolean(attributes,'pushes')
    paralysable = getBoolean(attributes,'paraimmune')
    senseinvis = getBoolean(attributes,'senseinvis', True)
    armor = getInteger(attributes,'armor')
    maxdmg = getMaxInteger(attributes,'maxdmg')
    physical = getInteger(attributes,'physicalDmgMod')
    holy = getInteger(attributes,'holyDmgMod')
    death = getInteger(attributes,'deathDmgMod')
    fire = getInteger(attributes,'fireDmgMod')
    energy = getInteger(attributes,'energyDmgMod')
    ice = getInteger(attributes,'iceDmgMod')
    earth = getInteger(attributes,'earthDmgMod')
    drown = getInteger(attributes,'drownDmgMod')
    lifedrain = getInteger(attributes,'hpDrainDmgMod')
    speed = getInteger(attributes,'speed')
    boss = False
    if 'isboss' in attributes and attributes['isboss'].lower().strip() == 'yes':
        boss = True
    abilities = None
    if 'abilities' in attributes:
        # first take care of [[Fire Rune||Great Fireball]] => Great Fireball
        b = re.sub(r'\[\[[^]|]+\|([^]]+)\]\]', '\g<1>', attributes['abilities'])
        # then take care of [[Fire Rune]] => Fire Rune
        b = re.sub(r'\[\[([^]]+)\]\]', '\g<1>', b)
        # sometimes there are links in single brackets [http:www.link.com] => remove htem
        b = re.sub(r'\[[^]]+\]', '', b)
        # if there are brackets without numbers, remove them (maybe not necessary)
        b = re.sub(r'\(([^0-9]+)\)', '', b)
        # replace double spaces with single spaces
        b = b.replace('  ', ' ')
        # if there are commas in brackets (300-500, Fire Damage) => replace the comma with a semicolon (for later splitting purposes)
        abilities = re.sub(r'(\([^,)]+)\,([^)]+\))', '\g<1>;\g<2>', b)
    url = "http://tibia.wikia.com/wiki/%s" % (title.replace(' ', '_'))
    image = getImage(url, getURL, imageRegex, crop_image)
    if image == None or image == False:
        url = "http://tibia.wikia.com/wiki/File:%s.gif" % (title.replace(' ', '_'))
        image = getImage(url, getURL, imageRegex2, crop_image)
        if image == None or image == False:
            print('failed to get image for creature', title)
            return False

    if physical == None:
        print('pass', title)

    # add stuff to database
    c.execute('INSERT INTO Creatures (title,name,health,experience,maxdamage,summon,illusionable,pushable,pushes,physical,holy,death,fire,energy,ice,earth,drown,lifedrain,paralysable,senseinvis,image,abilities,speed,armor,boss) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)',
        (title,name, hp, exp, maxdmg, summon, illusionable, pushable, pushes, physical, holy, death, fire, energy, ice, earth, drown, lifedrain, paralysable, senseinvis, image, abilities, speed, armor,boss))
    creatureid = c.lastrowid

    creaturedrops[creatureid] = dict()
    # for some reason loot statistics are not in the xml file, so we get it from the website
    url = 'http://tibia.wikia.com/wiki/Loot_Statistics:%s' % title.replace(' ','_')
    stats = getURL(url, True)
    if stats != None:
        loot_stats = list()
        current_index = 0
        while True:
            match = lootListRegex.search(stats[current_index:])
            if match == None: break
            index = match.end()
            match = lootListRegex.search(stats[current_index + index:])
            if match == None: endindex = len(stats) - current_index
            else: endindex = index + match.start()
            kill_count = 0
            match = re.search('([0-9]+) kills', stats[current_index + index:current_index + endindex])
            if match != None: kill_count = int(match.groups()[0])
            list.append(loot_stats, [current_index + index, current_index + endindex, kill_count])
            current_index = current_index + endindex
        lootdrops = dict()
        killcount = dict()
        score = dict()
        for i in range(len(loot_stats)):
            index = loot_stats[i][0]
            endindex = loot_stats[i][1]
            kills = loot_stats[i][2]
            lootdrops[i] = dict()
            killcount[i] = kills
            bag = False
            highpercentage = False
            while True:
                match = wikiURLRegex.search(stats[index:endindex])
                if match == None: break
                item_name = filterItemName(match.groups()[0]).lower()
                    # creatures don't drop bags, but they used to in the past
                    # if there is a bag in the creature kills, we know it's old
                startindex = index
                index = index + match.end()
                if index > endindex or item_name == "loot": break
                match = lootCountRegex.search(stats[startindex:index])
                if match == None:
                    mindrop = 1
                    maxdrop = 1
                else:
                    mindrop = int(match.groups()[0])
                    maxdrop = int(match.groups()[1])
                match = lootChanceRegex.search(stats[index:])
                if match == None: break
                percentage = float(match.groups()[0])
                if percentage > 100:
                    highpercentage = True
                index = index + match.end()
                if item_name.strip() == "bag":
                    bag = True
                else: 
                    lootdrops[i][item_name] = (percentage, mindrop, maxdrop)
            score[i] = 0 if bag else (1 if highpercentage else len(lootdrops[i]))
        maxdict = dict()
        maxkey = ""
        killcount[maxkey] = -1
        score[maxkey] = -1
        # pick the loot statistics with the most items
        for key in lootdrops.keys():
            if score[key] > score[maxkey]:
                maxdict = lootdrops[key]
                maxkey = key
            elif score[key] == score[maxkey] and killcount[key] > killcount[maxkey]:
                # if the items are equal, pick the one with the highest kills
                maxdict = lootdrops[key]
                maxkey = key
        creaturedrops[creatureid] = maxdict
    # read the dropped items from the 'loot' attribute
    if 'loot' in attributes:
        lootItems = [filterItemName(x).lower() for x in parseLoot(attributes['loot'])]
        for item in lootItems:
            if item not in creaturedrops[creatureid]:
                creaturedrops[creatureid][item] = (None, 1, 1)
    return True

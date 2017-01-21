    
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

lookRegex = re.compile('(<div id="twbox-look">[^\n]*)')
htmlTagRegex = re.compile('<[^>]*>')
numberRegex = re.compile('([0-9]+[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*)')
imageRegex = re.compile('<a href="([^"]*)"[ \t\n]*class="image image-thumbnail"')
imageRegex2 = re.compile('src="([^"]*vignette[^"]*)"')

from imageoperations import crop_image, gif_is_animated, convert_to_png, properly_crop_item
from urlhelpers import getImage

consumableMap = {
    'pair of soft boots': ('4 hours', '10000', 'Gold Coin'), 
    'helmet of the ancients (Enchanted)': ('30 minutes', '1', 'Small Ruby'), 
    'sparking rainbow shield': ('15 minutes', '1', 'Small Amethyst'),
    'terran rainbow shield': ('15 minutes', '1', 'Small Emerald'),
    'fiery rainbow shield': ('15 minutes', '1', 'Small Ruby'),
    'icy rainbow shield': ('15 minutes', '1', 'Small Sapphire'),
    'enchanted werewolf amulet': ('60 minutes', '1', 'Moonlight Crystals'),
    'enchanted werewolf helmet (axe)': ('60 minutes', '1', 'Moonlight Crystals'),
    'enchanted werewolf helmet (club)': ('60 minutes', '1', 'Moonlight Crystals'),
    'enchanted werewolf helmet (sword)': ('60 minutes', '1', 'Moonlight Crystals'),
    'enchanted werewolf helmet (distance)': ('60 minutes', '1', 'Moonlight Crystals'),
    'enchanted werewolf helmet (magic)': ('60 minutes', '1', 'Moonlight Crystals'),
    'enchanted werewolf helmet (sword)': ('60 minutes', '1', 'Moonlight Crystals')
}


def parseItem(title, attributes, c, buyitems, sellitems, currencymap, durationMap, getURL):
    npcValue = None
    if 'npcvalue' in attributes:
        try: npcValue = int(attributes['npcvalue'])
        except: pass
    if (npcValue == None or npcValue == 0) and 'npcprice' in attributes:
        try: npcValue = int(attributes['npcprice'])
        except: pass
    actualValue = None
    if 'value' in attributes: 
        match = numberRegex.search(attributes['value'])
        if match != None: 
            actualValue = int(match.groups()[0].replace(',','').replace('.',''))
    npcPrice = None
    if 'npcprice' in attributes:
        try: 
            npcPrice = int(attributes['npcprice'])
            if actualValue != None and npcPrice > 0 and actualValue > npcPrice:
                actualValue = npcPrice
        except: 
            pass
    if npcValue != None and (actualValue == None or npcValue > actualValue):
        actualValue = npcValue
    name = title
    if 'actualname' in attributes and len(attributes['actualname']) > 0:
        name = attributes['actualname']
    capacity = None
    if 'weight' in attributes:
        try: capacity = float(attributes['weight'])
        except: pass
    stackable = False
    if 'stackable' in attributes:
        stackable = 'yes' in attributes['stackable'].strip().lower()
    category = None
    if 'primarytype' in attributes:
        category = attributes['primarytype']
    elif 'itemclass' in attributes:
        category = attributes['itemclass']
    duration, itemcost, itemcostcount = (None, None, None)
    durationText = None
    if title.lower() in consumableMap:
        tpl = consumableMap[title.lower()]
        durationText = tpl[0]
        itemcostcount = tpl[1]
        itemcost = tpl[2]
    elif 'duration' in attributes:
        durationText = attributes['duration']
        itemcost = title
        itemcostcount = 1
    if durationText != None:
        match = numberRegex.search(durationText)
        if match != None:
            duration = float(match.groups()[0])
            if duration != None:
                if 'minute' in durationText:
                    duration *= 60
                elif 'hour' in durationText:
                    duration *= 3600
    if duration != None:
        durationMap[title] = (duration, itemcost, itemcostcount)
    # tibia wiki uses some function to get the image url rather than storing it explicitly, and I don't really want to bother to decipher it
    url = "http://tibia.wikia.com/wiki/%s" % (title.replace(' ', '_'))
    itemHTML = getURL(url, True)
    if itemHTML == None: return False
    image = getImage(url, getURL, imageRegex, properly_crop_item)
    if image == None or image == False:
        url = "http://tibia.wikia.com/wiki/File:%s.gif" % (title.replace(' ', '_'))
        image = getImage(url, getURL, imageRegex2, properly_crop_item)
        if image == None or image == False:
            print('failed to get image for item', title)
    match = lookRegex.search(itemHTML)
    look_text = None
    if match != None:
        look_text = match.groups()[0]
        look_text = look_text.replace('&#32;', ' ').replace('\n', ' ').replace('.', '. ').replace('.  ', '. ')
        look_text = re.sub(r'[.] (\d)', '.\g<1>', look_text).strip()
        while True:
            match = htmlTagRegex.search(look_text)
            if match == None: break
            look_text = look_text[:match.start()] + look_text[match.end():]
    convert_to_gold, discard = False, False

    gold_ratio = max(0 if npcValue == None else npcValue, 0 if actualValue == None else actualValue) / (1 if capacity == None or capacity == 0 else capacity)
    if gold_ratio < 10: discard = True
    if gold_ratio < 20 and stackable == False: convert_to_gold = True
    else: convert_to_gold = False

    c.execute('INSERT INTO Items (title,name, vendor_value, actual_value, capacity, stackable, image, category, discard, convert_to_gold, look_text) VALUES (?,?,?,?,?,?,?,?,?,?,?)', (title,name, npcValue, actualValue, capacity, stackable, image, category, discard, convert_to_gold, look_text))
    itemid = c.lastrowid
    if 'itemid' in attributes:
        splits = attributes['itemid'].split(',')
        for item in splits:
            try: 
                tibiaid = int(item.strip())
                c.execute('INSERT INTO ItemIDMap (tibiaid, itemid) VALUES (?,?)', (tibiaid, itemid))
            except:
                pass
    if 'buyfrom' in attributes:
        buyitems[itemid] = dict()
        npcs = attributes['buyfrom'].split(',')
        for n in npcs:
            npc = n
            if npc == '' or npc == '-' or npc == '--': continue
            value = npcPrice
            if ';' in npc: 
                npc = npc.split(';')[0]
            if ':' in npc:
                token = npc.split(':')[1].strip()
                npc = npc.split(':')[0]
                try: 
                    value = math.ceil(float(token))
                except: 
                    match = re.search('\\[\\[([^]]+)\\]\\]', token)
                    if match == None:
                        continue
                    currencymap[itemid] = match.groups()[0]
                    match = numberRegex.search(token)
                    if match == None:
                        continue
                    value = float(match.groups()[0])
            if value == None: continue
            buyitems[itemid][npc.strip()] = value
    if 'sellto' in attributes:
        sellitems[itemid] = dict()
        npcs = attributes['sellto'].split(',')
        for n in npcs:
            npc = n
            if npc == '' or npc == '-' or npc == '--': continue
            value = npcValue
            if ';' in npc: 
                npc = npc.split(';')[0]
            if ':' in npc:
                value = math.ceil(float(npc.split(':')[1].strip()))
                npc = npc.split(':')[0]
            if value == None: continue
            sellitems[itemid][npc.strip()] = value
    if 'vocrequired' in attributes and len(attributes['vocrequired'].strip()) > 1:
        attributes['vocrequired'] = attributes['vocrequired'].replace('knights', 'k').replace('druids', 'd').replace('sorcerers', 's').replace('paladins', 'p').replace(' and ','+')
        c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Voc', attributes['vocrequired'].strip()))
    if 'resist' in attributes and len(attributes['resist'].strip()) > 1:
        elemental = attributes["resist"].split(",")
        for element in elemental:
            element = element.strip()
            m = re.search(r'([a-zA-Z0-9_ ]+) +(-?\+?\d+)%',element)
            if m:
                prop = m.group(1)+"%"
                try:
                    value = int(m.group(2))
                except ValueError:
                    value = 0
                c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, prop, value))
    if 'plural' in attributes:
        plural = attributes['plural'].strip().lower()
        if len(plural) > 2:
            f = open('pluralMap.txt', 'a')
            f.write('%s=%s\n' % (plural, name.strip().lower()))
            f.close()
    if 'damagetype' in attributes and len(attributes['damagetype'].strip()) > 1:
        c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Type', attributes['damagetype'].strip()))
    if 'attrib' in attributes and len(attributes['attrib'].strip()) > 1:
        attrib = attributes['attrib'].strip().replace("fighting", "").replace("level", "").replace("[","").replace("]","").replace("faster regeneration", "regen +1")
        c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Attrib', attrib))
    if 'armor' in attributes and len(attributes['armor'].strip()) > 0:
        try: 
            armor = int(attributes['armor'].strip())
            c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Arm', str(armor)))
        except: 
            print(name)
            print(attributes['armor'])
            exit(1)
            pass
    if 'attack' in attributes and len(attributes['attack'].strip()) > 0:
        try: 
            attack = int(attributes['attack'].strip())
            c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Atk', str(attack)))
        except: 
            attack = 0
            for atk in re.findall('[0-9]+', attributes['attack']):
                attack += int(atk)
            if attack == 0:
                print(name)
                print(attributes['attack'])
                exit(1)
                pass
            else: 
                c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Atk', str(attack)))
    if 'defense' in attributes and len(attributes['defense'].strip()) > 0:
        try: 
            defense = int(attributes['defense'].strip())
            if 'defensemod' in attributes and len(attributes['defensemod'].strip()) > 1: 
                defense = str(defense) + ' (' + attributes['defensemod'].strip() + ')'
            c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Def', str(defense)))
        except: 
            print(name)
            print(attributes['defense'])
            exit(1)
            pass
    if 'levelrequired' in attributes and len(attributes['levelrequired'].strip()) > 0:
        try: 
            levelrequired = int(attributes['levelrequired'].strip())
            c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Level', str(levelrequired)))
        except: 
            print(name)
            print(attributes['levelrequired'])
            exit(1)
    if 'damage' in attributes and len(attributes['damage'].strip()) > 0:
        try: 
            damage = int(attributes['damage'].strip())
            c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Atk', str(damage)))
        except: 
            try: 
                spl = attributes['damage'].split('-')
                damage = int((int(spl[0].strip()) + int(spl[1].strip())) / 2)
                c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Atk', str(damage)))
            except:
                pass
    if 'range' in attributes and len(attributes['range'].strip()) > 0:
        try: 
            rng = int(attributes['range'].strip())
            c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Range', str(rng)))
        except: 
            pass
    if 'atk_mod' in attributes and len(attributes['atk_mod'].strip()) > 0:
        c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Atk+', attributes['atk_mod'].strip()))
    if 'hit_mod' in attributes and len(attributes['hit_mod'].strip()) > 0:
        c.execute('INSERT INTO ItemProperties(itemid, property, value) VALUES (?,?,?)', (itemid, 'Hit+', attributes['hit_mod'].strip()))
    return True

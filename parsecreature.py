

import math
import re
import urllib.request
import sqlite3
import time

from imageoperations import crop_image, gif_is_animated, convert_to_png
from urlhelpers import getImage
from parseattribs import parseLoot

numberRegex = re.compile('([0-9]+[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*)')
imageRegex = re.compile('<a href="([^"]*)"[ \t\n]*class="image image-thumbnail"')
imageRegex2 = re.compile('src="([^"]*vignette[^"]*)"')

def getBoolean(attributes, attrib):
    if attrib not in attributes:
        return False
    return attributes[attrib].strip().lower() == 'yes'

def getInteger(attributes, attrib):
    if attrib not in attributes:
        return None
    match = numberRegex.search(attributes[attrib])
    if match != None: 
        return int(match.groups()[0].replace(',','').replace('.',''))
    return None

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
        name = attributes['actualname']
    elif 'name' in attributes and len(attributes['name']) > 0:
        name = attributes['name']
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
    senseinvis = getBoolean(attributes,'senseinvis')
    armor = getInteger(attributes,'armor')
    maxdmg = getInteger(attributes,'maxdmg')
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
    abilities = None
    if 'abilities' in attributes:
        abilities = re.sub('\\[[^|\\]]+\\|([^|\\]]+)\]\]', '\g<1>', attributes['abilities'].strip()).replace('[','').replace(']','')
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
    c.execute('INSERT INTO Creatures (title,name,health,experience,maxdamage,summon,illusionable,pushable,pushes,physical,holy,death,fire,energy,ice,earth,drown,lifedrain,paralysable,senseinvis,image,abilities,speed,armor) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)',
        (title,name, hp, exp, maxdmg, summon, illusionable, pushable, pushes, physical, holy, death, fire, energy, ice, earth, drown, lifedrain, paralysable, senseinvis, image, abilities, speed, armor))
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
        if len(loot_stats) > 0:
            loot_stats.sort(key=lambda x: x[2], reverse=True)
            index = loot_stats[0][0]
            endindex = loot_stats[0][1]
            while True:
                match = wikiURLRegex.search(stats[index:endindex])
                if match == None: break
                item_name = filterItemName(match.groups()[0]).lower()
                index = index + match.end()
                if index > endindex or item_name == "loot": break
                match = lootChanceRegex.search(stats[index:])
                if match == None: break
                percentage = float(match.groups()[0])
                index = index + match.end()
                creaturedrops[creatureid][item_name] = percentage
        else:
            print("Warning: found loot statistics page but no loot statistics for creature", name)
    # read the dropped items from the 'loot' attribute
    if 'loot' in attributes:
        lootItems = [filterItemName(x).lower() for x in parseLoot(attributes['loot'])]
        for item in lootItems:
            if item not in creaturedrops[creatureid]:
                creaturedrops[creatureid][item] = None
    return True

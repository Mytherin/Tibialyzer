

import math
import re
import urllib.request
import sqlite3
import time

htmlTagRegex = re.compile('<[^>]*>')
numberRegex = re.compile('([0-9]+[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*)')

def parseKey(title, attributes, c, keyImages, buyitems, sellitems, getURL):
    if 'number' not in attributes:
        return False
    try: 
        name = 'Key %s' % int(attributes['number'].lower().strip())
    except: 
        return False
    if 'aka' in attributes and len(attributes['aka'].strip()) > 0:
        aka_text = attributes['aka'].strip()
        # first take care of [[Fire Rune||Great Fireball]] => Great Fireball
        aka_text = re.sub(r'\[\[[^]|]+\|([^]]+)\]\]', '\g<1>', aka_text)
        # then take care of [[Fire Rune]] => Fire Rune
        aka_text = re.sub(r'\[\[([^]]+)\]\]', '\g<1>', aka_text)
        name = "%s (%s)" % (name, aka_text)
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

    stackable = False
    capacity = 1
    image = None
    category = "Keys"
    convert_to_gold, discard = False, False
    look_text = None
    if 'longnotes' in attributes:
        look_text = attributes['longnotes']
    elif 'shortnotes' in attributes:
        look_text = attributes['shortnotes']
    if look_text != None:
        # first take care of [[Fire Rune||Great Fireball]] => Great Fireball
        look_text = re.sub(r'\[\[[^]|]+\|([^]]+)\]\]', '\g<1>', look_text)
        # first take care of {{Character|<Name>}} => <Name>
        look_text = re.sub(r'\{\{[^}|]+\|([^}]+)\}\}', '\g<1>', look_text)
        # then take care of [[Fire Rune]] => Fire Rune
        look_text = re.sub(r'\[\[([^]]+)\]\]', '\g<1>', look_text)
        # sometimes there are links in single brackets [http:www.link.com] => remove htem
        look_text = re.sub(r'\[[^]]+\]', '', look_text)
        # remove html tags
        look_text = look_text.replace("<br />", " ").replace("<br/>", " ").replace("<br>", " ").replace("\n", " ")
        # replace double spaces with single spaces
        look_text = look_text.replace('  ', ' ')
        # Remove spoilers 
        if "{{JSpoiler" in look_text:
            look_text = look_text.replace(look_text[look_text.find("{{JSpoiler"):], "")
        if "<gallery>" in look_text:
            look_text = look_text.replace(look_text[look_text.find("<gallery>"):], "")

    c.execute('INSERT INTO Items (title,name, vendor_value, actual_value, capacity, stackable, image, category, discard, convert_to_gold, look_text) VALUES (?,?,?,?,?,?,?,?,?,?,?)', (title,name, npcValue, actualValue, capacity, stackable, image, category, discard, convert_to_gold, look_text))
    itemid = c.lastrowid
    keyImages[itemid] = attributes['primarytype']
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
    return True
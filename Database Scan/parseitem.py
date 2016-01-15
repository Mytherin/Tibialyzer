
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

def parseItem(title, attributes, c, buyitems, sellitems, currencymap, getURL):
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
            return False
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
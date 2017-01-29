
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

numberRegex = re.compile('([0-9]+[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*)')
imageRegex = re.compile('<a href="([^"]*)"[ \t\n]*class="image image-thumbnail"')

exclude_spells = ["Ultimate Explosion", "Force Strike"]

def parseSpell(title, attributes, c, getURL):
    if title in exclude_spells:
        print("Skipped excluded spell",title)
        return True
    name = title
    if 'name' in attributes:
        name = attributes['name']
    words = None
    if 'words' in attributes:
        words = attributes['words']
    premium = False
    if 'premium' in attributes:
        premium = attributes['premium'] == 'yes'
    promotion = False
    if 'promotion' in attributes:
        promotion = attributes['promotion'] == 'yes'
    mana = None
    if 'mana' in attributes:
        match = numberRegex.search(attributes['mana'])
        if match != None: 
            mana = int(match.groups()[0].replace(',','').replace('.',''))
        else: mana = -1
    levelrequired = None
    if 'levelrequired' in attributes:
        levelrequired = int(attributes['levelrequired'])
    spellcost = None
    if 'spellcost' in attributes:
        spellcost = int(attributes['spellcost'])
    element = None
    if 'damagetype' in attributes:
        element = attributes['damagetype']
    if mana == None or words == None:
        return False
    knight = False
    druid = False
    sorcerer = False
    paladin = False
    if 'voc' in attributes:
        knight = 'knight' in attributes['voc'].lower()
        druid = 'druid' in attributes['voc'].lower()
        sorcerer = 'sorcerer' in attributes['voc'].lower()
        paladin = 'paladin' in attributes['voc'].lower()
    cooldown = None
    if 'cooldown' in attributes:
        cooldown = attributes['cooldown']
    url = "http://tibia.wikia.com/wiki/%s" % (title.replace(' ', '_'))
    itemHTML = getURL(url, True)
    if itemHTML == None: 
        print('Failed to get url', url)
        return False
    image_index = 0
    match = imageRegex.search(itemHTML)
    while (match != None and '.gif' not in match.groups()[0]):
        image_index = image_index + match.end()
        match = imageRegex.search(itemHTML[image_index:])
    image = None
    if match != None:
        url = match.groups()[0].replace('&amp;', '&')
        imageBinary = getURL(url, False)
        image = sqlite3.Binary(imageBinary)
    elif title == "Strong Ice Strike":
        imageBinary = getURL("http://vignette4.wikia.nocookie.net/tibia/images/0/04/Strong_Ice_Strike.gif/revision/latest?cb=20101205171332&path-prefix=en", False)
        image = sqlite3.Binary(imageBinary)
    if image == None:
        print('failed to get image for spell', title)
        return False
    c.execute('INSERT INTO Spells (name,words,element,cooldown,premium,promotion,levelrequired,goldcost,manacost,knight,paladin,sorcerer,druid,image) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?)', (name, words, element, cooldown, premium, promotion, levelrequired, spellcost, mana, knight, paladin, sorcerer, druid, image))
    return True



    



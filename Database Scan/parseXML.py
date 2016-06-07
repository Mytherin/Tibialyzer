
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
tibia_xml_file = 'tibiawiki_pages_current.xml'
database_file = 'database.db'
cache_file = 'tempcache'
tag_prefix = '{http://www.mediawiki.org/xml/export-0.6/}'

def getTag(tag): return tag_prefix + tag

import pickle
import urllib.request
import sqlite3

from format import formatTitle

from parseitem import parseItem
from parsehunt import parseHunt
from parsecreature import parseCreature
from parsenpc import parseNPC
from parsespell import parseSpell
from parsequest import parseQuest
from parseattribs import parseAttributes
from parseoutfit import parseOutfit
from parsemount import parseMount
from parseobject import parseObject
from parsekey import parseKey
from parseachievement import parseAchievement

webcache = dict()

saveCounter = 0

def saveCache(always = False):
    global saveCounter
    saveCounter += 1
    if saveCounter >= 50 or always:
        saveCounter = 0
        print('Saving cache...')
        f = open(cache_file, 'wb')
        pickle.dump(webcache, f)
        f.close()
        print('Done')

def loadCache():
    f = open(cache_file, 'rb')
    cache = pickle.load(f)
    f.close()
    return cache

try: webcache = loadCache()
except: pass

def getURL(url, decode=False):
    url = url.replace('ñ', '%C3%B1')
    global webcache
    if url in webcache:
        return webcache[url]
    try: 
        response = urllib.request.urlopen(url)
    except: 
        webcache[url] = None
        saveCache()
        return None
    if decode:
        result = response.read().decode('utf-8')
    else:
        result = response.read()
    webcache[url] = result
    saveCache()
    return result


skipLoading = False
if not skipLoading:
    import os
    try: os.remove(database_file)
    except: pass
    try: os.remove('pluralMap.txt')
    except: pass
    f = open('pluralMap.txt', 'w')
    f.close()

conn = sqlite3.connect(database_file)
c = conn.cursor()


if not skipLoading:
    root = xml.etree.ElementTree.parse(tibia_xml_file).getroot()
    c.execute('CREATE TABLE Items(id INTEGER PRIMARY KEY AUTOINCREMENT, title STRING, name STRING, vendor_value BIGINT, actual_value BIGINT, capacity FLOAT, stackable BOOLEAN, image BLOB, category STRING, discard BOOLEAN, convert_to_gold BOOLEAN, look_text STRING, currency INTEGER)')
    c.execute('CREATE TABLE ItemProperties(itemid INTEGER, property STRING, value STRING)')
    c.execute('CREATE TABLE NPCs(id INTEGER PRIMARY KEY AUTOINCREMENT, title STRING, name STRING, city STRING, job STRING, x INTEGER, y INTEGER, z INTEGER, image BLOB)')
    c.execute('CREATE TABLE NPCDestinations(npcid INTEGER, destination STRING, cost INTEGER, notes STRING)')
    c.execute('CREATE TABLE SellItems(itemid INTEGER, vendorid INTEGER, value INTEGER)')
    c.execute('CREATE TABLE BuyItems(itemid INTEGER, vendorid INTEGER, value INTEGER)')
    c.execute('CREATE TABLE Creatures(id INTEGER PRIMARY KEY AUTOINCREMENT, title STRING, name STRING, health INTEGER, experience INTEGER, maxdamage INTEGER, summon INTEGER, illusionable BOOLEAN, pushable BOOLEAN, pushes BOOLEAN, physical INTEGER, holy INTEGER, death INTEGER, fire INTEGER, energy INTEGER, ice INTEGER, earth INTEGER, drown INTEGER, lifedrain INTEGER, paralysable BOOLEAN, senseinvis BOOLEAN, image BLOB, abilities STRING, speed INTEGER, armor INTEGER, boss BOOLEAN)')
    c.execute('CREATE TABLE CreatureDrops(creatureid INTEGER, itemid INTEGER, percentage FLOAT, min INTEGER, max INTEGER)')
    c.execute('CREATE TABLE HuntingPlaces(id INTEGER PRIMARY KEY AUTOINCREMENT, name STRING, level INTEGER, exprating INTEGER, lootrating INTEGER, image STRING, city STRING)')
    c.execute('CREATE TABLE HuntingPlaceCoordinates(huntingplaceid INTEGER, x INTEGER, y INTEGER, z INTEGER)')
    c.execute('CREATE TABLE HuntingPlaceCreatures(huntingplaceid INTEGER, creatureid INTEGER)')  
    c.execute('CREATE TABLE Spells(id INTEGER PRIMARY KEY AUTOINCREMENT, name STRING, words STRING,element INTEGER, cooldown INTEGER, premium BOOLEAN, promotion BOOLEAN, levelrequired INTEGER, goldcost INTEGER, manacost INTEGER, knight BOOLEAN, paladin BOOLEAN, sorcerer BOOLEAN, druid BOOLEAN, image BLOB)')
    c.execute('CREATE TABLE SpellNPCs(spellid INTEGER, npcid INTEGER, knight BOOLEAN, druid BOOLEAN, paladin BOOLEAN, sorcerer BOOLEAN)')
    c.execute('CREATE TABLE Quests(id INTEGER PRIMARY KEY AUTOINCREMENT, title STRING, name STRING, minlevel INT, premium BOOLEAN, city STRING, legend STRING)')
    c.execute('CREATE TABLE QuestRewards(questid INT, itemid INT)')
    c.execute('CREATE TABLE QuestOutfits(questid INT, outfitid INT)')
    c.execute('CREATE TABLE QuestDangers(questid INT, creatureid INT)')
    c.execute('CREATE TABLE QuestNPCs(questid INTEGER, npcid INTEGER)')
    c.execute('CREATE TABLE Achievements(id INTEGER PRIMARY KEY, name STRING, grade INTEGER, points INTEGER, description STRING, spoiler STRING, image INT, imagetype INT)')
    c.execute('CREATE TABLE Outfits(id INTEGER PRIMARY KEY AUTOINCREMENT, title STRING, name STRING, premium BOOLEAN, tibiastore BOOLEAN)')
    c.execute('CREATE TABLE OutfitImages(outfitid INTEGER, male BOOLEAN, addon INTEGER, image BLOB)')
    c.execute('CREATE TABLE Mounts(id INTEGER PRIMARY KEY AUTOINCREMENT, title STRING, name STRING, tameitemid INTEGER, tamecreatureid INTEGER, speed INTEGER, tibiastore BOOLEAN, image BLOB)')
    c.execute('CREATE TABLE WorldObjects(title STRING, name STRING, image BLOB)')
    c.execute('CREATE TABLE ItemIDMap(tibiaid INTEGER PRIMARY KEY, itemid INTEGER)')
    c.execute('CREATE TABLE ConsumableItems(itemid INTEGER, duration INTEGER, costitemid INTEGER, costitemcount INTEGER)')


buyitems = dict()
sellitems = dict()
currencymap = dict()
spells = dict()
huntcreatures= dict()
creaturedrops = dict()
rewardItems = dict()
questDangers = dict()
mountStuff = dict()
questNPCs = dict()
keyItems = dict()
durationCostMap = dict()
achievementReferences = dict()

import re
def wordCount(input_string, word):
    return sum(1 for _ in re.finditer(r'%s' % re.escape(word), input_string))


if not skipLoading:
    for child in root.getchildren():
        titleTag = child.find(getTag('title'))
        if titleTag == None: continue
        title = titleTag.text
        if 'help:' in title.lower() or 'talk:' in title.lower() or 'template:' in title.lower() or 'updates/' in title.lower() or 'user:' in title.lower() or 'loot/' in title.lower() or 'tibiawiki:' in title.lower(): continue
        title = formatTitle(title)
        revisionTag = child.find(getTag('revision'))
        if revisionTag == None: continue
        textTag = revisionTag.find(getTag('text'))
        if textTag == None: continue
        content = textTag.text
        if content == None: continue
        attributes = parseAttributes(content)
        lcontent = content.lower()
        if '/Spoiler' in title:
            quest = title.replace('/Spoiler', '')
            questNPCs[quest] = re.findall('\[\[([^]|]+)(?:|[^]]+)?\]\]', lcontent)
        elif wordCount(lcontent, 'infobox hunt|') == 1 or wordCount(lcontent, 'infobox_hunt|') == 1:
            #print('Hunt', title)
            if not parseHunt(title, attributes, c, content, huntcreatures, getURL):
                print('Hunt failed', title)
        elif wordCount(lcontent, 'infobox quest') == 1 or wordCount(lcontent, 'infobox_quest') == 1:
            #print('Quest', title)
            if not parseQuest(title, attributes, c, rewardItems, questDangers, getURL):
                print('Quest failed', title)
        elif wordCount(lcontent, '{{infobox item') == 1 or wordCount(lcontent, '{{infobox_item') == 1:
            #print('Item', title)
            if not parseItem(title, attributes, c, buyitems, sellitems, currencymap, durationCostMap, getURL):
                print('Item failed', title)
        elif wordCount(lcontent, '{{infobox npc') == 1 or wordCount(lcontent, '{{infobox_npc') == 1:
            #print('NPC', title)
            if not parseNPC(title, attributes, c, spells, getURL):
                print('NPC failed', title)
        elif '{{infobox_spell' in lcontent or '{{infobox spell' in lcontent:
            #print('Spell', title)
            if not parseSpell(title, attributes, c, getURL):
                print('Spell failed', title)
        elif wordCount(lcontent, '{{infobox creature') == 1 or wordCount(lcontent, '{{infobox_creature') == 1:
            #print('Creature', title)
            if not parseCreature(title, attributes, c, creaturedrops, getURL):
                print('Creature failed', title)
        elif wordCount(lcontent, 'infobox outfit') == 1 or wordCount(lcontent, 'infobox_outfit') == 1:
            #print('Outfit', title)
            if not parseOutfit(title, attributes, c, getURL):
                print('Outfit failed', title)
        elif wordCount(lcontent, 'infobox mount') == 1 or wordCount(lcontent, 'infobox_mount') == 1:
            #print('Mount', title)
            if not parseMount(title, attributes, c, mountStuff, getURL):
                print('Mount failed', title)
        elif wordCount(lcontent, 'infobox object') == 1 or wordCount(lcontent, 'infobox_object') == 1:
            #print('Object', title)
            if not parseObject(title, attributes, c, getURL):
                print('Object failed', title)
        elif wordCount(lcontent, '{{infobox key') == 1 or wordCount(lcontent, '{{infobox_key') == 1:
            #print('Key', title)
            if not parseKey(title, attributes, c, keyItems, buyitems, sellitems, getURL):
                print('Key failed', title)
        elif wordCount(lcontent, '{{infobox achievement') == 1 or wordCount(lcontent, '{{infobox_achievement') == 1:
            #print('Achievement', title)
            if not parseAchievement(title, attributes, c, achievementReferences):
                print('Achievement failed', title)
    saveCache(True)

    conn.commit()

    d = dict()
    d['buyitems'] = buyitems
    d['sellitems'] = sellitems
    d['currencymap'] = currencymap
    d['spells'] = spells
    d['huntcreatures'] = huntcreatures
    d['creaturedrops'] = creaturedrops
    d['rewardItems'] = rewardItems
    d['questDangers'] = questDangers
    d['mountStuff'] = mountStuff
    d['questNPCs'] = questNPCs
    d['keyItems'] = keyItems
    d['durationCostMap'] = durationCostMap
    d['achievementReferences'] = achievementReferences

    f = open('valuedict', 'wb')
    pickle.dump(d, f)
    f.close()

f = open('valuedict', 'rb')
d = pickle.load(f)

buyitems = d['buyitems'] 
sellitems = d['sellitems'] 
currencymap = d['currencymap'] 
spells = d['spells'] 
huntcreatures = d['huntcreatures'] 
creaturedrops = d['creaturedrops'] 
rewardItems = d['rewardItems'] 
questDangers = d['questDangers'] 
mountStuff = d['mountStuff'] 
questNPCs = d['questNPCs']
keyItems = d['keyItems']
durationCostMap = d['durationCostMap']
achievementReferences = d['achievementReferences']

def attemptGetImage(reference):
    reference = reference.lower().strip()
    c.execute('SELECT id FROM Outfits WHERE LOWER(name)=? OR LOWER(title)=?', (reference, reference))
    results = c.fetchall()
    if len(results) > 0:
        return (results[0][0], 1)
    c.execute('SELECT id FROM Mounts WHERE LOWER(name)=? OR LOWER(title)=?', (reference, reference))
    results = c.fetchall()
    if len(results) > 0:
        return (results[0][0], 2)
    c.execute('SELECT id FROM Quests WHERE LOWER(name)=? OR LOWER(title)=?', (reference, reference))
    results = c.fetchall()
    if len(results) > 0:
        return (results[0][0], 3)
    c.execute('SELECT id FROM HuntingPlaces WHERE LOWER(name)=?', (reference,))
    results = c.fetchall()
    if len(results) > 0:
        return (results[0][0], 4)
    c.execute('SELECT id FROM Creatures WHERE LOWER(name)=? OR LOWER(title)=?', (reference, reference))
    results = c.fetchall()
    if len(results) > 0:
        return (results[0][0], 5)
    c.execute('SELECT id FROM NPCs WHERE LOWER(name)=? OR LOWER(title)=?', (reference, reference))
    results = c.fetchall()
    if len(results) > 0:
        return (results[0][0], 6)
    c.execute('SELECT id FROM Items WHERE LOWER(name)=? OR LOWER(title)=?', (reference, reference))
    results = c.fetchall()
    if len(results) > 0:
        return (results[0][0], 7)
    c.execute('SELECT id FROM Spells WHERE LOWER(name)=?', (reference, ))
    results = c.fetchall()
    if len(results) > 0:
        return (results[0][0], 8)
    return(None,None)


# add images to achievements
for achievementid,references in iter(achievementReferences.items()):
    for reference in references:
        (imageid,imagetype) = attemptGetImage(reference)
        if imageid != None:
            c.execute('UPDATE Achievements SET image=? WHERE id=?', (imageid, achievementid))
            c.execute('UPDATE Achievements SET imagetype=? WHERE id=?', (imagetype, achievementid))
            break

# add images to keys
for keyid,primarytype in iter(keyItems.items()):
    primaryname = "%s key" % primarytype.lower().strip()
    c.execute('SELECT image FROM Items WHERE LOWER(name)=? OR LOWER(title)=?', (primaryname,primaryname))
    results = c.fetchall()
    if len(results) > 0:
        imageBinary = results[0][0]
        c.execute("UPDATE Items SET image=? WHERE id=?", (imageBinary,keyid))
    else:
        print("unrecognized primary type %s", primarytype)
        exit()


c.execute('DROP TABLE ConsumableItems')
c.execute('CREATE TABLE ConsumableItems(itemid INTEGER, duration INTEGER, costitemid INTEGER, costitemcount INTEGER)')
for itemname,tpl in iter(durationCostMap.items()):
    name = itemname.strip().lower()
    c.execute('SELECT id FROM Items WHERE LOWER(name)=? OR LOWER(title)=?', (name, name))
    results = c.fetchall()
    itemid = results[0][0]
    duration = tpl[0]
    itemcost = tpl[1]
    itemcostcount = tpl[2]
    name = itemcost.strip().lower()
    print(tpl)
    c.execute('SELECT id FROM Items WHERE LOWER(name)=? OR LOWER(title)=?', (name, name))
    results = c.fetchall()
    costid = results[0][0]
    c.execute('INSERT INTO ConsumableItems (itemid, duration, costitemid, costitemcount) VALUES (?,?,?,?)', (itemid, duration, costid, itemcostcount))


#fix typos in spells
spellMap = {'desintegrate':'disintegrate', 'paralyze':'paralyse','avalanche (rune)':'avalanche'}

# Druid, Paladin, Sorcerer, Knight
npcTeachMap = { 'gundralph': (True, False, True, False),
                'eroth': (True, True, False, False),
                'faluae': (True, True, False, False),
                'eremo': (True, True, True, True),
                'ursula': (False, True, False, False),
                'zoltan': (True, True, True, True),
                'hjaern': (True, False, False, False),
                'elathriel': (True, False, False, False),
                'shalmar': (True, False, True, False),
                'razan': (False, True, False, True),
                'puffels': (False, False, False, True), 
                'asrak': (False, True, False, True),
                'maealil': (True, True, False, False),
                'eliza': (True, True, True, True),
                'ser tybald': (False, True, False, True),
                'garamond': (True, False, True, False),
                'smiley': (True, False, False, False),}

c.execute('DROP TABLE SpellNPCs')
c.execute('CREATE TABLE SpellNPCs(spellid INTEGER, npcid INTEGER, knight BOOLEAN, druid BOOLEAN, paladin BOOLEAN, sorcerer BOOLEAN)')
for npcid,spelllist in iter(spells.items()):
    for spell in spelllist:
        _spell = spell.strip().lower()
        if _spell in spellMap:
            _spell = spellMap[_spell]
        c.execute('SELECT id, druid, paladin, sorcerer, knight FROM Spells WHERE LOWER(name)=?', (_spell,))
        spellresults = c.fetchall()[0]
        if len(spellresults) > 0:
            spellid = spellresults[0]
            c.execute('SELECT spellid,npcid FROM SpellNPCs WHERE spellid=? AND npcid=?', (spellid, npcid))
            if len(c.fetchall()) > 0:
                continue
            c.execute('SELECT name,job FROM NPCs WHERE id=?', (npcid,))
            results = c.fetchall()[0]
            name = results[0].lower()
            job = results[1].lower()
            druid = False
            paladin = False
            sorcerer = False
            knight = False
            if 'druid' in job: druid = spellresults[1]
            elif 'sorcerer' in job: sorcerer = spellresults[3]
            elif 'knight' in job: knight = spellresults[4]
            elif 'paladin' in job: paladin = spellresults[2]

            if name in npcTeachMap:
                druid = npcTeachMap[name][0] and spellresults[1]
                paladin = npcTeachMap[name][1] and spellresults[2]
                sorcerer = npcTeachMap[name][2] and spellresults[3]
                knight = npcTeachMap[name][3] and spellresults[4]

            if druid == False and paladin == False and sorcerer == False and knight == False:
                print('Unknown NPC spell vocation', name)
            c.execute('INSERT INTO SpellNPCs(spellid, npcid, druid, paladin, sorcerer, knight) VALUES (?,?,?,?,?,?)', (spellid, npcid, druid, paladin, sorcerer, knight))
        else:
            pass#print("Unrecognized spell", spell)

c.execute('DROP TABLE BuyItems')
c.execute('CREATE TABLE BuyItems(itemid INTEGER, vendorid INTEGER, value INTEGER)')
for itemid, npclist in iter(buyitems.items()):
    for npc,value in iter(npclist.items()):
        c.execute('SELECT id FROM NPCs WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (npc.strip().lower(),npc.strip().lower(),npc.strip().lower()))
        results = c.fetchall()
        if len(results) > 0:
            npcid = results[0][0]
            c.execute('INSERT INTO BuyItems(itemid, vendorid, value) VALUES(?,?,?)', (itemid,npcid,value))
        else:
            pass#print("Unrecognized NPC", npc)

c.execute('DROP TABLE SellItems')
c.execute('CREATE TABLE SellItems(itemid INTEGER, vendorid INTEGER, value INTEGER)')
for itemid, npclist in iter(sellitems.items()):
    for npc,value in iter(npclist.items()):
        c.execute('SELECT id FROM NPCs WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (npc.strip().lower(),npc.strip().lower(),npc.strip().lower()))
        results = c.fetchall()
        if len(results) > 0:
            npcid = results[0][0]
            c.execute('INSERT INTO SellItems(itemid, vendorid, value) VALUES(?,?,?)', (itemid,npcid,value))
        else:
            pass#print("Unrecognized NPC", npc)


for itemid,currency in iter(currencymap.items()):
    c.execute('SELECT id FROM Items WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (currency.strip().lower(),currency.strip().lower(),currency.strip().lower()))
    results = c.fetchall()
    if len(results) > 0:
        currencyid = results[0][0]
        c.execute('UPDATE Items SET currency=? WHERE id=?', (currencyid,itemid))
    else:
        pass#print("Unrecognized Item", currency)


c.execute('DROP TABLE QuestNPCs')
c.execute('CREATE TABLE QuestNPCs(questid INTEGER, npcid INTEGER)')
for questname, npcs in iter(questNPCs.items()):
    questname = questname.strip().lower()
    c.execute('SELECT id FROM Quests WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (questname, questname,questname))
    results = c.fetchall()
    if len(results) > 0:
        questid = results[0][0]
        for npc in npcs:
            npc = npc.strip().lower()
            c.execute('SELECT id FROM NPCs WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (npc,npc,npc))
            results2 = c.fetchall()
            if len(results2) > 0:
                npcid = results2[0][0]
                c.execute('SELECT questid FROM QuestNPCs WHERE questid=? AND npcid=?', (questid, npcid))
                if len(c.fetchall()) == 0:
                    c.execute('INSERT INTO QuestNPCs (questid, npcid) VALUES (?,?)', (questid, npcid))
    else:
        print("Unrecognized Quest", questname)

# fix typos in creatures
creatureMap = {'bone beast':'bonebeast', 'crystal crusher':'crystalcrusher', 'carniphilia': 'carniphila', 
'slime (creature)': 'slime', 'ghouls':'ghoul', 'orc spearmen':'orc spearman', 'monks': 'monk', 'beholder': 'bonelord', 'demon skeletons':'demon skeleton',
'glooth glob': 'glooth blob', 'tiquanda\'s revenge':'tiquandas revenge'}

c.execute('DROP TABLE HuntingPlaceCreatures')
c.execute('CREATE TABLE HuntingPlaceCreatures(huntingplaceid INTEGER, creatureid INTEGER)')  
for huntingplaceid,creaturelist in iter(huntcreatures.items()):
    for creature in creaturelist:
        _creature = creature.strip().lower().replace("_", " ")
        if _creature in creatureMap:
            _creature = creatureMap[_creature]
        c.execute('SELECT id FROM Creatures WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (_creature, _creature,_creature))
        results = c.fetchall()
        if len(results) > 0:
            creatureid = results[0][0]
            c.execute('SELECT huntingplaceid FROM HuntingPlaceCreatures WHERE huntingplaceid=? AND creatureid=?', (huntingplaceid,creatureid))
            if len(c.fetchall()) == 0:
                c.execute('INSERT INTO HuntingPlaceCreatures(huntingplaceid,creatureid) VALUES (?,?)', (huntingplaceid,creatureid))
        else:
            pass#print("Unrecognized Creature", creature)


# fix some typos/plurals/incorrect links in quest reward items
itemMap = {'jalape%c3%92o pepper': 'jalapeño pepper', 'jalapeno pepper':'jalapeño pepper', 'cookies': 'cookie', 'small rubies': 'small ruby', 'ultimate healing potion':'ultimate health potion','lethal lissy\'s shirt':'the lethal lissy\'s shirt', 
  'soft boots': 'pair of soft boots', 'brown bag':'bag','gp': 'gold coin','sudden death':'sudden death rune', 'power bolts':'power bolt', 'apple':'red apple', 
  'platinum coins':'platinum coin', 'gold': 'gold coin', 'elephant tusk': 'tusk', 'present box': 'present', 'rust remover':'flask of rust remover'}

c.execute('DROP TABLE CreatureDrops')
c.execute('CREATE TABLE CreatureDrops(creatureid INTEGER, itemid INTEGER, percentage FLOAT, min INTEGER, max INTEGER)')
for creatureid,drops in iter(creaturedrops.items()):
    for itemname,dropinfo in iter(drops.items()):
        _item = itemname.strip().lower()
        if _item in itemMap:
            _item = itemMap[_item]
        c.execute('SELECT id FROM Items WHERE LOWER(name)=? OR LOWER(title)=?', (_item,_item))
        results = c.fetchall()
        if len(results) > 0:
            itemid = results[0][0]
            c.execute('INSERT INTO CreatureDrops(creatureid, itemid, percentage, min, max) VALUES (?,?,?,?,?)', (creatureid, itemid, dropinfo[0], dropinfo[1], dropinfo[2]))
        else:
            pass#print("Unrecognized Item", itemname)

c.execute('DROP TABLE QuestOutfits')
c.execute('DROP TABLE QuestRewards')
c.execute('CREATE TABLE QuestRewards(questid INT, itemid INT)')
c.execute('CREATE TABLE QuestOutfits(questid INT, outfitid INT)')
for questid,items in iter(rewardItems.items()):
    for itemname in items:
        _item = itemname.strip().lower()
        if _item in itemMap:
            _item = itemMap[_item]
        if 'image:' in _item: 
            match = re.search('image:outfit(?:_| )([^.]+)(?:_| )male[^.]+\.gif', _item)
            if match != None:
                _item = match.groups()[0].replace('_', ' ').strip().lower()
        if 'file:' in _item:
            match = re.search('file:outfit(?:_| )([^.]+)(?:_| )male[^.]+\.gif', _item)
            if match != None:
                _item = match.groups()[0].replace('_', ' ').strip().lower()

        if '|' in _item:
            _item = _item.split('|')[0]
        c.execute('SELECT id FROM Items WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (_item,_item,_item))
        results = c.fetchall()
        if len(results) > 0:
            itemid = results[0][0]
            c.execute('INSERT INTO QuestRewards(questid, itemid) VALUES (?,?)', (questid, itemid))
        else: 
            c.execute('SELECT id FROM Outfits WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (_item,_item,_item))
            results = c.fetchall()
            if len(results) > 0:
                outfitid = results[0][0]
                c.execute('SELECT outfitid FROM QuestOutfits WHERE questid=? AND outfitid=?', (questid, outfitid))
                results = c.fetchall()
                if len(results) == 0:
                    c.execute('INSERT INTO QuestOutfits(questid, outfitid) VALUES (?,?)', (questid, outfitid))
            else:
                print("Unrecognized item or outfit", itemname)


# some quests contain 'plural' definitions of creatures, this map translates those plurals to a set of creatures
pluralMap = {
'dwarves|kind of dwarf': ['dwarf', 'dwarf soldier', 'dwarf guard', 'dwarf geomancer'], 
            'dwarves': ['dwarf', 'dwarf soldier', 'dwarf guard', 'dwarf geomancer'],
            'orcs': ['orc', 'orc spearman', 'orc warrior', 'orc shaman', 'orc berserker'],
            'cyclops|cyclopes': ['cyclops', 'cyclops smith', 'cyclops drone'],
            'cyclopes': ['cyclops', 'cyclops smith', 'cyclops drone'],
            'apes': ['kongra', 'sibang', 'merlkin'],
            'minotaurs': ['minotaur', 'minotaur archer', 'minotaur guard', 'minotaur mage'],
            'lizards': ['lizard sentinel', 'lizard snakecharmer', 'lizard templar'],
            'dworcs': ['dworc fleshhunter', 'dworc venomsniper', 'dworc voodoomaster'],
            'quara': ['quara constrictor', 'quara hydromancer', 'quara mantassin', 'quara pincher', 'quara predator'],
            'quaras': ['quara constrictor', 'quara hydromancer', 'quara mantassin', 'quara pincher', 'quara predator'],
            'quara scout': ['quara constrictor scout', 'quara hydromancer scout', 'quara mantassin scout', 'quara pincher scout', 'quara predator scout'],
            'pirates': ['pirate buccaneer', 'pirate corsair', 'pirate cutthroat', 'pirate marauder'],
            'slimes': ['slime'],
            'dragons': ['dragon', 'dragon hatchling'],
            'dragon lords': ['dragon lord', 'dragon lord hatchling'],
            'cultists': ['acolyte of the cult', 'adept of the cult', 'enlightened of the cult', 'novice of the cult'],
            'voodoo cultists|cultists': ['acolyte of the cult', 'adept of the cult', 'enlightened of the cult', 'novice of the cult'],
            #'demons|demonic creatures': [],
            #'undead': [],
            'rotworms': ['rotworm', 'carrion worm'],
            'outlaws': ['bandit', 'wild warrior', 'assassin', 'hunter', 'smuggler', 'poacher'],
            'barbarians': ['barbarian bloodwalker', 'barbarian brutetamer', 'barbarian headsplitter', 'barbarian skullhunter', 'ice witch'],
            'barbarian': ['barbarian bloodwalker', 'barbarian brutetamer', 'barbarian headsplitter', 'barbarian skullhunter', 'ice witch'],
            'goblins': ['goblin', 'goblin assassin', 'goblin scavenger'],
            'trolls': ['troll', 'troll champion'],
            'high class lizards': ['lizard legionnaire', 'lizard high guard', 'lizard dragon priest', 'lizard chosen'],
            'lizards|high class lizards': ['lizard legionnaire', 'lizard high guard', 'lizard dragon priest', 'lizard chosen'],
            'enslaved dwarf|enslaved dwarves': ['lost thrower', 'lost husher', 'lost basher'],
            'elves': ['elf', 'elf scout', 'elf arcanist'],
            'chakoyas': ['chakoya toolshaper', 'chakoya tribewarden', 'chakoya windcaller'],
            'deeplings': ['deepling warrior', 'deepling spellsinger', 'deepling guard', 'deepling elite'],
            'corym': ['corym charlatan', 'corym skirmisher', 'corym vanguard']
            }

c.execute('DROP TABLE QuestDangers')
c.execute('CREATE TABLE QuestDangers(questid INT, creatureid INT)')
for questid,creatures in iter(questDangers.items()):
    for creature in creatures:
        _creature = creature.strip().lower().replace("_", " ")

        c.execute('SELECT id FROM HuntingPlaces WHERE LOWER(name)=?', (_creature,))
        results = c.fetchall()
        if len(results) > 0:
            huntingplaceid = results[0][0]
            c.execute('SELECT creatureid FROM HuntingPlaceCreatures WHERE huntingplaceid=?', (huntingplaceid,))
            results = c.fetchall()
            for creatureid in results:
                c.execute('INSERT INTO QuestDangers(questid,creatureid) VALUES (?,?)', (questid,creatureid[0]))
            continue

        creatureList = None
        if _creature in pluralMap: 
            creatureList = pluralMap[_creature]

        if creatureList != None:
            for _cr in creatureList:
                c.execute('SELECT id FROM Creatures WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (_cr,_cr,_cr))
                results = c.fetchall()
                if len(results) > 0:
                    creatureid = results[0][0]
                    c.execute('INSERT INTO QuestDangers(questid,creatureid) VALUES (?,?)', (questid,creatureid))
                else:
                    c.execute('SELECT title FROM Quests WHERE id=?', (questid,))
                    print("(%s) Unrecognized Creature" % c.fetchall()[0][0], _cr)
        else:
            if '|' in _creature: _creature = _creature.split('|')[0].strip()
            if _creature in creatureMap:
                _creature = creatureMap[_creature]
            c.execute('SELECT id FROM Creatures WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (_creature,_creature,_creature))
            results = c.fetchall()
            if len(results) > 0:
                creatureid = results[0][0]
                c.execute('INSERT INTO QuestDangers(questid,creatureid) VALUES (?,?)', (questid,creatureid))
            else:
                c.execute('SELECT title FROM Quests WHERE id=?', (questid,))
                print("(%s) Unrecognized Creature" % c.fetchall()[0][0], creature, '-', _creature)


for mountid,crlist in iter(mountStuff.items()):
    tameitemid = None
    tamecreatureid = None
    for cr in crlist:
        _cr = cr.strip().lower()
        c.execute('SELECT id FROM Creatures WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (_cr,_cr,_cr))
        results = c.fetchall()
        if len(results) > 0:
            tamecreatureid = results[0][0]
        else:
            c.execute('SELECT id FROM Items WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (_cr,_cr,_cr))
            results = c.fetchall()
            if len(results) > 0:
                tameitemid = results[0][0]
            elif cr.strip().lower() == "tibia store": 
                c.execute('UPDATE Mounts SET tibiastore=1 WHERE id=?', (mountid,))
            else:
                print("Unrecognized mount thingy", cr)
    c.execute('UPDATE Mounts SET tameitemid=? WHERE id=?', (tameitemid, mountid))
    c.execute('UPDATE Mounts SET tamecreatureid=? WHERE id=?', (tamecreatureid, mountid))


# remove some hardcoded items that are known to be incorrect on the wiki
removeFromDrops = {'demon': ['small stone', 'leather armor', 'bone', 'mouldy cheese']} # demons drop some goblin loot according to the wiki because of the Demon (Goblin) creature
for crname,items in iter(removeFromDrops.items()):
    c.execute('SELECT id FROM Creatures WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (crname,crname,crname))
    creatureid = c.fetchall()[0][0]
    for itemname in items:
    	c.execute('SELECT id FROM Items WHERE LOWER(name)=? OR LOWER(title)=? ORDER BY LOWER(title)=? DESC', (itemname,itemname,itemname))
    	itemid = c.fetchall()[0][0]
    	c.execute('DELETE FROM CreatureDrops WHERE creatureid=? AND itemid=?', (creatureid, itemid))


conn.commit()

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

def parseAttributes(content):
    attributes = dict()
    depth = 0
    parseValue = False
    attribute = ""
    value = ""
    for i in range(len(content)):
        if content[i] == '{' or content[i] == '[':
            depth += 1
            if depth >= 3:
                if parseValue:
                    value = value + content[i]
                else: 
                    attribute = attribute + content[i]
        elif content[i] == '}' or content[i] == ']':
            if depth >= 3:
                if parseValue:
                    value = value + content[i]
                else: 
                    attribute = attribute + content[i]
            if depth == 2: 
                attributes[attribute.strip()] = value.strip()
                parseValue = False
                attribute = ""
                value = ""
            depth -= 1
        elif content[i] == '=' and depth == 2: 
            parseValue = True
        elif content[i] == '|' and depth == 2:
            attributes[attribute.strip()] = value.strip()
            parseValue = False
            attribute = ""
            value = ""
        elif parseValue:
            value = value + content[i]
        else: 
            attribute = attribute + content[i]
    return attributes

def parseSpells(content):
    spells = list()
    depth = 0
    spell = ""
    split = 0
    for i in range(len(content)):
        if content[i] == '{':
            depth += 1
            split = 0
            spell = ""
        elif content[i] == '}':
            depth -= 1
            if split == 2 and spell.strip() not in spells: 
                spells.append(spell.strip())
            split = 0
            spell = ""
        elif content[i] == '|':
            if split == 2 and spell.strip() not in spells: 
                spells.append(spell.strip())
            elif spell.strip().lower() == 'teaches' or split > 0: 
                split += 1
            spell = ""
        else: spell = spell + content[i]
    return spells


import re
countRegex = re.compile('[ \t]*[0-9]+[ \t]*[-][ \t]*[0-9]+[ \t]*')

def parseLoot(content):
    loot = list()
    depth = 0
    item = ""
    split = 0
    for i in range(len(content)):
        if content[i] == '{':
            depth += 1
            split = 0
            item = ""
        elif content[i] == '}':
            depth -= 1
            if split == 1: 
                loot.append(item.strip())
            split = 0
            item = ""
        elif content[i] == '|':
            if split == 1: 
                if countRegex.match(item) == None:
                    loot.append(item.strip())
                    split = -100
            elif item.strip().lower() == 'loot item':
                split += 1
            item = ""
        else: item = item + content[i]
    return loot

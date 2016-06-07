
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

from format import formatTitle
import re
import collections


def flatten(x):
    return [item for sublist in x for item in sublist]

def sanitizeText(text, references):
    # first take care of [[Fire Rune||Great Fireball]] => Great Fireball
    references.append(re.findall(r'\[\[[^]|]+\|([^]]+)\]\]', text))
    text = re.sub(r'\[\[[^]|]+\|([^]]+)\]\]', '\g<1>', text)
    # then take care of [[Fire Rune]] => Fire Rune
    references.append(re.findall(r'\[\[([^]]+)\]\]', text))
    text = re.sub(r'\[\[([^]]+)\]\]', '\g<1>', text)
    # sometimes there are links in single brackets [http:www.link.com] => remove htem
    text = re.sub(r'\[[^]]+\]', '', text)
    # replace double spaces with single spaces
    text = text.replace('  ', ' ')
    return(text)


def parseAchievement(title, attributes, c, imageCandidates):
    name = title
    if 'actualname' in attributes and len(attributes['actualname']) > 0:
        name = formatTitle(attributes['actualname'])
    elif 'name' in attributes and len(attributes['name']) > 0:
        name = formatTitle(attributes['name'])
    description = None
    if 'description' in attributes:
        description = attributes['description']
    spoiler = None
    if 'spoiler' in attributes:
        spoiler = attributes['spoiler']
    grade = None
    if 'grade' in attributes:
        try: grade = int(attributes['grade'])
        except: pass
    points = None
    if 'points' in attributes:
        try: points = int(attributes['points'])
        except: pass
    references = []
    if description != None:
        description = sanitizeText(description, references)
    if spoiler != None:
        spoiler = sanitizeText(spoiler, references)
    references = flatten(references)
    c.execute('INSERT INTO Achievements (name, grade, points, description, spoiler, image, imagetype) VALUES (?,?,?,?,?,?,?)',
        (name, grade, points, description, spoiler, None, None))
    achievementid = c.lastrowid
    imageCandidates[achievementid] = references
    return(True)

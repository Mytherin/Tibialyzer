
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
import sqlite3
import urllib.request
import re

database_file = 'database.db'
#database_file = 'E:\\Github Projects\\Tibialyzer\\Tibialyzer\\Database\\database.db'

conn = sqlite3.connect(database_file)
c = conn.cursor()

c.execute('DROP TABLE IF EXISTS Events')
c.execute('DROP TABLE IF EXISTS EventMessages')
c.execute('DROP TABLE IF EXISTS EventCreatures')
c.execute('CREATE TABLE Events(id INTEGER PRIMARY KEY AUTOINCREMENT, title STRING, location STRING, creatureid INTEGER)')
c.execute('CREATE TABLE EventMessages(eventid STRING, message STRING)')
c.execute('CREATE TABLE EventCreatures(eventid INTEGER, creatureid INTEGER)')

headerRegex = re.compile('<h3><span class="mw-headline" id="([^"]+)">([^<]+)')
imageRegex = re.compile('<a href="(/wiki/[^"]+)"')
raidRegex = re.compile('<b>[0-9]+:[0-9]+</b>:[ \t]*<i>([^<]+)</i>')


raidurls = ["http://tibia.wikia.com/wiki/Ab'Dendriel_Raids", "http://tibia.wikia.com/wiki/Ankrahmun_Raids", "http://tibia.wikia.com/wiki/Carlin_Raids", 
"http://tibia.wikia.com/wiki/Darashia_Raids", "http://tibia.wikia.com/wiki/Edron_Raids", "http://tibia.wikia.com/wiki/Gnomebase_Raids", 
"http://tibia.wikia.com/wiki/Kazordoon_Raids", "http://tibia.wikia.com/wiki/Liberty_Bay_Raids", "http://tibia.wikia.com/wiki/Port_Hope_Raids", 
"http://tibia.wikia.com/wiki/Svargrond_Raids", "http://tibia.wikia.com/wiki/Thais_Raids", "http://tibia.wikia.com/wiki/Venore_Raids", 
"http://tibia.wikia.com/wiki/Yalahar_Raids", "http://tibia.wikia.com/wiki/Rookgaard_Raids", "http://tibia.wikia.com/wiki/Plains_of_Havoc_Raids", 
"http://tibia.wikia.com/wiki/Zao_Raids"]


additionalCreatures = {'Djinn Raids near Ankrahmun': '/wiki/Efreet'}

for url in raidurls:
    response = urllib.request.urlopen(url)
    a = response.read().decode('utf-8')
    location = url.split('/')[-1].split('_Raids')[0].replace('_', ' ')
    print("----")
    print(location)
    print("----")
    index = 0
    prevIndex = None
    title = None
    while True:
        match = headerRegex.search(a[index:])
        if match == None: break
        index += match.end()
        if prevIndex != None:
            print(title)
            messages = raidRegex.findall(a[prevIndex:index])
            if len(messages) > 0:
                images = imageRegex.findall(a[prevIndex:index])
                if title in additionalCreatures:
                    images.append(additionalCreatures[title])
                creatures = []
                for image in images:
                    creaturename = image.split('/')[-1].lower().strip().replace('%20', ' ').replace('_', ' ')
                    c.execute('SELECT id, experience FROM Creatures WHERE LOWER(name)=? OR LOWER(title)=?', (creaturename, creaturename))
                    results = c.fetchall()
                    if len(results) > 0:
                        creatures.append((results[0][0], results[0][1] if results[0][1] != None else 0))
                creatures = sorted(creatures, key=lambda x: -x[1])
                creatureid = creatures[0][0]
                c.execute('INSERT INTO Events(title, location, creatureid) VALUES (?,?,?)', (title, location, creatureid))
                eventid = c.lastrowid
                for cr in creatures:
                    crid = cr[0]
                    c.execute('INSERT INTO EventCreatures(eventid, creatureid) VALUES (?,?)', (eventid, creatureid))
                for message in messages:
                    c.execute('INSERT INTO EventMessages(eventid, message) VALUES (?,?)', (eventid, message))
        prevIndex = index
        title = match.groups()[0].strip().replace('.27', "'").replace('_', ' ')

conn.commit()

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
import urllib.request
import sqlite3

database_file = 'database.db'

conn = sqlite3.connect(database_file)
c = conn.cursor()

c.execute('DROP TABLE IF EXISTS WorldMap')
c.execute('CREATE TABLE IF NOT EXISTS WorldMap(z INTEGER, image BLOB)')
map_images = [
'http://images4.wikia.nocookie.net/tibia/en/images/f/ff/Minimap_Floor_15.png', 'http://images3.wikia.nocookie.net/tibia/en/images/9/90/Minimap_Floor_14.png', 
'http://images3.wikia.nocookie.net/tibia/en/images/c/cf/Minimap_Floor_13.png', 'http://images4.wikia.nocookie.net/tibia/en/images/4/44/Minimap_Floor_12.png', 
'http://images4.wikia.nocookie.net/tibia/en/images/e/e4/Minimap_Floor_11.png', 'http://images3.wikia.nocookie.net/tibia/en/images/a/a3/Minimap_Floor_10.png', 
'http://images2.wikia.nocookie.net/tibia/en/images/e/e6/Minimap_Floor_9.png', 'http://images4.wikia.nocookie.net/tibia/en/images/f/fc/Minimap_Floor_8.png', 
'http://images1.wikia.nocookie.net/tibia/en/images/6/64/Minimap_Floor_7.png', 'http://images2.wikia.nocookie.net/tibia/en/images/b/be/Minimap_Floor_6.png', 
'http://images4.wikia.nocookie.net/tibia/en/images/6/61/Minimap_Floor_5.png', 'http://images2.wikia.nocookie.net/tibia/en/images/f/f7/Minimap_Floor_4.png', 
'http://images3.wikia.nocookie.net/tibia/en/images/8/87/Minimap_Floor_3.png', 'http://images3.wikia.nocookie.net/tibia/en/images/2/2a/Minimap_Floor_2.png', 
'http://images2.wikia.nocookie.net/tibia/en/images/d/df/Minimap_Floor_1.png', 'http://images4.wikia.nocookie.net/tibia/en/images/1/1c/Minimap_Floor_0.png']
map_images.reverse()

for depth in range(0, len(map_images)):
    image = None
    while True:
        try: 
            r = urllib.request.urlopen(map_images[depth])
            image = sqlite3.Binary(r.read())
            break
        except: 
            print("Failed to acquire image %s, trying again..." % map_images[depth])
            time.sleep(5)
    c.execute('INSERT INTO WorldMap (z, image) VALUES (?,?)', (depth, image))
conn.commit()

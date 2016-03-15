
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
    'https://tibiamaps.github.io/tibia-map-data/floor-15-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-14-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-13-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-12-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-11-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-10-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-09-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-08-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-07-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-06-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-05-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-04-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-03-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-02-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-01-map.png',
    'https://tibiamaps.github.io/tibia-map-data/floor-00-map.png'
]
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


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
import sqlite3

database_file = 'database.db'
#database_file = 'E:\\Github Projects\\Tibialyzer\\Tibialyzer\\Database\\database.db'
cities_xmlfile = "Extra Information/cities.xml"

from coordinates import convert_x, convert_y

conn = sqlite3.connect(database_file)
c = conn.cursor()

# parcel id
c.execute('SELECT id FROM Items WHERE LOWER(name)="parcel"')
parcelid = c.fetchall()[0][0]

# cities
c.execute('DROP TABLE IF EXISTS Cities');
c.execute('DROP TABLE IF EXISTS CityUtilities');
c.execute('CREATE TABLE Cities(id INTEGER PRIMARY KEY AUTOINCREMENT, name STRING, x INTEGER, y INTEGER, z INTEGER)')
c.execute('CREATE TABLE CityUtilities(cityid INTEGER, name STRING, x INTEGER, y INTEGER, z INTEGER)')
root = xml.etree.ElementTree.parse(cities_xmlfile).getroot()
for child in root.getchildren():
    cityname = child.find('Name').text
    location = child.find('Location').text.split(',')
    c.execute('INSERT INTO Cities(name, x, y, z) VALUES (?,?,?,?)', (cityname, convert_x(location[0]), convert_y(location[1]), int(location[2])))
    cityid = c.lastrowid
    utilities = child.find('Utilities')
    for utility in utilities.getchildren():
        utilityname = utility.find('Name').text
        uloc = utility.find('Location').text.split(',')
        c.execute('INSERT INTO CityUtilities(cityid, name, x, y, z) VALUES (?,?,?,?,?)', (cityid, utilityname, convert_x(uloc[0]), convert_y(uloc[1]), int(uloc[2])))
    c.execute('SELECT x, y, z FROM NPCs INNER JOIN BuyItems ON BuyItems.vendorid=NPCs.id AND BuyItems.itemid=? AND LOWER(city)=?', (parcelid, cityname.lower()))
    results = c.fetchall()
    if len(results) > 0:
        c.execute('INSERT INTO CityUtilities(cityid, name, x, y, z) VALUES (?,?,?,?,?)', (cityid, "Post Office", results[0][0] * 2048, results[0][1] * 2304, results[0][2]))

conn.commit()

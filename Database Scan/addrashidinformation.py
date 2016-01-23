
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
from coordinates import convert_x, convert_y

database_file = 'database.db'

conn = sqlite3.connect(database_file)
c = conn.cursor()

c.execute('DROP TABLE IF EXISTS RashidPositions')
c.execute('CREATE TABLE RashidPositions(day STRING, city STRING, x INTEGER, y INTEGER, z INTEGER)')

x,y,z = convert_x('125.210'),convert_y('121.182'),7
c.execute('INSERT INTO RashidPositions(day, city, x, y, z) VALUES (?,?,?,?,?)', 
    ['Monday', 'Svargrond', x, y, z])

x,y,z = convert_x('126.47'),convert_y('128.67'),7
c.execute('INSERT INTO RashidPositions(day, city, x, y, z) VALUES (?,?,?,?,?)', 
    ['Tuesday', 'Liberty Bay', x, y, z])

x,y,z = convert_x('127.65'),convert_y('127.241'),7
c.execute('INSERT INTO RashidPositions(day, city, x, y, z) VALUES (?,?,?,?,?)', 
    ['Wednesday', 'Port Hope', x, y, z])

x,y,z = convert_x('129.47'),convert_y('128.117'),6
c.execute('INSERT INTO RashidPositions(day, city, x, y, z) VALUES (?,?,?,?,?)', 
    ['Thursday', 'Ankrahmun', x, y, z])

x,y,z = convert_x('129.215'),convert_y('126.228'),7
c.execute('INSERT INTO RashidPositions(day, city, x, y, z) VALUES (?,?,?,?,?)', 
    ['Friday', 'Darashia', x, y, z])

x,y,z = convert_x('129.148'),convert_y('124.57'),6
c.execute('INSERT INTO RashidPositions(day, city, x, y, z) VALUES (?,?,?,?,?)', 
    ['Saturday', 'Edron', x, y, z])

x,y,z = convert_x('126.71'),convert_y('124.39'),6
c.execute('INSERT INTO RashidPositions(day, city, x, y, z) VALUES (?,?,?,?,?)', 
    ['Sunday', 'Carlin', x, y, z])

conn.commit()

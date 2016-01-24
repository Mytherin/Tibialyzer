
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
cities_xmlfile = "Extra Information/help.xml"

from coordinates import convert_x, convert_y

conn = sqlite3.connect(database_file)
c = conn.cursor()


# cities
c.execute('DROP TABLE IF EXISTS CommandHelp');
c.execute('CREATE TABLE CommandHelp(command STRING, description STRING)')
root = xml.etree.ElementTree.parse(cities_xmlfile).getroot()
for child in root.getchildren():
    c.execute('INSERT INTO CommandHelp(command,description) VALUES (?,?)', (child.find('Com').text, child.find('Desc').text))

conn.commit()






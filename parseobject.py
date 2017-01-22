
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
from urlhelpers import getImage

imageRegex = re.compile('<a href="([^"]*)"[ \t\n]*class="image image-thumbnail"')
imageRegex2 = re.compile('src="([^"]*vignette[^"]*)"')

def parseObject(title, attributes, c, getURL):
    name = title
    if 'actualname' in attributes:
        name = attributes['actualname']
    elif 'name' in attributes:
        name = attributes['name']
    c.execute('SELECT title FROM WorldObjects WHERE LOWER(title)=?', (title.lower(),))
    results = c.fetchall()
    if len(results) > 0:
        print("Error on object {}: Already exists".format(title))
        return False
    url = "http://tibia.wikia.com/wiki/%s" % (title.replace(' ', '_'))
    image = getImage(url, getURL, imageRegex)
    if image == None or image == False:
        url = "http://tibia.wikia.com/wiki/File:%s.gif" % (title.replace(' ', '_'))
        image = getImage(url, getURL, imageRegex2)
        if image == None or image == False:
            print("Error on object {}: Failed to get image".format(title))
            return False
    c.execute('INSERT INTO WorldObjects(title, name, image) VALUES (?,?,?)', (title, name, image))
    return True


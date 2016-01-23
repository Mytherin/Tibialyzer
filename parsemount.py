
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

import re
from imageoperations import crop_image, gif_is_animated, convert_to_png
from urlhelpers import getImage

imageRegex = re.compile('<a href="([^"]*)"[ \t\n]*class="image image-thumbnail"')
imageRegex2 = re.compile('src="([^"]*vignette[^"]*)"')
bracketRegex = re.compile('\\[\\[([^]]+)\\]\\]')

def parseMount(title, attributes, c, mountStuff, getURL):
    name = title
    if 'name' in attributes:
        name = attributes['name']
    speed = 10
    if 'speed' in attributes:
        speed = int(attributes['speed'])
    url = "http://tibia.wikia.com/wiki/%s" % (title.replace(' ', '_'))
    image = getImage(url, getURL, imageRegex, crop_image)
    if image == None:
        url = "http://tibia.wikia.com/wiki/File:%s.gif" % (title.replace(' ', '_'))
        image = getImage(url, getURL, imageRegex2, crop_image)
        if image == None:
            print('failed to get image for item', title)
            return False
    c.execute('INSERT INTO Mounts (title, name, tameitemid, tamecreatureid, speed, tibiastore, image) VALUES (?,?,?,?,?,?,?)', 
        (title, name, None, None, speed, False, image))
    mountid = c.lastrowid
    if 'taming_method' in attributes:
        mountStuff[mountid] = list()
        index = 0
        while True:
            match = bracketRegex.search(attributes['taming_method'][index:])
            if match == None:
                break
            mountStuff[mountid].append(match.groups()[0])
            index += match.end()
    return True


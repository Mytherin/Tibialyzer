
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
        print('object already exists', title)
        return False
    url = "http://tibia.wikia.com/wiki/%s" % (title.replace(' ', '_'))
    image = getImage(url, getURL, imageRegex)
    if image == None or image == False:
        url = "http://tibia.wikia.com/wiki/File:%s.gif" % (title.replace(' ', '_'))
        image = getImage(url, getURL, imageRegex2)
        if image == None or image == False:
            print('failed to get image for object', title)
            return False
    c.execute('INSERT INTO WorldObjects(title, name, image) VALUES (?,?,?)', (title, name, image))
    return True


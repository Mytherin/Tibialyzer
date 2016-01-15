
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


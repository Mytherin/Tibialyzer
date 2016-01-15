
import re
import sqlite3

from imageoperations import crop_image, gif_is_animated, convert_to_png
from urlhelpers import getImage
from parseattribs import parseLoot

numberRegex = re.compile('([0-9]+[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*)')
imageRegex = re.compile('<a href="([^"]*)"[ \t\n]*class="image image-thumbnail"')
imageRegex2 = re.compile('src="([^"]*vignette[^"]*)"')


genders = ["Female", "Male"]
addons = ["", "_Addon_1", "_Addon_2", "_Addon_3"]


def parseOutfit(title, attributes, c, getURL):
    name = title
    if 'name' in attributes:
        name = attributes['name']
    premium = False
    if 'premium' in attributes:
        premium = attributes['premium'].strip().lower() == 'yes'
    tibiastore = False
    if 'notes' in attributes:
        if 'You can buy the outfit with no addons, one addon or the full outfit from the official Tibia website.' in attributes['notes']:
            tibiastore = True
            print(title)
    c.execute('INSERT INTO Outfits (title, name, premium, tibiastore) VALUES (?,?,?,?)', (title, name, premium,tibiastore))
    outfitid = c.lastrowid
    url = "http://tibia.wikia.com/wiki/%s" % (title.replace(' ', '_'))
    mainHTML = getURL(url, True)
    if mainHTML == None: return False
    imageCount = 0
    for gender in genders:
        index = -1
        for addon in addons:
            index += 1
            match = re.search("href=\"([^\"]+/Outfit_%s_%s%s.gif/[^\"]+)\"" % (name.replace(' ','_'), gender, addon), mainHTML)
            if match == None: continue
            imageURL = match.groups()[0].replace("&amp;", "&")
            image = sqlite3.Binary(getURL(imageURL, False))
            c.execute('INSERT INTO OutfitImages (outfitid, male, addon, image) VALUES (?,?,?,?)', (outfitid, gender == "Male", index, image))
            imageCount += 1
    if imageCount == 0:
        c.execute('DELETE FROM Outfits WHERE id=?', (outfitid,))

    return True




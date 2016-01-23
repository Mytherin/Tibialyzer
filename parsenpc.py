
import math
import re
import urllib.request
import sqlite3
import time

from coordinates import convert_x, convert_y
from imageoperations import crop_image, gif_is_animated, convert_to_png
from parseattribs import parseSpells
from urlhelpers import getImage

imageRegex = re.compile('<a href="([^"]*)"[ \t\n]*class="image image-thumbnail"')
imageRegex2 = re.compile('src="([^"]*vignette[^"]*)"')

def parseNPC(title, attributes, c, spells, getURL):
    name = title
    if 'name' in attributes:
        name = attributes['name']
    posx = None
    if 'posx' in attributes:
        try: posx = convert_x(attributes['posx'])
        except: pass
    posy = None
    if 'posy' in attributes:
        try: posy = convert_y(attributes['posy'])
        except: pass
    posz = None
    if 'posz' in attributes:
        try: posz = int(attributes['posz'])
        except: pass
    city = None
    if 'city' in attributes:
        city = attributes['city'].lower()
    job = None
    if 'job2' in attributes:
        job = attributes['job2']
    if 'job' in attributes and (job == None or 'guild leader' in attributes['job'].lower()):
        job = attributes['job']
    url = "http://tibia.wikia.com/wiki/%s" % (title.replace(' ', '_'))
    image = getImage(url, getURL, imageRegex)
    if image == None or image == False:
        url = "http://tibia.wikia.com/wiki/File:%s.gif" % (title.replace(' ', '_'))
        image = getImage(url, getURL, imageRegex2)
        if image == None or image == False:
            print('failed to get image for npc', title)
            return False
    c.execute('INSERT INTO NPCs (title,name, city, job, x, y, z, image) VALUES (?,?,?,?,?,?,?,?)', (title,name, city, job, posx, posy, posz, image))
    npcid = c.lastrowid
    if 'sells' in attributes and 'teaches' in attributes['sells'].lower():
        spells[npcid] = parseSpells(attributes['sells'])
    if 'notes' in attributes:
        match = re.search('{{Transport([^}]+)', attributes['notes'])
        if match != None:
            splits = match.groups()[0].split('|')
            for spl in splits:
                spl = spl.strip()
                if ',' in spl:
                    splits2 = spl.split(',')
                    dest = splits2[0].strip()
                    notes = ""
                    if ';' in splits2[1]:
                        splits3 = splits2[1].split(';')
                        splits2[1] = splits3[0]
                        notes = splits3[1].replace('[', '').replace(']', '')
                    try: cost = int(splits2[1].strip())
                    except: continue
                    c.execute('INSERT INTO NPCDestinations(npcid, destination, cost, notes) VALUES (?,?,?,?)', (npcid, dest, cost, notes))
                        


    return True

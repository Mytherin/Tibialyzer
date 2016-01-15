
import math
import re
import urllib.request
import sqlite3
import time
import pickle
import os

image_cache_file = 'imagecache'
savedimages = dict()
saveimageCounter = 0

if os.path.isfile(image_cache_file):
    print('Loading image cache...')
    f = open(image_cache_file, 'rb')
    savedimages = pickle.load(f)
    f.close()

def addImage(url, image):
    global savedimages
    global saveimageCounter
    savedimages[url] = image
    saveimageCounter += 1
    if saveimageCounter >= 50:
        saveimageCounter = 0
        print('Saving image cache...')
        f = open(image_cache_file, 'wb')
        pickle.dump(savedimages, f)
        f.close()
        print('Done')

def getImage(url, getURL, regex, operation=None):
    if url in savedimages:
        return sqlite3.Binary(savedimages[url])
    initialURL = url
    itemHTML = getURL(url, True)
    if itemHTML == None: return False
    image_index = 0
    match = regex.search(itemHTML)
    while (match != None and '.gif' not in match.groups()[0]):
        image_index = image_index + match.end()
        match = regex.search(itemHTML[image_index:])
    image = None
    if match != None:
        url = match.groups()[0].replace('&amp;', '&')
        if 'vignette' not in url: 
            print(url)
        imageBinary = getURL(url, False)
        if operation == None:
            image = sqlite3.Binary(imageBinary)
            addImage(initialURL, imageBinary)
        else: 
            opResult = operation(imageBinary)
            addImage(initialURL, opResult)
            image = sqlite3.Binary(opResult)
    if image == False: return None
    return image
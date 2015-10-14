import re
import webbrowser
import urllib
import sys

address = "http://tibia.wikia.com/api/v1/Search/List?query=%s&limit=1&minArticleQuality=10&batch=1&namespaces=0" % _parameter

response = urllib.urlopen(address)
a = response.read()
regex = re.compile("\"url\":\"([^\"]+)\"")
match = regex.search(a)
if match == None: 
	print "No matches found."
	exit()
wikiaddress = match.groups()[0]

wikiaddress = wikiaddress.replace('\\/', '/')

webbrowser.open(wikiaddress)
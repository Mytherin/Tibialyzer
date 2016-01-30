# Tibia Wikia XML Parser

This set of python scripts parses the Tibia Wikia XML file and generates a database from them. The database dump can be downloaded from http://tibia.wikia.com/wiki/Special:Statistics and is about 200MB~ large. Note that the database dump does not contain image files. Instead, image files are downloaded from the tibia.wikia.com site automatically. This script will download them while running. Downloading these files takes by far the longest time of this script, but the script will cache all downloads, so the script can be interrupted/resumed and will not redownload things it has already downloaded.

# Usage
The scripts are build for python3 and probably do not work with python2. For image processing, the imagemagick library is used and must be installed.

To create a full database usable by Tibialyzer, run each of these commands (starting with parseXML.py; the order of the other commands does not matter). If you simply want to update, say, quest instructions or hunt directions, you only need to run the addextrainformation.py script (which is much faster than building the database again from scratch).

Create the initial database with the following command. This creates mostly everything (creatures, items, hunts, quests, outfits, mounts, spells) and downloads all the images necessary.
```bash
	python parseXML.py
```
Additional information is then added through the following additional scripts.

#### Rashid Locations
This script adds the locations of Rashid (as these vary based on the days of the week). The values are hardcoded in the script.
```bash
	python addrashidinformation.py
```
#### World Map Images
This script downloads the world map files from the tibia.wikia site.
```bash
	python addmapinformation.py
```
#### City Information
This script adds information about cities, parsed from the "Extra Information/cities.xml" file.
```bash
	python addcityinformation.py
```
#### Event Information
This script adds information about events from the "Extra Information/events.xml" file.
```bash
	python addeventinformation.py
```
#### Task Information
This script adds information about Killing In The Name Of... tasks from the "Extra Information/killinginthenameof.xml" file.
```bash
	python addkillinginthenameof.py
```
#### Tibialyzer Help
This script adds information about Tibialyzer commands to be displayed in the Help section to the database.
```bash
	python addhelpinformation.py
```
#### Additional Information
This script adds creature skins, hunting directions, hunting requirements and quest instructions from the corresponding .xml files in the "Extra Information" directory.
```bash
	python addextrainformation.py
```

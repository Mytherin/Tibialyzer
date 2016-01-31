## Database Directory

This directory is the main storage location used by Tibialyzer. The directory contains a number of files.

#### Shipped Files
- `database.db`: This is the main database, it contains all information about Tibia. It contains all images of Tibia Objects as well. For more information about the structure of this database, see the `Database Structure` section,
- `nodes.db`: This contains a number of nodes used for pathfinding. The pathfinding is used for hunting and quest directions.
- `pluralMap.txt`: This contains the plural forms of certain items. Some items have irregular plural forms (such as small topaz: small topazes). This file contains a map of irregular plurals to their singular form so they can be correctly recognized in loot messages.

#### Generated Files
- `settings.txt`: This file contains the users' settings. 
- `loot.db`: This file contains the loot of the various hunts of the user.

## Database Structure
The main database `database.db` contains all information about the Tibia world. The base tables hold information about the various objects, as well as a unique identifier that is used to link back to these base objects. The base tables are `Creatures`, `Items`, `NPCs`, `HuntingPlaces`, `Quests`, `Mounts`, `Outfits`, `Tasks`.

###### Creatures
The `Creatures` table contains the base information about Creatures, as well as a unique identifier for each creature. This identifier is then used to link the specific creature to other Tibia objects.

- `CreatureDrops`: Contains information about which items the creature drops.
- `Skins`: Contains information about i the creature is skinnable, and with what item it is skinnable.
- `EventCreatures`: Contains information about which creatures you might encounter in specific events. 
- `HuntingPlaceCreatures`: Contains information about which creatures you might encounter in specific hunting places.
- `QuestDangers`: Contains information about which creatures you might encounter during specific quests.
- `TaskCreatures`: Contains information about which creatures are necessary for which tasks. 

###### Items
The `Items` table contains the base information about items, as well as a unique identifier for each item. 

- `ItemProperties`: Contains additional properties for items that not all items have (such as Armor or Attack).
- `BuyItems`: Contains information about which NPCs will sell this item to the player.
- `SellItems`: Contains information about which NPCs will buy this item from the player.
- `QuestRewards`: Contains information about which quests reward which items.
- `QuestItemRequirements`: Contains information about which items are required for which quests.

###### NPCs
The `NPCs` table contains the base information about NPCs, as well as a unique identifier for each NPC. 

- `BuyItems`: Contains information about which NPCs will sell this item to the player.
- `SellItems`: Contains information about which NPCs will buy this item from the player.
- `QuestNPCs`: Contains information about which NPCs are involved in which quests.
- `NPCDestinations`: Contains information about locations where the NPC will transport you to.
- `SpellNPCs`: Contains information about spells that the NPC can teach you.

###### HuntingPlaces
The `HuntingPlaces` table contains the base information about hunting places, as well as a unique identifier for each hunting place. 

- `HuntDirections`: Contains a set of directions for how to get to a specific hunting place.
- `HuntRequirements`: Contains a set of requirements for hunting at each hunting place. 
- `HuntingPlaceCoordinates`: Contains a list of locations that mark where the hunting place is.
- `HuntingPlaceCreatures`: Contains information about which creatures you might encounter in specific hunting places.

###### Quests
The `Quests` table contains the base information about quests, as well as a unique identifier for each quest.

- `QuestOutfits`: Contains information about which quests reward which outfits.
- `QuestDangers`: Contains information about which creatures you might encounter during specific quests.
- `QuestRewards`: Contains information about which quests reward which items.
- `QuestItemRequirements`: Contains information about which items are required for which quests.
- `QuestAdditionalRequirements`: Contains a set of additional requirements for quests, that are not item requirements.
- `QuestInstructions`: Contains instructions for how to complete this quest.

###### Outfits
The `Outfits` table contains the base information about outfits, as well as a unique identifier for each outfit.

- `OutfitImages`: Contains images for various variations of each outfit (male/female, and with various addon combinations)
- `QuestOutfits`: Contains information about which quests reward which outfits.

###### Tasks
The `Tasks` table contains the base information about tasks (from the Killing In The Name Of... quest), as well as a unique identifier for each task.

- `TaskCreatures`: Contains information about creatures which you need to kill to complete the task.
- `TaskHunts`: Contains suggested hunting places for completing this task.
- `TaskGroups`: A set of groups to which each of the tasks belong (such as Level 50-79 Tasks).

## Loading Images from the Database
Below is a simple Python script that illustrates how you can load images from the database.

```python
import sqlite3
conn = sqlite3.connect('database.db')
c = conn.cursor()
# Load the Demon image from the database
c.execute('SELECT image FROM Creatures WHERE LOWER(name)="demon"')
results = c.fetchall()
# Save the image in the file 'demon.gif'
f = open('demon.gif', 'wb')
f.write(results[0][0])
f.close()
```


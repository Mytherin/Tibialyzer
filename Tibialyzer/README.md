This is the main code repository for Tibialyzer. It contains all code that the Tibialyzer program uses to run. Everything is written in C#.

#### Important Files
###### `MainFormReadMemory.cs` 

This file contains all functions that are used for reading the memory of the Tibia application. The function `ReadMemory()` performs the reading of the memory using the Win32 API. The memory is first scanned by the `FindTimestamps()` function, which searches for null-delimited strings that start with a timestamp (in the form `00:00`). The resulting set of strings is then passed to the `SearchChunk()` function, which parses the messages and checks which type of message they are (chat messages, loot messages, damage messages, etc.) and parses relevant information from the messages. This information is stored in a `ReadMemoryResults` structure.

The `ReadMemoryResults` structure is then passed to the `ParseLogResults()` function. This function does some more refined checking of the messages. It checks if the messages have already been handled, and performs some aggregation of the messages (such as computing experience per hour from the experience messages, and computing the total damage dealt by each player). It also increments the total time spent hunting of the current hunt.

###### `MainFormCommandProcessing.cs` 

This file contains all functions that are used for executing Tibialyzer commands. Note that Tibialyzer uses commands to do everything. If you click on the `Stats` button on the creature view, the command `stats@[creature]` is silently executed. The `ExecuteCommand()` function is a big switch statement that checks which command should be executed, and then performs the execution of that command.

This file also contains the `ScanMemory()` function. This is the big function that performs the routine scanning that Tibialyzer performs in the background. This function calls `ReadMemory() and `ParseLogResults()`, and then executes any commands submitted by the player. 

###### `MainFormStorage.cs`

This file contains the storage structures that Tibialyzer uses to store all Tibia objects, such as creatures and items. It provides access to a large number of functions that allow you to query the database. The simple functions that are provided for each Tibia object are `getObject(id)` and `getObject(name)`. In addition, there are various functions such as `searchObject(searchterm)` that allow more advanced querying of the database.

Note that Tibialyzer loads data from the database when it is needed. When it first starts up, only a small amount of data is loaded from the database. Then, when additional information from the database is needed it is loaded from the database. Once data has been loaded from the database it will be cached in one of the dictionaries (such as `_creatureIdMap`).

###### `MainForm.cs`

This file is the main entry point of the application (and is a bit of a mess). It contains everything that has to do with the initial loading of Tibialyzer, and contains all the event handling of various controls on the main Tibialyzer form. In addition, it performs all handling of Tibialyzer settings. 

###### `Structures.cs`

This file contains all the structures used to store various Tibia objects. In Tibialyzer, every object inherits from the abstract `TibiaObject` class. This class exposes a number of abstract methods that allow certain functions (such as displaying a list of TibiaObjects) to transparently work with all the different types of Tibia objects. 

###### `NotificationForm.cs`

This file contains the `NotificationForm` class. This is the base class that all rich notifications inherit from. 

###### `SimpleNotification.cs`

This file contains the `SimpleNotification` class. This is the base class that all simple notifications inherit from. 


###### `RichTextBoxAutoHotkey.cs`

This file contains the textbox that is used for syntax highlighting the AutoHotkey script.

#### Rich Notifications
Aside from these important files, the other files mainly contain the various types of rich notifications. Here's a quick overview of what each file contains.
- `AutoHotkeySuspendedMode.cs`: The `Suspended` dialog box that pops up when you suspend the integrated AutoHotkey support.
- `CityDisplayForm.cs`: `city@[cityname]`
- `CreatureDropsForm.cs`: `creature[creaturename]`
- `CreatureList.cs`: Generic list structure that is used for displaying multiple Tibia objects. For example, `creature@[searchterm]` or `category@[categoryname]`
- `CreatureStatsForm.cs`: `stats@[creaturename]`
- `DamageChart.cs`: `damage@`
- `HuntingPlaceForm.cs`: `hunt@[huntname]`
- `ItemViewForm.cs`: `item@[itemname]`
- `ListNotification.cs`: Used for displaying a simple list for `url@` and `recent@` commands.
- `LootDropForm.cs`: `loot@`
- `MountForm.cs`: `mount@[mountname]`
- `NPCForm.cs`: `npc@[npcname]`
- `OutfitForm.cs`: `outfit@[outfitname]`
- `QuestForm.cs`: `quest[questname]`
- `QuestGuideForm.cs`: Contains the guide used for both quest and hunt directions.
- `SpellForm.cs`: `spell@[spellname]`
- `TaskForm.cs`: `task@[taskname]`


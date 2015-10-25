# Tibialyzer
Tibialyzer is an extension made for the MMORPG Tibia. It automatically scans the server log and messages from the Tibia client by reading its memory, and gathers various statistics, such as loot found from creatures, damage dealt by party members and experience gained every hour. 

## Feature List
* Automatically gathers loot information, and allows you to save loot information to file.
* Automatically copies the advance messages to the clipboard when you gain a experience or skill level.
* Gathers damage records from the server log and can display a damage meter.
* Displays loot of the current hunt, as well as creature kills, in a fancy notification.
* Displays simple notification messages when a creature drops specific valuable items.
* Displays various statistics about Tibia, such as creature drop rates, creature stats, location of NPCs and what they buy/sell and hunting place locations in fancy notifications.
* All of this is possible without leaving the Tibia window, through the use of chat commands.

# Usage
When you start up Tibialyzer, it automatically starts scanning the Tibia client if one is running. Note that Tibialyzer relies on the server log for everything. You **must** have timestamps enabled, as Tibialyzer relies on timestamps to identify console messages. Enabling other console settings are optional, however, disabling them will make various parts of Tibialyzer not function properly. Enabling all options is recommended, as in the below screenshot.

![enabled console options](https://raw.githubusercontent.com/Mytherin/Tibialyzer/master/Images/console.png)

# Commands
In addition to scanning the server log, Tibialyzer also scans chat messages you send and receive. You can use this to send commands to Tibialyzer, similar to a terminal. Note that Tibialyzer does not know the name of your character, so before being able to send commands you will have to enter the names of your character in the Names list, as in the screenshot below. 

After adding your name to the list, Tibialyzer will recognize your commands. There are a lot of commands, they are all briefly shown and explained in the Command List tab of Tibialyzer. We will provide a more detailed list of the commands and their functions here.

Every command is structured in the following way: **command@parameter**. Some commands have optional parameters, while others require parameters to work. Most commands will show a notification window when executed, while more advanced commands will only make changes in the database. 

## Loot Management

*loot@*

# Installation
The Tibialyzer.zip file in the root of the repository contains all the necessary binaries. Download the zip file, extract it and run the Tibialyzer.exe file. That's it, Tibialyzer is now scanning (if your Tibia client is running). Note that if you want to pass commands to Tibialyzer, you must add your character name to the name list (separated by newlines).

## Requirements
Tibialyzer is written in a combination of C# and Python, linked together through IronPython. For visualization it uses WinForms, which is part of the .NET framework. IronPython is included in the binary release file, but the .NET Framework must be installed separately. Tibialyzer requires .NET Framework 4.0 or higher.

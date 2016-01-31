<img align="right" src="https://raw.githubusercontent.com/Mytherin/Tibialyzer/master/Images/tibiagroup.png" alt="Me and my Friends in Tibia, Left To Right: Mytherin, Amel Cyrom, Martincc">

# Tibialyzer
Tibialyzer is an extension made for the MMORPG Tibia. It scans the server log and messages from the Tibia client in real-time by reading its memory, and gathers various statistics, such as loot found from creatures and damage dealt by party members. 

See the [Wiki section](https://github.com/Mytherin/Tibialyzer/wiki) for an in-depth description of all features that Tibialyzer has.

For information on downloading and installing Tibialyzer, see the [Installation Instructions Section](https://github.com/Mytherin/Tibialyzer/wiki/Installation-Instructions). For help getting started, visit the [Quick Start Section](https://github.com/Mytherin/Tibialyzer/wiki/Quick-Start-Guide).

# Why did you make Tibialyzer?

It started a couple of months ago. I was hunting Apes in Banuta on a low-level sorcerer alt using avalanche runes. I wanted to power-level (gain a lot of experience fast). However, `Ape Fur` was worth a lot on my server (about `6000` gold each). I felt it was a waste to leave any ape fur behind in the corpses. My plan was to look at the server log and check if any ape fur dropped, and if any dropped I would pick it up. 

While doing this I thought, if these strings are in the server log, why can't I just make a program to read them instead of manually reading them? And thus the first `Tibialyzer` was born. This was a simple Python script that I hacked together in an hour. This script would simply scan for the words `Ape Fur` in Tibia's memory and would call `notify-send` if it found any. This was still on Linux.

It worked very well, and at that point I thought, why not keep track of more things this way? I can keep track of all the loot you find and all the creatures you killed. I switched development to Windows because that is what my friends were using and I was already familiar with WinForms. I proceeded to make the first real Tibialyzer application, and shared it with my friends. We kept on using it, and I kept on adding more and more features until it grew into what it is today.

# This is a `.exe` file, are you trying to hack me?

Tibialyzer is completely open source, you can look at the complete source code and verify that there is nothing shady going on in there for yourself. The code is reasonably well documented, and I will be happy to answer any questions you have about the code.

Rest assured that if I wanted to make a program to hack people, I would not have made it open source. I would also not have spend this much time polishing it. Tibialyzer does not track any of the keys you type, and it does not send anything over the internet at all. It's completely safe. 

# Building Tibialyzer
Tibialyzer is written using C#. For visualization it uses WinForms, which is part of the .NET framework. Tibialyzer requires the .NET framework 4.5 or higher to run. In addition, Tibialyzer uses SQLite. The necessary SQLite libraries are shipped with the Tibialyzer application.

Tibialyzer is build using Visual Studio 2015. If you wish to build it yourself, open the Tibialyzer solution using Visual Studio and press F5. If you do not have Visual Studio installed, you can download the [Express Edition](https://www.visualstudio.com/en-us/products/visual-studio-express-vs.aspx) that is provided for free by Microsoft. 

As Tibialyzer is performance intensive, I recommend compiling with optimization flags (i.e. Release Build) if you are compiling a version that you intend to use.

# Is Tibialyzer allowed? Isn't this against the rules?
Tibialyzer does not do anything that is against the rules. [The Tibia rules](http://www.tibia.com/support/?subtopic=tibiarules&rule=3b) specify two rules with regard to external applications.

> #### Manipulating the official client program. 
>Manipulating the client program to try gaining an advantage compared to other players is not only extremely unfair but may also cause serious errors in the game. In such cases you cannot expect any assistance from CipSoft concerning level, skill, item or character loss due to manipulating their software or the game protocol. Also, manipulating the client program may lead to a punishment.
> #### Using additional software to play the game
> Keep in mind that you are supposed to play the game yourself, not to have a tool or program play it for you. Doing so gives you an unfair advantage over players who invest time and effort to gain power. Using unofficial software such as a macro program or a so-called "tasker" or "bot" to automatically execute actions in Tibia for you may lead to a punishment. Thus, play fair.

> Using additional software to play the game such as cheat programs is not only forbidden, but also poses a high hacking risk. These programs - especially if offered to you by other players - often contain a virus, trojan or some other sort of backdoor through which a hacker can access your computer and take over your account. It is highly recommended not to ever download and use such a cheat program.

Note that Tibialyzer does neither of these. It does not alter the client program in any way, and it does not play the game for you in any way. Tibialyzer only passively scans the Tibia program to check for server log and chat messages. To verify that Tibialyzer does not break any rules, I have also send an email to Cipsoft to ask them for their opinion on the matter. Below is the full content of these emails.

#### My Email Request
> Hey Cipsoft,
>
>I'm a big fan of your game Tibia, and as a developer I've been working on a helper application for Tibia in my free time to try and make Tibia an even better game for me and my friends. The reason I'm sending this e-mail is because I'm wondering if you think my application is doing anything that is against the rules, as I would like to release it so other players can use it as well.
>
>What my application does is it scans the memory of the Tibia application for strings, more specifically for server log and chat messages. The idea is that the application can then automatically gather various statistics about what's been going on by looking through the log messages, such as the total loot found, the total experience gained, how much damage each person in a party dealt and when you gain a level. As it's keeping track of the server log it can also automatically save the server log to a file, which makes things a lot easier for people that like to keep track of their hunts.
>
>Looking through the rules I don't see anything that would indicate this not being allowed, as there are two rules regarding unofficial software. Manipulating the official client program: which my program does not do. It only passively reads the memory of the application, it does not actually interact with the Tibia application in any way or change the way it behaves. Using additional software to play the game: It does not do anything to play the game for you, it merely gathers statistics and displays information that might be useful to the player.
>
>If you need any more information regarding my program, don't hesitate to ask me. It's open source and currently hosted on github (but not yet polished enough to share with other people). I would really like to confirm that my program is indeed not breaking any rules and that it's alright for me to share it with other people.
>
>Thanks for your time,
>
>    Mark

#### Cipsoft Reply

> Dear Tibia player,
>
>Thank you for your request.
>
>We have read your email interest. Based on the information you have provided, we agree that your tool does not violate the Tibia Rules, so people using it would not have to fear be punished for doing so. Please note, however, that this is not an official statement of approval, either. Rather, it means that using this tool is tolerated, which means that while we would not punish players for doing so, we also cannot take any guarantees for players who do. Thus, if a player had, for example, technical difficulties with the client resulting from your software (unlikely, but we cannot exclude the possibility) we could and indeed would not offer support, since players would use unsupported software to play the game.
>
>I hope this explanation clarifies the issue for you. We appreciate your dedication, and we wish you good luck and lots of fun with your tool!
>
>Kind regards,
>
>Solkrin
>
>Tibia Customer Support




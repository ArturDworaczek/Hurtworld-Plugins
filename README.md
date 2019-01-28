# Hurtworld Plugin Development
###### Plugins created by Artur 'NijoMeisteR' Dworaczek in C# for a Hurtworld server 'NewHurtworldOrder'.
###### Please read the licensing before using any of the plugins.

## What plugins are there currently available?
### BetterChat.cs
Changes the chat for the 'better' by only allowing you to chat to people in a certain radius away from you and also, adding the time in front of the player username using the syntax:
[Hour:Minutes:Seconds] (ID)Username: Message
#### Proximity Chat
![ProximityChat](/Screenshots/ProximityChat.jpg)

All of the messages get stored in the server logs.
#### Server Logs
![ServerLogs](/Screenshots/ServerLogs.jpg)

### IDSystem.cs
Adds a unique number from the maximum amount of players to a player for easier recognition.
This unique number will appear in front of your username on stakes, chat and in-game therefore, when you leave the unique number in front of your username will change to 'OFFLINE' and will appear on the stakes.
This unique number will be only be available during your time on the server and when you leave the number will most likely change to a different one.
#### Identification Numbers on stakes
![ID](/Screenshots/IDStakes.jpg)

### ItemDrop.cs
Doesn't allow the player to drop more than 10 items every 60 seconds to avoid spamming the server which can cause it to crash.
These values are changable in the script and the countdown starts from the last dropped item.
#### Notification after dropping too many items too fast
![DropNotification](/Screenshots/DropNotification.jpg)

### Teleportation.cs
Allows the admin to use the command '/TPR {Amount Of Players}' to randomly teleport the specified amount of players to them (Not including themselves during the pick).
If the amount of players specified is more than the amount of players currently on the server the request will be canceled and you will recieve a reply from the server.
#### Teleportation request message
![TPRequestMessage](/Screenshots/TPRequestMessage.jpg)
#### Teleportation accept message
![TPAcceptMessage](/Screenshots/TPAcceptMessage.jpg)

### Announcements.cs
Allows the server to send prefixed messages by the server owner to everyone on the server.
#### Server Announcement
![ServerAnnouncement](/Screenshots/ServerAnnouncement.jpg)

Allows the admin to send an Admin Message using the command '/am {Message}' that will show up for everyone in the syntax:
[Admin Message ~ {Sender Name}] Message
#### Admin Message
![AdminMessage](/Screenshots/AdminMessage.jpg)

### Groups.cs
Creates new groups if they are not already existing.

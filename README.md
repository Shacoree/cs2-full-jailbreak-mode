# CS2 Full JailBreak Mode by Shacore
## Installation
```
Download the latest release from the releases tab and copy it into your server folder
Config will be created at addons/counterstrikesharp/configs/plugins/JailBreak
Configure database in config file if you want jailshop credits to work.
Check dependendices!
```
## Features
### Warden
```
Blue player model
Tag in chat
Give/Remove FreeDay to player
Give/Remove Rebel to player
TeamGames
Heads/Tails
Start EventDay
Swap CT
Toggle Block
Laser while holding usekey
```
### Last Request
```
Activated when last T is alive, he challenges CTs one by one for various games:

Shot 4 Shot - Simultanious shots with deagle
Knife - Knife fight
NoScope - AWP noscope fight
GunToss - Custom game, both players get an empty weapon which they should throw and who throws it further wins
Russian Roullete - Players are frozen and shoot at each other, random chance that the bullet is real
Rebel - Player becomes red, gets increased HP and AK47 and starts shooting all CT players
```
### EventDay
```
FreeDay - Gives Freeday for all players, lasting 2 minutes
FreezeDay - Classic Catch & Freeze game, CT freezes T, T can unfreeze T, after 15 seconds frozen player dies
HideDay - Hide and Seek game, T has time to hide then CT looks for them
KnifeDay - Team knife fight
PredatorDay - CT has only knife but 5000 hp, T has guns
PresidentDay - Both Teams have guns, CT has 200hp
SniperDay - CT has AWP and 150hp, T has scout
```
### JailShop
```
Player gets credits when wins LR and every X seconds online
With credits player can buy various items:

AllBombs - HE, molotov, smoke, flashbang
FreeDay - FreeDay for the entire round
God - God mode for 5 seconds
Gravity - Lighter gravity for a player
Health Shot
HP - increased HP for a player
NoClip(default is only vip) - Noclip for 5 seconds
Speed - Faster movement for a player
```
### Jail Admin
```
Flag for this is @css/jbadmin

Teleport to a player
Remove Warden from player
Ban a Player from CT
Unban a Player from CT
Give Freeday to a Player
Disarm a Player
Set HP to a Player
Set GOD to a Player
Freeze a Player
```
### Team Balance
```
Classic Jailbreak 3:1 T:CT ratio
```
## Commands
### Warden Commands
```
css_w - Assigns a player Warden
css_uw - Removes Warden from a player
css_wmenu - Opens Warden menu with its features
```
### JailShop Commands
```
css_shop - Opens Shop menu for a player
css_gift <userid> <amount> - Gift credits to a player
```
### LastRequest Command
```
css_lr - Starts LR, available to last alive T player
```
### Admin Command
```
css_admin - Opens Admin menu
```
## Dependencies
```
CounterStrikeSharp - https://github.com/roflmuffin/CounterStrikeSharp
CT Ban - https://github.com/DeadSwimek/cs2-ctban
MenuManager - https://github.com/NickFox007/MenuManagerCS2
MenuExport - https://github.com/NickFox007/MenusExportCS2
Cs2 Admin - https://github.com/schwarper/cs2-admin
```

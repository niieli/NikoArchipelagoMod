# Here Comes Niko! Archipelago WIP
This mod integrates Archipelago into Here Comes Niko! and randomizes a bunch of different things

Here Comes Niko! apworld: [Latest apworld](https://github.com/niieli/Niko-Archipelago/releases)

## Installation

1. Download and install [BepInEx 5.4.x](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.22) in your Here Comes Niko root folder. 
2. Start the game once so that BepInEx creates its stuff
3. Download the latest zip from the [release page](https://github.com/niieli/NikoArchipelagoMod/releases) and extract its content into `BepInEx/plugins`

## Connecting

After loading into the game you insert the server address and port, your slot name and a password if necessary in the top right of the game.

![image](https://github.com/user-attachments/assets/2112698d-d144-4873-9c31-9b457a69d0f1)

Then click 'Connect' and you should load into Home with a blank save. 

(If you already have a save with the same address, port, name and seed, it will load into that one instead)

You can delete Archipelago saves at `...\AppData\LocalLow\Frog Vibes\Here Comes Niko!\Archipelago` (not sure for other systems)

## Known issues

- Reconnecting after disconnecting will sometimes give you a duplicate item (Desync)
  - This can be fixed by restarting the game and connecting again
- Quitting crashes the game LOL!

## What is randomized?
- Coins
- Cassettes
  - Including Mitch & Mai
  
    *they currently work on a levelbasis, so Hairball City they want 5/10 Cassettes, Turbine Town 15/20 etc.*
    || *I am currently working on a progressive system*
- Keys
- Letters
- Both Contact Lists
  - Contact List 1 & 2 are seperate items, so you can have 2 without having the first one *Visible on the in-game "tracker"* 
- Reward from Sensei (Super Jump)
- Fish? Fishsanity? *(Depending on settings)*
- Every Kiosk (except Tadpole HQ elevator)*(Depending on settings)*
  - *There is a problem with the Kiosk not showing the price, when you get access to the next level, 
but you can still talk to the Dispatcher and buy the item! The in-game "tracker" will help you with that.*
- Gary's Garden *(Depending on settings)*
- Achievements *(Depending on settings)*
- Handsome Frog *(Depending on settings)*
- Levels
  - Hairball City
  - Turbine Town
  - Salmon Creek Forest
  - Public Pool
  - Bathhouse
  - Tadpole HQ

The levels are randomized and unlocked via a Ticket *(Depending on settings)*

## Planned features

- Progressive Contact List
- Progressive Cassette Cost (Progressive Locations)
- Different Goals (Employee of the Month) & change elevator repair cost based on a value set inside the yaml
- Make only the needed amount of Coins progressive and the rest useful
- Dialogue and text reflecting the correct cost and item
- MORE INSANITY?!?!
- Better connect UI *not sure how*

# Here Comes Niko! Archipelago
This mod integrates [Archipelago](https://archipelago.gg/) into Here Comes Niko! and randomizes a bunch of different things.

Here Comes Niko! apworld: [Latest apworld](https://github.com/niieli/Niko-Archipelago/releases/latest)

## Installation

1. Download and install [BepInEx 5.4.x](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.22) in your Here Comes Niko root folder (where Here Comes Niko!.exe is located). 
2. Start the game once so that BepInEx creates its stuff
3. Download the latest zip from the [release page](https://github.com/niieli/NikoArchipelagoMod/releases/latest) and extract its content into `BepInEx/plugins`
4. If done correctly it should look like this
```swift
BepInEx/
├── plugins/
│   ├── NikoArchipelago.dll
│   ├── Archipelago.MultiClient.Net.dll
│   └── Newtonsoft.Json.dll
```

## Connecting

After loading into the game open the menu and then click on the Archipelago icon in the middle of the screen and adjust everything to your liking.

![image](https://github.com/user-attachments/assets/c3d67e7f-723a-4dbf-a10d-ac25dabb97fd)


Then click 'Connect' and you should load into Home with a blank save. 

(If you already have a save with the same address, port, name and seed, it will load into that one instead)

You can delete Archipelago saves at `%localappdata%low\Frog Vibes\Here Comes Niko!\Archipelago` (not sure for other systems)

### Custom Playables
If you want to use custom Playables, you can use the [PlayableLoaderBepInEx](https://github.com/niieli/PlayableLoaderBepInEx/releases/latest)

## Known issues
- Connecting to another slot without restarting the game can lead to unchecked locations being sent 
## What is randomized?
- Coins
- Cassettes
  - Including Mitch & Mai
    - Either LevelBased or Scattered *(Depending on settings)*
- Keys... Keysanity?!
- Letters
- Both Contact Lists
  - Contact List 1 & 2 are separate items, so you can have 2 without having the first one
  - *Visible on the in-game "tracker"* 
  - Or change via the yaml to Progressive
- Reward from Sensei (Super Jump)
- Fish? Fishsanity? *(Depending on settings)*
- Every Kiosk *(Depending on settings)*
- Gary's Garden *(Depending on settings)*
- Achievements *(Depending on settings)*
  - Frog Fan only needs 10 bumps
  - Volley Dreams only needs a highscore of 5 in every level
- Handsome Frog *(Depending on settings)*
- Snail Shop *(Depending on settings)*
- Levels & their Kiosk (with random costs based on yaml settings)
  - Hairball City
  - Turbine Town
  - Salmon Creek Forest
  - Public Pool
  - Bathhouse
  - Tadpole HQ
- Apples *(Depending on settings)*
- Bugs *(Depending on settings)*
- Bones *(Depending on settings)*
- Talking to NPCs *(Depending on settings)*

The levels are randomized and unlocked via a Ticket *(Depending on settings)*

## Planned features
- Shuffle Tickets on Kiosk locations
- Set total Coin count in the multiworld 

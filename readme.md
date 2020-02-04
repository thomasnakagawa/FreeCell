# Freecell
Solitare game make in Unity, based on the game from Microsoft Windows 
https://en.wikipedia.org/wiki/FreeCell

![Screenshot](Images/screenshot.png)

## About
Built for a technical assessment. Code, prefabs, scenes and sounds were built by me within 24 hours, except:
* Box sprites, fonts and the button UI element from my own asset store package https://assetstore.unity.com/packages/2d/gui/90-s-desktop-os-ui-159547, in  `Assets/Desktop90_UI`
* SimpleJSON https://github.com/Bunny83/SimpleJSON, in `Assets/SimpleJSON`

## Functionality
### Required functionality
- [x] Implement rules of the game
- [x] Three screens. Intro, gameplay and ending
- [x] Random deal (seed for randomness is in `Assets/Configuration/config.json`)
- [x] Click open card to send to free cell
- [x] Drag and drop single cards
- [x] Cannot move cards to illegal positions
- [x] JSON I/O. The file `Assets/Configuration/config.json` is used to configure different variables (see Configuration section below). A results.json file is output when the player completes the game

### Extra functionality
- [x] Different highlight colors when dragging a card over a valid or invalid position
- [x] Deal animation
- [x] Card moving animation
- [x] End of game card visual
- [x] Sounds
- [x] Timer
- [x] Redeal button
- [x] 'Cheats' mode for debugging. Enable from config file to be able to drag any card
- [ ] Drag multiple cards (Didn't get to this but it would be nice to have)

## Dev setup
* Clone the repo
* Open the project in Unity
* The UI layout is built for <b>16:9 or 16:10</b> aspect ratio, please play the game in one of these aspect ratios.

## Compatibility
Built with Unity 2018.4.13f1

## Configuration
The game can be configured from the file `Assets/Configuration/config.json`. Available variables are:
* EnabledHoverColor: Hex color.The color a card highlights when dragging a card that can be placed on top of it
* DisabledHoverColor: Hex color. The color a card highlights wheb dragging a card that cannot be place on top of it
* CheatsEnabled: true/false. When true, pressing the 'A' key triggers the end of the game. Also when true, you can pick up card from anywhere in the columns.
* RNGSeed: integer. The seed for the random number generator for shuffling the cards.
* OutputFile: file. Relative file path for where to store the results when the game is won.

## Code overview
The programming of the game centers around the idea that PlayingCard objects are always attached to some anchor object. Anchors are things on screen where a card can be placed (in a column, foundation space, or free cell). The class CardAnchor.cs contains the main anchor behavior, and then the decendent classes implement specific behavior for the foundation, column and freecell spaces. The `Assets/Scripts/Cards` and `Assets/Scripts/Anchors` folder contain the code for the main interactive gameplay.

The `Assets/Scripts/GameManagement` folder contains scripts that do other functionality around the game. GameConfiguration reads in a JSON file to provide config variables for other classes. ResultsExporter writes a JSON file with the player's results when the game is completed.

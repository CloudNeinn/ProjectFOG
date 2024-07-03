# ProjectFOG
## Summary
This is my first original Unity game, which currently doesn't have a name so its code name is ProjecFOG (Project First Own Game). This is a 2D pixel styled Metroidvania inspired by Hollow Knight, the Ori series, and Celeste. I am developing this game as a pet project for myself, my experience, and my portfolio in hopes that it will help me get into game dev. I chose this genre and style because I wanted to create something I would want to play myself, I could create myself, so it would look decent. I chose Metroidvania instead of Platformer because I wanted something more interesting and hard to deliver and pixel style because as I see it it's easier to make pixel art look decent than some other art style.

## How to play the game
Currently game has a small map with enemies in it also there are traps (only spikes for now), checkpoints, and altars where you can acquire new abilities. Checkpoints - green rectangles, Altars - white rectanglish stuff, Spikes - dark blue rectangles. Also currently all of the "art" in the game is placeholders and it has almost no animations, it has only ones that were mandatory to implement game mechanics.

## Enemy types:
+ Crawler - just an enemy that walks back and forth giving no attention to the player,
+ Charger - patrols certain area when sees the player charges towards him, but cannot go out of his patrol area,
+ Ranger - patrols certain areas when sees the player he shoots and walks back from him while reloading his shot,
+ Chaser - patrols certain areas when sees the player he starts chasing him until looses the player. He loses him if the player is out of sight for long enough (~6 seconds). Then he goes back to his patroling area
+ Karasu - essentially a crow that is near its nest. When sees player it starts circling around them and after a certain amount of circles dashes in the player to attack. If it flies away from the nest a certain distance it loses interest in the player and returns back to the nest.
+ More enemies will be added soon

## Controls:
+ A/D - move left/right,
+ Space - jump,
+ Left Ctrl - crouch/fast fall,
+ Move into the wall - slide on it,
+ Jump while moving into wall - wall jump,
+ R - Hook,
+ H - World switch,
+ M - Map,
+ Left Alt - Glider,
+ Left Shift - Dash,
+ Mouse 1 - Attack/Deflect,
+ Mouse 2 - Block,
+ B - Barrier,
+ E/F - Interaction,
+ Esc - Menu,
+ ` - Noclip,
+ P - Unlocks all skills

## Tools used in this game:
+ Unity game engine
+ Object-Oriented C#

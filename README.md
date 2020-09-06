# Flying Frisbee

With this little Ultimate Frisbee game I want to get more familiar with Unity.

**[Try it out!](https://thommynator.github.io/FlyingFrisbeeGame/)** 😉

## Changelog

### 06.09.2020
#### Bugfixes
* Fixed #25 wrong score count
* Fixed #20 automated movement while holding the frisbee
* Fixed #21 skewed camera

### 03.09.2020 (#26)
#### Animations
* Idle, Running, Selected animation for player
* Idle, Running animation for opponens

![](gifs/demo7.gif)

### 04.06.2020 (#17, #18)
#### Scoring System
* Point for the player, when frisbee is caught in the endzone.
* Point for the opponent, when the frisbee touches the ground or is out of bound.

#### Pause Menu
* For pausing and resuming the game
* Very basic "menu". So far, only for resetting the score
* Possibility to quit the game

![](gifs/demo6.gif)

### 11.05.2020
#### Movement Manager (#12)
* Can be accessed during game (Space key)
* Movement Manager pauses the game (frisbee stops in air, players stop moving, ...)
* You can plan some paths for your players. They will start running this path when resuming the game.

![](gifs/demo5.gif)

### 07.05.2020 (#11)
#### Minimap, Defense, Player-Selection
* New minimap in the top-left of the screen
* Opponents are defending the frisbee, reacting on the aim of the player
* Players can be selected by clicking on them

![](gifs/demo4.gif)

### 28.04.2020
#### Dynamic Camera
More dynamic camera movement:
* Camera is much closer to the player, like 3rd person view
* Camera is always filming in aiming direction (rotating around frisbee)
* Camera follows frisbee very smoothly using linear interpolation

![](gifs/demo3.gif)

### 25.04.2020
#### Following, Throwing
* Camera and spotlight are following the frisbee. 
* Throw angle can be adjusted to throw higher or lower.
* Thrower can switch his throw hand (left/right). 

![](gifs/demo2.gif)

### 19.04.2020
#### Basics
* Basic aim & throw functionality. 
* Player can move while not holding the frisbee.

![](gifs/demo1.gif)


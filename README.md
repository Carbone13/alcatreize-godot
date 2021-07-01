Alcatreize Godot
===================

[![GitHub license](https://img.shields.io/github/license/Naereen/StrapDown.js.svg)](https://github.com/Carbone13/alcatreize-godot/tree/demo/LICENSE)
[![Godot](https://img.shields.io/badge/Godot-v3.2-%23478cbf?logo=godot-engine&logoColor=white)](https://godotengine.org)


> Alcatreize is a custom, deterministic physics system, with pixel perfection in mind. Designed for use in the Godot Engine. Many game who need rollback netcode should be interested in it.
> It only work for C# but a C++ module is planned, which would allow cleaner internal Nodes & GDScript support. Note this is still actively in development and is not yet suitable for production

> The demo branch contains a little Godot Project showcasing what is achievable with Alcatreize

:bangbang: | Work only in Godot Mono
:---: | :---

### Limitations
Alcatreize is designed toward Pixel Art game. Every position of any given Entity need to be in pixel coordinate (= integers).
This is not really suitable for anything else that pixel-perfect games. \
For the moment only AABB (= rectangles) are allowed as collisions shapes, note that this may change in the future.

### What's inside ?
Alcatreize classes entities as two types (static level geometry does not count as an entity). \
But first, what is an Entity ?
> An entity is an object who can regroup boxes (= hitbox, hurtbox or collision box) and move in the scene

As said earlier, their are 2 types of entities :
> Actors : they are entities that move in the scene while taking care to **not** overlap any present colliders

> Solids : they move freely in the scene without taking care of any collisions in their path, their only interactions is pushing any Actors in their path with them

### How to use it
#### Importing Alcatreize into Godot
You need to use the MONO version of Godot, as Alcatreize is writtent in C#. \
Alcatreize will only work with other C# classes, as inheriting is mandatory (Althought you could do some glue code to limit the c# usage) \
The final goal would be to convert this to an internal C++ Godot Module

Simply Drag & Drop the "alcatreize" folder of the main branch anywhere in your Godot Project.

#### Using it

TODO

#### The demo branch
The Demo Branch is a Godot sample Project to showcase the usage of this module

### Okay, but why not just using the default Godot physics ?
Godot physics are not pixel-perfect by default. A pixel-perfect behaviour can still be reproduced but their is a second point : \
**DETERMINISM** \
A determinist physics is a system where rolling back to a state, and doing the exact same things will result in the exact same result.
This don't happens when using the Godot physics as rolling back and re-simulating the physics frame is not possible. Also the use of floats break the determinism, especially when targeting different platforms

### Used Technologies & Credits
Alcatreize is using soft float in order to achieve its determinism.
The soft floats are based on [this repository](https://github.com/Kimbatt/soft-float-starter-pack), it is contained in the alcatreize/maths folders (except the sfloat2 which is a simple Vector2 struct where floats have been replaced with sfloats)

The colliders query broadphase is done using [SuperGrid2D](https://github.com/bartofzo/SuperGrid2D)

The main logic for Actor & Solid follow [this article](https://medium.com/@MattThorson/celeste-and-towerfall-physics-d24bd2ae0fc5) by Maddy Thorson \
Everything else is done by Carbone13 (Lucas Michaudel)
### License(s)
Alcatreize is published with the MIT License, but SuperGrid and Soft Floats SP both have their respective licenses that must be respected
# InventoryDemo

## Requirements:
#### 1. Gameplay
- Implement character movement logic. [x]
- Implement character animations. [x]
- Implement character interaction with the world (e.g., picking up items, talking to
NPCs). [x]
#### 2. Inventory Structure
- Implement a UI slot-based inventory. [x]
- Implement interactions for:
  - Adding items to the inventory. [x]
  - Removing items from the inventory. [x]
  - Moving items within the inventory. [x]
  - Dragging and swapping items between inventory slots. [x]
  - Using or equipping items (e.g., consuming health potions, equipping
weapons). [x]
#### 3. UI Design
- Ensure the UI dynamically updates based on inventory contents. [x]
- Design a clear, intuitive, and visually appealing inventory UI. (Done clear and intuitive but not really visually appealing)
- Show item details when selected or hovered over (via tooltip or dedicated panel). [x]
#### 4. Save and Load System
- Implement a save system for inventory state. [x]
- Implement loading of inventory data when the game starts. [x]
- Ensure slot-based persistence for each item. [x]

## About the project
I was recently playing Valheim, so that was my main inspiration for what to do for this demo. 
I wanted the basic mechanics for collecting resources and equipping items, so I first looked 
for assets to match the most basic things I could think of: wood, axe and trees.

This was my first time using the terrain tool in Unity, I only used landscaping from Unreal, so I had some problems with it.
The primary focus was speed because of the time constraint of 48h, so code quality suffered, especially on the second day, 
so there's a lot of room for improvement.

I believe the basic inventory system works great, as well as the item assets creation and database. I made custom editors 
to help populate and manipulate them.
Item data is separated in what defines the item and is shared (icons, prefab, max stacks...) and what is the runtime data (quantity, instance id).
Shared data could have one more separation for assets that need loading like icons and prefabs, to support content versioning/delivery via addressables.

The worst part of the code is definitely input handling, it's really messy. There are parts 
using InputActions, there's one piece of code using the old Input api and the UI is using 
the IPointer interfaces, so there are 3 different input sources. There is also no gamepad support.

The inventory UI works nicely with the inventory controller and inventory slots, but there is no
View/ViewController separation, only the data layer is properly separated, in the form of ItemData.

Save system is really simple and crude but works well enough. The most obvious improvement would be to have
proper save points so the code doesn't write to file every change. Since I have no save screens, every modification
must be saved for now. Chopped trees are not yet saved but the same method to save picked objects could be used:
generate a tree instance id, save chopped instances and them destroy them (or better yet, don't even load them).
Also, all trees are loaded from start, which is bad, but at least they have proper LOD.

Movement has some jank, but it works, so I left as is, but this should definitely be very well polished for a final product.

The player controller class started getting a bit too many responsibilities. There's a bunch os logic that could be 
moved to other places. The controlled should only listen to input and fire off events.

### 3rd party assets:
- Forest landscape assets: https://assetstore.unity.com/packages/3d/environments/fantasy/fantasy-forest-environment-free-demo-35361
- Model and animations:
    - https://assetstore.unity.com/packages/3d/characters/humanoids/humans/human-character-dummy-178395
    - https://assetstore.unity.com/packages/3d/animations/human-basic-motions-free-154271
    - https://assetstore.unity.com/packages/3d/animations/human-melee-animations-free-165785
- Wood icon: <a href="https://www.flaticon.com/free-icons/wood" title="wood icons">Wood icons created by imaginationlol - Flaticon</a>
- Axe icon: <a href="https://www.flaticon.com/free-icons/axe" title="axe icons">Axe icons created by Freepik - Flaticon</a>

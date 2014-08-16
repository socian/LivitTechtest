Livit Tech Test
=============

**Simple & lightweight yet fully customazible and scalable point 'n click enging.**

Web Demo:

http://www.dihardja.de/livittechtest/webdemo/


This is a demonstration of a simple and leightweight yet fully customazible and scalable point n' click engine. Interactions can be created without the need of any additional coding. Only by manipulating the game data inside the xml file and by creating the view prefabs, an infinite chain of interactions can be made.

The elements that builts up the game are Levels, Steps, Items and Quests. A level contains several steps. A single step can contain several items and quests. Rules can be defined in the XML to control under what kind of condition the player can move from one step to an other.

Some steps for example requires that the player pick up all the items while other steps are only then completed if the quest is solved either by clicking it or by dragging the right item into it.

To clarify this, I will go through the gamedata.xml that is used in the simple point 'n click demonstration.


**1. The gamedata.xml** 
```
<game>
</game>
```
This is the root tag of the gamedata.xml


**2. Available items**
```
<itemlist>
  <item id="1" name="Key" inventorysprite="ItemKey" />
  <item id="2" name="Light Bulb" inventorysprite="light bulb" />
  <item id="3" name="Pepper" inventorysprite="pepper" />
  <item id="4" name="Poison" inventorysprite="poison-icon" />    
  <item id="5" name="Burger" inventorysprite="burger-icon" />    
</itemlist>
```
All available items in the game are defined within the <itemlist> tag inside the root tag of the gamedata. The attributes "name" and the "inventorysprite" is used to display the items in the inventory panel. Items that are defined in each step are using a "refid" attribute for referencing to these items.


**3. A simle click quest**
```
<step id="STEP_1" prefab="Prefabs/Steps/StepStart" clearinventory="true">
  <questlist>
     <quest id="START_QUEST" placeholder="LabelStart" onsolved="STEP_2" />
  </questlist>        
</step>   
```
This is the first step in the demonstration. The "clearinventory" attribute of the step node which is set to true means that the inventory should always be cleared if this step gets initialized.

The step contains only one quest that doesn't have an accepted item list. This means that this quest can be solved by clicking it. The "placeholder" attribute of the quest means that the engine will look for a game object with a name equal to the placeholder value and register it events. After the quest is solved, in this case by clicking it, the game will move to the next step that is defined in the "onsolved" attribute of the quest.  



**4. Picking up an item**
```
<step id="STEP_2" prefab="Prefabs/Steps/Step_1_1">
  <itemlist>
    <item refid="1" placeholder="ItemKey" onpicked="STEP_3" />
  </itemlist>
</step>
```
This is a step that contains one item. The "refid" attribute of the item means that this item is refering to one of the available item in the game whose id is equal the the refid. The "onpicked" attribute of the item defines that the game should move to the given step after the item is picked up by the player



**5. Picking up multiple items**
```
<step id="STEP_3" prefab="Prefabs/Steps/Step_1_2" oncomplete="STEP_4">
  <itemlist>
    <item refid="2" placeholder="ItemLightBulb" required="true" />
    <item refid="4" placeholder="ItemPoison" required="true" />
    <item refid="3" placeholder="ItemCabe_1" required="true" />
    <item refid="3" placeholder="ItemCabe_2" required="true" />
    <item refid="3" placeholder="ItemCabe_3" required="true" />
  </itemlist>
</step>
```

This step containes 5 items. Each item has an attribute "required" that is set to true. this means that after all items are picked up by the player the step will be completed. the game will then move to the next step that is defined in the "oncomplete" attribute of the step



**6. Solving a quest by dragging an item into the quest**
```
<step id="STEP_4" prefab="Prefabs/Steps/Step_1_3">
  <questlist>
    <quest placeholder="Lamp" onsolved="STEP_5" >
      <accepteditemlist>
        <item refid="2" />
      </accepteditemlist>
    </quest>
  </questlist>
</step>
```
This is a step that containes one quest which contains one accepted item. This means that this quest has to be solved by dragging the right item into it. The quest will only be solved if the items refid is equal to the items refid defined in the accepted list. After the quest is solved, the game will move to the step defined in the "onsolved" attribute of the quest.

**Go through the gamedata.xml**

Please feel free to take a closer look at the gamedata.xml to get a better understanding about how to create point 'n click adventures.

https://github.com/socian/LivitTechtest/blob/master/StreamingAssets/Data/gamedata.xml

The engine it self is written in C# based on the MVCVM and Dependency Injection pattern. Feel free to browse through the commented source code.

https://github.com/socian/LivitTechtest/tree/master/Scripts

Enjoy :)



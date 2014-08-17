Livit Tech Test
=============

Web Demo:

http://www.dihardja.de/livittechtest/webdemo/

**Note:**
The web demo doesn't support the save & load functionality. Please download the desktop version,
available for PC & Mac, which provides the save and load feature.

http://www.dihardja.de/livittechtest/LivitTechtest_PC.zip

http://www.dihardja.de/livittechtest/LivitTechtest_MAC.zip

--
**Simple & lightweight yet fully customizable and scalable point 'n click engine.**
This is a demonstration of a simple and lightweight yet fully customizable and scalable point n' click engine. Interactions can be created without the need of any additional coding. Only by manipulating the game data inside the xml file and by creating the view prefabs, an infinite chain of interactions can be made.

The elements that built up the game are Levels, Steps, Items and Quests. A level contains several steps. A single step can contain several items and quests. Rules can be defined in the XML to control if a player can move from one step to an other.

Some steps for example requires that the player picks up all of the items while other steps will only then complete if the quest is solved either by click or by dragging the right item into it.

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
All available items in the game are defined in an itemlist tag inside the root tag of the gamedata. The attributes "name" and the "inventorysprite" is used to display the items in the inventory panel. Items that are defined in the steps are using the "refid" attribute for holding a reference to one of these items.



**3. A simle click quest**
```
<step id="STEP_1" prefab="Prefabs/Steps/StepStart" clearinventory="true">
  <questlist>
     <quest id="START_QUEST" placeholder="LabelStart" onsolved="STEP_2" />
  </questlist>        
</step>   
```
This is the first step in the demonstration. The step tag has a “clearinventory” attribute that is set to true which means that the inventory of the player will be cleared every time this step gets initialized.

The step contains only one quest that doesn't contains a list of acceptable items. This means that this quest can be solved by clicking it. The "placeholder" attribute of the quest means that the engine will look for a game object with a name equal to the placeholder value and register its events. After the quest is solved, in this case by clicking it, the game will move to the next step that is defined in the "onsolved" attribute of the quest.
 


**4. Picking up an item**
```
<step id="STEP_2" prefab="Prefabs/Steps/Step_1_1">
  <itemlist>
    <item refid="1" placeholder="ItemKey" onpicked="STEP_3" />
  </itemlist>
</step>
```
This is a step that contains only one item. The "refid" attribute of the item means that this item is holding a reference to one of the available items in the game whose id is equal to the “refid”. The "onpicked" attribute of the item defines that the game should move to the given step after the item is picked up by the player.



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
This step contains 5 items where each one of them has the attribute "required" that is set to true.
this means that after all items are picked up by the player the step will be completed. the game will
then move to the next step that is defined in the "oncomplete" attribute of the step.



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
This is a step that contains one quest which contains one accepted item. This means that this quest has to be solved by dragging the right item into it. The quest will only be solved if the items “refid” is equal to the items “refid” defined in the accepted list. After the quest is solved, the game will move to the step that is defined in the "onsolved" attribute of the quest.



**Go through the gamedata.xml**

Please feel free to take a closer look at the gamedata.xml to get a better understanding about how to create point 'n click adventures.

https://github.com/socian/LivitTechtest/blob/master/StreamingAssets/Data/gamedata.xml

The engine it self is written in C# based on the MVCVM and the Dependency Injection pattern. Feel free to browse through the commented source code.

https://github.com/socian/LivitTechtest/tree/master/Scripts

Enjoy :)



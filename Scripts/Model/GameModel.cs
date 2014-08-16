using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;
using System.IO;
	
// xml mapping class for an Item
public class Item
{
	[XmlAttribute("id", typeof(string))]
	public string Id {get;set;}

	[XmlAttribute("refid", typeof(string))]
	public string RefId {get;set;}

	[XmlAttribute("name", typeof(string))]
	public string Name {get;set;}

	[XmlAttribute("placeholder", typeof(string))]
	public string Placeholder {get;set;}

	[XmlAttribute("onpicked", typeof(string))]
	public string OnPicked {get;set;}

	[XmlAttribute("inventorysprite", typeof(string))]
	public string InventorySprite {get;set;}

	[XmlAttribute("required", typeof(bool))]
	public bool Required {get;set;}

	public bool IsPicked {get;set;}

	public Item()
	{
		IsPicked = false;
	}
}

// xml mapping class for a quest
public class Quest
{
	[XmlAttribute("id", typeof(string))]
	public string Id {get;set;}

	[XmlAttribute("placeholder", typeof(string))]
	public string Placeholder {get;set;}

	[XmlAttribute("onsolved", typeof(string))]
	public string OnSolved {get;set;}

	[XmlArray("accepteditemlist")]
	[XmlArrayItem("item", typeof(Item))]
	public Item[] AcceptedItemList {get;set;}

	public bool IsSolved {get;set;}

	public Quest()
	{
		IsSolved = false;
	}
}

// xml mapping class for a level step
public class Step
{
	[XmlAttribute("id", typeof(string))]
	public string Id {get;set;}

	[XmlAttribute("prefab", typeof(string))]
	public string Prefab {get;set;}

	[XmlAttribute("clearinventory", typeof(bool))]
	public bool ClearInventory {get;set;}

	[XmlAttribute("oncomplete", typeof(string))]
	public string OnComplete {get;set;}

	[XmlArray("itemlist")]
	[XmlArrayItem("item", typeof(Item))]
	public Item[] ItemList {get;set;}

	[XmlArray("questlist")]
	[XmlArrayItem("quest", typeof(Quest))]
	public Quest[] QuestList {get;set;}
}

public class Level
{
	[XmlAttribute("id", typeof(string))]
	public string Id {get;set;}

	[XmlArray("steplist")]
	[XmlArrayItem("step", typeof(Step))]
	public Step[] StepList {get;set;}
}

// xml mapping class for the game data
[XmlRoot("game")]
public class Game
{
	[XmlArray("itemlist")]
	[XmlArrayItem("item", typeof(Item))]
	public Item[] ItemList { get;set; }

	[XmlArray("levellist")]
	[XmlArrayItem("level", typeof(Level))]
	public Level[] LevelList { get;set; }

}

public class GameModel  
{

	public string DataPath {get;set;}

	Game gameData;
	public Game Data 
	{
		get 
		{
			return gameData;
		}
	}

	// returns the level based on its index
	public Level GetLevel( int levelIndex )
	{
		return gameData.LevelList[levelIndex];
	}

	// returns a level based on its id
	public Level GetLevel(string levelId)
	{
		for(int i=0; i<gameData.LevelList.Length; i++) 
		{
			Level level = gameData.LevelList[i];
			if(level.Id == levelId) return level;
		}
		return null;
	}

	// returns the available items in the game
	public Item[] AvailableItems
	{
		get 
		{
			return gameData.ItemList;
		}
	}

	public void Load()
	{
		try 
		{
			// load the xml file and deserialize it into a Game instance
			XmlSerializer serializer = new XmlSerializer( typeof( Game ) );
			FileStream fileStream = new FileStream(DataPath, FileMode.Open);
			gameData = (Game) serializer.Deserialize(fileStream);
			fileStream.Close();
		}
		catch(IOException e) 
		{
			throw e;
		}
	}
}





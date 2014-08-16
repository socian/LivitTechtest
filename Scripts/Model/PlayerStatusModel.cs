using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;


[XmlRoot("player")]
public class Player 
{
	[XmlElement("level", typeof(string))]
	public string Level {get;set;}

	[XmlElement("step", typeof(string))]
	public string Step {get;set;}

	[XmlArray("itemlist")]
	[XmlArrayItem("item")]
	public List<string> ItemIdList {get;set;}
}


public class PlayerStatusModel  
{

	public delegate void OnChandedHandler();
	public event OnChandedHandler OnChanged;

	public string DataPath {get;set;}

	Player player;

	public PlayerStatusModel()
	{
		player = new Player();
		Clear();
	}

	public void Clear()
	{
		player.Level = "";
		player.Step = "";
		player.ItemIdList = new List<string>();
	}

	public string Level 
	{
		get 
		{
			return player.Level;
		} 
		set 
		{
			player.Level = value;
		}
	}

	public string Step 
	{
		get 
		{
			return player.Step;
		} 
		set 
		{
			player.Step = value;
		}
	}

	public List<string> Items 
	{
		get 
		{
			return player.ItemIdList;
		} 
		set 
		{
			player.ItemIdList = value;
		}
	}

	public void Load()
	{
		// load a 
		XmlSerializer serializer = new XmlSerializer( typeof( Player ) );
		FileStream fileStream = new FileStream(DataPath, FileMode.Open);
		player = (Player) serializer.Deserialize(fileStream);
		fileStream.Close();

		if( OnChanged != null ) OnChanged();
	}

	public void Save()
	{

		// save a serialized Player class into an xml
		XmlSerializer serializer = new XmlSerializer( typeof( Player ) );
		FileStream fileStream = new FileStream(DataPath, FileMode.Create); 
		serializer.Serialize(fileStream, player);
		fileStream.Close();
	}
}




using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInfo
{
	public Item Item {get;set;}
	public int Amount {get;set;}
}


public class InventoryVM 
{
	// event that is fired every time an item is added or removed
	// from the inventory
	public delegate void OnItemsChangedHandler();
	public event OnItemsChangedHandler OnItemsChanged;

	Dictionary<string, ArrayList> itemList;

	public InventoryVM()
	{
		itemList = new Dictionary<string, ArrayList>();
	}

	// all available items inside the game
	public Item[] AvailableItems {get;set;}

	public void AddItem(Item item)
	{
		// check if an item with the same refid already exists
		// if yes, add this one
		if(itemList.ContainsKey(item.RefId))
		{
			ArrayList arrList = itemList[item.RefId];
			arrList.Add(item);
		}
		// create a new arraylist for containing
		// items with the new refid
		else 
		{
			ArrayList arrList = new ArrayList();
			arrList.Add(item);
			itemList.Add(item.RefId, arrList);
		}

		if( OnItemsChanged != null ) OnItemsChanged();
	}

	// returns the real items that 
	Item GetAvailableItem(string refid)
	{
		for(int i=0; i<AvailableItems.Length; i++) 
		{
			Item item = AvailableItems[i];
			if(item.Id == refid) return item;
		}
		return null;
	}

	public ItemInfo[] ItemInfos
	{
		get 
		{
			ItemInfo[] displayInfos = new ItemInfo[itemList.Count];
			int index = 0;
			foreach(ArrayList arrList in itemList.Values)
			{
				ItemInfo itemDI = new ItemInfo();
				itemDI.Amount = arrList.Count;

				Item refItem = (Item) arrList[0];
				Item aitem = GetAvailableItem(refItem.RefId);

				itemDI.Item = aitem;

				displayInfos[index] = itemDI;
				index ++;
			}
			return displayInfos;
		}
	}

	public void RemoveItem(string itemRefID)
	{
		ArrayList arrList = itemList[itemRefID];
		if(arrList.Count == 1) 
		{
			arrList.RemoveAt(0);
			itemList.Remove(itemRefID);
		}
		else 
		{
			arrList.RemoveAt(arrList.Count);
		}

		if( OnItemsChanged != null ) OnItemsChanged();
	}

	public List<string> ItemsIdList
	{
		get 
		{
			// convert the inventory items into a list
			// which is needed for serialization
			List<string> itemIdList = new List<string>();
			foreach(ArrayList arrList in itemList.Values)
			{
				for(int i=0; i<arrList.Count; i++) 
				{
					Item item = (Item) arrList[i];
					itemIdList.Add(item.RefId);
				}
			}
			return itemIdList;
		}
		set 
		{
			// add a list into the internal collections
			// this heapens after the user loads a saved game
			itemList.Clear();
			List<string> itemsIdList = value;
			foreach(string itemId in itemsIdList) 
			{
				Item item = new Item();
				item.RefId = itemId;
				AddItem(item);
			}

			// fire the event to notify the view to
			// update their display
			if( OnItemsChanged != null ) OnItemsChanged();
		}
	}

	public void Clear()
	{
		itemList.Clear();
		if( OnItemsChanged != null ) OnItemsChanged();
	}
}




using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepVM 
{

	Step step;
	List<string> idListFilter;

	public StepVM(Step s)
	{
		step = s;
		idListFilter = new List<string>();
		Reset ();
	}

	public void Reset()
	{
		if(step.ItemList == null) return;
		for(int i=0; i<step.ItemList.Length; i++) 
		{
			Item item = step.ItemList[i];
			item.IsPicked = false;
		}
	}

	// retuns the step object which represents the raw
	// xml data
	public Step Step
	{
		get 
		{
			return step;
		}
	}

	// set items pickedup attribute to true
	// if they are already picked up by the player.
	// this is the case after the player loads a saved game
	public List<string>  ItemIdsListFilter 
	{
		set 
		{
			if(step.ItemList == null) return;
			idListFilter = value;
			for(int i=0; i<step.ItemList.Length; i++) 
			{
				Item item = step.ItemList[i];
				foreach(string itemId in idListFilter) 
				{
					if(itemId == item.RefId)
					{
						item.IsPicked = true;
						idListFilter.Remove(itemId);
						break;
					}
				}
			}
		}
	}
	// returns a quest object by its name
	public Quest GetQuest(string placeholderName)
	{
		for(int i=0; i<step.QuestList.Length; i++) 
		{
			Quest q = step.QuestList[i];
			if(q.Placeholder == placeholderName) return q;
		}
		return null;
	}
	
	// retunr the Item object bysed on its placeholder name
	public Item GetItem(string placeholderName)
	{
		for(int i=0; i<step.ItemList.Length; i++) 
		{
			Item item = step.ItemList[i];
			if(item.Placeholder == placeholderName) return item;
		}
		return null;
	}

	// check if a step is complete
	public bool StepComplete
	{
		get 
		{
			for(int i=0; i<step.ItemList.Length; i++) 
			{
				Item item = step.ItemList[i];
				if(item.Required && ! item.IsPicked) return false;
			}
			return true;
		}
	}

	// check if the quest accepting the game object that is dropped by the player
	public bool IsAcceptedByQuest(string questPlacaholderName, string dropedItemId)
	{
		Quest quest = GetQuest(questPlacaholderName);
		for(int i=0; i<quest.AcceptedItemList.Length; i++) 
		{
			Item item = quest.AcceptedItemList[i];
			if( item.RefId == dropedItemId ) return true;
		}
		return false;
	}
}




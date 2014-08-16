using UnityEngine;
using System.Collections;

public class GameController : Controller 
{

	public GameComponent gameComponent;

	override protected void OnInitComplete()
	{
		// get the first level
		Level level = gameModel.GetLevel(0);
		LevelVM levelVM = new LevelVM( level );

		// register the player status model events
		playerStatusModel.Level = level.Id;
		playerStatusModel.OnChanged -= OnPlayerStatusModelChanged;
		playerStatusModel.OnChanged += OnPlayerStatusModelChanged;

		// register the game component events
		gameComponent.OnItemPicked -= OnItemPicked;
		gameComponent.OnItemPicked += OnItemPicked;

		gameComponent.OnItemAcceptedByQuest -= OnItemAcceptedByQuest;
		gameComponent.OnItemAcceptedByQuest += OnItemAcceptedByQuest;

		gameComponent.OnStepStart -= OnLevelStepStart;
		gameComponent.OnStepStart += OnLevelStepStart;

		// register the inventory view model events
		inventoryVM.OnItemsChanged -= OnInventoryItemsChanged;
		inventoryVM.OnItemsChanged += OnInventoryItemsChanged;

		// start the game view
		gameComponent.Init( levelVM );
	}

	// the player loads a saved game
	void OnPlayerStatusModelChanged()
	{
		inventoryVM.ItemsIdList = playerStatusModel.Items;

		Level level = gameModel.GetLevel(playerStatusModel.Level);
		LevelVM levelVM = new LevelVM(level);
		StepVM stepVM = levelVM.GetStepVM(playerStatusModel.Step);

		stepVM.ItemIdsListFilter = inventoryVM.ItemsIdList;

		gameComponent.ShowLoadingPrefab();

		// give the game a bit time to destroy the current content
		// before loading the new one

		// this is a Workaround since NGUI seems to have a problem
		// get the game objects if the the game os loaded
		// in the same step as it were saved
		StartCoroutine(DelayInit(levelVM, stepVM));
	}

	IEnumerator DelayInit(LevelVM lvm, StepVM svm)
	{
		yield return new WaitForSeconds(.5f);
		gameComponent.Init(lvm, svm);
	}
	
	void OnInventoryItemsChanged()
	{
		// update the player status model every time the player
		// picks up an item
		playerStatusModel.Items = inventoryVM.ItemsIdList;
	}

	void OnLevelStepStart(Step step)
	{
		// if the "clearinventory" attribute of a step is set in the xml
		// then clear the inventory. this is usually the case if the player
		// repeats the game from the start
		if(step.ClearInventory) inventoryVM.Clear();

		// udate the player status model every time a new step level
		// is initialized
		playerStatusModel.Step = step.Id;
	}

	void OnItemAcceptedByQuest(string itemRefID)
	{
		// remove the item from the inventory view model
		// if it is accepted by the quest
		inventoryVM.RemoveItem( itemRefID );
	}

	void OnItemPicked(Item item)
	{
		// add the item into the inventory view model
		inventoryVM.AddItem( item );
	}
}



using UnityEngine;
using System.Collections;
using System.IO;

// This class prepares the game data needed by the controllers and also initializes 
// and inject the controllers with the models they need for their view components.

public class Main : MonoBehaviour 
{

	// this controller initializes and handle the event of the toolbar component
	public ToolbarController toolbarController;

	// this controller initializes and handle the inventory view component events
	public InventoryController inventoryController;

	// this controller initializes and handle the game view component events
	public GameController gameController;

	// the game model containes the game data
	GameModel gameModel;

	// the player status model contains the status of the player like Level, Step and Items
	// that the playe have picked up
	PlayerStatusModel playerStatusModel;	

	// the inventory view model is used to notify the inventory view component
	// every time an item is added or removed from it.
	InventoryVM inventoryVM;

	void Start () 
	{
		// load the gamedata within a coroutine
		// so the game is not blocked while loading.
		StartCoroutine( LoadGameData() );
	}
	
	IEnumerator LoadGameData()
	{
		// get the game model
		gameModel = new GameModel();

		// set the gamedata.xml file path 
		gameModel.DataPath = Application.streamingAssetsPath + "/Data/gamedata.xml";
		// load the game data into the model
		gameModel.Load();

		// get the player status model
		playerStatusModel = new PlayerStatusModel();
		playerStatusModel.DataPath = Application.streamingAssetsPath + "/Data/player_status.xml";
		playerStatusModel.Clear();

		// init the inventory VM
		inventoryVM = new InventoryVM();
		inventoryVM.AvailableItems = gameModel.AvailableItems;

		yield return null;

		// the game data should be ready at this point. 
		OnGameDataReady();
	}
	
	void OnGameDataReady()
	{
		// start the controllers by injecting the models they need for their view components
		toolbarController.Init ( gameModel, playerStatusModel, inventoryVM );
		inventoryController.Init ( gameModel, playerStatusModel, inventoryVM );
		gameController.Init ( gameModel, playerStatusModel, inventoryVM );
	}
}


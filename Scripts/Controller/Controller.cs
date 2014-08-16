using UnityEngine;
using System.Collections;

// the base controller 
public class Controller : MonoBehaviour 
{
	// injected models that the controllers needs for their view components
	protected GameModel gameModel;
	protected PlayerStatusModel playerStatusModel;
	protected InventoryVM inventoryVM;

	// all controlers that inherit from this will have
	// the models that are injected into its constructor
	public void Init( GameModel model, PlayerStatusModel psModel, InventoryVM ivm )
	{

		gameModel = model;
		playerStatusModel = psModel;
		inventoryVM = ivm;

		OnInitComplete();
	}


	protected virtual void OnInitComplete()
	{
		// this method will be overriden by the controllers
		// that are inheriting this base controller class
	}
}

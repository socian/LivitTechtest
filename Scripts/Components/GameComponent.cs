using UnityEngine;
using System.Collections;


public class GameComponent : MonoBehaviour 
{
	// this event will be fired when the player picks up an item
	public delegate void OnItemPickedHandler(Item i);
	public event OnItemPickedHandler OnItemPicked;

	// this event will be fired every time the player drags the right
	// inventory item into the quest
	public delegate void OnItemAcceptedByQuestHandler(string itemRefId);
	public event OnItemAcceptedByQuestHandler OnItemAcceptedByQuest;

	// this events is fired every time a new step is 
	// initialized
	public delegate void OnStepStartHandler(Step s);
	public event OnStepStartHandler OnStepStart;

	// level View Mode
	LevelVM levelVM;

	// level step view model
	StepVM stepVM;
	
	int currStep;
	GameObject currStepGO;
	
	public string CurrentStep {get;set;}

	public void Init( LevelVM lVM, StepVM svm = null )
	{
		levelVM = lVM;
		currStep = 0;
		if(svm == null) 
		{
			// if no step view model is passed in to the init method
			// the game will start the first step of the level
			InitStep ( levelVM.GetFirstStepVM() );
		}
		else 
		{
			// if a step view model is passed into the Init method
			// then the given step will be initialized.
			InitStep ( svm );
		}
	}

	// shows the loading screen everytime the player loads
	// the saved player status
	public void ShowLoadingPrefab()
	{
		Destroy (currStepGO);
		GameObject prefabGO = (GameObject) Instantiate(Resources.Load ("Prefabs/Steps/StepLoading"));
		prefabGO.transform.parent = transform;
		prefabGO.transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		currStepGO = prefabGO;
	}

	// intialize a new step level
	void InitStep( StepVM svm )
	{
		// remove the current step view from the tree
		if(currStepGO != null)
		{
			Destroy( currStepGO );
		}
		
		stepVM = svm;
		
		CurrentStep = stepVM.Step.Id;

		// load the step prefab and display it
		string prefab = stepVM.Step.Prefab;
		GameObject go = (GameObject) Instantiate( Resources.Load (prefab) );
		
		
		go.transform.parent = transform;
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.zero;

		// init the quests of teh level step 
		if( stepVM.Step.QuestList != null ) InitStepQuests();

		// init the items of the level step
		if( stepVM.Step.ItemList != null) InitStepItems();
		
		currStepGO = go;

		// fire the event to notify that a new step has been initialized
		if(OnStepStart != null) OnStepStart( stepVM.Step );

	}

	void InitStepQuests()
	{

		for(int i=0; i<stepVM.Step.QuestList.Length; i++) 
		{
			Quest q = stepVM.Step.QuestList[i];
			GameObject questGO = GameObject.Find(q.Placeholder);

			// show an error if no game object with the given name was found
			if(questGO == null)
			{
				Debug.LogError("Could not find the game object " + q.Placeholder);
				return;
			}

			// if the quest doesn't have an accepted item list then the click
			// event is registerd. the quest will be solved by clicking it then.
			if(q.AcceptedItemList == null) 
			{
				// register a click event
				UIEventListener.Get (questGO).onClick -= OnQuestClick;
				UIEventListener.Get (questGO).onClick += OnQuestClick;
			}
			// if the quest do have an an accepted item list the the drop
			// event is registered. 
			else 
			{
				// register the drag drop event 
				UIEventListener.Get (questGO).onDrop -= OnQuestDrop;
				UIEventListener.Get (questGO).onDrop += OnQuestDrop;
			}
		}

	}

	void InitStepItems()
	{

		for(int i=0; i<stepVM.Step.ItemList.Length; i++) 
		{
			//Item item = stepVM.GetItemByIndex(i);

			Item item = stepVM.Step.ItemList[i];

			// get the gameobject that act as the placeholder
			// for the item
			GameObject itemGO = GameObject.Find(item.Placeholder);

			// check if the item is already picked up. this is the case
			// if the player loads a saved game
			if(item.IsPicked) 
			{
				itemGO.collider.enabled = false;
				UISprite sprite = itemGO.GetComponent<UISprite>();
				sprite.alpha = 0;
			}
			else 
			{
				// show an error if the placeholder game object is not found
				if(itemGO == null)
				{
					Debug.LogError("Could not find the game object " + item.Placeholder);
					return;
				}

				// register the click event of the item
				UIEventListener.Get (itemGO).onClick -= OnItemClick;
				UIEventListener.Get (itemGO).onClick += OnItemClick;
			}
		}

	}

	// handles the event after the player drops an item into the quest game object
	void OnQuestDrop(GameObject questPlaceholder, GameObject droppedItem)
	{
		// Workaround since Unity always adds the "(Clone)" string to the game objects name
		string droppedItemRefID = droppedItem.name.Replace("(Clone)", "");
		if( stepVM.IsAcceptedByQuest(questPlaceholder.name, droppedItemRefID) ) 
		{
			Quest quest = stepVM.GetQuest(questPlaceholder.name);
			quest.IsSolved = true;
			
			if(OnItemAcceptedByQuest != null) OnItemAcceptedByQuest( droppedItemRefID );
			
			// quest is solved. go to the quest.OnSolved step
			if(quest.OnSolved != null || quest.OnSolved != "") 
			{
				StepVM svm = levelVM.GetStepVM( quest.OnSolved );
				InitStep( svm );
			}
		}
	}

	// handles the event if the player clicks the quest game object
	void OnQuestClick(GameObject go)
	{

		string placeholderName = go.name;
		Quest quest = stepVM.GetQuest(placeholderName);
		quest.IsSolved = true;
		
		// quest is solved. go to the quest.OnSolved step
		if(quest.OnSolved != null || quest.OnSolved != "") 
		{
			StepVM svm = levelVM.GetStepVM( quest.OnSolved );
			InitStep( svm );
		}
	}
	// handles the event if the player clicks on an item
	void OnItemClick(GameObject go)
	{
		string placeholderName = go.name;
		Item item = stepVM.GetItem(placeholderName);
		item.IsPicked = true;

		// if the items onpicked attribute is set then
		// initialize the next step
		if(item.OnPicked != null && item.OnPicked != "") 
		{
			StepVM svm = levelVM.GetStepVM( item.OnPicked );
			InitStep( svm );
		}
		// check if the step is complete after clicking an item
		// since some steps will be set to complete if all the items
		// inside the step is picked up
		else if(stepVM.StepComplete) 
		{
			StepVM svm = levelVM.GetStepVM( stepVM.Step.OnComplete );
			InitStep( svm );
		}
		if( OnItemPicked != null ) OnItemPicked(item);
		
		go.collider.enabled = false;
		UISprite sprite = go.GetComponent<UISprite>();
		sprite.alpha = 0;
	}
}

using UnityEngine;
using System.Collections;

public class ToolbarController : Controller 
{
	// ref to the toolbar view component
	public ToolbarComponent toolbarComponent;

	override protected void OnInitComplete()
	{
		// register the toolbar view component events
		toolbarComponent.OnToolbarClick -= OnToolbarButtonClick;
		toolbarComponent.OnToolbarClick += OnToolbarButtonClick;

		// initialize the toolbar view component
		toolbarComponent.Init();
	}

	void OnToolbarButtonClick(string command)
	{
		if(command == "save")
		{
			// tell the player status model to save its state
			playerStatusModel.Save();
		}
		else if(command == "load") 
		{
			// tell the player status model to load its saved data
			// the player status model will notify en event after
			// the saved data is loaded. 
			playerStatusModel.Load();
		}
	}
}


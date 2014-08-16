using UnityEngine;
using System.Collections;

public class ToolbarComponent : MonoBehaviour 
{

	// this event is fired every time a toolbar buton, either the save
	// or the load button, is clicked
	public delegate void OnToolbarClickHandler(string command);
	public event OnToolbarClickHandler OnToolbarClick;

	// ref to the buttons
	public GameObject buttonLoad;
	public GameObject buttonSave;

	public void Init()
	{
		UIEventListener.Get ( buttonLoad ).onClick -= OnButtonClickLoad;
		UIEventListener.Get ( buttonLoad ).onClick += OnButtonClickLoad;

		UIEventListener.Get ( buttonSave ).onClick -= OnButtonClickSave;
		UIEventListener.Get ( buttonSave ).onClick += OnButtonClickSave;
	}

	void OnButtonClickLoad(GameObject go)
	{
		// fire the "OnToolbarClick" event with a "load" command param
		if(OnToolbarClick != null) OnToolbarClick("load");
	}

	void OnButtonClickSave(GameObject go)
	{
		// fire the "OnToolbarClick" event with a "save" command param
		if(OnToolbarClick != null) OnToolbarClick("save");
	}
}

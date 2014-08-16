using UnityEngine;
using System.Collections;

public class InventoryController : Controller 
{

	public InventoryComponent inventoryComponent;

	override protected void OnInitComplete()
	{
		// start the inventory view component
		inventoryComponent.Init( inventoryVM );
	}
}




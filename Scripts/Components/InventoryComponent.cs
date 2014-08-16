using UnityEngine;
using System.Collections;


public class InventoryComponent : MonoBehaviour 
{
	// ref to the inventory item prefab. 
	public GameObject itemPrefab;

	// inventory view model
	InventoryVM inventoryVM;

	Vector3 startPos = Vector3.zero;

	public void Init( InventoryVM ivm )
	{
		// register th inventory view model events
		// in order to update the displays of the items
		// everytime there is a change inside the 
		// inventory view model

		inventoryVM = ivm;
		inventoryVM.OnItemsChanged -= OnItemChanged;
		inventoryVM.OnItemsChanged += OnItemChanged;
	}

	void OnItemChanged()
	{
		// remove all items that are listed in the
		// inventory panel
		ClearItems();

		// get the data from inventory view model that is needed
		// to display the view items. such as item amount, item name
		// and item sprite name.

		ItemInfo[] displayInfos = inventoryVM.ItemInfos;
		Vector3 pos = startPos;

		// display the inventory items
		for(int i=0; i<displayInfos.Length; i++) 
		{
			ItemInfo itemDI = displayInfos[i];

			// instantiate the inventory item prefab and fill it with
			// data
			GameObject itemDisplayGO = (GameObject) Instantiate(itemPrefab);
			itemDisplayGO.transform.parent = transform;
			itemDisplayGO.transform.localScale = Vector3.one;
			itemDisplayGO.transform.localPosition = pos;

			// get the inventory item which provides the public variables
			// to the labelAmount, labelSprite and the sprite
			InventoryItem invItem = itemDisplayGO.GetComponent<InventoryItem>();

			invItem.labelName.text = itemDI.Item.Name;
			invItem.labelAmount.text = "" + itemDI.Amount;
			invItem.sprite.spriteName = itemDI.Item.InventorySprite;

			// set the name of the gameobject equal to the item id
			// for dragging purposes since this object will be cloned everytime
			// it is dragged and destroyed after it is dropped. 

			// we need the id value inside the drop event handler
			invItem.sprite.gameObject.name = itemDI.Item.Id;

			pos += new Vector3(128,0,0);
		}
	}

	void ClearItems()
	{
		// simply destroy all child gameobjects
		for(int i=0; i<transform.childCount; i++) 
		{
			Transform t = transform.GetChild(i);
			Destroy(t.gameObject);
		}
	}
}




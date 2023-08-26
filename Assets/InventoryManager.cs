using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField] InventoryItem[] items;

	int selectedItemIndex = 0;
	void Start()
	{
		
	}


	void Update()
	{
		if(Input.mouseScrollDelta.y != 0){
			ScrollInventory((int)Input.mouseScrollDelta.y);
		}
	}
	void ScrollInventory(int delta){
		items[selectedItemIndex].Unequip();
		selectedItemIndex += delta;
		if(selectedItemIndex < 0){
			selectedItemIndex = (items.Length+1) - selectedItemIndex;
		}
		selectedItemIndex = selectedItemIndex % items.Length;
		items[selectedItemIndex].Equip();
	}
}

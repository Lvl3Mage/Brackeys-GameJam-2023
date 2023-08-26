using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
	protected bool equipped;
	public void Equip(){
		equipped = true;
		OnEquip();
	}
	public void Unequip(){
		equipped = false;
		OnUnequip();
	}
	protected virtual void OnEquip(){}
	protected virtual void OnUnequip(){}
}

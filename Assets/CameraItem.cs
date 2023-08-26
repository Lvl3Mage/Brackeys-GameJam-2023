using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraItem : InventoryItem
{
	float cooldownLeft = 0;
	[SerializeField] float cooldown;
	[SerializeField] GameObject sprite;
	[SerializeField] Animator animator;
	protected override void OnEquip(){
		sprite.SetActive(true);
		PhotoManager.instance.Toggle(true);
	}
	protected override void OnUnequip(){
		sprite.SetActive(false);
		PhotoManager.instance.Toggle(false);
	}
	void Update(){
		cooldownLeft -= Time.deltaTime;
		cooldownLeft = Mathf.Max(cooldownLeft,0);

		if(!equipped){return;}
		if(Input.GetMouseButtonDown(0) && cooldownLeft <= 0){
			StartCoroutine(TakePhoto());
		}
	}
	bool flashOn = false;
	void FlashOn(){
		flashOn = true;
	}
	void FlashOff(){
		flashOn = false;
	}

	IEnumerator TakePhoto(){
		cooldownLeft = cooldown;
		animator.SetTrigger("Flash");
		while(!flashOn){
			yield return null;
		}
		PhotoManager.instance.CapturePhoto();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class FlashlightItem : InventoryItem
{
	[SerializeField] GameObject sprite;
	[SerializeField] Transform flash;
	[SerializeField] Light2D light;
	protected override void OnEquip(){
		sprite.SetActive(true);
		light.enabled = true;
	}
	protected override void OnUnequip(){
		sprite.SetActive(false);
		light.enabled = false;
	}
	void Update(){

		if(!equipped){return;}
		Vector2 targetDelta = WorldCamera.GetWorldMousePos() - (Vector2)transform.position;
		float targetAngle = -Mathf.Atan2(targetDelta.x,targetDelta.y)*Mathf.Rad2Deg - 90f;	
		transform.rotation = Quaternion.Euler(0,0,targetAngle);	


		FlashlightConfig stats = ShopManager.instance.flashLightStats.value;

		light.transform.localScale = new Vector3(stats.reach,stats.size,light.transform.transform.localScale.z);
		light.transform.localPosition = new Vector3(-stats.reach,light.transform.localPosition.y,light.transform.localPosition.z);
		light.intensity = stats.intensity;
	}
}
[System.Serializable]
public struct FlashlightConfig
{
	public float reach;
	public float intensity;
	public float size;
}

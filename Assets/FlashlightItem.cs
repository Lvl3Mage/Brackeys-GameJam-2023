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

		light.intensity = stats.intensity;
		light.pointLightOuterAngle = stats.outerAngle;
		light.pointLightInnerAngle = stats.innerAngle;
		light.pointLightOuterRadius = stats.outerRadius;
		light.pointLightInnerRadius = stats.innerRadius;
	}
}
[System.Serializable]
public struct FlashlightConfig
{
	public float intensity;
	public float innerRadius;
	public float outerRadius;
	public float innerAngle;
	public float outerAngle;
}

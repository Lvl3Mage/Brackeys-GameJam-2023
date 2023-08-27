using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraFlashController : MonoBehaviour
{
	[SerializeField] Transform flash;
	[SerializeField] Light2D light;
	void Start()
	{
		
	}


	void Update()
	{
		Vector2 targetDelta = PhotoManager.instance.transform.position - transform.position;
		float targetAngle = -Mathf.Atan2(targetDelta.x,targetDelta.y)*Mathf.Rad2Deg - 90f;	
		transform.rotation = Quaternion.Euler(0,0,targetAngle);	


		PhotoCameraConfig stats = ShopManager.instance.cameraStats.value;
		light.pointLightOuterAngle = stats.flashOuterSize;
		light.pointLightInnerAngle = stats.flashInnerSize;
		light.pointLightOuterRadius = stats.flashRange;
		light.pointLightInnerRadius = stats.flashInnerRadius;
		// light.falloffIntensity = stats.flashFalloff;
	}
}
/*
[System.Serializable]
public struct PhotoCameraConfig
{
	public float reach;
	public float zoom;
	public float speed;
	public float flashRange;
	public float flashInnerSize;
	public float flashOuterSize;
	public float flashFalloff;
	public Sprite frameSprite;
}
*/
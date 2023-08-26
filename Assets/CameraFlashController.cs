using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlashController : MonoBehaviour
{
	[SerializeField] Transform flash;
	void Start()
	{
		
	}


	void Update()
	{
		Vector2 targetDelta = PhotoManager.instance.transform.position - transform.position;
		float targetAngle = -Mathf.Atan2(targetDelta.x,targetDelta.y)*Mathf.Rad2Deg - 90f;	
		transform.rotation = Quaternion.Euler(0,0,targetAngle);	


		PhotoCameraConfig stats = ShopManager.instance.cameraStats.value;

		flash.transform.localScale = new Vector3(flash.transform.localScale.x,stats.flashSize,flash.transform.localScale.z);
	}
}

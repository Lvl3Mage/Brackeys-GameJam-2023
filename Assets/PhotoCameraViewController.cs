using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCameraViewController : MonoBehaviour
{
	[SerializeField] Camera photoCamera;
	[SerializeField] Transform cameraViewVisual;
	[SerializeField] SpriteRenderer frameSR;
	void Start()
	{
		
	}


	void FixedUpdate()
	{
		PhotoCameraConfig stats = ShopManager.instance.cameraStats.value;
		photoCamera.orthographicSize = stats.zoom;
		cameraViewVisual.localScale = Vector3.one*stats.zoom;
		Vector2 cursorPos = WorldCamera.GetWorldMousePos();
		Vector2 playerPos = PlayerInfo.GetPlayerPosition();
		Vector2 playerDelta = cursorPos - playerPos;
		playerDelta = Vector2.ClampMagnitude(playerDelta, stats.reach);


		Vector2 targetPos = playerPos + playerDelta;

		transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z), stats.speed*Time.fixedDeltaTime);
		frameSR.sprite = stats.frameSprite;
	}
}
[System.Serializable]
public struct PhotoCameraConfig
{
	public float reach;
	public float zoom;
	public float speed;
	public float flashRange;
	public float flashSize;
	public Sprite frameSprite;
}

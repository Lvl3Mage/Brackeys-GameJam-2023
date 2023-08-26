using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCameraViewController : MonoBehaviour
{
	[SerializeField] Camera photoCamera;
	[SerializeField] Transform cameraViewVisual;
	[SerializeField] float maxPlayerDistance;
	[SerializeField] float cameraSize;
	[SerializeField] float movementSpeed;
	void Start()
	{
		
	}


	void FixedUpdate()
	{
		photoCamera.orthographicSize = cameraSize;
		cameraViewVisual.localScale = Vector3.one*cameraSize;
		Vector2 cursorPos = WorldCamera.GetWorldMousePos();
		Vector2 playerPos = PlayerInfo.GetPlayerPosition();
		Vector2 playerDelta = cursorPos - playerPos;
		playerDelta = Vector2.ClampMagnitude(playerDelta, maxPlayerDistance);


		Vector2 targetPos = playerPos + playerDelta;

		transform.position = Vector2.Lerp(transform.position, targetPos, movementSpeed*Time.fixedDeltaTime);
	}
}

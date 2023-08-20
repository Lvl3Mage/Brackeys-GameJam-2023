using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCamera : MonoBehaviour
{
	static WorldCamera instance;
	Camera camera;
	void Awake()
	{
		camera = GetComponent<Camera>();
		if(instance != null){
			Debug.LogError("An instance of WorldCamera already exists!");
			Destroy(this);
			return;
		}
		instance = this;
	}

	
	Vector2 getWorldMousePos(){
		Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
		return worldPosition;
	}
	public static Vector2 GetWorldMousePos(){
		return instance.getWorldMousePos();
	}
	Camera getCamera(){
		return camera;
	}
	public static Camera GetCamera(){
		return instance.getCamera();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoTarget : MonoBehaviour
{
	[SerializeField] PhotoTargetType type;
	bool active = true;
	void Start()
	{
		
	}


	void Update()
	{
		
	}
	public void DisableTarget(){
		active = false;
	}
	public bool isActive(){
		return active;
	}
	public PhotoTargetType GetType(){
		return type;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoTarget : MonoBehaviour
{
	[SerializeField] PhotoTargetType type;
	void Start()
	{
		
	}


	void Update()
	{
		
	}
	public PhotoTargetType GetType(){
		return type;
	}
}

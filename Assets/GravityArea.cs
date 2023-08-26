using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArea : MonoBehaviour
{
	[SerializeField] Vector2 acceleration;
	void OnTriggerStay2D(Collider2D col){
		Rigidbody2D rb = col.attachedRigidbody;
		// Debug.Log(rb);
		if(rb == null){
			return;
		}
		rb.velocity += acceleration*Time.fixedDeltaTime;
	}
	void Start()
	{
		
	}


	void Update()
	{
		
	}
}

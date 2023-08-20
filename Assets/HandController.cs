using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
	[SerializeField] Rigidbody2D rb;
	[SerializeField] Rigidbody2D bodyRB;
	[SerializeField] float force;

	void Start()
	{
		
	}


	void Update()
	{
		Vector2 cursorPos = WorldCamera.GetWorldMousePos();
		Vector2 cursorDelta = cursorPos - (Vector2)transform.position;
		rb.AddForceAtPosition(cursorDelta.normalized*force*Time.deltaTime, transform.position + transform.forward*0.5f, ForceMode2D.Impulse);
		bodyRB.AddForce(-cursorDelta.normalized*force*Time.deltaTime, ForceMode2D.Impulse);
	}
}

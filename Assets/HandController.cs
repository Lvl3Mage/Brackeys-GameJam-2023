using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
	[SerializeField] Rigidbody2D rb;
	[SerializeField] Rigidbody2D bodyRB;
	[SerializeField] float maxForce;
	[SerializeField] float maxForceDistance;

	void Start()
	{
		
	}


	void Update()
	{
		Vector2 cursorPos = WorldCamera.GetWorldMousePos();
		Vector2 cursorDelta = cursorPos - (Vector2)transform.position;
		float curForce = Mathf.Clamp01(cursorDelta.magnitude/maxForceDistance)*maxForce;
		rb.AddForceAtPosition(cursorDelta.normalized*curForce*Time.deltaTime, transform.position - transform.right*0.5f, ForceMode2D.Impulse);
		bodyRB.AddForce(-cursorDelta.normalized*curForce*Time.deltaTime, ForceMode2D.Impulse);
	}
}

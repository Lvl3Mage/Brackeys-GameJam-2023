using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverController : MonoBehaviour
{
	[SerializeField] Rigidbody2D rb;
	[Header("Rotation Settings")]
	[SerializeField] float angularAcceleration;
	[SerializeField] float angularSpeed;
	[SerializeField] float maxAngularSpeed;
	[SerializeField] Vector2 localForwardVector;
	[Header("Movement Settings")]
	[SerializeField] float linearAcceleration;
	[SerializeField] float slowSpeed,fastSpeed;
	[Space]
	[SerializeField] [Range(0.0f, 1.0f)] float speedSwitchStartThreshold, speedSwitchEndThreshold;
	[Space]
	[Header("Animation Settings")]
	[SerializeField] HingeOscilator[] oscilators;
	[SerializeField] float animationSpeed = 1;
	void Start()
	{
		
	}


	void Update()
	{

		//Rotation Handling
		Vector2 cursorPos = WorldCamera.GetWorldMousePos();
		Vector2 cursorDelta = cursorPos - (Vector2)transform.position;

		float targetAngle = Mathf.Atan2(cursorDelta.x,cursorDelta.y)*Mathf.Rad2Deg;

		Vector2 facingDir = transform.TransformDirection(localForwardVector).normalized;
		float facingAngle = Mathf.Atan2(facingDir.x,facingDir.y)*Mathf.Rad2Deg;


		float targetAngularVelocity = (Mathf.DeltaAngle(targetAngle,facingAngle)/180f)*angularSpeed;
		targetAngularVelocity = Mathf.Clamp(targetAngularVelocity,-maxAngularSpeed,maxAngularSpeed);
		rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, targetAngularVelocity, Time.deltaTime*angularAcceleration);
		
		//MovementHandling
		Vector2 inputAxis = GetAxis();
		// Vector2 aimDir = cursorDelta.normalized;
		Vector2 targetDir = inputAxis;
		// Vector2 targetDir = (-Vector2.Perpendicular(aimDir)*inputAxis.x + aimDir*inputAxis.y).normalized;
		float allignment = (Vector2.Dot(facingDir,targetDir) + 1) / 2;
		float transformedAllignment = Mathf.Clamp01(MathUtils.TransformRange(allignment,speedSwitchStartThreshold,speedSwitchEndThreshold,0,1));
		float movementSpeed = Mathf.Lerp(slowSpeed, fastSpeed, transformedAllignment);
		rb.velocity = Vector2.Lerp(rb.velocity, targetDir*movementSpeed, Time.deltaTime*linearAcceleration);

		foreach(HingeOscilator oscilator in oscilators){
			oscilator.timeScale = (movementSpeed/fastSpeed) * inputAxis.magnitude*animationSpeed;
		}
	}
	Vector2 GetAxis(){
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
	}
}

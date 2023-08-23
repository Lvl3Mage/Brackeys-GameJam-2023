using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearBehaviour : FishBehaviour
{
	[Header("Fear Settings")]
	[SerializeField] float fearAccelerationMaxInfluence;
	[SerializeField] float fearAccelerationActivationPoint;
	[SerializeField] float fearAccelerationActivationPower;
	[SerializeField] float fearAccelerationHalfStrengthDistance;
	[Space]
	[SerializeField] float fearApproachInfluence;
	[Space]
	[SerializeField] float fearProximityInfluenceMax;
	[SerializeField] float fearProximityInfluenceHalfDistance;
	[Space]
	[SerializeField] float fearDecaySpeed = 1;
	[SerializeField] float fearMinValue = 0;
	[SerializeField] float fearMaxValue = 2;
	[SerializeField] [Range(0,1)] float fearActiveBonus;
	[SerializeField] float preferenceMultiplier = 1;
	[Header("Movement Settings")]
	[SerializeField] float angularSpeed = 800;
	[SerializeField] float maxAngularSpeed = 300;
	[SerializeField] float angularAcceleration = 3;
	[Space]
	[SerializeField] float linearSpeed = 8;
	[SerializeField] float linearAcceleration = 10;
	[SerializeField] float trajectorySharpness = 1.5f;
	[Header("Collision Avoidance Settings")]
	[SerializeField] LayerMask pathBlockingLayers;
	[SerializeField] float scanAngleStepSize; 
	[SerializeField] float maxScanAngle;
	[SerializeField] float scanDistance;
	[SerializeField] [Range(0,2)] float obstacleRepulsion;
	[Header("References")]
	[SerializeField] Rigidbody2D rb;
	float fearLevel;
	void Start(){
	}
	void Update(){
		Vector2 playerDelta = (Vector2)transform.position - PlayerInfo.GetPlayerPosition();
		fearLevel += Vector2.Dot(PlayerInfo.GetAverageVelocityVector(),playerDelta.normalized)*fearApproachInfluence;
		fearLevel += (fearProximityInfluenceMax/(playerDelta.magnitude * 1/fearProximityInfluenceHalfDistance + 1)) * Time.deltaTime;

		
		ApplyAcceleration();

		fearLevel -= fearDecaySpeed*Time.deltaTime;
		fearLevel = Mathf.Max(fearLevel,fearMinValue);
		fearLevel = Mathf.Min(fearLevel,fearMaxValue);
		// Debug.Log(fearLevel);
	}
	void ApplyAcceleration(){
		float acceleration = PlayerInfo.GetMaxVelocityMagnitude();
		// Debug.Log(acceleration);
		float activationValue = acceleration / (fearAccelerationActivationPoint*2);
		activationValue = Mathf.Clamp01(activationValue);
		float power = fearAccelerationActivationPower*2*fearAccelerationActivationPoint;
		float activationValueElevated = Mathf.Pow(activationValue, power);

		float computedValue = (activationValueElevated/(activationValueElevated+Mathf.Pow(1-activationValue,power)))*fearAccelerationMaxInfluence;
		// if(acceleration > fearAccelerationActivationPoint){
		// 	Debug.Log(fearLevel + " : " + computedValue);

		// }
		Vector2 playerDelta = (Vector2)transform.position - PlayerInfo.GetPlayerPosition();

		float distanceMultiplier = 1/(playerDelta.magnitude*(1/fearProximityInfluenceHalfDistance)+1);
		fearLevel += computedValue*distanceMultiplier;
	}
	public override float GetPreferenceValue(){
		return fearLevel*preferenceMultiplier;
	}
	public override void UpdateBehaviour(){
		Escape();
	}
	void Escape(){
		fearLevel+=fearActiveBonus*Time.deltaTime;
		Vector2 targetDelta = (Vector2)transform.position - PlayerInfo.GetPlayerPosition();//from player to fish
		float targetAngle = Mathf.Atan2(targetDelta.x,targetDelta.y)*Mathf.Rad2Deg;

		Vector2 facingDir = transform.right;
		float facingAngle = Mathf.Atan2(facingDir.x,facingDir.y)*Mathf.Rad2Deg;


		//Collision avoidance
		targetAngle = ScanValidAngle(targetAngle);


		//Angular velocity calculation
		float targetAngularVelocity = (Mathf.DeltaAngle(targetAngle,facingAngle)/180f)*angularSpeed;
		targetAngularVelocity = Mathf.Clamp(targetAngularVelocity,-maxAngularSpeed,maxAngularSpeed);
		rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, targetAngularVelocity, Time.deltaTime*angularAcceleration);


		//Linear velocity calculation
		float speed = linearSpeed*Mathf.Pow((Vector2.Dot(facingDir, targetDelta.normalized) + 1)/2,trajectorySharpness);
		rb.velocity = Vector2.Lerp(rb.velocity, facingDir*speed,linearAcceleration*Time.deltaTime);
	}
	float ScanValidAngle(float initialAngle){
		if(!ScanObstaclesInDirection(DirFromAngle(initialAngle), scanDistance, pathBlockingLayers)){
			return initialAngle;
		}
		int steps = (int)Mathf.Round(maxScanAngle/scanAngleStepSize);
		for(int i = 1; i < steps; i++){
			float variationAngle = i*scanAngleStepSize;
			if(!ScanObstaclesInDirection(DirFromAngle(initialAngle + variationAngle), scanDistance, pathBlockingLayers)){
				return initialAngle + variationAngle*obstacleRepulsion;
			}
			else if(!ScanObstaclesInDirection(DirFromAngle(initialAngle - variationAngle), scanDistance, pathBlockingLayers)){
				return initialAngle - variationAngle*obstacleRepulsion;
			}
		}
		return initialAngle;
	}
	Vector2 DirFromAngle(float angle){
		return new Vector2(Mathf.Sin(angle*Mathf.Deg2Rad), Mathf.Cos(angle*Mathf.Deg2Rad));
	}
	bool ScanObstaclesInDirection(Vector2 dir, float distance, LayerMask obstacleMask){
		RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distance, obstacleMask);
		Debug.DrawRay(transform.position, dir.normalized*distance, hit.collider==null? Color.green : Color.red);
		return hit.collider != null;
	}
}

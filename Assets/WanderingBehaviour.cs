using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingBehaviour : FishBehaviour
{
	[SerializeField] Rigidbody2D rb;
	[SerializeField] float behaviourStrength;

	[Header("Target settings")]
	[SerializeField] float minTargetDistance;
	[SerializeField] float maxTargetDistance;
	[SerializeField] float minTargetSelectionRange;
	[SerializeField] float maxTargetSelectionRange;

	[Header("Rotation settings")]
	[SerializeField] float angularAcceleration;
	[SerializeField] float angularSpeed;

	[Header("Target movement settings")]
	[SerializeField] float targetAcceleration;
	[SerializeField] float slowdownStartDistance;
	[SerializeField] float slowdownEndDistance;
	[SerializeField] float maxSpeed;
	[SerializeField] float minSpeed;
	[SerializeField] float trajectorySharpness = 1;
	[SerializeField] LayerMask pathBlockingLayers;

	[Header("Rotation Noise Settings")]
	[SerializeField] float noiseMagnitude;
	[SerializeField] float noiseScale;
	[SerializeField] float scrollSpeed;

	[Header("Collision Avoidance settings")]
	[SerializeField] float scanAngleStep; 
	[SerializeField] float maxScanAngle; 

	void Start(){
		SelectNewTarget();
	}
	public override float GetScore(){
		return behaviourStrength;
	}
	Vector2 target;
	public override void UpdateBehaviour(){
		Vector2 targetDelta = target - (Vector2)transform.position;
		float distToTarget = targetDelta.magnitude;
		if(distToTarget > maxTargetDistance || distToTarget < minTargetDistance){
			SelectNewTarget();
			targetDelta = target - (Vector2)transform.position;
			distToTarget = targetDelta.magnitude;
		}
		float targetAngle = Mathf.Atan2(targetDelta.x,targetDelta.y)*Mathf.Rad2Deg;

		Vector2 facingDir = transform.right;
		float facingAngle = Mathf.Atan2(facingDir.x,facingDir.y)*Mathf.Rad2Deg;

		Vector2 noiseCoords = (Vector2)transform.position/noiseScale;
		float rotationOffset = (Mathf.PerlinNoise(noiseCoords.x + Time.time*scrollSpeed, noiseCoords.y + Time.time*scrollSpeed)*2 - 1) * noiseMagnitude;

		Debug.Log(rotationOffset);
		targetAngle += rotationOffset;

		//SCAN CALL HERE

		float targetAngularVelocity = (Mathf.DeltaAngle(targetAngle,facingAngle)/180f)*angularSpeed - rotationOffset;
		rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, targetAngularVelocity, Time.deltaTime*angularAcceleration);

		float speed = Mathf.Lerp(minSpeed,maxSpeed,MathUtils.TransformRange(distToTarget, slowdownEndDistance, slowdownStartDistance, 0, 1));
		speed *= Mathf.Pow((Vector2.Dot(facingDir, targetDelta.normalized) + 1)/2,trajectorySharpness);
		rb.velocity = Vector2.Lerp(rb.velocity, facingDir*speed,targetAcceleration*Time.deltaTime);

	}
	// float ScanValidAngle(float initialAngle){
	// 	//calculate steps by maxScanAngle/scanAngleStep
	// 	//then check both ways starting from the intial angle until you find a free one or run out of steps
	// 	// for(int i = 0; i< )
	// }
	void SelectNewTarget(){
		Vector2 point = GenerateTarget();
		while(true){
			Vector2 pointDelta = point-(Vector2)transform.position;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, pointDelta, pointDelta.magnitude, pathBlockingLayers);
			if(hit.collider == null){//not blocked
				break;
			}
			point = GenerateTarget();
		}
		target = point;

	}
	Vector2 GenerateTarget(){
		Vector2 randPoint = Random.insideUnitCircle;
		if(randPoint.sqrMagnitude == 0.0){
			randPoint = Vector2.up;
		}
		randPoint = randPoint.normalized;
		randPoint *= Random.Range(minTargetSelectionRange,maxTargetSelectionRange);
		return (Vector2)transform.position + randPoint;
	}
	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(target, minTargetDistance);
	}
}
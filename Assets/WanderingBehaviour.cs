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
	[SerializeField] float maxAngularSpeed;

	[Header("Target movement settings")]
	[SerializeField] float targetAcceleration;
	[SerializeField] float slowdownStartDistance;
	[SerializeField] float slowdownEndDistance;
	[SerializeField] float maxSpeed;
	[SerializeField] float minSpeed;
	[SerializeField] float trajectorySharpness = 1;
	[SerializeField] LayerMask pathBlockingLayers;
	[SerializeField] float targetMinObstacleDistance;
	[SerializeField] float targetMaxLifetime;

	[Header("Rotation Noise Settings")]
	[SerializeField] float noiseMagnitude;
	[SerializeField] float noiseScale;
	[SerializeField] float scrollSpeed;

	[Header("Collision Avoidance settings")]
	[SerializeField] float scanAngleStepSize; 
	[SerializeField] float maxScanAngle;
	[SerializeField] float scanDistance;
	[SerializeField] [Range(0,2)] float obstacleRepulsion;

	void Start(){
		SelectNewTarget();
	}
	public override float GetPreferenceValue(){
		return behaviourStrength;
	}
	float targetSpawnTime;
	Vector2 target;
	public override void UpdateBehaviour(){

		Vector2 targetDelta = target - (Vector2)transform.position;
		float distToTarget = targetDelta.magnitude;
		if(distToTarget > maxTargetDistance || distToTarget < minTargetDistance || (Time.time - targetSpawnTime) > targetMaxLifetime){
			SelectNewTarget();
		}
		TravelToTarget();

	}
	void TravelToTarget(){

		Vector2 targetDelta = target - (Vector2)transform.position;
		float targetAngle = Mathf.Atan2(targetDelta.x,targetDelta.y)*Mathf.Rad2Deg;

		Vector2 facingDir = transform.right;
		float facingAngle = Mathf.Atan2(facingDir.x,facingDir.y)*Mathf.Rad2Deg;


		//Noise
		Vector2 noiseCoords = (Vector2)transform.position/noiseScale;
		float rotationOffset = (Mathf.PerlinNoise(noiseCoords.x + Time.time*scrollSpeed, noiseCoords.y + Time.time*scrollSpeed)*2 - 1) * noiseMagnitude;

		targetAngle += rotationOffset;

		//Collision avoidance
		targetAngle = ScanValidAngle(targetAngle);


		//Angular velocity calculation
		float targetAngularVelocity = (Mathf.DeltaAngle(targetAngle,facingAngle)/180f)*angularSpeed;
		targetAngularVelocity = Mathf.Clamp(targetAngularVelocity,-maxAngularSpeed,maxAngularSpeed);
		rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, targetAngularVelocity, Time.deltaTime*angularAcceleration);


		//Linear velocity calculation
		float speed = Mathf.Lerp(minSpeed,maxSpeed,MathUtils.TransformRange(targetDelta.magnitude, slowdownEndDistance, slowdownStartDistance, 0, 1));
		speed *= Mathf.Pow((Vector2.Dot(facingDir, targetDelta.normalized) + 1)/2,trajectorySharpness);
		rb.velocity = Vector2.Lerp(rb.velocity, facingDir*speed,targetAcceleration*Time.deltaTime);
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
	void SelectNewTarget(){
		Vector2 point = GenerateTarget();
		int iter = 0;
		while(true){
			Vector2 pointDelta = point-(Vector2)transform.position;
			if(!ScanObstaclesInDirection(pointDelta, pointDelta.magnitude, pathBlockingLayers)){//not blocked
				Collider2D col = Physics2D.OverlapCircle(point, targetMinObstacleDistance, pathBlockingLayers);
				if(col == null){
					break;
				}
			}
			point = GenerateTarget();
			iter++;
			if(iter > 1000){
				Debug.LogWarning("Iteration count exceeded maximum!");
				break;
			}
		}
		targetSpawnTime = Time.time;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class FearBehaviour : FishBehaviour
{

	[Foldout("Misc Fear Settings",true)] 
	[SerializeField] float fearDecaySpeed = 1;
	[SerializeField] float fearMinValue = 0;
	[SerializeField] float fearMaxValue = 2;
	[SerializeField] float fearActiveBonus;
	[SerializeField] float preferenceMultiplier = 1;
	[Foldout("Surprise Fear Settings",true)] 
	[SerializeField] bool enableSurpriseFear;
	[ConditionalField(nameof(enableSurpriseFear))] [SerializeField] float surpriseFearMaxStrength;
	[ConditionalField(nameof(enableSurpriseFear))] [SerializeField] float surpriseFearHalfStrengthDistance;
	[ConditionalField(nameof(enableSurpriseFear))] [SerializeField] float surpriseFearActivationVelocity;
	[ConditionalField(nameof(enableSurpriseFear))] [SerializeField] float surpriseFearActivationSteepness;
	[Space]

	[Foldout("Approach Fear Settings",true)] 
	[SerializeField] bool enableApproachFear;
	[ConditionalField(nameof(enableApproachFear))] [SerializeField] float approachFearStrength;
	[ConditionalField(nameof(enableApproachFear))] [SerializeField] float approachFearHalfStrengthDistance;
	[Space]

	[Foldout("Proximity Fear Settings")]
	[SerializeField] bool enableProximityFear;
	[Foldout("Proximity Fear Settings")]
	[ConditionalField(nameof(enableProximityFear))] [SerializeField] float proximityFearMaxStrength;
	[Foldout("Proximity Fear Settings")]
	[ConditionalField(nameof(enableProximityFear))] [SerializeField] float proximityFearMinStrength;
	[Foldout("Proximity Fear Settings")]
	[ConditionalField(nameof(enableProximityFear))] [SerializeField] float proximityFearHalfStrengthDistance;


	[ReadOnly] [SerializeField] float fearLevel = 0;
	
	[Space]
	[Header("Movement Settings")]
	[SerializeField] float angularSpeed = 800;
	[SerializeField] float maxAngularSpeed = 300;
	[SerializeField] float angularAcceleration = 3;
	[Space]
	[SerializeField] float linearSpeed = 8;
	[SerializeField] float linearAcceleration = 10;
	[SerializeField] float trajectorySharpness = 1.5f;
	[SerializeField] NoiseSampler movementNoise;
	[Header("Collision Avoidance Settings")]
	[SerializeField] LayerMask pathBlockingLayers;
	[SerializeField] float scanAngleStepSize; 
	[SerializeField] float maxScanAngle;
	[SerializeField] float scanDistance;
	[SerializeField] [Range(0,2)] float obstacleRepulsion;
	[Header("References")]
	[SerializeField] Rigidbody2D rb;
	void Start(){
	}
	void Update(){
		Vector2 playerDelta = (Vector2)transform.position - PlayerInfo.GetPlayerPosition();

		if(enableProximityFear){
			ApplyProximityFear(playerDelta);
		}
		if(enableApproachFear){
			ApplyApproachFear(playerDelta);
		}
		if(enableSurpriseFear){
			ApplySurpriseFear(playerDelta);
		}

		fearLevel -= fearDecaySpeed*Time.deltaTime;
		fearLevel = Mathf.Clamp(fearLevel,fearMinValue,fearMaxValue);
		// Debug.Log(fearLevel);
	}
	void ApplyProximityFear(Vector2 playerDelta){
		fearLevel += MathUtils.ValueDecay(playerDelta.magnitude,proximityFearMaxStrength,proximityFearMinStrength,proximityFearHalfStrengthDistance) * Time.deltaTime;
	}
	void ApplyApproachFear(Vector2 playerDelta){
		float approachFactor = Vector2.Dot(PlayerInfo.GetAverageVelocityVector(),playerDelta.normalized); //vel to -vel
		float distanceFactor = MathUtils.ValueDecay(playerDelta.magnitude,1,0,approachFearHalfStrengthDistance);//1-0
		fearLevel += approachFactor*distanceFactor*approachFearStrength*Time.deltaTime;
	}
	void ApplySurpriseFear(Vector2 playerDelta){
		float maxVel = PlayerInfo.GetMaxVelocityMagnitude();
		// Debug.Log(maxVel);
		float activationValue = maxVel / (surpriseFearActivationVelocity*2);
		if(surpriseFearActivationVelocity == 0){//NAN Checking
			activationValue = 1;
		}
		activationValue = Mathf.Clamp01(activationValue);
		float power = surpriseFearActivationSteepness;
		float activationValueElevated = Mathf.Pow(activationValue, power);

		float surpriseFactor = (activationValueElevated/(activationValueElevated+Mathf.Pow(1-activationValue,power)))*surpriseFearMaxStrength;
		if(surpriseFactor != surpriseFactor){
			Debug.LogError($"SurpriseFactor was NaN! activationValue: {activationValue}, activationValueElevated: {activationValueElevated}");
		}
		float distanceFactor = 1/(playerDelta.magnitude*(1/surpriseFearHalfStrengthDistance)+1);
		fearLevel += surpriseFactor*distanceFactor*Time.deltaTime;
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

		targetAngle += movementNoise.SampleAt(transform.position);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class HingeVelocityOscillator : MonoBehaviour
{
	[SerializeField] bool autoAssignHinge = true;
	[ConditionalField(nameof(autoAssignHinge),inverse:true)][SerializeField] HingeJoint2D joint;
	[SerializeField] Rigidbody2D velocityReference;

	[Header("Cycle settings")]
	[SerializeField] float maxFrequencyVelocity;
	[SerializeField] float maxFrequencyAngularVelocity;
	[SerializeField] [Range(0,1)] float angularVelocityInfluence = 0.5f;
	[SerializeField] float maxFrequency;
	[SerializeField] bool initialState;
	[SerializeField] [Range(0,1)] float cycleOffset;

	[Header("Movement settings")]
	[SerializeField] float approachSpeed;
	[SerializeField] float approachForce;
	[SerializeField] float slowdownAngle;
	[SerializeField] Vector2 angleRange;

	[Header("Resting settings")]
	[SerializeField] float restingFrequency;
	[SerializeField] float restingPose;
	[SerializeField] float restingApproachSpeed;
	[SerializeField] float restingForce;

	[Header("Debug Settings")]
	[SerializeField] bool debug;
	[ConditionalField(nameof(debug))] [SerializeField] float simulatedVelocity;
	[ConditionalField(nameof(debug))] [SerializeField] float simulatedAngularVelocity;
	[ConditionalField(nameof(debug))] [SerializeField] float simulatedTargetAngle;
	[ConditionalField(nameof(debug))] [SerializeField] float simulatedFaseTime;
	[ConditionalField(nameof(debug))] [SerializeField] float simulatedFrequency;
	void Start()
	{
		if(autoAssignHinge){
			joint = GetComponent<HingeJoint2D>();
		}
		directionPositive = initialState;
		faseTime = cycleOffset;
	}

	float faseTime = 0;
	bool directionPositive;
	void Update()
	{
		float velocity = velocityReference.velocity.magnitude;
		float angularVelocity = Mathf.Abs(velocityReference.angularVelocity);
		if(debug){
			velocity = simulatedVelocity;
			angularVelocity = simulatedAngularVelocity;
		}
		float velFreq = Mathf.Clamp01(velocity/maxFrequencyVelocity)*maxFrequency;
		float angFreq = Mathf.Clamp01(angularVelocity/maxFrequencyAngularVelocity)*maxFrequency;
		float frequency = velFreq * (1-angularVelocityInfluence) + angFreq * angularVelocityInfluence;
		faseTime += Time.deltaTime*frequency*(directionPositive ? 1 : -1);
		if(faseTime > 1){
			directionPositive = false;
			faseTime = 2 - faseTime;
		}
		else if(faseTime < 0){
			directionPositive = true;
			faseTime = -faseTime;
		}

		float targetAngle = Mathf.Lerp(angleRange.x, angleRange.y, faseTime);
		float currentApproachSpeed = approachSpeed;
		float currentForce = approachForce;

		if(frequency < restingFrequency){
			targetAngle = restingPose;
			currentApproachSpeed = restingApproachSpeed;
			currentForce = restingForce;
		}
		float deltaAngle = Mathf.DeltaAngle(joint.jointAngle,-targetAngle);
		float angleFactor = Mathf.Clamp(deltaAngle/slowdownAngle,-1,1);


		JointMotor2D motor = joint.motor;

		motor.motorSpeed = Mathf.LerpUnclamped(0,currentApproachSpeed,angleFactor);
		motor.maxMotorTorque = currentForce;
		
		joint.motor = motor;
		if(debug){
			simulatedTargetAngle = targetAngle;
			simulatedFaseTime = faseTime;
			simulatedFrequency = frequency;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class HingeOscilator : MonoBehaviour
{

	[SerializeField] HingeJoint2D joint;
	[SerializeField] float strength;
	[SerializeField] float slowdownAngle;
	[SerializeField] float period;
	[SerializeField] bool initialState;
	[SerializeField] Vector2 angleRange;
	[SerializeField] [Range(0,1)] float cycleOffset;
	[HideInInspector] public float timeScale = 1;
	[SerializeField] float restingPose;
	[SerializeField] float restingApproachStrength;
	void Start()
	{
		directionPositive = initialState;
		faseTime = cycleOffset*period*0.5f;
	}

	float faseTime = 0;
	bool directionPositive;
	void Update()
	{

		faseTime += Time.deltaTime*timeScale*(directionPositive ? 1 : -1);
		if(faseTime > period*0.5f){
			directionPositive = false;
			faseTime = period - faseTime;
		}
		else if(faseTime < 0){
			directionPositive = true;
			faseTime = -faseTime;
		}

		float targetAngle = Mathf.Lerp(angleRange.x, angleRange.y, faseTime/(period*0.5f));
		float currentStrength = strength;
		if(timeScale < 0.01f){
			targetAngle = restingPose;
			currentStrength = restingApproachStrength;
		}
		float deltaAngle = Mathf.DeltaAngle(joint.jointAngle,-targetAngle);
		float angleFactor = Mathf.Clamp(deltaAngle/slowdownAngle,-1,1);
		JointMotor2D motor = joint.motor;
		motor.motorSpeed = Mathf.LerpUnclamped(0,currentStrength,angleFactor);
		joint.motor = motor;
	}
}

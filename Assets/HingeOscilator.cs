using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeOscilator : MonoBehaviour
{

	[SerializeField] HingeJoint2D joint;
	[SerializeField] float strength;
	[SerializeField] float period;
	[SerializeField] bool initialState;
	[HideInInspector] public float timeScale = 1;
	void Start()
	{
		highFase = initialState;
	}

	float accumulatedTime = 0;
	bool highFase;
	void Update()
	{
		accumulatedTime += Time.deltaTime*timeScale;
		if(accumulatedTime > period*0.5){
			accumulatedTime = 0;
			highFase = !highFase;
		}
		JointMotor2D motor = joint.motor;
		motor.motorSpeed = strength*timeScale*(highFase ? 1 : -1);
		joint.motor = motor;
	}
}

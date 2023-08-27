using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
	[SerializeField] NoiseSampler driftNoiseX;
	[SerializeField] NoiseSampler driftNoiseY;
	[SerializeField] NoiseSampler rotationNoise;
	[SerializeField] bool angular = false;
	Rigidbody2D rb;
	void Start(){
		rb = GetComponent<Rigidbody2D>();
	}
	void Update(){
		Vector2 drift = new Vector2(driftNoiseX.SampleAt(transform.position), driftNoiseY.SampleAt(transform.position));
		float rotation = rotationNoise.SampleAt(transform.position);
		rb.velocity += drift*Time.deltaTime;
		if(angular){
			rb.angularVelocity = rotation;

		}
	}
}

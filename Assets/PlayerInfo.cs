using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
	static PlayerInfo instance;
	[System.Serializable]
	public class BodyPart{
		[SerializeField] Transform part;
		Vector2 pastPosition;
		Vector2 velocity;
		Vector2 pastVelocity;
		Vector2 acceleration;
		public void ComputeChanges(){
			pastVelocity = velocity;
			velocity = ((Vector2)part.position - pastPosition)/Time.deltaTime;
			acceleration = (velocity - pastVelocity)/Time.deltaTime;
			pastPosition = part.position;
		}
		public Vector2 GetVelocity(){
			return velocity;
		}
		public Vector2 GetAcceleration(){
			return acceleration;
		}
	}
	[SerializeField] BodyPart[] bodyParts;
	Vector2 averageVelocityVector;
	Vector2 averageAccelerationVector;
	float averageVelocityMagnitude;
	float averageAccelerationMagnitude;
	float maxVelocityMagnitude;
	void Awake()
	{
		if(instance != null){
			Debug.LogError("Instance of PlayerInfo already exists!");
			return;
		}
		instance = this;
	}

	void FixedUpdate()
	{
		averageVelocityVector = Vector2.zero;
		averageAccelerationVector = Vector2.zero;
		averageVelocityMagnitude = 0;
		averageAccelerationMagnitude = 0;
		maxVelocityMagnitude = 0;
		foreach(BodyPart part in bodyParts){
			part.ComputeChanges();
			averageVelocityVector += part.GetVelocity();
			averageAccelerationVector += part.GetAcceleration();
			averageVelocityMagnitude += part.GetVelocity().magnitude;
			averageAccelerationMagnitude += part.GetAcceleration().magnitude;
			if(part.GetVelocity().magnitude > maxVelocityMagnitude){
				maxVelocityMagnitude = part.GetVelocity().magnitude;
			}
		}
		// Debug.Log(maxVelocityMagnitude);
		averageVelocityVector /= bodyParts.Length;
		averageAccelerationVector /= bodyParts.Length;
		averageVelocityMagnitude /= bodyParts.Length;
		averageAccelerationMagnitude /= bodyParts.Length;
	}
	public static Vector2 GetAverageVelocityVector(){
		return instance.averageVelocityVector;
	}

	public static Vector2 GetAverageAccelerationVector(){
		return instance.averageAccelerationVector;
	}

	public static float GetAverageVelocityMagnitude(){
		return instance.averageVelocityMagnitude;
	}

	public static float GetAverageAccelerationMagnitude(){
		return instance.averageAccelerationMagnitude;
	}
	public static Vector2 GetPlayerPosition(){
		return instance.transform.position;
	}
	public static float GetMaxVelocityMagnitude(){
		return instance.maxVelocityMagnitude;
	}
	public static bool isInitialized(){
		return instance != null;
	}
}

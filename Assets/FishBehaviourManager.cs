using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviourManager : MonoBehaviour
{
	[SerializeField] FishBehaviour[] behaviors;
	void Start()
	{
		
	}


	void Update()
	{
		behaviors[0].UpdateBehaviour();
	}
}

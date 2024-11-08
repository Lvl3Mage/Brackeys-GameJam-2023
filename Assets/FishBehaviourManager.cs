using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class FishBehaviourManager : MonoBehaviour
{
	[AutoProperty(AutoPropertyMode.Children)] [SerializeField] FishBehaviour[] behaviors;
	void Start()
	{
	}


	void Update()
	{
		float maxPreferenceValue = Mathf.NegativeInfinity;
		FishBehaviour highestPreferenceBehaviour = null;
		foreach(FishBehaviour behavior in behaviors){
			float preference = behavior.GetPreferenceValue();
			if(preference > maxPreferenceValue){
				maxPreferenceValue = preference;
				highestPreferenceBehaviour = behavior;
			}
		}
		highestPreferenceBehaviour.UpdateBehaviour();
	}
}

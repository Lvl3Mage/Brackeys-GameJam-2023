using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class OxygenController : MonoBehaviour
{
	[SerializeField] float oxygenSurfaceRechargeRate;
	[SerializeField] float gracePeriodSurfaceRechargeRate = 5;
	[SerializeField] float surfaceDepth;
	[SerializeField] SliderController slider;
	[SerializeField] Volume drowningVolume;
	[SerializeField] float gracePeriodLength;
	[SerializeField] [Range(0,1)] float drowningStart, drowningEnd;
	float oxygenTime;
	void Start()
	{
		gracePeriodLeft = gracePeriodLength;
		oxygenTime = ShopManager.instance.oxygenDuration.value;
	}

	float gracePeriodLeft;
	void Update()
	{
		if(gracePeriodLeft <= 0){
			Debug.Log("You ded");
			return;
		}
		if(PlayerInfo.GetPlayerPosition().y > surfaceDepth){
			oxygenTime += oxygenSurfaceRechargeRate*Time.deltaTime;
			gracePeriodLeft += gracePeriodSurfaceRechargeRate*Time.deltaTime;
		}
		else{
			oxygenTime -= Time.deltaTime;
		}
		if(oxygenTime <= 0){
			gracePeriodLeft -= Time.deltaTime;
		}
		gracePeriodLeft = Mathf.Clamp(gracePeriodLeft,0,gracePeriodLength);
		oxygenTime = Mathf.Clamp(oxygenTime,0,ShopManager.instance.oxygenDuration.value);
		slider.SetValue(oxygenTime);
		slider.SetRange(0,ShopManager.instance.oxygenDuration.value);


		drowningVolume.weight = Mathf.Clamp01(MathUtils.TransformRange(gracePeriodLeft/gracePeriodLength, drowningStart, drowningEnd, 0, 1));

		
		
	}
}

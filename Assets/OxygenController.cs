using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenController : MonoBehaviour
{
	[SerializeField] float oxygenSurfaceRechargeRate;
	[SerializeField] float surfaceDepth;
	[SerializeField] SliderController slider;
	float oxygenTime;
	void Start()
	{
		
	}


	void Update()
	{
		if(PlayerInfo.GetPlayerPosition().y > surfaceDepth){
			oxygenTime += oxygenSurfaceRechargeRate*Time.deltaTime;
		}
		else{
			oxygenTime -= Time.deltaTime;
		}
		slider.SetValue(oxygenTime);
		slider.SetRange(0,ShopManager.instance.oxygenDuration.value);


	}
}

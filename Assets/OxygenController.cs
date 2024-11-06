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
	[SerializeField] SliderController slider2;
	[SerializeField] Volume drowningVolume;
	[SerializeField] float gracePeriodLength;
	[SerializeField] [Range(0,1)] float drowningStart, drowningEnd;
	[SerializeField] PoppupController oxygenPoppup;
	float oxygenTime;
	void Start()
	{
		gracePeriodLeft = gracePeriodLength;
		oxygenTime = ShopManager.instance.oxygenDuration.value;
	}

	float gracePeriodLeft;
	bool halfO2Notif = false;
	bool quarterO2Notif = false;
	bool noO2Notif = false;
	void Update()
	{
		if(gracePeriodLeft <= 0){
			GameManager.ToggleGameOver();
			return;
		}
		if(PlayerInfo.GetPlayerPosition().y > surfaceDepth){
			oxygenTime += oxygenSurfaceRechargeRate*Time.deltaTime*ShopManager.instance.oxygenDuration.value;
			gracePeriodLeft += gracePeriodSurfaceRechargeRate*Time.deltaTime;
			ReloadWarnings();
		}
		else{
			oxygenTime -= Time.deltaTime;
		}
		DisplayWarnings();
		if(oxygenTime <= 0){
			gracePeriodLeft -= Time.deltaTime;
		}
		gracePeriodLeft = Mathf.Clamp(gracePeriodLeft,0,gracePeriodLength);
		oxygenTime = Mathf.Clamp(oxygenTime,0,ShopManager.instance.oxygenDuration.value);
		slider.SetValue(oxygenTime);
		slider.SetRange(0,ShopManager.instance.oxygenDuration.value);
		slider2.SetValue(oxygenTime);
		slider2.SetRange(0,ShopManager.instance.oxygenDuration.value);


		drowningVolume.weight = Mathf.Clamp01(MathUtils.TransformRange(gracePeriodLeft/gracePeriodLength, drowningStart, drowningEnd, 0, 1));

		
		
	}
	void DisplayWarnings(){
		float oxygenPercentage = oxygenTime / ShopManager.instance.oxygenDuration.value;
		if(oxygenPercentage <= 0.5f && !halfO2Notif){
			Instantiate(oxygenPoppup, transform.position, Quaternion.identity).SetText("50%");
			halfO2Notif = true;
		}
		if(oxygenPercentage <= 0.25f && !quarterO2Notif){
			NotificationManager.instance.AddNotification("Low Oxygen", () => {return oxygenTime / ShopManager.instance.oxygenDuration.value > 0.25f;});
			Instantiate(oxygenPoppup, transform.position, Quaternion.identity).SetText("25%");
			quarterO2Notif = true;
		}
		if(oxygenPercentage <= 0f && !noO2Notif){
			Instantiate(oxygenPoppup, transform.position, Quaternion.identity).SetText("no");
			noO2Notif = true;
		}
	}
	void ReloadWarnings(){
		float oxygenPercentage = oxygenTime / ShopManager.instance.oxygenDuration.value;
		if(oxygenPercentage >= 0.5f && halfO2Notif){
			halfO2Notif = false;
		}
		if(oxygenPercentage >= 0.25f && quarterO2Notif){
			quarterO2Notif = false;
		}
		if(oxygenPercentage >= 0f && noO2Notif){
			noO2Notif = false;
		}
	}
}

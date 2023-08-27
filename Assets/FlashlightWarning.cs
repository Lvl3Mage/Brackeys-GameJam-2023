using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightWarning : MonoBehaviour
{

	void Start()
	{
		
	}

	bool triggered = false;
	void Update()
	{
		if(triggered){return;}
		if(PlayerInfo.GetPlayerPosition().y < transform.position.y){
			triggered = true;
			NotificationManager.instance.AddNotification("I should come back here when I have a better flashlight...", () => {return PlayerInfo.GetPlayerPosition().y > transform.position.y || ShopManager.instance.flashLightStats.UpgradeLevel() >= 1;}, Callback);
		}
	}
	void Callback(){
		triggered = false;
	}
}

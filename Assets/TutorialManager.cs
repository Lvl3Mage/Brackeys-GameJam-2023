using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TutorialManager : MonoBehaviour
{
	void OnTutorialComplete(){
		PlayerPrefs.SetInt("tutorialComplete",1);
		PlayerPrefs.Save();
	}
	Coroutine TutorialProcess;
	void Start()
	{
		if(PlayerPrefs.HasKey("tutorialComplete")){
			if(PlayerPrefs.GetInt("tutorialComplete") == 1){
				return;
			}
		}
		TutorialProcess = StartCoroutine(Tutorial());

	}
	void OnDestroy(){
		if(TutorialProcess != null){
			StopCoroutine(TutorialProcess);
		}
	}
	IEnumerator Tutorial(){
		NotificationManager notifs = NotificationManager.instance;
		Func<bool> WASDCondition = () => {return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);};
		notifs.AddNotification("Use WASD to move around", WASDCondition);
		while(!WASDCondition()){
			yield return null;
		}
		Func<bool> ScrollCondition = () => {return Input.mouseScrollDelta.y != 0;};
		notifs.AddNotification("Use the scrollwheen to switch between inventory items", ScrollCondition);
		while(!ScrollCondition()){
			yield return null;
		}

		Func<bool> PhotoCondition = () => {return Input.GetMouseButtonDown(0) && !GameManager.isUIOpen();};
		notifs.AddNotification("Equip the camera and use LMB to take a picture", PhotoCondition);
		while(!PhotoCondition()){
			yield return null;
		}

		Func<bool> FishPhotoCondition = () => {return PhotoManager.instance.GetGroups().Length > 0;};
		notifs.AddNotification("Use your camera to take a photo of a fish", FishPhotoCondition);
		while(!FishPhotoCondition()){
			yield return null;
		}
		Func<bool> ViewPhotoCondition = () => {return Input.GetKeyDown(KeyCode.Q) && !GameManager.isUIOpen();};
		notifs.AddNotification("Press Q to view your photos", ViewPhotoCondition);
		while(!ViewPhotoCondition()){
			yield return null;
		}
		Func<bool> GainMoneyCondition = () => {return ShopManager.instance.money >= 50;};
		notifs.AddNotification("Gather money by taking pictures of fish", GainMoneyCondition);
		while(!GainMoneyCondition()){
			yield return null;
		}
		Func<bool> UpgradeGearCondition = () => {return ShopManager.instance.cameraStats.UpgradeLevel() != 0 || ShopManager.instance.oxygenDuration.UpgradeLevel() != 0 || ShopManager.instance.flashLightStats.UpgradeLevel() != 0;};
		notifs.AddNotification("Get an upgrade at the shop on the surface", UpgradeGearCondition);
		while(!UpgradeGearCondition()){
			yield return null;
		}
		float timeout = 2;
		Func<bool> TimoutCondition = () => {return timeout <= 0;};
		notifs.AddNotification("Happy swimming!", TimoutCondition);
		while(!TimoutCondition()){
			if(!GameManager.isUIOpen()){
				timeout -= Time.deltaTime;
			}
			yield return null;
		}
		OnTutorialComplete();
	}


	void Update()
	{
		
	}
}

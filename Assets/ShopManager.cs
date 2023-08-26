using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ShopManager : MonoBehaviour
{
	[SerializeField] CanvasGroup shopManagerPanel;
	[SerializeField] CanvasGroup shopNotification;
	[SerializeField] float accessibleDepth;
	[SerializeField] float notificationFadeSpeed;
	void Start()
	{
		ToggleShop(false);
	}
	bool shopOpen = false;
	void Update(){
		bool shopAccessible = PlayerInfo.GetPlayerPosition().y > accessibleDepth;
		shopNotification.alpha = Mathf.Lerp(shopNotification.alpha,shopAccessible ? 1 : 0, notificationFadeSpeed*Time.deltaTime);
		if(Input.GetKeyDown(KeyCode.Space) && shopAccessible){
			ToggleShop(!shopOpen);
		}
	}
	
	public void Open()
	{
		// On Something do open the shop
		ToggleShop(true);
	}
	public void Close()
	{
		// On Something do close the shop
		ToggleShop(false);
	}
	void ToggleShop(bool value){
		shopOpen = value;
		shopManagerPanel.alpha = value ? 1 : 0;
		shopManagerPanel.interactable = value;
		shopManagerPanel.blocksRaycasts = value;
	}
}

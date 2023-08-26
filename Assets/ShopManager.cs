using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ShopManager : MonoBehaviour
{
	public static ShopManager instance;
	void Awake(){
		if(instance != null){
			Debug.LogError("Another instance of ShopManager already exists!");
			return;
		}
		instance = this;
	}
	public float money { get; private set;}
	[SerializeField] CanvasGroup shopManagerPanel;
	[SerializeField] CanvasGroup shopNotification;
	[SerializeField] float accessibleDepth;
	[SerializeField] float notificationFadeSpeed;
	public UpgradableValue<PhotoCameraConfig> cameraStats;
	public UpgradableValue<float> oxygenDuration;
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
		if(!shopAccessible && shopOpen){
			ToggleShop(false);
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
[System.Serializable]
public class UpgradableValue<T>
{
	[SerializeField] T Value;
	[SerializeField] Upgrade[] upgrades;
	int upgradeIndex = 0;
	public void AddUpgrade(){
		Upgrade upgrade = upgrades[upgradeIndex];
		Value = upgrade.value;
		if(OnChange != null){
			OnChange();
		}
	}
	public float GetUpgradeCost(){
		Upgrade upgrade = upgrades[upgradeIndex];
		return upgrade.cost;
	}
	[System.Serializable]
	public struct Upgrade{
		public T value;
		public float cost;
	}
	[HideInInspector] public T value {get{return Value;}}
	public delegate void OnChangeHandler();
	public event OnChangeHandler OnChange;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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
	[HideInInspector] public float money { get; private set;}
	[SerializeField] private float startingMoney;
	public void AddMoney(float amount){
		money += amount;
		UpdateAll();

	}
	[SerializeField] CanvasGroup shopManagerPanel;
	[SerializeField] CanvasGroup shopNotification;
	[SerializeField] float accessibleDepth;
	[SerializeField] float notificationFadeSpeed;
	[SerializeField] TextWriter moneyDisplay;
	[SerializeField] Button camUpgradeButton;
	[SerializeField] TextWriter camCostDisplay;
	[SerializeField] Button oxygenUpgradeButton;
	[SerializeField] TextWriter oxygenCostDisplay;
	[SerializeField] Button flashUpgradeButton;
	[SerializeField] TextWriter flashCostDisplay;
	[SerializeField] AudioSource photoManagerOpen;
	[SerializeField] AudioSource photoManagerClose;
	public UpgradableField<PhotoCameraConfig> cameraStats;
	public UpgradableField<float> oxygenDuration;
	public UpgradableField<FlashlightConfig> flashLightStats;
	void Start()
	{
		//ToggleShop(false);
		money = startingMoney;
		UpdateAll();
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
	public void UpgradeCamera(){
		UpgradeField<PhotoCameraConfig>(cameraStats, camUpgradeButton, camCostDisplay);
	}
	public void UpgradeOxygen(){
		UpgradeField<float>(oxygenDuration, oxygenUpgradeButton, oxygenCostDisplay);
	}
	public void UpgradeFlashlight(){
		UpgradeField<FlashlightConfig>(flashLightStats, flashUpgradeButton, flashCostDisplay);
	}

	void UpgradeField<T>(UpgradableField<T> field, Button relatedButton, TextWriter relatedCostDisplay){
		float cost = field.GetUpgradeCost();
		money -= cost;
		field.AddUpgrade();
		UpdateAll();
	}
	void UpdateAll(){
		UpdateUpgradeButton(camUpgradeButton ,camCostDisplay,cameraStats);
		UpdateUpgradeButton(oxygenUpgradeButton, oxygenCostDisplay, oxygenDuration);
		UpdateUpgradeButton(flashUpgradeButton, flashCostDisplay, flashLightStats);
		UpdateMoneyDisplay();
	}
	void UpdateUpgradeButton<T>(Button button, TextWriter costDisplay, UpgradableField<T> field){
		if(!field.UpgradesLeft()){
			costDisplay.Set("--");
			button.interactable = false;
			return;
		}
		float cost = field.GetUpgradeCost();
		costDisplay.Set(cost.ToString("0.00"));
		button.interactable = money >= cost;
	}
	void UpdateMoneyDisplay(){
		moneyDisplay.Set(money.ToString("0.00"));
	}
	void ToggleShop(bool value){
		if(value && GameManager.isUIOpen()){
			return;
		}
		GameManager.ToggleUI(value);
		shopOpen = value;
		shopManagerPanel.alpha = value ? 1 : 0;
		shopManagerPanel.interactable = value;
		shopManagerPanel.blocksRaycasts = value;
		if (value){ photoManagerOpen.Play(); }
		else { photoManagerClose.Play(); }
	}
}
[System.Serializable]
public class UpgradableField<T>
{
	[SerializeField] T Value;
	[SerializeField] Upgrade[] upgrades;
	int upgradeIndex = 0;
	public void AddUpgrade(){
		if(!UpgradesLeft()){
			Debug.LogWarning("No upgrades left!");
			return;
		}
		Upgrade upgrade = upgrades[upgradeIndex];
		Value = upgrade.value;
		upgradeIndex ++;
		if(OnChange != null){
			OnChange();
		}
	}
	public float GetUpgradeCost(){
		if(!UpgradesLeft()){
			Debug.LogWarning("No upgrades left!");
			return -1;
		}
		Upgrade upgrade = upgrades[upgradeIndex];
		return upgrade.cost;
	}
	public bool UpgradesLeft(){
		return upgradeIndex < upgrades.Length;
	}
	public int UpgradeLevel(){
		return upgradeIndex;
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
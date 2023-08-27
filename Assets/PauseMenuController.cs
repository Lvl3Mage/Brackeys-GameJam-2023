using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{

	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] AudioSource menuOpen, menuClosed;
	void Start()
	{
		
	}



	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)){
			ToggleWindow(!open);
		}
	}
	bool open = false;
	void ToggleWindow(bool value){
		if(value && GameManager.isUIOpen()){
			return;
		}
		GameManager.ToggleUI(value);

		open = value;
		canvasGroup.alpha = value ? 1 : 0;
		canvasGroup.interactable = value;
		canvasGroup.blocksRaycasts = value;
		if (value){ menuOpen.Play(); }
		else { menuClosed.Play(); }
		if(value){
			SlowMotion.SlowDown(0.01f);
			
		}
		else{
			SlowMotion.SpeedUp();
		}
	}
	public void CloseMenu(){
		ToggleWindow(false);
	}
}

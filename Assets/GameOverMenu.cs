using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
	[SerializeField] float fadeSpeed = 3;
	[SerializeField] CanvasGroup canvasGroup;
	bool open = false;
	void Update(){
		if(!open){return;}
		canvasGroup.alpha += fadeSpeed*Time.unscaledDeltaTime;
		canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}
	public void OpenMenu(){
		GameManager.ToggleUI(true);
		SlowMotion.SlowDown(0.01f);
		open = true;
	}
}

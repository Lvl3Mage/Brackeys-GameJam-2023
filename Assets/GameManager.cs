using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	[SerializeField] GameOverMenu gameOverMenu; 
	void Awake()
	{
		if(instance != null){
			Debug.LogError("Another instance of GameManager already exists!");
			return;
		}
		instance = this;
	}
	bool UIOpen = false;
	public static void ToggleUI(bool val){
		instance.UIOpen = val;
	}
	public static bool isUIOpen(){
		return instance.UIOpen;
	}
	public static void ToggleGameOver(){
		instance.gameOverMenu.OpenMenu();
	}
}

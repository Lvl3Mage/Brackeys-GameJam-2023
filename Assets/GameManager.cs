using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	[SerializeField] GameOverMenu gameOverMenu; 
	[SerializeField] GameOverMenu gameWonMenu; 
	void Awake()
	{
		if(instance != null){
			Debug.LogError("Another instance of GameManager already exists!");
			return;
		}
		instance = this;
	}
	bool gameWon = false;
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
	public static void ToggleGameWon(){
		instance.gameWon = true;
	}
	public static bool isGameWon(){
		return instance.gameWon;
	}
	public static void ShowGameWonScreen(){
		instance.gameWonMenu.OpenMenu();
	}
}

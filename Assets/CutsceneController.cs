using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
 	[SerializeField] Joint2D grabJoint;
 	[SerializeField] Rigidbody2D playerRB;
 	[SerializeField] Animator animator;
	void Start()
	{
		
	}

	bool active = false;
	void Update()
	{
		if(active){
			return;
		}
		float distance = ((Vector2)transform.position - PlayerInfo.GetPlayerPosition()).magnitude;
		if(distance < 50){
			active = true;
			Cutscene();

		}
		
	}
	void Cutscene(){
		GameManager.ToggleGameWon();
		transform.position = PlayerInfo.GetPlayerPosition();
		animator.SetTrigger("StartCutscene");
	}
	void AttachPlayer(){
		grabJoint.connectedBody = playerRB;
	}
	void CutsceneOver(){
		GameManager.ShowGameWonScreen();
	}
}

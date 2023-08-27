using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoppupController : MonoBehaviour
{
	[SerializeField] float rotationRange;
	[SerializeField] TextWriter display;
	void FadoutOver(){
		Destroy(gameObject);
	}
	public void SetText(string text){
		display.Set(text);
	}
	void Start()
	{
		transform.rotation = Quaternion.Euler(0,0,Random.Range(-rotationRange, rotationRange));
	}


	void Update()
	{
		
	}
}

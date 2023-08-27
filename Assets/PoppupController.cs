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
	public void SetMoney(float money){
		display.Set(money.ToString("0.00"));
	}
	void Start()
	{
		transform.rotation = Quaternion.Euler(0,0,Random.Range(-rotationRange, rotationRange));
	}


	void Update()
	{
		
	}
}

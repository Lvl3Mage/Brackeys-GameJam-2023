using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlipper : MonoBehaviour
{
	[SerializeField] Transform baseObj;
	void Start()
	{
		
	}


	void Update()
	{
		float rotation = baseObj.rotation.eulerAngles.z;
		bool flip = Mathf.Abs(Mathf.DeltaAngle(rotation, 180)) < 90f;
		transform.localPosition = new Vector3(transform.localPosition.x,Mathf.Abs(transform.localPosition.y)*(flip?-1:1),transform.localPosition.z);

	}
}

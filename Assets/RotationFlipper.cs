using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class RotationFlipper : MonoBehaviour
{
	[SerializeField] float oppositeDirectionAngle = 180;
	[SerializeField] bool flipSprite;
	[ConditionalField(nameof(flipSprite))] [SerializeField] SpriteRenderer spriteRenderer;
	void Start()
	{
		
	}


	void Update()
	{
		float rotation = transform.rotation.eulerAngles.z;
		bool flip = Mathf.Abs(Mathf.DeltaAngle(rotation, oppositeDirectionAngle)) < 90f;
		if(flipSprite){
			spriteRenderer.flipY = flip;
		}
		else{
			transform.localScale = new Vector3(transform.localScale.x,flip ? -1 : 1, transform.localScale.z);
		}
	}
}

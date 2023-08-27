using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class HeadlightController : MonoBehaviour
{

	[SerializeField] Light2D light;
	[SerializeField] float intensityMultiplier;
	void Start()
	{
		
	}


	void Update()
	{
		

		FlashlightConfig stats = ShopManager.instance.flashLightStats.value;
		light.intensity = stats.intensity*intensityMultiplier;
	}
}

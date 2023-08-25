using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class GlobalLightController : MonoBehaviour
{
	[SerializeField] Light2D light;
	[SerializeField] float maxIntensity;
	[SerializeField] float maxIntensityDepth;
	[SerializeField] float minIntensity;
	[SerializeField] float minIntensityDepth;
	void Start()
	{
		
	}


	void Update()
	{
		float depth = PlayerInfo.GetPlayerPosition().y;
		float t = MathUtils.TransformRange(depth, minIntensityDepth, maxIntensityDepth, 0, 1);
		float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
		light.intensity = intensity;
	}
}

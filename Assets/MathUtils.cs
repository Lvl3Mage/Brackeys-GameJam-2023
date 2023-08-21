using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
	public static float TransformRange(float value, float min, float max, float newMin, float newMax){
		return ( (value - min) / (max - min) ) * (newMax - newMin) + newMin;
	}
}

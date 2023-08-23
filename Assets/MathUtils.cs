using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
	public static float TransformRange(float value, float min, float max, float newMin, float newMax){
		return ( (value - min) / (max - min) ) * (newMax - newMin) + newMin;
	}
	public static float ValueDecay(float val, float maxValue, float minValue, float halfPoint)
	{
		float top = maxValue - minValue;
		float bottom = val*(1/halfPoint) + 1;
		return top/bottom + minValue;
	}
}

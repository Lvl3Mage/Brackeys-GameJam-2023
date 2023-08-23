using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishBehaviour : MonoBehaviour
{
	public abstract float GetPreferenceValue();
	public abstract void UpdateBehaviour();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishBehaviour : MonoBehaviour
{
	public abstract float GetScore();
	public abstract void UpdateBehaviour();
}

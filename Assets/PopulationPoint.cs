using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationPoint : MonoBehaviour
{
	[SerializeField] int fishPopulation;
	public int population {get{return fishPopulation;}}
}

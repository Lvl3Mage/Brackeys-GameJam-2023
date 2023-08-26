using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationPoint : MonoBehaviour
{
	[SerializeField] int fishPopulation;
	[SerializeField] [Range(0.1f,1)] float populationInfluence = 1;
	[SerializeField] bool debug;
	public int population {get{return fishPopulation;}}
	public float influence {get{return populationInfluence;}}
	void OnDrawGizmos(){
		if(!debug){
			return;
		}
		Gizmos.color = new Color(0,0,1,populationInfluence);
		Gizmos.DrawSphere(transform.position,fishPopulation/10);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class FishSpawnManager : MonoBehaviour
{

	[SerializeField] bool autoSetSpawnRegions = true;
	[SerializeField] SpawnRegion[] spawnRegions;
	[SerializeField] float unloadDistanceFromRegion = 35;
	[SerializeField] float minUnloadDistanceFromPlayer = 20;
	List<Transform> spawnedFish = new List<Transform>();

	float spawnCooldown;
	void Start()
	{
		if(autoSetSpawnRegions){
			spawnRegions = GameObject.FindObjectsOfType<SpawnRegion>();
		}
		
	}


	void Update()
	{
		spawnCooldown -= Time.deltaTime;
		spawnCooldown = Mathf.Max(0, spawnCooldown);
		if(spawnCooldown > 0){
			return;
		}
		UnloadFish();
		TrySpawnFish();
	}
	// float GetPopulationAtPosition(Vector2 position){
	// 	float minDist = Mathf.Infinity;
	// 	float minDistPopulation = 0;

	// 	for(int i=0; i<populationPoints.Length; i++){
	// 		PopulationPoint point = populationPoints[i];
	// 		float distance = (position - (Vector2)point.transform.position).magnitude;
	// 		distance /= point.influence;
	// 		if(distance < minDist){
	// 			minDistPopulation = point.population;
	// 			minDist = distance;
	// 		}
	// 	}
	// 	return minDistPopulation;

	// 	// float totalWeight = 0;
	// 	// float[] weights = new float[populationPoints.Length];
	// 	// for(int i=0;i<populationPoints.Length;i++){
	// 	// 	PopulationPoint point = populationPoints[i];
	// 	// 	float distance = (position - (Vector2)point.transform.position).magnitude;
	// 	// 	if(distance < 0.01f){
	// 	// 		return point.population;
	// 	// 	}
	// 	// 	float weight = 1/Mathf.Pow(distance,point.influence);
	// 	// 	weights[i] = weight;
	// 	// 	totalWeight += weight;
			
	// 	// }
	// 	// float population = 0;
	// 	// for(int i=0; i < populationPoints.Length; i++){
	// 	// 	population += (weights[i]/totalWeight)*populationPoints[i].population;
	// 	// }
	// 	// return population;
	// }
	void TrySpawnFish(){
		SpawnRegion region = GetSpawnRegionAt(PlayerInfo.GetPlayerPosition());
		if(spawnedFish.Count > region.population){
			return;
		}
		
		Transform[] groupFish = region.Spawn();
		if(groupFish.Length == 0){
			spawnCooldown = 1;
			return;
		}
		foreach(Transform fish in groupFish){
			spawnedFish.Add(fish);
		}
	}
	void UnloadFish(){
		SpawnRegion region = GetSpawnRegionAt(PlayerInfo.GetPlayerPosition());
		Vector2 regionPos = region.transform.position;
		Vector2 playerPos = PlayerInfo.GetPlayerPosition();
		for(int i = spawnedFish.Count-1; i >= 0; i--){
			Transform fish = spawnedFish[i];
			float regionDistSqr = ((Vector2)fish.position - regionPos).sqrMagnitude;
			float playerDistSqr = ((Vector2)fish.position - playerPos).sqrMagnitude;
			if(regionDistSqr > unloadDistanceFromRegion*unloadDistanceFromRegion && playerDistSqr > minUnloadDistanceFromPlayer * minUnloadDistanceFromPlayer){
				Destroy(fish.gameObject);
				spawnedFish.RemoveAt(i);
			}
		}
	}
	SpawnRegion GetSpawnRegionAt(Vector2 position){
		float minDistSqr = Mathf.Infinity;
		SpawnRegion minDistRegion = null;
		foreach(SpawnRegion region in spawnRegions){
			float distSqr = (position - (Vector2)region.transform.position).sqrMagnitude;
			if(distSqr < minDistSqr){
				minDistSqr = distSqr;
				minDistRegion = region;
			}
		}
		return minDistRegion;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawnManager : MonoBehaviour
{

	[SerializeField] SpawnRegion[] spawnRegions;
	[SerializeField] float maxSpawnDistance;
	[SerializeField] float minSpawnDistance;
	[SerializeField] float maxSpawnHeight = 0;
	[SerializeField] float unloadDistance;
	[SerializeField] int desiredFishAmount;
	[SerializeField] PopulationPoint[] populationPoints;
	[SerializeField] float populationInterpolationSmoothness;
	[SerializeField] int hardPopulationCap;
	List<Transform> spawnedFish = new List<Transform>();

	float spawnCooldown;
	void Start()
	{
		
	}


	void Update()
	{
		spawnCooldown -= Time.deltaTime;
		spawnCooldown = Mathf.Max(0, spawnCooldown);
		if(spawnCooldown > 0){
			return;
		}

		float population = Mathf.Min(GetPopulationAtPosition(GetSpawnCenter()),hardPopulationCap);
		
		UnloadFish();
		if(spawnedFish.Count < population){
			SpawnFish();
		}
	}
	float GetPopulationAtPosition(Vector2 position){
		float minDist = -1;
		float minDistPopulation = 0;

		for(int i=0;i<populationPoints.Length;i++){
			PopulationPoint point = populationPoints[i];
			float distance = (position - (Vector2)point.transform.position).magnitude;
			distance /= point.influence;
			if(distance < minDist){
				minDistPopulation = point.population;
				minDist = distance;
			}
		}
		return minDistPopulation

		// float totalWeight = 0;
		// float[] weights = new float[populationPoints.Length];
		// for(int i=0;i<populationPoints.Length;i++){
		// 	PopulationPoint point = populationPoints[i];
		// 	float distance = (position - (Vector2)point.transform.position).magnitude;
		// 	if(distance < 0.01f){
		// 		return point.population;
		// 	}
		// 	float weight = 1/Mathf.Pow(distance,point.influence);
		// 	weights[i] = weight;
		// 	totalWeight += weight;
			
		// }
		// float population = 0;
		// for(int i=0; i < populationPoints.Length; i++){
		// 	population += (weights[i]/totalWeight)*populationPoints[i].population;
		// }
		// return population;
	}
	void SpawnFish(){
		(Vector2,SpawnGroup) spawn = GetValidSpawn();
		if(spawn.Item2 == null){//No valid positions found
			spawnCooldown = 1;
			return;
		}
		Transform[] groupFish = spawn.Item2.SpawnAt(spawn.Item1);
		foreach(Transform fish in groupFish){
			spawnedFish.Add(fish);
		}
	}
	void UnloadFish(){
		Vector2 playerPos = GetSpawnCenter();
		for(int i = spawnedFish.Count-1; i >= 0; i--){
			Transform fish = spawnedFish[i];
			if(((Vector2)fish.position - playerPos).sqrMagnitude > unloadDistance*unloadDistance){
				Destroy(fish.gameObject);
				spawnedFish.RemoveAt(i);
			}
		}
	}
	(Vector2,SpawnGroup) GetValidSpawn(){
		int iter = 0;
		Camera cam = WorldCamera.GetCamera();
		while(true){
			Vector2 spawnPos = GetRandomSpawnPosition();
			SpawnRegion region = GetSpawnRegionAt(spawnPos);
			SpawnGroup[] validGroups = region.GetValidSpawnGroupsAt(spawnPos);
			if(validGroups.Length > 0){
				SpawnGroup group = validGroups[Random.Range(0,validGroups.Length)];
				return (spawnPos,group);
			}
			iter++;
			if(iter > 999){
				Debug.LogWarning("FishSpawn iteration count exceeded maximum!");
				break;
			}
		}
		return (Vector2.zero,null);
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
	Vector2 GetRandomSpawnPosition(){
		Vector2 randPoint = Random.insideUnitCircle;
		if(randPoint.sqrMagnitude == 0.0f){
			randPoint = Vector2.up;
		}
		randPoint = randPoint.normalized;
		randPoint *= Random.Range(minSpawnDistance,maxSpawnDistance);
		Vector2 pos = GetSpawnCenter() + randPoint;
		pos.y = Mathf.Min(maxSpawnHeight, pos.y);
		return pos;
	}
	Vector2 GetSpawnCenter(){
		return PlayerInfo.GetPlayerPosition();
	}
}
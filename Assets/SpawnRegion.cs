using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRegion : MonoBehaviour
{
	[SerializeField] SpawnGroup[] spawnGroups;
	[SerializeField] int fishPopulation;
	[SerializeField] float maxSpawnDistance;
	[SerializeField] float minSpawnDistance = 0;
	[SerializeField] float maxSpawnHeight = 0;
	public float population {get{return fishPopulation;}}
	public Transform[] Spawn(){
		(Vector2,SpawnGroup) spawn = GetValidSpawn();
		if(spawn.Item2 == null){//No valid positions found
			return new Transform[0];
		}
		Transform[] groupFish = spawn.Item2.SpawnAt(spawn.Item1);
		return groupFish;
	}
	SpawnGroup[] GetValidSpawnGroupsAt(Vector2 position){
		List<SpawnGroup> groups = new List<SpawnGroup>();
		foreach(SpawnGroup group in spawnGroups){
			if(group.isValidPosition(position)){
				groups.Add(group);
			}
		}
		return groups.ToArray();
	}
	(Vector2,SpawnGroup) GetValidSpawn(){
		int iter = 0;
		Camera cam = WorldCamera.GetCamera();
		while(true){
			Vector2 spawnPos = GetRandomSpawnPosition();
			SpawnGroup[] validGroups = GetValidSpawnGroupsAt(spawnPos);
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
	Vector2 GetRandomSpawnPosition(){
		Vector2 randPoint = Random.insideUnitCircle;
		if(randPoint.sqrMagnitude == 0.0f){
			randPoint = Vector2.up;
		}
		randPoint = randPoint.normalized;
		randPoint *= Random.Range(minSpawnDistance,maxSpawnDistance);
		Vector2 pos = (Vector2)transform.position + randPoint;
		pos.y = Mathf.Min(maxSpawnHeight, pos.y);
		return pos;
	}
	void OnDrawGizmos(){
		Gizmos.color = new Color(0,0,1,0.3f);
		Gizmos.DrawSphere(transform.position, maxSpawnDistance);
		Gizmos.DrawSphere(transform.position, minSpawnDistance);
	}
}
[System.Serializable]
public class SpawnGroup
{
	[SerializeField] Transform fishPrefab;
	[SerializeField] int minCount;
	[SerializeField] int maxCount;
	[SerializeField] float maxSpawnDistance;
	[SerializeField] float minCameraDistance;
	[SerializeField] LayerMask obstacleMask;
	
	

	public bool isValidPosition(Vector2 position){
		Camera cam = WorldCamera.GetCamera();

		Vector2 camPos = (Vector2)cam.transform.position;
		Vector2 camSize = new Vector2(cam.orthographicSize*2*cam.aspect, cam.orthographicSize*2);
		float camRectDistance = RectangleSDF(position, camSize, camPos);
		bool outsideCamera = camRectDistance > minCameraDistance;

		Collider2D col = Physics2D.OverlapCircle(position, maxSpawnDistance, obstacleMask);
		bool outsideObstacles = col == null;
		return outsideObstacles && outsideCamera;
	}
	public Transform[] SpawnAt(Vector2 position){
		int fishToSpawn = Random.Range(minCount, maxCount);
		Quaternion orientation = Quaternion.Euler(0, 0, Random.Range(0f,360f));
		List<Transform> spawnedFish = new List<Transform>();
		for(int i = 0; i <= fishToSpawn; i++){
			spawnedFish.Add(SpawnFish(position+GetRandomSpawnPosition(), orientation));
		}
		return spawnedFish.ToArray();
	}
	Transform SpawnFish(Vector2 position, Quaternion orientation){
		return GameObject.Instantiate(fishPrefab, position, orientation);
	}
	float RectangleSDF(Vector2 point, Vector2 rectSize, Vector2 rectPosition){
		point -= rectPosition;
	    Vector2 CWED = new Vector2(Mathf.Abs(point.x), Mathf.Abs(point.y)) - rectSize;
	    float outsideDistance = new Vector2(Mathf.Max(CWED.x,0),Mathf.Max(CWED.y,0)).magnitude;
	    float insideDistance = Mathf.Min(Mathf.Max(CWED.x, CWED.y), 0);
	    return outsideDistance + insideDistance;
	}
	Vector2 GetRandomSpawnPosition(){
		Vector2 randPoint = Random.insideUnitCircle;
		if(randPoint.sqrMagnitude == 0.0f){
			randPoint = Vector2.up;
		}
		randPoint = randPoint.normalized;
		randPoint *= Random.Range(0,maxSpawnDistance);
		return randPoint;
	}	
}
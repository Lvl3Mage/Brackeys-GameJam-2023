using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishSpawnGroup
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
		Vector2 camSize = new Vector2(cam.orthographicSize*2, cam.orthographicSize*2*cam.aspect);
		float camRectDistance = RectangleSDF(position, camSize, camPos);
		bool outsideCamera = camRectDistance > minCameraDistance;

		Collider2D col = Physics2D.OverlapCircle(position, maxSpawnDistance, obstacleMask);
		bool outsideObstacles = col == null;
		return outsideObstacles && outsideCamera;
	}
	public Transform[] SpawnGroup(Vector2 position){
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

public class FishSpawnManager : MonoBehaviour
{

	[SerializeField] FishSpawnGroup[] fishGroups;
	[SerializeField] float maxSpawnDistance;
	[SerializeField] float minSpawnDistance;

	[SerializeField] float unloadDistance;
	[SerializeField] int desiredFishAmount;
	List<Transform> spawnedFish = new List<Transform>();


	void Start()
	{
		
	}


	void Update()
	{
		UnloadFish();
		if(spawnedFish.Count < desiredFishAmount){
			SpawnFish();
		}
	}
	void SpawnFish(){

		FishSpawnGroup fishGroup = GetFishGroupAt(PlayerInfo.GetPlayerPosition());
		Vector2 spawnPos = GetValidSpawnPosition(fishGroup);
		Transform[] groupFish = fishGroup.SpawnGroup(spawnPos);
		foreach(Transform fish in groupFish){
			spawnedFish.Add(fish);
		}
	}
	FishSpawnGroup GetFishGroupAt(Vector2 point){
		return fishGroups[Random.Range(0,fishGroups.Length)];
	}
	void UnloadFish(){
		Vector2 playerPos = PlayerInfo.GetPlayerPosition();
		for(int i = spawnedFish.Count-1; i >= 0; i--){
			Transform fish = spawnedFish[i];
			if(((Vector2)fish.position - playerPos).sqrMagnitude > unloadDistance*unloadDistance){
				Destroy(fish.gameObject);
				spawnedFish.RemoveAt(i);
			}
		}
	}
	Vector2 GetValidSpawnPosition(FishSpawnGroup group){
		int iter = 0;
		Vector2 spawnPos = Vector2.zero;
		Camera cam = WorldCamera.GetCamera();
		while(true){
			spawnPos = GetRandomSpawnPosition();
			if(group.isValidPosition(spawnPos)){
				break;
			}
			iter++;
			if(iter > 9999){
				Debug.LogWarning("FishSpawn iteration count exceeded maximum!");
				break;
			}
		}
		return spawnPos;
	}
	Vector2 GetRandomSpawnPosition(){
		Vector2 randPoint = Random.insideUnitCircle;
		if(randPoint.sqrMagnitude == 0.0f){
			randPoint = Vector2.up;
		}
		randPoint = randPoint.normalized;
		randPoint *= Random.Range(minSpawnDistance,maxSpawnDistance);
		return PlayerInfo.GetPlayerPosition() + randPoint;
	}	
}
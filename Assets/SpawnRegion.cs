using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRegion : MonoBehaviour
{
	[SerializeField] SpawnGroup[] spawnGroups;
	public SpawnGroup[] GetValidSpawnGroupsAt(Vector2 position){
		List<SpawnGroup> groups = new List<SpawnGroup>();
		foreach(SpawnGroup group in spawnGroups){
			if(group.isValidPosition(position)){
				groups.Add(group);
			}
		}
		return groups.ToArray();
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
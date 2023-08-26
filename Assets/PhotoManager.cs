using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class PhotoManager : MonoBehaviour
{

	public static PhotoManager instance;
	[SerializeField] Camera camera;
	[SerializeField] Vector2Int photoResolution;
	[SerializeField] float maxQualityPerTargetType;
	PhotoLibrary library;
	[SerializeField] LayerMask photoTargetLayers;
	[SerializeField] GameObject cameraFrame;
	void Awake()
	{
		if(instance != null){
			Debug.LogError("Another instance of PhotoManager already exists!");
			return;
		}
		instance = this;
		library = new PhotoLibrary(maxQualityPerTargetType);
	}
	public void Toggle(bool val){
		cameraFrame.SetActive(val);
	}
	public void CapturePhoto(){
		//camera flash --> make coroutine instead of void
		RenderTexture rt = new RenderTexture(photoResolution.x, photoResolution.y, 16, RenderTextureFormat.RGB565);//might want to assign new rt after sucessfull photo capture?
		rt.Create();
		camera.targetTexture = rt;
		camera.Render();


		PhotoTarget[] targets = GetTargetsInFrame();
		Debug.Log(targets.Length);
		if(targets.Length == 0){
			return;
		}
		else{
			Vector2 cameraPosition = camera.transform.position;
			Photo[] photos = new Photo[targets.Length];
			for(int i = 0; i < targets.Length; i++){
				photos[i] = new Photo(targets[i], cameraPosition, rt);
			}
			foreach(Photo photo in photos){
				float gain = library.AddPhoto(photo);
				ShopManager.instance.AddMoney(gain);
			}

			// Photo bestPhoto = BestPhotoOfTargets(targets, camera.transform.position, rt);
			// float gain = library.AddPhoto(bestPhoto);
			// Debug.Log("Money gained: " + gain);
			//play picture effect
			return;
		}
	}
	// Photo BestPhotoOfTargets(PhotoTarget[] targets, Vector2 cameraPosition, RenderTexture rt){//kinda gross to create a photo per target even if isn't gonna get added but it'll work
	// 	Photo[] photos = new Photo[targets.Length];
	// 	for(int i = 0; i < targets.Length; i++){
	// 		photos[i] = new Photo(targets[i], cameraPosition, rt);
	// 	}

	// 	float bestEval = Mathf.NegativeInfinity;
	// 	Photo bestPhoto = null;
	// 	foreach(Photo photo in photos){
	// 		float eval = library.EvaluatePhoto(photo);
	// 		if(eval > bestEval){
	// 			bestPhoto = photo;
	// 			bestEval = eval;
	// 		}
	// 	}
	// 	return bestPhoto;
	// }
	PhotoTarget[] GetTargetsInFrame(){
		Vector2 cameraHalfSize = new Vector2(camera.orthographicSize*camera.aspect, camera.orthographicSize);
		Vector2 bottomLeft = (Vector2)camera.transform.position - cameraHalfSize;
		Vector2 topRight = (Vector2)camera.transform.position + cameraHalfSize;
		Collider2D[] cols = Physics2D.OverlapAreaAll(bottomLeft, topRight, photoTargetLayers);

		PhotoTarget[] targets = new PhotoTarget[cols.Length];
		for(int i = 0; i < cols.Length; i++){
			PhotoTarget target = cols[i].gameObject.GetComponent<PhotoTarget>();
			if(target == null){
				Debug.LogError("No PhotoTarget component found in object", cols[i].gameObject);
			}
			targets[i] = target;
		}
		return targets;
	}
	public PhotoGroup[] GetGroups(){
		return library.GetGroups();
	}
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Vector2 cameraHalfSize = new Vector2(camera.orthographicSize*((float)photoResolution.x/(float)photoResolution.y), camera.orthographicSize);
		Vector2 bottomLeft = (Vector2)camera.transform.position - cameraHalfSize;
		Vector2 topRight = (Vector2)camera.transform.position + cameraHalfSize;
		Gizmos.DrawSphere(bottomLeft,0.1f);
		Gizmos.DrawSphere(topRight,0.1f);
	}
}
public class PhotoLibrary
{
	Dictionary<string,PhotoCollection> library = new Dictionary<string,PhotoCollection>();
	public PhotoLibrary(float _qualityPerCollection){
		qualityPerCollection = _qualityPerCollection;
	}
	float qualityPerCollection;

	public float EvaluatePhoto(Photo photo){
		string id = photo.type.id;
		PhotoCollection collection = GetCollectionWithId(id);
		return collection.EvaluatePhoto(photo);
	}
	public float AddPhoto(Photo photo){
		string id = photo.type.id;
		PhotoCollection collection = GetCollectionWithId(id);
		return collection.AddPhoto(photo);
	}
	PhotoCollection GetCollectionWithId(string id){
		if(!library.ContainsKey(id)){
			library.Add(id,new PhotoCollection(qualityPerCollection));
		}
		return library[id];
	}
	public PhotoGroup[] GetGroups(){
		List<PhotoGroup> groups = new List<PhotoGroup>();
		foreach(var pair in library){
			groups.Add(pair.Value.GetAsGroup());
		}
		return groups.ToArray();
	}
	public class PhotoCollection
	{

		public PhotoCollection(float _remainingQuality){
			remainingQuality = _remainingQuality;
		}

		float remainingQuality;

		public List<Photo> photos = new List<Photo>();
		public Photo bestPhoto;

		public float AddPhoto(Photo photo){
			photos.Add(photo);
			if(bestPhoto == null){
				bestPhoto = photo;
			}
			else if(bestPhoto.quality < photo.quality){
				bestPhoto = photo;
			}

			float gainedQuality = Mathf.Min(photo.quality, remainingQuality); 
			remainingQuality -= gainedQuality;
			return gainedQuality*photo.type.value;
		}
		public float EvaluatePhoto(Photo photo){
			return Mathf.Min(remainingQuality,photo.quality)*photo.type.value;
		}
		public PhotoGroup GetAsGroup(){
			if(photos.Count == 0){
				return null;
			}
			return new PhotoGroup(bestPhoto, bestPhoto.type, photos.ToArray());
		}
	}
}
public class PhotoGroup
{
	public PhotoGroup(Photo _bestPhoto, PhotoTargetType _type, Photo[] _photos){
		bestPhoto = _bestPhoto;
		type = _type;
		photos = _photos;
	}
	public Photo bestPhoto;
	public PhotoTargetType type;
	public Photo[] photos;
}
public class Photo
{
	public Photo(PhotoTarget target, Vector2 cameraPosition, RenderTexture relatedTexture){
		quality = 1/(((Vector2)target.transform.position - cameraPosition).magnitude+1);
		texture = relatedTexture;
		type = target.GetType();
	}
	public float quality {get; private set;}
	public RenderTexture texture {get; private set;}
	public PhotoTargetType type {get; private set;}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoManager : MonoBehaviour
{
	[SerializeField] float photoSize;
	[SerializeField] Camera camera;
	[SerializeField] Vector2Int photoResolution;
	List<RenderTexture> photos = new List<RenderTexture>();
	void Start()
	{
		
	}


	void Update()
	{
		if(Input.GetKeyDown(KeyCode.D)){
			CapturePhoto();
		}
	}
	void CapturePhoto(){
		RenderTexture rt = new RenderTexture(photoResolution.x, photoResolution.y, 16, RenderTextureFormat.RGB565);
		photos.Add(rt);
		camera.targetTexture = rt;

		camera.Render();
	}
}

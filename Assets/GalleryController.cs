using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryController : MonoBehaviour
{
	[SerializeField] CollectionPreview previewPrefab;
	[SerializeField] GameObject noPreviewsPrefab;
	[SerializeField] Transform previewContainer;
	[SerializeField] CanvasGroup canvasGroup;
	void Start()
	{
		
	}

	bool open = false;
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P)){
			ToggleWindow(!open);
		}
	}
	void ToggleWindow(bool value){
		if(value){
			LoadPreviews();
		}
		open = value;
		canvasGroup.alpha = value ? 1 : 0;
		canvasGroup.interactable = value;
		canvasGroup.blocksRaycasts = value;
	}
	void LoadPreviews(){
		foreach(Transform child in previewContainer) {
			Destroy(child.gameObject);
		}

		
		PhotoGroup[] groups = PhotoManager.instance.GetGroups();
		if(groups.Length == 0){
			Instantiate(noPreviewsPrefab, previewContainer);
			return;
		}
		foreach(PhotoGroup group in groups){
			Instantiate(previewPrefab, previewContainer).SetGroup(group);
		}
	}
}

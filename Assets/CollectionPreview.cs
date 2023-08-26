using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPreview : MonoBehaviour
{
	[SerializeField] Image image;
	[SerializeField] TextWriter nameDisplay;
	PhotoGroup photoGroup;
	public void SetGroup(PhotoGroup _photoGroup){
		photoGroup = _photoGroup;
		image.material = Instantiate(image.material);
		image.material.SetTexture("_MainTex", photoGroup.bestPhoto.texture);
		nameDisplay.Set(photoGroup.type.name);
	}
	void Awake()
	{
	}


	void Update()
	{
		
	}
}

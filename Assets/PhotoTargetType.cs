using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
[CreateAssetMenu(menuName = "Game/PhotoTargetType")]
public class PhotoTargetType : ScriptableObject
{
	[SerializeField] string Id;
	public string id { get{return Id;}}
	[SerializeField] string Name;
	public string name { get{return Name;}}
	[SerializeField] string Description;
	public string description { get{return Description;}}
	[SerializeField] float Value;
	public float value { get{return Value;}}
	[ButtonMethod]
	void NameToId(){
		Id = Name;
	}
	[ButtonMethod]
	void RandomizeId(){
		Id = Random.Range(0, System.Int32.MaxValue).ToString();
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float oxygen;
    //[SerializeField] private Slider oxyBar;
    
    IEnumerator Oxygen()
    {
        while (oxygen > 0)
        {
            oxygen -= 0.01f;
            //oxyBar.value -= 0.01f;
            yield return null;
        }
    }
    
    public void Start()
    {
        StartCoroutine(Oxygen());
    }
}

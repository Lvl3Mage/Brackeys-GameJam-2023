using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class TestCamera : MonoBehaviour
{
    public AudioSource diverAudio;
    public Light2D camLight;
    public bool canPhoto = true;

    private void Start()
    {
        camLight.intensity = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && canPhoto)
        {
            diverAudio.Play();
            canPhoto = false;
            StartCoroutine(CamCooldown());
            StartCoroutine(CamLight());
        }

    }

    IEnumerator CamCooldown()
    {
        yield return new WaitForSeconds(2.5f);
        canPhoto = true;
        
    }
    
    IEnumerator CamLight()
    {
        while (camLight.intensity < 2.5f)
        {
            camLight.intensity += 10f * Time.deltaTime;
            yield return null;
        }
        while (camLight.intensity > 0f)
        {
            camLight.intensity -= 6f * Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}

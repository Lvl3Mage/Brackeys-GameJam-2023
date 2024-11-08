using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraLerper : MonoBehaviour
{
    public Transform target;
    [SerializeField] float lerpSpeed = 1f;
    [SerializeField] private AudioSource uiSounds;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 144;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,target.position,lerpSpeed * Time.deltaTime);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void CloseGame()
    {
        Debug.Log("The game has bloody closen");
        Application.Quit();
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        uiSounds.Play();
    }
}

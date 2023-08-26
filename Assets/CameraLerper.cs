using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraLerper : MonoBehaviour
{
    public Transform target;
    [SerializeField] float lerpSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,target.position,lerpSpeed * Time.deltaTime);
    }

    public void PlayGame(string newScene)
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
    }
}

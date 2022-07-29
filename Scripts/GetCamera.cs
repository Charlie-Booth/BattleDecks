using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetCamera : MonoBehaviour
{
    //Gets the camera
    public Canvas thisCanvas;
    public Camera theCamera;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        thisCanvas.worldCamera = theCamera;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Tutorial Found: https://www.youtube.com/watch?v=rnqF6S7PfFA by Game Dev Guide "Building a Camera Controller for a Strategy Game" uploaded June 24 2019
    public float moveSpeed;
    public float movementTime;
    public float rotationAmount;
    public Transform cameraTransform;
    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 zoomAmount;
    public Vector3 newZoom;
    public float minZoom;
    public float maxZoom;
    
    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        
        HandleMovement();
       
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -moveSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -moveSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        if (Input.mouseScrollDelta.y !=0) // forward
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;

            newZoom.y = Mathf.Clamp(newZoom.y, -minZoom, maxZoom); // clamps the zoom
            newZoom.z = Mathf.Clamp(newZoom.z, -maxZoom, minZoom);
        }
        newPosition = new Vector3(Mathf.Clamp(newPosition.x, -7f, 7f), 0.1f, Mathf.Clamp(newPosition.z, -7f, 7f)); // clamps the camera movement
       
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime); // moves the camera controller to the new position over time 
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime); // rotates the camera controller to the new position over time
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime); // zooms the camera in to the new zoom position over time
    }
}



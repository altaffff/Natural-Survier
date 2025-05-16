using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControler : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed ;
    [SerializeField] private float verticalSpeed;

    public Transform playerBody;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MouseRotations();
    }
    
    void MouseRotations()
    {
        float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -39f, 39f);
        
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    
}

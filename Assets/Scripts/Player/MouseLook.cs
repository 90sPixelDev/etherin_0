using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSen = 1.1f;

    public Transform playerBody;

    public float yRotation = -0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSen;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSen;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);


        transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        //Setting the variable for direction + length at the end
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 5f;
        //Making the actual ray and showing it on screen by position, direction + length, and finally color
        Debug.DrawRay(transform.position, forward, Color.red);
    }
}

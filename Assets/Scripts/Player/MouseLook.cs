using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private float mouseX, mouseY;
    public float mouseSensitivity = 100f;
    private float rotationX;

    private Transform playerT;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerT = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        playerT.Rotate(Vector3.up * mouseX);
    }
}

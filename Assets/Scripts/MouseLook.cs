using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float clampTop, clampBottom;
    public string chairName = "M_Chair";
    public Transform chairTransform;

    private float xRotation = 0f, yRotation = 0f;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        chairTransform = GameObject.Find(chairName).transform;
    }

    void Update()
    {
        xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -clampTop, clampBottom);

        yRotation += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        chairTransform.rotation = Quaternion.Euler(chairTransform.rotation.x, yRotation, chairTransform.rotation.z);
    }
}

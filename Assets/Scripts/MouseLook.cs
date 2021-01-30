using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float clampTop, clampBottom;
    public float defaultFOV = 60f, minFOV = 30f, zoomSpeed = 250f;
    public string chairName = "M_Chair";
    public Transform chairTransform;

    private float xRotation = 0f, yRotation = 90f;
    private float currentFOV;
    private Camera cam;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        chairTransform = GameObject.Find(chairName).transform;
        cam = transform.GetComponent<Camera>();
        cam.fieldOfView = currentFOV = defaultFOV;
    }

    void Update()
    {
        xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -clampTop, clampBottom);

        yRotation += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        chairTransform.rotation = Quaternion.Euler(chairTransform.rotation.x, yRotation + 90f, chairTransform.rotation.z);

        if (Input.GetMouseButton(1))
        {
            if (currentFOV > minFOV)
                currentFOV -= zoomSpeed * Time.deltaTime;
        }
        else
        {
            if (currentFOV < defaultFOV)
                currentFOV += zoomSpeed * Time.deltaTime;
        }

        cam.fieldOfView = currentFOV;
    }
}

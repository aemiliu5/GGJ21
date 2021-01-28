using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    [Header("Parameters")]
    public float horizontalSpeed;
    public float verticalSpeed;
    public Vector2 horizontalLimits;
    public Vector2 verticalLimits;
    
    [Header("Axes")]
    public Rigidbody clawAxisLeftRightRigidbody;
    public Rigidbody clawAxisFrontBackRigidbody;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime;
        float verticalMovement = Input.GetAxis("Vertical") * verticalSpeed * Time.deltaTime;
        Vector3 horizontalVector = new Vector3(horizontalMovement, 0, 0);
        Vector3 verticalVector = new Vector3(0, 0, verticalMovement);

        clawAxisLeftRightRigidbody.AddForce(horizontalVector, ForceMode.Acceleration);
        clawAxisFrontBackRigidbody.AddForce(verticalVector, ForceMode.Acceleration);
    }
}

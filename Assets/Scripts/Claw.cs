﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    [Header("Parameters")]
    public float speed;
    private float horizontalMovement;
    private float verticalMovement;
    public Vector2 topLeft;
    public Vector2 bottomRight;

    [Header("Axes")]
    public Rigidbody clawRigidbody;

    [Header("References")] 
    public Rigidbody rb;
    public GameObject cubeLr;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Inputs
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        
        // "Parent" only the Z position axis of the Cube LR object to the cube.
        cubeLr.transform.position = new Vector3(cubeLr.transform.position.x, cubeLr.transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        Movement();
        /*
        // Keep rigidbodies inside claw limits.
        Vector3 endPosition = rb.position + rb.velocity * Time.fixedDeltaTime;
        endPosition.x = Mathf.Clamp(endPosition.x, topLeft.x, bottomRight.x);
        endPosition.z = Mathf.Clamp(endPosition.z, topLeft.y, bottomRight.y);
        rb.position = endPosition;
        */
    }

    /// <summary>
    /// Applies the movement forces, checking constraints.
    /// </summary>
    private void Movement()
    {
        // First, check the limits/constraints...
        
        // Horizontal limit
        if ((horizontalMovement < 0 && rb.position.x <= topLeft.x) ||
            (horizontalMovement > 0 && rb.position.x >= bottomRight.x))
            horizontalMovement = 0;
        
        // Vertical limit
        if ((verticalMovement > 0 && rb.position.z >= bottomRight.y) ||
            (verticalMovement < 0 && rb.position.z <= topLeft.y))
            verticalMovement = 0;
        
        // Then apply the proper movement forces.
        Vector3 movementVector = new Vector3(horizontalMovement, 0, verticalMovement).normalized * (Time.fixedDeltaTime * speed);
        
        clawRigidbody.AddForce(movementVector, ForceMode.Acceleration);
    }
}

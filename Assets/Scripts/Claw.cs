using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    [Header("Parameters")]
    //public string axisName = "Claw Axis LR";
    public float speed;
    public Vector2 topLeft;
    public Vector2 bottomRight;
    public Transform clawVertical;
    public Transform clawVertical2;
    public float height;
    //public float height2;

    #region Private Variables
    private Rigidbody rb;
    private Transform axisLR;
    private float horizontalMovement;
    private float verticalMovement;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //axisLR = GameObject.Find(axisName).transform;
    }

    // Update is called once per frame
    private void Update()
    {
        // Inputs
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        // "Parent" only the Z position axis of the Cube LR object to the cube.
        //clawVertical.position = transform.position + new Vector3(0, height, 0);
        //clawVertical2.GetComponent<Rigidbody>().position = transform.position + new Vector3(0, height2, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            clawVertical.transform.position -= new Vector3(0, 0.01f, 0);
            //height2 -= 0.025f;
            horizontalMovement = verticalMovement = 0;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            clawVertical.transform.position += new Vector3(0, 0.01f, 0);
            //height2 -= 0.025f;
            horizontalMovement = verticalMovement = 0;
        }
    }

    private void FixedUpdate()
    {
        Movement();
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
        Vector3 movementVector = new Vector3(horizontalMovement, 0, (verticalMovement)).normalized * (Time.fixedDeltaTime * speed);

        rb.AddForce(movementVector, ForceMode.Acceleration);
    }
}

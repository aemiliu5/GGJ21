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
    public float height;
    public float heightDiff;

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
        
        // Vertical
        if (Input.GetKey(KeyCode.Space) && transform.parent.transform.position.y > 1.3f)
        {
            clawVertical.transform.position -= new Vector3(0, heightDiff * Time.deltaTime, 0);
            horizontalMovement = verticalMovement = 0;
        }
        else if(!Input.GetKey(KeyCode.Space) && transform.parent.transform.position.y < 2.3f)
            clawVertical.transform.position += new Vector3(0, heightDiff * Time.deltaTime, 0);
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

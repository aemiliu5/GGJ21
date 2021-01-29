using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    [Header("Parameters")]
    public float speed;
    public Vector2 topLeft;
    public Vector2 bottomRight;
    public Transform clawVertical;
    public float verticalMovementSpeed;

    public bool hooked;
    public Animator anim;
    
    #region Private Variables
    private Rigidbody rb;
    private Transform axisLR;
    private float xMovement;
    private float zMovement;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Inputs
        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");
        
        // Vertical movement
        if (Input.GetKey(KeyCode.Space) && transform.parent.transform.position.y > 1.3f)
        {
            clawVertical.transform.position -= new Vector3(0, verticalMovementSpeed * Time.deltaTime, 0);
            xMovement = zMovement = 0;
        }
        else if(!Input.GetKey(KeyCode.Space) && transform.parent.transform.position.y < 2.3f)
            clawVertical.transform.position += new Vector3(0, verticalMovementSpeed * Time.deltaTime, 0);
        
        // Claw Lock
        if (Input.GetMouseButton(0))
        {
            hooked = true;
            anim.SetBool("hooked", hooked);
        }
        else
        {
            hooked = false;
            anim.SetBool("hooked", hooked);
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
        if ((xMovement < 0 && rb.position.x <= topLeft.x) ||
            (xMovement > 0 && rb.position.x >= bottomRight.x))
            xMovement = 0;

        // Vertical limit
        if ((zMovement > 0 && rb.position.z >= bottomRight.y) ||
            (zMovement < 0 && rb.position.z <= topLeft.y))
            zMovement = 0;

        // Then apply the proper movement forces.
        Vector3 movementVector = new Vector3(xMovement, 0, (zMovement)).normalized * (Time.fixedDeltaTime * speed);

        rb.AddForce(movementVector, ForceMode.Acceleration);
    }
}

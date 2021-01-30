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
    public HookSensor hookSensor;
    public SphereCollider hookCollider;
    public Transform hookParent;
    public GameObject hookedObject;

    [Header("Audio")]
    public AudioSource aud;
    public AudioClip audioLrMoveStart;
    public AudioClip audioLrMoveLoop;
    public AudioClip audioLrMoveEnd;
    public AudioClip audioUdMoveStart;
    public AudioClip audioUdMoveLoop;
    public AudioClip audioUdMoveEnd;

    #region Private Variables
    private Rigidbody rb;
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
        if (Input.GetMouseButtonDown(0))
        {
            hooked = true;
            anim.SetBool("hooked", hooked);

            if (hookSensor.sensedObject != null)
            {
                hookedObject = hookSensor.sensedObject;
                hookedObject.GetComponent<Rigidbody>().isKinematic = true;
                hookedObject.layer = 8;
                hookedObject.transform.parent = hookParent.transform;
            }
        }
        else if((Input.GetMouseButtonUp(0)))
        {
            hooked = false;
            anim.SetBool("hooked", hooked);

            if (hookedObject != null)
            {
                hookedObject.GetComponent<Rigidbody>().isKinematic = false;
                hookedObject.layer = 0;
                hookedObject.transform.parent = null;
            }

            hookedObject = null;
        }
        
        if(hookedObject)
            Debug.Log(hookedObject.GetComponent<Rigidbody>().velocity);
        
        // Audio
        bool xzDown = (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S));
        bool xzHold = (Input.GetKey(KeyCode.A)     || Input.GetKey(KeyCode.D)     || Input.GetKey(KeyCode.W)     || Input.GetKey(KeyCode.S));
        bool xzUp   = (Input.GetKeyUp(KeyCode.A)   || Input.GetKeyUp(KeyCode.D)   || Input.GetKeyUp(KeyCode.W)   || Input.GetKeyUp(KeyCode.S));

        bool yDown  = (Input.GetKeyDown(KeyCode.Space));
        bool yHold  = (Input.GetKey(KeyCode.Space));
        bool yUp    = (Input.GetKeyUp(KeyCode.Space));
        
        if(xzDown && !aud.isPlaying)
        {
            aud.clip = audioLrMoveStart;
            aud.loop = false;
            aud.Play();
        }
        else if (xzHold && (aud.time == 0 || aud.time > 3f))
        {
            aud.clip = audioLrMoveLoop;
            aud.loop = true;
            aud.Play();
        }
        else if (xzUp)
        {
            aud.clip = audioLrMoveEnd;
            aud.loop = false;
            aud.Play();
        }
        
        if(yDown && !aud.isPlaying)
        {
            aud.clip = audioUdMoveStart;
            aud.loop = false;
            aud.Play();
        }
        else if (yHold && (aud.time == 0 || aud.time > 3f))
        {
            aud.clip = audioUdMoveLoop;
            aud.loop = true;
            aud.Play();
        }
        else if (yUp)
        {
            aud.clip = audioUdMoveEnd;
            aud.loop = false;
            aud.Play();
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

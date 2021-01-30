using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public List<GameObject> onBelt;
    
    private void FixedUpdate()
    {
        for (int i = 0; i <= onBelt.Count - 1; i++)
        { 
            onBelt[i].GetComponent<Rigidbody>().velocity = speed * direction * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Hookable"))
            onBelt.Add(other.gameObject);
    }
    
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Hookable"))
            onBelt.Remove(other.gameObject);
    }
}

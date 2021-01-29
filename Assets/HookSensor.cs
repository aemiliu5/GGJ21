using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSensor : MonoBehaviour
{
    public GameObject sensedObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hookable"))
            sensedObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Hookable"))
            sensedObject = null;
    }
}

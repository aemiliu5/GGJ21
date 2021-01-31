using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    public ConveyorBelt conveyorBeltEnd;
    
    private void OnTriggerEnter(Collider other)
    {
        GameObject collided = other.gameObject;
        if (collided.CompareTag("Hookable"))
        {
            transform.parent.GetComponent<Procedural>().HandleDespawn(collided);
            conveyorBeltEnd.onBelt.Remove(collided);
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

    }
}

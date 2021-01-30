using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject collided = other.gameObject;
        if (collided.CompareTag("Hookable"))
            transform.parent.GetComponent<Procedural>().HandleDelivery(collided);
    }
}

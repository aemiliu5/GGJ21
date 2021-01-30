using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        GameObject collided = other.gameObject;
        if (collided.CompareTag("lugage"))
            transform.parent.GetComponent<Procedural>().HandleDelivery(collided);
    }
}

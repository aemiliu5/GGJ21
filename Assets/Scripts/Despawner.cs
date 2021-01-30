using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        GameObject collided = other.gameObject;
        if (collided.CompareTag("lugage"))
            transform.parent.GetComponent<Procedural>().HandleDespawn(collided);
    }
}

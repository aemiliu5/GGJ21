using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLamp : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, speed*Time.deltaTime, 0f));
    }
}

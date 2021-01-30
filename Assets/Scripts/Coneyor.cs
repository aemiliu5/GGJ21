using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coneyor : MonoBehaviour
{
    public MeshRenderer mr;
    public float yOffset;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        Material conveyorMat = mr.material;
        Vector2 offset = conveyorMat.mainTextureOffset;

        yOffset += Time.deltaTime * speed;
        if (yOffset >= 1)
        {
            yOffset = 0;
        }


        offset.y = yOffset;
        mr.material.SetTextureOffset("_BaseColorMap", offset);
    }
}

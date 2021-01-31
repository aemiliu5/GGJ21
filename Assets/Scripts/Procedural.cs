using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Procedural : MonoBehaviour
{
    [Header("Parameters")]
    public int maxNumberOfLugages = 25;
    public Vector2 minMaxOfWaitTime;
    [Space(10)]
    public Vector3 newLugageSpawnLocation;
    public Vector3 existingLugageSpawnLocation;

    public GameObject correctCase;
    public GameObject correctCaseClone;
    public GameObject correctCaseSpawnpoint;
    public int correctTotal = 0;
    public GameObject clientPrefab;
    public GameObject clientInstance;
    public Vector3 clientSpawnpoint;

    public GameManager gm;

    // [Header("References")]
    // public GameObject despawner;
    // public GameObject delivery;

    [Header("Prefabs And Stuff")]
    public List<Color> colors;
    public PhysicMaterial physicsMaterial;
    public Texture fallbackTexture;
    [Space(10)]
    public List<Lugages> prefabs;

    [Header("<----- DEBUGGING ONLY! ----->")]
    [Header("<----- DO NOT TOUCH! ----->")]
    [Space(20)]
    public List<GameObject> createdLugages;
    
    #region Private Variables
    private List<GameObject> despawnedLugages;
    [SerializeField] private List<GameObject> spawnedLugages;
    [SerializeField]
    private float nextSpawnTimer;
    #endregion

    void Awake()
    {
        // despawner = gameObject.transform.Find("Despawner").gameObject;
        // delivery = gameObject.transform.Find("Delivery").gameObject;
        createdLugages = new List<GameObject>(maxNumberOfLugages);
        spawnedLugages = new List<GameObject>(maxNumberOfLugages);
        despawnedLugages = new List<GameObject>();

        gm = FindObjectOfType<GameManager>();

        nextSpawnTimer = Random.Range(minMaxOfWaitTime.x, minMaxOfWaitTime.y);    // find the next spawn timer
        Debug.Log(string.Format("Next Spawn in: {0} seconds", nextSpawnTimer));
        
        CreateAllLugage();
        StartCoroutine(GetCorrectCase());
    }

    void Update()
    {
        nextSpawnTimer -= Time.deltaTime;

        if (nextSpawnTimer <= 0f)
        {
            if (createdLugages.Count != 0)
                SpawnLugage();
            else if (despawnedLugages.Count > 0)
                HandleExistingLugage();

            nextSpawnTimer = Random.Range(minMaxOfWaitTime.x, minMaxOfWaitTime.y);
            Debug.Log(string.Format("Next Spawn in: {0} seconds", nextSpawnTimer));
        }
    }

    void CreateAllLugage()
    {
        for (int i = 0; i < maxNumberOfLugages; i++)
        {
            HandleNewLugage();
        }
    }

    void CreateLugage()
    {
        HandleNewLugage();
    }

    void SpawnLugage()
    {
        spawnedLugages.Add(createdLugages[0]);
        createdLugages[0].SetActive(true);
        createdLugages.Remove(createdLugages[0]);
    }

    private IEnumerator GetCorrectCase()
    {
        yield return new WaitForSeconds(5f);
        
        // Spawn client
        clientInstance = Instantiate(clientPrefab, clientSpawnpoint, Quaternion.Euler(0, -180, 0));

        yield return new WaitForSeconds(3f);
        
        // Combine spawned, created, and despawned luggage.
        List<GameObject> combinedLugages = spawnedLugages.Concat(createdLugages).Concat(despawnedLugages).ToList();
        
        // Select one at random, then clone it so it appears on screen.
        correctCase = combinedLugages[Random.Range(0, combinedLugages.Count)];
        correctCaseClone = Instantiate(correctCase, correctCaseSpawnpoint.transform.position,
            correctCaseSpawnpoint.transform.rotation);
        correctCaseClone.SetActive(true);
        correctCaseClone.GetComponent<Rigidbody>().isKinematic = true;
    }

    void HandleNewLugage()
    {
        int prefabPos = Random.Range(0, prefabs.Count);
        Debug.Log("Prefab Pos: " + prefabPos);
        Lugages target = prefabs[prefabPos];
        int materialPos = Random.Range(0, target.materials.Count);
        Debug.Log("Material Pos: " + materialPos);

        // Better to use the fallback texture by default, it looks better.
        Texture texture = fallbackTexture;

        /*if (target.textures.Count != 0)
        {
            int texturePos = Random.Range(0, target.textures.Count);
            Debug.Log("Texture Pos: " + texturePos);
            texture = target.textures[texturePos];
        }*/


        createdLugages.Add(NewLugage(target.model, target.scale, target.minMaxMass, target.materials[materialPos], texture, target.isRandomScale, target.minMaxScaleFactor));
    }

    GameObject NewLugage(GameObject model, Vector3 scale, Vector2 minMaxMass, Material material, Texture texture, bool isRandomScale, Vector2 minMaxScaleFactor)
    {
        GameObject newInstance = Instantiate(model, newLugageSpawnLocation, model.transform.rotation);   // new instance of object

        // tag for the triggers
        newInstance.tag = "Hookable";

        // scale it
        if (isRandomScale) newInstance.transform.localScale = scale * Random.Range(minMaxScaleFactor.x, minMaxScaleFactor.y);
        else newInstance.transform.localScale = scale;

        // new material with texture
        Material nm = new Material(material);
        nm.SetTexture("_BaseColorMap", texture);
        nm.SetColor("_BaseColor", colors[Random.Range(0, colors.Count)]);
        newInstance.GetComponent<MeshRenderer>().material = nm;

        // If the mesh contains sub-meshes/children, apply the same material to them.
        if (newInstance.GetComponentsInChildren<MeshRenderer>().Length > 0)
        {
            foreach (MeshRenderer childMr in newInstance.GetComponentsInChildren<MeshRenderer>())
            {
                childMr.material = nm;
            }
        }

        // rigidbody check and add if needed
        Rigidbody rb = newInstance.GetComponent<Rigidbody>() ? newInstance.GetComponent<Rigidbody>() : null;
        if (rb == null) rb = newInstance.AddComponent<Rigidbody>();
        rb.mass = Random.Range(minMaxMass.x, minMaxMass.y);
        rb.interpolation = RigidbodyInterpolation.None;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        // collider check and add if needed
        /*MeshCollider mc = newInstance.GetComponent<MeshCollider>() ? newInstance.GetComponent<MeshCollider>() : null;
        if (mc == null) mc = newInstance.AddComponent<MeshCollider>();
        mc.convex = true;   // need that, no idea why
        mc.material = physicsMaterial;
        */

        newInstance.SetActive(false);
        return newInstance;
    }

    void HandleExistingLugage()
    {
        spawnedLugages.Add(despawnedLugages[0]);
        despawnedLugages[0].SetActive(true);
        despawnedLugages.RemoveAt(0);
    }


    public void HandleDespawn(GameObject despawning)
    {
        spawnedLugages.Remove(despawning);
        despawnedLugages.Add(despawning);
        despawning.transform.localPosition = existingLugageSpawnLocation;
        despawning.SetActive(false);
    }

    public void HandleDelivery(GameObject delivering)
    {
        spawnedLugages.Remove(delivering);
        
        // check if bag was correct, then destroy
        
            Destroy(delivering);
            Destroy(correctCaseClone);
            CreateLugage();

            // Bag was correct.
            if (delivering.Equals(correctCase))
            {
                gm.stamps++;
                clientInstance.GetComponent<Client>().currentState = Client.State.LeavingHappy;
                Destroy(clientInstance, 5f);
                Debug.Log(":)");
            }
            else
            {
                gm.stamps--;
                clientInstance.GetComponent<Client>().currentState = Client.State.LeavingAngry;
                Destroy(clientInstance, 5f);
                Debug.Log(":(");
            }
            
            StartCoroutine(GetCorrectCase());

            /* if the gameobject is being delivered outside the player view, uncomment next line */
            // Destroy(delivering);

            /* any other callback you want when a lugage is delivered */
    }
}

[System.Serializable]
public class Lugages
{
    public GameObject model;
    [Space(10)]
    public Vector3 scale;
    public Vector2 minMaxMass;
    [Space(10)]
    public bool isRandomScale = false;
    public Vector2 minMaxScaleFactor;
    [Space(10)]
    public List<Material> materials;
    public List<Texture> textures;
}
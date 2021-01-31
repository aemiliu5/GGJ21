using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicCrossfade : MonoBehaviour
{
    public AudioSource filtered;
    public AudioSource unfiltered;
    public Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        filtered.volume = 0.5f * Mathf.Sin(mainCamera.transform.eulerAngles.y * Mathf.Deg2Rad) + 0.5f;
        unfiltered.volume = -0.5f * Mathf.Sin(mainCamera.transform.eulerAngles.y * Mathf.Deg2Rad) + 0.5f;
    }
}

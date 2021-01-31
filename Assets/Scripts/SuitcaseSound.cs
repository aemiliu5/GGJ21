using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SuitcaseSound : MonoBehaviour
{
    public AudioClip[] hitSounds;
    private AudioSource aud;

    // Start is called before the first frame update
    private void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // Play sound on hit
        aud.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
    }
}

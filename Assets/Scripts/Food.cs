﻿using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour
{
    //Define audio source.
    private AudioSource hitAudio;

    void Start()
    {
        //Find the audio source.
        hitAudio = GetComponent<AudioSource>();
        //Delete the food after 5 seconds.
        Destroy(gameObject, 5);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Play the audio if it hits any environment assets.
        if (!hitAudio.isPlaying && collision.gameObject.tag == "Environment")
        {
            //Change to a random pitch.
            hitAudio.pitch = Random.Range(0.5f, 1);
            //Play the audio.
            hitAudio.Play();
        }
    }
}

﻿using UnityEngine;
using System.Collections;

public class House : MonoBehaviour
{
    private Transform[] door = new Transform[2];
    private bool activeHouse = false;
    private AudioSource rewardSound;

    private GameMaster gameMaster;
    private Animation anim;
    private int tipValue = 10;

    private Transform pointer;
    private Animation pointerAnim;
    private Material pointerMat;
    private int timeToDeliver = 10;

    void Start()
    {
        //Finds the game master reference.
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        //Null check.
        if (gameMaster == null)
        {
            Debug.Log("Cant find Game Master");
        }

        //Finds the door game objects.
        door[0] = transform.Find("Door[0]");
        door[1] = transform.Find("Door[1]");

        //finds the audio source
        rewardSound = GetComponent<AudioSource>();
        anim = GetComponent<Animation>();

        pointer = transform.Find("Pointer");
        pointerAnim = pointer.gameObject.GetComponent<Animation>();
        CheckActive();
    }


    void CheckActive()
    {
        if (activeHouse)
        {
            if (!pointerAnim.isPlaying)
            {
                pointerAnim.Play();
            }
        }
        else
        {
            if (pointerAnim.isPlaying)
            {
                pointerAnim.Stop();
            }
            pointer.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //If food collides with the house and the house is a currently active house.
        if (collision.gameObject.tag == "Food" && activeHouse)
        {
            //Gets the distance from the food to the door.
            float distanceToDoor0 = Vector3.Distance(collision.transform.position, door[0].position);
            float distanceToDoor1 = Vector3.Distance(collision.transform.position, door[1].position);

            //If it is less that the width of the door.
            if (distanceToDoor0 <= 2f || distanceToDoor1 <= 2f)
            {
                //Turns the house inactive.
                activeHouse = false;
                //Destroys the food.
                Destroy(collision.gameObject);
                //Makes a random audio pitch.
                rewardSound.pitch = Random.Range(0.5f, 1);
                //Plays the audio
                rewardSound.Play();
                //Play preset animation.
                anim.Play();
                //Checks if the house is active.
                CheckActive();
                //Calls the add tips function in the game master.
                gameMaster.AddTips(tipValue);
            }
        }
    }

    public bool GetActiveState()
    {
        return activeHouse;
    }
    public void SetActive()
    {
        activeHouse = true;
    }
}

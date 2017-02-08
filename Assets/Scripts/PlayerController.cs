using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    //Which layer the Raycast will hit (Currently set to ground layer).
    public LayerMask groundCheckLayer = 8;
    //Prefab of food game object.
    public Transform foodPrefab;

    //Declares rigidbody.
    private Rigidbody rb;
    //Delares an array of trail renderers (2).
    private TrailRenderer[] trailRend = new TrailRenderer[2];
    //Defines movement vector.
    private Vector3 movement;
    //Declares the players move speed.
    private float moveSpeed = 1600;
    //Declares the speed which the player will rotate at when mpving in a different direction.
    private float rotationSpeed = 20;

    //Throw distance.
    private float throwForce = 10f;
    //Stops firing after just firing.
    private bool justFired = false;
    //Fire Points, one on each side.
    private Transform[] firePoints = new Transform[2];
    //Anchor point for both ground check and the throw vector.
    private Transform anchorPoint;

	void Start ()
    {
        //Defines rigidbody.
        rb = GetComponent<Rigidbody>();
        //Defines both trail renderers.
        trailRend[0] = transform.Find("Trail[0]").GetComponent<TrailRenderer>();
        trailRend[1] = transform.Find("Trail[1]").GetComponent<TrailRenderer>();
        //Trail renderer null check.
        if (trailRend[0] == null || trailRend[1] == null)
        {
            Debug.Log("Trail Renderer not found");
        }
        //Defines fire points.
        firePoints[0] = transform.Find("FirePoint[0]");
        firePoints[1] = transform.Find("FirePoint[1]");
        //Fire point null check.
        if (firePoints[0] == null || firePoints[1] == null)
        {
            Debug.Log("Fire Point not found");
        }
        //Defines anchor.
        anchorPoint = transform.Find("Anchor");
    }
	
    void FixedUpdate()
    {
        Drive();
    }

	void Update ()
    {

        //If Fire 1 is clicked and has not just fired.
        if (Input.GetButton("Fire1") && !justFired)
        {
            //Fire.
            StartCoroutine(FireFood(0));
        }
        //If Fire 2 is clicked and has not just fired.
        else if (Input.GetButton("Fire2") && !justFired)
        {
            //Fire.
            StartCoroutine(FireFood(1));
        }
        //LT BUTTON.
        else if (Input.GetAxis("LT/RT") > 0.8f && !justFired)
        {
            StartCoroutine(FireFood(0));
        }
        //RT BUTTON.
        else if (Input.GetAxis("LT/RT") < -0.8f && !justFired)
        {
            StartCoroutine(FireFood(1));
        }
	}

    //Moves player.
    void Drive()
    {
        //Defines a movement vector.
        movement = new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        //Normalizes vector so that the player can not move faster diagonally.
        Vector3.Normalize(movement);

        //Checks if there is no movement input
        if (movement != Vector3.zero)
        {
            //rotates towards movement direction.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed * Time.deltaTime);
        }

        //moves position.
        rb.AddForce(movement, ForceMode.Force);
    }

    //Checks if player is on ground.
    bool GroundCheck()
    {
        //Declares a hit.
        RaycastHit hit;

        //Draws the raycast so that I'm not blind while guessing the length.
        Debug.DrawRay(transform.position, -Vector3.up * 0.5f, Color.green, 0.2f);

        //Checks if raycast has hit anything.
        if (Physics.Raycast(anchorPoint.position, -Vector3.up * 2.4f, out hit, 0.5f))
        {
            Debug.Log("Floor was hit.");

            //if either of the trail renderers are not enabled.
            if (trailRend[0].enabled == false || trailRend[1].enabled == false)
            {
                //Enable both trail renderers in a for loop.
                for (int i = 0; i < trailRend.Length; i++)
                {
                    //Enable both.
                    trailRend[i].enabled = true;
                }
            }

            //returns true informing that the ray did infact hit.
            return true;
        }
        //If the raycast did not hit anything.
        else
        {
            //Disable both of the trail renderers in a for loop.
            for (int i = 0; i < trailRend.Length; i++)
            {
                //Disable current renderer.
                trailRend[i].enabled = false;
            }

            //returns false informing that the ray did not hit.
            return false;
        }
    }

    //Fires food and takes a parameter (0 or 1)
    IEnumerator FireFood(short fireSide)
    {
        //Set fire to true so that player cant re-fire
        justFired = true;

        AudioSource throwSound = firePoints[fireSide].GetComponent<AudioSource>();
        throwSound.pitch = Random.Range(0.5f, 1);
        throwSound.Play();

        //Instantiates food transform.
        Transform food = Instantiate(foodPrefab, firePoints[fireSide].position, transform.rotation) as Transform;
        //Delacres the throw vector to be the point between the food and the anchor.
        Vector3 throwVector = food.position - anchorPoint.position;

        //Gets the rigidbody of the food just instantiated.
        Rigidbody foodRB = food.GetComponent<Rigidbody>();

        //Adds force to food.
        foodRB.AddForce((throwVector * throwForce) + (movement * 0.035f), ForceMode.Impulse);
        
        //Adds a cool little spin to the pizza box.
        foodRB.AddTorque
            (
            //Spin is little even though numbers are big.
            new Vector3(0,Random.Range(900,1000),0),
            ForceMode.Impulse
            );

        //Waits for 0.2 seconds and resets justFired.
        yield return new WaitForSeconds(0.15f);
        //Reset justFired.
        justFired = false;
    }


}

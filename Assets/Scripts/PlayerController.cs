using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    //Which layer the Raycast will hit (Currently set to ground layer).
    public LayerMask groundCheckLayer = 8;
    //Prefab of food game object.
    public Transform foodPrefab;

    //Declares a game master.
    private GameMaster gameMaster;
    //Declares rigidbody.
    private Rigidbody rb;
    //Delares an array of trail renderers (2).
    private TrailRenderer[] trailRend = new TrailRenderer[2];
    //Defines movement vector.
    private Vector3 movement;
    //Declares the players move speed.
    private float moveSpeed = 1200;
    //Declares the speed which the player will rotate at when mpving in a different direction.
    private float rotationSpeed = 20;
    //Declares an audio source for the motor.
    private AudioSource motorAudio;

    private AudioSource[] crashAudio = new AudioSource[2];

    //Accessor for disableInput for security.
    private bool disableInput = false;
    public bool DisableInput
    {
        set
        {
            disableInput = value;
        }
    }

    //Throw distance.
    private float throwForce = 10f;
    //Rate of fire.
    private float fireRate = 1f;
    //Checks when last shot was taken.
    private float lastShot = 0;
    //Fire Points, one on each side.
    private Transform[] firePoints = new Transform[2];
    //Anchor point for both ground check and the throw vector.
    private Transform anchorPoint;

    void Start()
    {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();

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

        //Declares audio source.
        motorAudio = GetComponent<AudioSource>();

        //Finds the two crash audios (Minor and Major).
        crashAudio[0] = transform.Find("CrashAudio[0]").GetComponent<AudioSource>();
        crashAudio[1] = transform.Find("CrashAudio[1]").GetComponent<AudioSource>();

        //Sets the upgrades to the players upgrades.
        SetUpgrades();
    }


    void FixedUpdate()
    {
        //Bool just to stop player movement while menu is up.
        if (!disableInput)
        {
            Drive();
        }
    }


    void Update()
    {
        //If input is not disabled.
        if (!disableInput)
        {
            //If Fire 1 is clicked and has not just fired.
            if (Input.GetButton("Fire1") && Time.time - lastShot > 1 / fireRate)
            {
                lastShot = Time.time;
                //Fire.
                //StartCoroutine(FireFood(0));
                FireFood(0);
            }
            //If Fire 2 is clicked and has not just fired.
            else if (Input.GetButton("Fire2") && Time.time - lastShot > 1 / fireRate)
            {
                lastShot = Time.time;
                //Fire.
                //StartCoroutine(FireFood(1));
                FireFood(1);
            }
            //LT BUTTON.
            else if (Input.GetAxis("LT/RT") > 0.8f && Time.time - lastShot > 1 / fireRate)
            {
                lastShot = Time.time;
                //StartCoroutine(FireFood(0));
                FireFood(0);
            }
            //RT BUTTON.
            else if (Input.GetAxis("LT/RT") < -0.8f && Time.time - lastShot > 1 / fireRate)
            {
                lastShot = Time.time;
                //StartCoroutine(FireFood(1));
                FireFood(1);
            }
        }

        //If the pause button is pressed.
        if (Input.GetButtonDown("Pause"))
        {
            //Either pause or unpause depending on current state.
            gameMaster.PauseGame();
        }
    }

    //Enters the colliders trigger zone.
    public void OnTriggerEnter(Collider other)
    {
        //If it is a pickup.
        if (other.tag == "pickup")
        {
            //Destroy the gameo object.
            Destroy(other.gameObject);
        }
    }

    //Actually hits the collider.
    public void OnCollisionEnter(Collision collision)
    {
        //If the collision isnt with the food or the ground.
        if (collision.gameObject.tag != "Food" && collision.gameObject.tag != "Ground")
        {
            //If the player is travelling faster than a certain speed.
            if (Vector3.SqrMagnitude(rb.velocity) > 50)
            {
                //Play the major crash.
                crashAudio[1].Play();
            }
            //If the player is travelling at less than the previous speed but still faster than a tap.
            else if (Vector3.SqrMagnitude(rb.velocity) > 10)
            {
                //Play the minor crash.
                crashAudio[0].Play();
            }
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

        //Moves position.
        rb.AddForce(movement, ForceMode.Force);

        //Changes the pitch of the audio depending on the speed of the player.
        motorAudio.pitch = Vector3.SqrMagnitude(rb.velocity) * 0.002f;
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

    void FireFood(int fireSide)
    {
        //Finds the audio source connected to the fire point.
        AudioSource throwSound = firePoints[fireSide].GetComponent<AudioSource>();
        //Randomises the pitch.
        throwSound.pitch = Random.Range(0.5f, 1);
        //Plays the sound.
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
            new Vector3(0, Random.Range(900, 1000), 0),
            ForceMode.Impulse
            );
    }


    //Applies vehicle upgrades to the vehicle.
    void SetUpgrades()
    {
        //Declares 3 floats to act as upgrades.
        float[] bonuses = new float[3];

        //Sets the 3 floats to the values of the saved bonuses.
        bonuses[0] = PlayerPrefs.GetFloat("SpeedBonus");
        bonuses[1] = PlayerPrefs.GetFloat("BrakingBonus");
        bonuses[2] = PlayerPrefs.GetFloat("DeliveryBonus");


        //If the values of the bonuses are 0, set them to their default values.
        if (moveSpeed < 1200)
        {
            moveSpeed = 1200;
        }
        //If not, set them to their upgraded values.
        else
        {
            moveSpeed = bonuses[0];
        }

        if (rb.drag < 1.2f)
        {
            rb.drag = 1.2f;
        }
        else
        {
            rb.drag = bonuses[1];
        }

        if (fireRate < 1.5f)
        {
            fireRate = 1.5f;
        }
        else
        {
            fireRate = bonuses[2];
        }

        //Debug for testing.
        Debug.Log("New Speed: " + moveSpeed + "New FireRate: " + fireRate + " Drag: " + rb.drag);
    }


}

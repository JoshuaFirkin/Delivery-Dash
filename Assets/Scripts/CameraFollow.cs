using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    //Gets players transform component.
    private Transform playerTransform;
    //Defines the speed of the camera.
    private float followSpeed = 10f;


	void Start ()
    {
        //Defines where to find the players transform component.
        playerTransform = GameObject.Find("Player").transform;
	}

    //Executes after both update and fixed update.
    void LateUpdate()
    {
        //Sets target position X as the player position X.
        Vector3 targetPos = playerTransform.position;
        //Sets target position Y to be the cameras Y value.
        targetPos.y = transform.position.y;
        //Sets target position Z as the player position Z - focus range.
        targetPos.z = playerTransform.position.z - 15;

        //Lerps the cameras transform.position towards the players X and Z coordinates.
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, followSpeed * Time.deltaTime), targetPos.y, Mathf.Lerp(transform.position.z, targetPos.z, followSpeed * Time.deltaTime));
    }
}

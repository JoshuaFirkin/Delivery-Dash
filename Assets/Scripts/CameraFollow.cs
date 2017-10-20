using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    //Defines the speed of the camera.
    public float followSpeed = 10f;
    public float offsetY;
    public float offsetZ;

    //Gets players transform component.
    private Transform playerTransform;


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
        targetPos.y = playerTransform.position.y + offsetY;
        //Sets target position Z as the player position Z - focus range.
        targetPos.z = playerTransform.position.z - offsetZ;

        //Lerps the cameras transform.position towards the players X and Z coordinates.
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, followSpeed * Time.deltaTime), targetPos.y, Mathf.Lerp(transform.position.z, targetPos.z, followSpeed * Time.deltaTime));
    }
}

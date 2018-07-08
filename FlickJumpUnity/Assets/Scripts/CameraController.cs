using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public BlobController blob;
    [Tooltip("Maximum allowed distance between jumper and camera center")]
    public float distanceFromCam;
    public float moveSpeed;

    // These values don't change and are used for quick access
    private float posX;
    private float posZ;

	void Awake () {
        posX = transform.position.x;
        posZ = transform.position.z;
	}
	
	void Update () {
        float jumperY = blob.transform.position.y;
        float desiredCameraY;

        if (blob.IsAscending()) {
            desiredCameraY = jumperY + distanceFromCam;
        } else {
            // Prevent too big movements when jumping down
            desiredCameraY = jumperY - (distanceFromCam / 2);
        }

        Vector3 targetPos = new Vector3(posX, desiredCameraY, posZ);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed);
	}
}

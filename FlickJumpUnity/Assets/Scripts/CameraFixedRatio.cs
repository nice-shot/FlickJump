using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixedRatio : MonoBehaviour {

    public float targetHeight = 16f;
    public float targetWidth = 9f;

	void Start () {
        Camera cam = GetComponent<Camera>();


        float targetRatio = targetWidth / targetHeight;
        float currentRatio = (float)Screen.width / (float)Screen.height;
        float scaleHeight = currentRatio / targetRatio;


        // Solution based on: http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
        cam.orthographicSize /= scaleHeight;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAngleDisplay : MonoBehaviour {

    public GameObject angleDisplay;
    public GameObject arrowPivot;

    private bool isHidden;
    private float angleLimit;
    private float minDragDistance;
    private Vector2 touchStart;
    private Direction facingDirection;
	
	void Update () {
        if (isHidden) {
            return;
        }
		
        if (Input.GetMouseButton(0)) {
            Vector2 swipe = (Vector2)Input.mousePosition - touchStart;
            float angle = Vector2.SignedAngle(Vector2.up, swipe);
            arrowPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            float ySize = Mathf.Min(swipe.sqrMagnitude / minDragDistance, 1f);
            arrowPivot.transform.localScale = new Vector3(1f, ySize);
        }
	}

    public void Show(Vector2 touchStart, Direction facingDirection,
                     float angleLimit, float minDragDistance) {
        isHidden = false;
        angleDisplay.SetActive(true);

        this.touchStart = touchStart;
        this.facingDirection = facingDirection;
        this.minDragDistance = minDragDistance;
        this.angleLimit = angleLimit;
            
        float angle = 0f;
        switch (facingDirection) {
            case Direction.LEFT:
                angle = 90f;
                break;
            case Direction.RIGHT:
                angle = -90f;
                break;
            case Direction.DOWN:
                angle = 180f;
                break;
        }

        angleDisplay.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        // Probably should play animation here
    }

    public void Hide() {
        isHidden = true;
        angleDisplay.SetActive(false);
    }

    public bool IsHidden() {
        return isHidden;
    }
}

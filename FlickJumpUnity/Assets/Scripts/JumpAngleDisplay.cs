using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAngleDisplay : MonoBehaviour {


    void Start () {
		
	}
	
	void Update () {
		
	}

    public void Show(Vector2 touchStart, Direction facingDirection,
                     float angleLimit, bool useShortGraphic=false) {
        Debug.Log("Showing arrow");
    }

    public void Hide() {
        Debug.Log("Hiding arrow");
    }
}

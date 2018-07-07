using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour {

    public Vector2 initialVelocity;

    // Components
    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer sprite;
    private Vector3 startingPosition;

    // Animation params
    private int animPreparing = Animator.StringToHash("Preparing");
    private int animFacingUp = Animator.StringToHash("animFacingUp");
    private int animJump = Animator.StringToHash("Jump");
    private int animHitWall = Animator.StringToHash("HitWall");

    // Drag variables
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private float minDragDistance;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        startingPosition = transform.position;
        // Drag distnace is 15% height of screen
        minDragDistance = Mathf.Pow((Screen.height * 15 / 100), 2);
        
        // Blob should start mid air and move to wall
        body.velocity = initialVelocity;
    }
	
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Wall")) {
            animator.SetTrigger(animHitWall);
            body.velocity = Vector2.zero;
        }
    }
}

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
        float jumpAngle = Vector2.SignedAngle(Vector2.right, initialVelocity);
        sprite.transform.rotation = Quaternion.Euler(0f, 0f, jumpAngle);
    }
	
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Wall")) {
            animator.SetTrigger(animHitWall);
            body.velocity = Vector2.zero;
            sprite.transform.rotation = Quaternion.identity;
            // Change sprite side

            // Creating an array to safely get the contact point without creating garbage
            ContactPoint2D[] contactPoints = new ContactPoint2D[1];
            collision.GetContacts(contactPoints);
            bool facingRight = contactPoints[0].point.x < transform.position.x;
            sprite.flipX = facingRight;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour {

    public Vector2 initialDirection;
    public float jumpSpeed;

    // Components
    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer sprite;
    private Vector3 startingPosition;

    // Animation params
    private int animPreparing = Animator.StringToHash("Preparing");
    private int animFacingUp = Animator.StringToHash("FacingUp");
    private int animJump = Animator.StringToHash("Jump");
    private int animHitWall = Animator.StringToHash("HitWall");

    // Visual varriables
    private bool facingRight;
    private bool facingUp;
    
    
    // Drag variables
    private bool onWall = false;
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
        body.velocity = initialDirection.normalized * jumpSpeed;
        float jumpAngle = Vector2.SignedAngle(Vector2.right, initialDirection);
        sprite.transform.rotation = Quaternion.Euler(0f, 0f, jumpAngle);
    }
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
            touchStart = (Vector2)Input.mousePosition;
            touchEnd = touchStart;
            animator.ResetTrigger(animJump);
            animator.SetBool(animPreparing, true);
        }

        if (Input.GetMouseButtonUp(0)) {
            animator.SetBool(animPreparing, false);
            touchEnd = (Vector2)Input.mousePosition;
            Vector2 swipe = touchEnd - touchStart;
            facingUp = touchStart.y < touchEnd.y;
            animator.SetBool(animFacingUp, facingUp);

            if (onWall && swipe.sqrMagnitude > minDragDistance) {
                // Jump
                body.velocity = swipe.normalized * jumpSpeed;

                onWall = false;
                animator.SetTrigger(animJump);
                // Change sprite direction
                Vector2 compareAngle = facingRight ? Vector2.right : Vector2.left;
                float jumpAngle = Vector2.SignedAngle(compareAngle, swipe);
                sprite.transform.rotation = Quaternion.Euler(0f, 0f, jumpAngle);
            }
        }
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Wall")) {
            animator.SetTrigger(animHitWall);
            body.velocity = Vector2.zero;
            sprite.transform.rotation = Quaternion.identity;

            // Creating an array to safely get the contact point without creating garbage
            ContactPoint2D[] contactPoints = new ContactPoint2D[1];
            collision.GetContacts(contactPoints);

            // Change sprite direction
            facingRight = contactPoints[0].point.x < transform.position.x;
            sprite.flipX = facingRight;

            onWall = true;
        }
    }
}

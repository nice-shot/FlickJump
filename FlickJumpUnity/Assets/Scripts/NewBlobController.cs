using UnityEngine;
using System.Collections;

public class NewBlobController : MonoBehaviour {
    // Public parameters
    public float jumpSpeed;
    public float jumpAngleLimit;
    public float firstJumpAngleLimit;
    public Vector2 initialVelocity;
    public BlobState initialState;
    public Direction initialDirection;

    // States
    private bool firstJump;
    private Vector3 startingPosition;
    private Direction currentDirection;
    private BlobState currentState;

    // Components
    private Animator animator;
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private JumpAngleDisplay angleDisplay;

    // Animation params
    private int animPreparing = Animator.StringToHash("Preparing");
    private int animJumping = Animator.StringToHash("Jumping");
    private int animDied = Animator.StringToHash("Died");

    // Flick Variables
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private float minDragDistance;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        angleDisplay = GetComponent<JumpAngleDisplay>();

        // Drag distnace is 15% height of screen
        minDragDistance = Mathf.Pow((Screen.height * 15 / 100), 2);

        startingPosition = transform.position;
        Restart();
    }

    private void Restart() {
        body.position = startingPosition;
        body.velocity = initialVelocity.normalized * jumpSpeed;
        currentState = initialState;
        currentDirection = initialDirection;
        firstJump = true;

        // Allows starting with jump or with sticking to wall
        if (currentState == BlobState.JUMPING) {
            animator.Play("Blob_Jump");
            animator.SetBool(animJumping, true);
        } else {
            animator.Play("Blob_OnWall");
            animator.SetBool(animJumping, false);
        }

        ChangeSpriteDirection();
    }

    private void ChangeSpriteDirection() {
        // When jumping we don't care about the direction state
        if (currentState == BlobState.JUMPING) {
            float jumpAngle = Vector2.SignedAngle(Vector2.up, body.velocity);
            if (jumpAngle > 0f) {
                sprite.flipX = true;
                sprite.transform.rotation = Quaternion.Euler(0f, 0f, jumpAngle + 90f);
            } else {
                sprite.flipX = false;
                sprite.transform.rotation = Quaternion.Euler(0f, 0f, jumpAngle + -90f);
            }
            return;
        }

        // This happens on walls and when dead
        switch (currentDirection) {
            case Direction.LEFT:
                sprite.flipX = false;
                sprite.transform.rotation = Quaternion.identity;
                break;
            case Direction.RIGHT:
                sprite.flipX = true;
                sprite.transform.rotation = Quaternion.identity;
                break;
            case Direction.UP:
                sprite.flipX = false;
                sprite.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
            case Direction.DOWN:
                sprite.flipX = false;
                sprite.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
        }
    }


    void Update() {
        if (currentState == BlobState.DEAD) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            // Set touch position
            touchStart = (Vector2)Input.mousePosition;
            touchEnd = touchStart;
            // Set animation preparing
            animator.SetBool(animPreparing, true);
        }

        if (Input.GetMouseButtonUp(0)) {
            animator.SetBool(animPreparing, false);
        }
    }

    // Called via the animation so it'll only displayed when on the wall
    public void ShowAngleDisplay() {
        angleDisplay.Show(touchStart, currentDirection, jumpAngleLimit); 
    }
}

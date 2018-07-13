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
    private JumpAngleDisplay angleDisplay;

    // Animation params
    private int animPreparing = Animator.StringToHash("Preparing");

    // Flick Variables
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private float minDragDistance;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        } else {
            animator.Play("Blob_OnWall");
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

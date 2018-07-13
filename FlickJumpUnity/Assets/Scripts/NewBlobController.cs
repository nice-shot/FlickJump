using UnityEngine;
using System.Collections;

public class NewBlobController : MonoBehaviour {
    // Public parameters
    public float jumpSpeed;
    public float jumpAngleLimit;
    public bool useAngleDisplay;
    public Vector2 initialVelocity;
    public BlobState initialState;
    public Direction initialDirection;
    public GameObject gameOverScreen;

    // States
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

    public void Restart() {
        gameOverScreen.SetActive(false);
        angleDisplay.Hide();
        body.position = startingPosition;
        body.velocity = initialVelocity.normalized * jumpSpeed;
        currentState = initialState;
        currentDirection = initialDirection;

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
            angleDisplay.Hide();
            animator.SetBool(animPreparing, false);
            touchEnd = (Vector2)Input.mousePosition;
            Vector2 swipe = touchEnd - touchStart;
            Jump(swipe);
        }
    }

    private void Jump(Vector2 jumpVector) {
        // Can't jump in mid air
        if (currentState != BlobState.ON_WALL) {
            Debug.Log("Not on wall - can't jump");
            return;
        }

        // Didn't drag enough to register swipe
        if (jumpVector.sqrMagnitude < minDragDistance) {
            Debug.Log("Not enough drag to jump");
            return;
        }

        Vector2 compareAngle;

        switch (currentDirection) {
            case Direction.LEFT:
                compareAngle = Vector2.left;
                break;
            case Direction.RIGHT:
                compareAngle = Vector2.right;
                break;
            case Direction.UP:
                compareAngle = Vector2.up;
                break;
            default:
                compareAngle = Vector2.down;
                break;
        }

        // Drag is towards the wall we're stuck on
        if (Vector2.Angle(compareAngle, jumpVector) > (90f - jumpAngleLimit)) {
            Debug.Log("Trying to jump to wall we're on");
            return;   
        }

        // Jumping:
        body.velocity = jumpVector.normalized * jumpSpeed;
        currentState = BlobState.JUMPING;
        animator.SetBool(animJumping, true);
        ChangeSpriteDirection();
        angleDisplay.Hide();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Stop movement and jumping animation
        body.velocity = Vector2.zero;
        animator.SetBool(animJumping, false);

        // Check collision direction
        ContactPoint2D[] contactPoints = new ContactPoint2D[2];
        collision.GetContacts(contactPoints);
        Vector2 firstContact = contactPoints[0].point;
        Vector2 secondContact = contactPoints[1].point;

        // We've hit a vertical wall
        if (Mathf.Approximately(firstContact.x, secondContact.x)) {
            if (firstContact.x > transform.position.x) {
                currentDirection = Direction.LEFT;
            } else {
                currentDirection = Direction.RIGHT;
            }
        } else { // We've probably hit a horizontal wall
            if (firstContact.y > transform.position.y) {
                currentDirection = Direction.DOWN;
            } else  {
                currentDirection = Direction.UP;
            }
        }

        if (collision.transform.CompareTag("Spike")) {
            currentState = BlobState.DEAD;
            animator.SetTrigger(animDied);
            gameOverScreen.SetActive(true);
        }

        if (collision.transform.CompareTag("Wall")) {
            currentState = BlobState.ON_WALL;
        }

        ChangeSpriteDirection();
    }

    // Called via the animation so it'll only displayed when preparing
    public void ShowAngleDisplay() {
        if (useAngleDisplay && angleDisplay.IsHidden()) {
            angleDisplay.Show(touchStart, currentDirection,
                              jumpAngleLimit, minDragDistance); 
        }
    }
}

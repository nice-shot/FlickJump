using UnityEngine;
using System.Collections;

public class BlobControllerNew : MonoBehaviour {
    // Public parameters
    public Direction initialDirection;
    public float jumpAngleLimit;
    public float initialJumpAngleLimit;

    // States
    private Direction currentDirection;
    private BlobState currentState;
    private bool firstJump = true;

    // Components
    private Animator animator;
    private JumpAngleDisplay angleDisplay;

    // Animation params
    private int animPreparing = Animator.StringToHash("Preparing");

    // Flick Variables
    private Vector2 touchStart;
    private Vector2 touchEnd;

    void Awake() {
        animator = GetComponent<Animator>();
        angleDisplay = GetComponent<JumpAngleDisplay>();
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
            
        }
    }

    // Called via the animation so it'll only displayed when on the wall
    public void ShowAngleDisplay() {

        angleDisplay.Show(touchStart, currentDirection, jumpAngleLimit); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperController : MonoBehaviour {

    public float ascentSpeed;
    public float sideSpeed;
    public float jumpSpeed;
    public GameObject gameOverText;
    public GameObject restartButton;

    private Rigidbody2D body;
    private bool jumping = false;
    private bool facingRight = true;
    private Vector3 startingPosition;

    private Vector2 touchStart;
    private Vector2 touchEnd;
    private float minDragDistance;


	void Awake () {
        body = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        // Drag distnace is 15% height of screen
        minDragDistance = Mathf.Pow((Screen.height * 15 / 100), 2);
	}
	
	void Update () {
        if (!jumping) {
            // Run up wall
            body.AddForce(Vector2.up * ascentSpeed * Time.deltaTime);
            if (facingRight) {
                body.AddForce(Vector2.right * sideSpeed * Time.deltaTime);
            } else {
                body.AddForce(Vector2.left * sideSpeed * Time.deltaTime);
            }

            // Check for swipe
            if (Input.GetMouseButtonDown(0)) {
                touchStart = (Vector2)Input.mousePosition;
                touchEnd = touchStart;
            }

            if (Input.GetMouseButtonUp(0)) {
                touchEnd = (Vector2)Input.mousePosition;

                Vector2 swipe = touchEnd - touchStart;
                // Make sure it's a drag and not a tap
                if (swipe.sqrMagnitude > minDragDistance) {
                    // Stop all other forces before jump
                    body.velocity = Vector2.zero;

                    Debug.Log("Jumping:");
                    // Normalizing to only look at angle and not distance
                    Debug.Log((swipe.normalized * jumpSpeed).ToString("F4"));
                    body.AddForce(swipe.normalized * jumpSpeed);
                    jumping = true;
                }
            }
        }
	}

    void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.CompareTag("Spike")) {
            GameOver();
        }

        if (other.transform.CompareTag("Wall")) {
            body.velocity = Vector2.zero;
            jumping = false;
            facingRight = other.transform.position.x > transform.position.x;
        }
    }

    void GameOver() {
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        body.simulated = false;
        gameOverText.SetActive(true);
        // Maybe add a slight delay before showing this
        restartButton.SetActive(true);
    }

    public void Restart() {
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
        transform.position = startingPosition;
        jumping = false;
        facingRight = true;
        body.simulated = true;
        touchStart = Vector2.zero;
        touchEnd = Vector2.zero;
        Debug.Log("Current velocity:");
        Debug.Log(body.velocity.ToString("F4"));
    }
}

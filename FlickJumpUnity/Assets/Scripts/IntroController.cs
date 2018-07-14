using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour {

    public Image dragBtnImage;
    public Animator mainTitleAnimator;
    public BlobController blob;
    public float firstJumpAngleLimit;

    public Sprite dragBtnPressedSprite;

    private bool done = false;
    private float originalJumpAngleLimit;
    private Sprite dragBtnUnpressedSprite;

    // Animation params
    private int animFirstTouch = Animator.StringToHash("FirstTouch");
    private int animJumped = Animator.StringToHash("Jumped");

    private void Start() {
        dragBtnUnpressedSprite = dragBtnImage.sprite;
        originalJumpAngleLimit = blob.jumpAngleLimit;
        // Change blob's first jump so you can't jump to spikes on start
        blob.jumpAngleLimit = firstJumpAngleLimit;
    }

    void Update() {
        if (done) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            mainTitleAnimator.SetBool(animFirstTouch, true);
            dragBtnImage.sprite = dragBtnPressedSprite;
        }

        if (Input.GetMouseButtonUp(0)) {
            dragBtnImage.sprite = dragBtnUnpressedSprite;
        }

        if (blob.GetNumOfJumps() >= 1) {
            blob.jumpAngleLimit = originalJumpAngleLimit;
            //mainTitleAnimator.SetBool(animJumped, true);
            done = true;
            // Should use animation event or something
            mainTitleAnimator.gameObject.SetActive(false);
        }
    }
}

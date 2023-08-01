using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpFlash = 5f;
    [SerializeField] float ClimbSpeed = 5f;
    [SerializeField] float dieSpeedy = 10f;
    [SerializeField] float underWaterSpeed = 5f;
    Vector2 moveInput;
    Rigidbody2D flashRigidbody;
    Animator flashAnimator;
    CapsuleCollider2D flashCapsuleCollider;
    BoxCollider2D feetCollider;
    float gravityStart;
    bool isAlive = true;
    

    void Start()
    {
        flashRigidbody = GetComponent<Rigidbody2D>();
        flashAnimator = GetComponent<Animator>();
        flashCapsuleCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        gravityStart = flashRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isAlive){return; }
        Run();
        FlipSprite();
        Climb();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive){return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive){return; }
        if(!flashCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if(value.isPressed)
        {
             flashRigidbody.velocity += new Vector2(0f, jumpFlash);
        }

    }

    void Run()
    {
        Vector2 flashMove = new Vector2 (moveInput.x * runSpeed, flashRigidbody.velocity.y);
        flashRigidbody.velocity = flashMove;
        bool flashHorizontalSpeed = Mathf.Abs(flashRigidbody.velocity.x) > Mathf.Epsilon;
        flashAnimator.SetBool("isRunning", flashHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool flashHorizontalSpeed = Mathf.Abs(flashRigidbody.velocity.x) > Mathf.Epsilon;
        if (flashHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(flashRigidbody.velocity.x),1f);
        }
    }

    void Climb()
    {
        if(!flashCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            flashRigidbody.gravityScale = gravityStart;
            flashAnimator.SetBool("isClimbing", false);
            return;
        }
        Vector2 flashClimb = new Vector2 (flashRigidbody.velocity.x, moveInput.y * ClimbSpeed);
        flashRigidbody.velocity = flashClimb;
        flashRigidbody.gravityScale = 0f;
        bool flashVerticalSpeed = Mathf.Abs(flashRigidbody.velocity.y) > Mathf.Epsilon;
        flashAnimator.SetBool("isClimbing", flashVerticalSpeed);
    }

    void Die()
    {
        if(flashCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazard")))
        {
            isAlive = false;
            flashAnimator.SetTrigger("Dying");
            flashRigidbody.velocity = new Vector2(0f,  dieSpeedy);
            flashCapsuleCollider.isTrigger = true;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void Underwater()
    {
        while(flashCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            Vector2 flashMove = new Vector2 (1f, 0f);
            flashRigidbody.velocity = flashMove;
            bool flashHorizontalSpeed = Mathf.Abs(flashRigidbody.velocity.x) > Mathf.Epsilon;
            flashAnimator.SetBool("isRunning", flashHorizontalSpeed);
        }
    }

}

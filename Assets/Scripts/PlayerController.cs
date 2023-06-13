using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    

    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    CapsuleCollider2D playerCollider;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
                
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();

        if (!PlayerHasVerticalSpeed())
            playerAnimator.SetBool("isJumping", false);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && IsTouchingPlatforms())
        {
            Jump();
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * movementSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;

        playerAnimator.SetBool("isRunning", PlayerHasHorizontalSpeed());
    }

    void ClimbLadder()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            Vector2 playerVelocity = new Vector2(playerRigidbody.velocity.x, moveInput.y * movementSpeed);
            playerRigidbody.velocity = playerVelocity;

            playerAnimator.SetBool("isClimbing", PlayerHasVerticalSpeed());
        }
    }

    void Jump()
    {   
        playerRigidbody.velocity += new Vector2(0f, jumpSpeed);
        playerAnimator.SetBool("isJumping", true);
        
    }

    void FlipSprite()
    {
        if (PlayerHasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }
    }

    bool PlayerHasHorizontalSpeed()
    {
        return Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
    }

    bool PlayerHasVerticalSpeed()
    {
        return Mathf.Abs(playerRigidbody.velocity.y) > 1;
    }

    bool IsTouchingPlatforms()
    {
        return playerCollider.IsTouchingLayers(LayerMask.GetMask("Platforms"));
    }
}

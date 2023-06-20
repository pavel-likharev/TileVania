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
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float shootSpeed = 5f;
    [SerializeField] Vector2 deathkick = new Vector2(10f, 10f);
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletSpawnPoint;
    
    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    CapsuleCollider2D playerCollider;
    BoxCollider2D playerColliderJump;
    
    float gravityScale;

    bool isALive = true;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerColliderJump = GetComponent<BoxCollider2D>();

        gravityScale = playerRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isALive) { return; }

        Run();
        FlipSprite();
        ClimbLadder();


        playerAnimator.SetBool("isJumping", PlayerHasVerticalSpeed());

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isALive) { return; }

        if (value.isPressed && IsTouchingPlatforms())
        {
            Jump();
        }
    }

    void OnFire(InputValue value)
    {
        if (!isALive) { return; }

        if (value.isPressed)
        {
            Fire();

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
            Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x, moveInput.y * climbSpeed);
            playerRigidbody.velocity = climbVelocity;
            playerRigidbody.gravityScale = 0;


            playerAnimator.SetBool("isClimbing", PlayerHasVerticalSpeed());
            playerAnimator.SetBool("isClimbingPause", !PlayerHasVerticalSpeed());

        }
        else 
        {
            playerRigidbody.gravityScale = gravityScale;
            playerAnimator.SetBool("isClimbingPause", false);
            playerAnimator.SetBool("isClimbing", false);
        }
    }

    void Jump()
    {   
        playerRigidbody.velocity += new Vector2(0f, jumpSpeed);
        playerAnimator.SetBool("isJumping", true);
        
    }

    void Fire()
    {
        if (playerAnimator.GetBool("isRunning"))
            playerAnimator.Play("PlayerRunAndShoot");
        else
            playerAnimator.Play("PlayerShoot");

        Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);
    }

    void FlipSprite()
    {
        if (PlayerHasHorizontalSpeed())
        {
            Vector2 vectorFlip = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
            transform.localScale = vectorFlip;        
        }
    }

    bool PlayerHasHorizontalSpeed()
    {
        return Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
    }

    bool PlayerHasVerticalSpeed()
    {
        return Mathf.Abs(playerRigidbody.velocity.y) > 2;
    }

    void Die()
    {
        isALive = false;
        playerAnimator.SetTrigger("Dying");
        playerRigidbody.velocity = deathkick;
    }

    bool IsTouchingPlatforms()
    {
        return playerColliderJump.IsTouchingLayers(LayerMask.GetMask("Platforms"));
    }

    bool IsTouchingEnemy()
    {
        return playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy"));
    }

    bool IsTouchingLava()
    {
        return playerCollider.IsTouchingLayers(LayerMask.GetMask("Lava"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsTouchingPlatforms())
        {
            playerAnimator.SetBool("isJumping", false);
        }

        if (IsTouchingLava() && isALive || IsTouchingEnemy() && isALive)
        {
            Die();
        }
    }
}

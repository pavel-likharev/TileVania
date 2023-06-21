using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D enemyRigidbody;
    BoxCollider2D enemyCollider;

    void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Vector2 movementVelocity = new Vector2(moveSpeed, enemyRigidbody.velocity.y);
        enemyRigidbody.velocity = movementVelocity;
    }

    void FlipEnemyBody()
    {
        transform.localScale = transform.localScale = new Vector2(Mathf.Sign(enemyRigidbody.velocity.x), 1f);
    }

    bool IsTouchingPlatforms()
    {
        return enemyCollider.IsTouchingLayers(LayerMask.GetMask("Platforms"));
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Platforms")
        {
            moveSpeed = -moveSpeed;
            FlipEnemyBody();
        }
    }
}

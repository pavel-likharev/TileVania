using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 5f;

    Rigidbody2D rigidbody;
    PlayerController player;
    float xSpeed;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        Vector2 vectorFlip = new Vector2(Mathf.Sign(player.GetComponent<Rigidbody2D>().velocity.x), 1f);
        transform.localScale = vectorFlip;
        xSpeed = player.transform.localScale.x * bulletSpeed;    
    }

    void Update()
    {
        rigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        
        Destroy(gameObject);
    }
}

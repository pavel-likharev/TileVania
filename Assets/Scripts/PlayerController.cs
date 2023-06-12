using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();    
    }

    void Start()
    {
                
    }

    void Update()
    {
        Run();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * movementSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;
    }
}

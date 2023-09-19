using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed;
    [SerializeField] float health;

    private float moveX;
    private float moveY;
    private Vector2 movement;

    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY);
        
        if(movement != Vector2.zero)
        {
            transform.up = movement;
        }
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    public void DamagePlayer(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            Debug.Log("you ded :(");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed;

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
        rb.AddForce(movement * movementSpeed * Time.deltaTime);
    }
}

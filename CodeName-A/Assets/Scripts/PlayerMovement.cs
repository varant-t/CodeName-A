using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    public Animator anim;
    public float speed;
    public int jumpForce;
    Rigidbody2D myRigidBody;
    public Vector2 jump;

    public bool isGrounded;
    public LayerMask isGroundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();

        if (jumpForce <= 0)
        {
            jumpForce = 5;
        }
        if (speed <= 0)
        {
            speed = 5f;
        }
        jump = new Vector2(0.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        myRigidBody.velocity = new Vector2(moveHorizontal * speed, myRigidBody.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            myRigidBody.velocity = Vector2.zero;

            myRigidBody.AddForce(Vector2.up * jumpForce);
            anim.SetTrigger("isJumping");
            Debug.Log("Jump");
        }

        anim.ResetTrigger("isJumping");

        
        
    }
}


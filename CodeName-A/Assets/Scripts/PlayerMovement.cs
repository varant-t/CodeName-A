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

        anim.SetBool("isGrounded", true);
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        myRigidBody.velocity = new Vector2(moveHorizontal * speed, myRigidBody.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }

        

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //anim.SetTrigger("isJumping");
            //anim.SetBool("isGrounded", false);
            
            myRigidBody.velocity = Vector2.zero;

            myRigidBody.AddForce(Vector2.up * jumpForce);


            Debug.Log("Jump");

           // anim.ResetTrigger("isJumping");
        }

      
        // Swap direction of sprite depending on walk direction
        if (moveHorizontal > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (moveHorizontal < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //Animation Updates and Trigger
        if (Mathf.Abs(moveHorizontal) > Mathf.Epsilon)
            anim.SetTrigger("isMoving");

        else if (Mathf.Abs(moveHorizontal) < Mathf.Epsilon)
            anim.SetTrigger("isIdle");


    }
}


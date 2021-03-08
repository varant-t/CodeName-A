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

    public GameObject[] phasedObjects;
    public GameObject[] phasedObjs;

    // Testing with GameObjects
    public GameObject platform1;
    public GameObject platform2;
    public bool phased;
    public SpriteRenderer backgroundRenderer;

    // Start is called before the first frame update
    void Start()
    {
        phased = false;

        //backgroundRenderer = GetComponent<SpriteRenderer>();
        //phasedObjects = new GameObject[10];
        
        //Debug.Log(phasedObjects.Length);
        //phasedObjs = new GameObject[100];
        phasedObjs = GameObject.FindGameObjectsWithTag("Phased");
        for (int i = 0; i < phasedObjs.Length; i++)
        {
            phasedObjs[i].SetActive(false);
        }
        //phasedObjects = new GameObject[tempArray.Length];
        //phasedObjects.Length
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

        // Ability to see hiden gameObjects
        if (Input.GetKeyDown(KeyCode.Q) && !phased)
        {
            backgroundRenderer.color =  new Color(0.01960784f, 0.07723299f, 0.1803922f, 1f);
         
            phased = true;
            Debug.Log(phasedObjs.Length);
            for(int i = 0; i < phasedObjs.Length; i++)
            {
                Debug.Log("Hello");
                phasedObjs[i].SetActive(true);
            }
            //platform1.SetActive(true);
            //platform2.SetActive(true);
            //for (int i = 0; i < phasedObjects.Length; i++)
            //{
            //    GameObject gameObjects = phasedObjects[i];
            //    gameObject.activeInHierarchy.Equals(true);
            //}
        }
        else if (Input.GetKeyDown(KeyCode.Q) && phased)
        {
            backgroundRenderer.color = new Color(0.120448f, 0.01944643f, 0.1792453f, 1f);
            phased = false;
            //platform1.SetActive(false);
            //platform2.SetActive(false);
            for (int i = 0; i < phasedObjs.Length; i++)
            {
                phasedObjs[i].SetActive(false);
            }
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


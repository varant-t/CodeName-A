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

    public bool phased;
    public SpriteRenderer backgroundRenderer;

    //Dashing Mechanics
    public float dashSpeed;
    public float dashTime;
    public float startDashTime;
    private int direction;
    //Dashing Cooldown
    private bool canDash = true;
    private float dashCooldown = 2f;
    public float timeSinceLastDash = 0;

    //Attacking Var
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 2;
    public float attackRate = 2f;
    private float nextAttackTime = 0;

    //Throwing vars
    public Transform spawnPoint;
    public float nextThrowingTime = 0f;
    public float throwingRate = 1f;
    public GameObject throwingWeapon;
    public float throwingForce = 30f;



    // Start is called before the first frame update
    void Start()
    {
        //Initialize Dashing
        dashTime = startDashTime;
        

        // Initialize that the starting world isnt not phased, and add all objects with Phased tag into array
        phased = false;
        phasedObjs = GameObject.FindGameObjectsWithTag("Phased");
        for (int i = 0; i < phasedObjs.Length; i++)
        {
            phasedObjs[i].SetActive(false);
        }
 
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
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if(Time.time >= nextThrowingTime)
        {
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameObject thrownWeapon = Instantiate(throwingWeapon, spawnPoint.position, spawnPoint.rotation);
                thrownWeapon.GetComponent<Rigidbody2D>().velocity = transform.right * speed;

                nextThrowingTime = Time.time + 1f / throwingRate;

                // Needs to be fixed, more distance to cover 

            }
        }
      


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
         
        }
        else if (Input.GetKeyDown(KeyCode.Q) && phased)
        {
            backgroundRenderer.color = new Color(0.120448f, 0.01944643f, 0.1792453f, 1f);
            phased = false;
            for (int i = 0; i < phasedObjs.Length; i++)
            {
                phasedObjs[i].SetActive(false);
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        { 
            myRigidBody.velocity = Vector2.zero;
            myRigidBody.AddForce(Vector2.up * jumpForce);
            Debug.Log("Jump");
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
    private void FixedUpdate()
    {
        // Dashing Checks
        timeSinceLastDash += Time.deltaTime;
        if (canDash == false && timeSinceLastDash >= dashCooldown)
        {
            canDash = true;
        }
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift);

        if (direction == 0)
        {
            if (transform.localScale.x == 1 && isShiftKeyDown && canDash)
            {
                direction = 1;
                canDash = false;
                timeSinceLastDash = 0;
            }
            else if (transform.localScale.x == -1 && isShiftKeyDown && canDash)
            {
                direction = 2;
                canDash = false;
                timeSinceLastDash = 0;
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                myRigidBody.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;
                if (direction == 1)
                {
                    myRigidBody.velocity = Vector2.left * dashSpeed;
                    timeSinceLastDash = 0;
                }
                else if (direction == 2)
                {
                    myRigidBody.velocity = Vector2.right * dashSpeed;
                    timeSinceLastDash = 0;
                }
            }
        }
    }

    void Attack()
    {
        // Play animation
        anim.SetTrigger("isAttacking");

        //  Deteck enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //  Apply damage

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyAI>().TakeDamage(attackDamage);
        }
    }

   

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}


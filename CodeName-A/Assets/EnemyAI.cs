using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Rigidbody2D enemyRB;
    SpriteRenderer enemySprite;
    Animator enemyAnim;
    public float speed;
    private bool goingLeft = true;
    private bool moving = true;
    private int constantDirection = -1;
    public GameObject patrolNode1;
    public GameObject patrolNode2;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        int moveHorizontal = 0;
        OutsidePatrolBounds();
        if (moving)
        {
            enemyAnim.SetInteger("AnimState", 2);
            if (goingLeft)
            {
                moveHorizontal = -1;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else
            {
                moveHorizontal = 1;
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            enemyAnim.SetInteger("AnimState", 0);
        }
       
        enemyRB.velocity = new Vector2(moveHorizontal * speed, enemyRB.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PatrolNode"))
        {
            moving = false;
            goingLeft = !goingLeft;
            Invoke("ContinuePatrol", .5f);

        }
    }
    private void ContinuePatrol()
    {
        constantDirection = -constantDirection;
        
        moving = true;
    }
    private void OutsidePatrolBounds()
    {
        if(transform.position.x > patrolNode1.transform.position.x && transform.position.x > patrolNode2.transform.position.x)
        {
            moving = true;
            goingLeft = true;
            transform.localScale = new Vector3(-transform.localScale.x, 1.0f, 1.0f);
        }
        else if (transform.position.x < patrolNode1.transform.position.x && transform.position.x < patrolNode2.transform.position.x)
        {
            moving = true;
            goingLeft = false;
            transform.localScale = new Vector3(-transform.localScale.x, 1.0f, 1.0f);
        }
    }
}

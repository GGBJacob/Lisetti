using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement parameters")][Range(0.01f, 20.0f)][SerializeField] private float MoveSpeed = 0.1f;
    private bool isFacingRight = false;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private float startPositionX;
    public float moveRange = 1.0f;
    private bool isMovingRight = false;
    private bool isDead = false;
    public bool isImmortal = false;
    // Start is called before the first frame update

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;
        isMovingRight = !isMovingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void MoveRight()
    {
        if (!isFacingRight)
        {
            flip();
        }
        transform.Translate(MoveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void MoveLeft()
    {
        if(isFacingRight)
        {
            flip();
        }
        transform.Translate(-MoveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isImmortal)
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<Collider2D>().bounds.center.y > this.GetComponent<Collider2D>().bounds.center.y)
                {
                    isDead = true;
                    animator.SetBool("isDead", true);
                    this.GetComponent<Collider2D>().enabled = false;
                    StartCoroutine(KillOnAnimationEnd());
                }
            }
        }
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            if (!isDead)
            {
                if (isMovingRight)
                {
                    if (this.transform.position.x < startPositionX + moveRange)
                    {
                        MoveRight();
                    }
                    else
                        MoveLeft();
                }
                else
                {
                    if (this.transform.position.x > startPositionX - moveRange)
                    {
                        MoveLeft();
                    }
                    else
                        MoveRight();
                }
            }
        }
    }
}

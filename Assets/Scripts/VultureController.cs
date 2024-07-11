using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureController : MonoBehaviour
{
    [Header("Movement parameters")][Range(0.01f, 20.0f)][SerializeField] private float MoveSpeed = 0.1f;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private int flipOnWaypoint1=0;
    [SerializeField] private int flipOnWaypoint2=0;
    int currentWaypoint = 0;
    private bool isFacingRight = false;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private float startPositionX;
    public float moveRange = 1.0f;
    private bool isMovingRight = false;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        if (isFacingRight)
        {
            flip();
        }
        transform.Translate(-MoveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.y > this.transform.position.y)
            {
                isDead = true;
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            if (!isDead)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, MoveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 0.1f)
                {
                    currentWaypoint = (++currentWaypoint) % waypoints.Length;
                    if (currentWaypoint == flipOnWaypoint1 || currentWaypoint == flipOnWaypoint2)
                        flip();
                }
            }
        }
    }

}

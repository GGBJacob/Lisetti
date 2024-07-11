using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour
{
    [Header("Movement parameters")][Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    private float startPositionX;
    public float moveRange = 1.0f;
    private bool isMovingRight = true;
    private bool isActive = false;
    public GameObject player;
    void Awake()
    {
        startPositionX = this.transform.position.x;
    }

    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        isMovingRight = true;
    }

    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        isMovingRight = false;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isActive = true;
            if (GameManager.instance.currentGameState == GameState.GS_GAME && isActive)
            {
                StartCoroutine(HandleMovement());
            }
        }
    }

    private IEnumerator HandleMovement()
    {
        bool isPlayerOn = true;
        float playerX = player.GetComponent<BoxCollider2D>().bounds.center.x + player.GetComponent<BoxCollider2D>().bounds.extents.x;
        if (this.GetComponent<BoxCollider2D>().bounds.center.x - (4*this.GetComponent<BoxCollider2D>().bounds.extents.x) < playerX && isPlayerOn)
        {
            if (isMovingRight)
            {
                if (this.transform.position.x < startPositionX + moveRange)
                {
                    MoveRight();
                }
                else
                {
                    yield return new WaitForSeconds(2.0f);
                    MoveLeft();
                }
            }
            else
            {
                if (this.transform.position.x >= startPositionX)
                {
                    MoveLeft();
                }
                else
                    MoveRight();
            }
            
        }
        else
        {
            isPlayerOn = false;
            if (this.transform.position.x >= startPositionX)
            {
                MoveLeft();
            }
        }
        if (this.transform.position.x <= startPositionX + 0.05f && !isMovingRight)
        {
            isActive = false;
            isMovingRight = true;
        }
    }

    public void Update()
    {
        if (isActive)
        {
            StartCoroutine(HandleMovement());
        }
    }

}

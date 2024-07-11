using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement parameters")][Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    private float startPositionX;
    public float moveRange = 1.0f;
    private bool isMovingRight = false;

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



    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
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

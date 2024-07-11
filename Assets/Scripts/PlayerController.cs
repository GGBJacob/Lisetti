using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerController : MonoBehaviour
{
    //~~~~~~~~~~Ruch~~~~~~~~~~
    [Header("Movement parameters")][Range(0.01f, 20.0f)][SerializeField] private float MoveSpeed = 0.1f; // moving speed of the player
    [Range(0.01f, 20.0f)][SerializeField][Space(10)] private float JumpForce = 6.0f;
    [Range(1.0f, 4.0f)] public float gravityFallMultiplier = 1.5f;


    private Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    private float rayLength = 0.4f;
    private Animator animator;
    public bool isWalking = false;
    private bool isFacingRight = true;
    private bool isDoubleJumpPossible = true;
    private bool isLadder = false;
    public bool isClimbing = false;
    private bool isTunnel = false;
    private float vertical;
    private float startGravity;


    //~~~~~~~~~~Klucze~~~~~~~~~~
    public int keysFound = 0;
    public int keyCount = 4;


    //~~~~~~~~~~Respawn~~~~~~~~~~
    private Vector2 startPosition;

    //~~~~~~~~~~DŸwiêki~~~~~~~~~~
    [SerializeField] AudioClip BonusSound;
    [SerializeField] AudioClip KeySound;
    [SerializeField] AudioClip LifeSound;
    [SerializeField] AudioClip FinishSound;
    [Header("Audio Sources")]public AudioSource mainSource;
    public AudioSource cherrySource;
    private float timeSinceLastCoin = 0.0f;
    private const float pitchIncrease = 0.05f;
    private float currentCherryPitch;
    private float originalPitch;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = this.transform.position;
        originalPitch= mainSource.pitch;
        startGravity = rigidBody.gravityScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool isGrounded()
    {
        Vector2 right = transform.position;
        Vector2 left = transform.position;
        Vector2 middle = transform.position;
        if (isFacingRight)
        {
            right.x += GetComponent<SpriteRenderer>().bounds.size.x / 8f;//metod¹ prób i b³êdów dopasowa³em te liczby aby siê te
            left.x -= GetComponent<SpriteRenderer>().bounds.size.x / 5f;//raye skalowa³y razem z playerem i mu przylega³y do stópek
        }
        else
        {
            right.x += GetComponent<SpriteRenderer>().bounds.size.x / 5f;//zapobiega zbugowanemu skakaniu (mniej wiêcej)
            left.x -= GetComponent<SpriteRenderer>().bounds.size.x / 8f;
        }
        right.y -= 0.7f;
        left.y -= 0.7f;
        middle.y -= 0.7f;
        Debug.DrawRay(right, rayLength * Vector3.down, Color.yellow, 1, false);
        Debug.DrawRay(left, rayLength * Vector3.down, Color.red, 1, false);
        Debug.DrawRay(middle, rayLength * Vector3.down, Color.blue, 1, false);
        if (!Physics2D.Raycast(right, Vector2.down, rayLength, groundLayer.value))//prawy ray
        {
            if(!Physics2D.Raycast(middle, Vector2.down, rayLength, groundLayer.value))//œrodkowy ray
                return (Physics2D.Raycast(left,Vector2.down, rayLength, groundLayer.value));//lewy ray
            return true;
        }
        return true;
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void respawn()
    {
        GameManager.instance.RemoveLife();
        //Debug.Log("You died!");
        //if (lives <= 0)
        //{
        //    isOver = true;
        //    this.gameObject.SetActive(false);
        //    Debug.Log("GAME OVER!");
        //}
        //else
        if(GameManager.instance.currentGameState != GameState.GS_GAME_OVER)
            this.transform.position = new Vector2(startPosition.x, startPosition.y);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bonus"))
        {
            Debug.Log(timeSinceLastCoin);
            GameManager.instance.AddPoints(10);
            other.gameObject.SetActive(false);
            if(timeSinceLastCoin<0.8f)
            {
                currentCherryPitch += pitchIncrease;
            }
            else
            {
                currentCherryPitch = originalPitch;
            }
            cherrySource.pitch = currentCherryPitch;
            cherrySource.PlayOneShot(BonusSound);
            timeSinceLastCoin = 0.0f;
        }
        else if(other.CompareTag("Flame"))
        {
            respawn();
        }

        else if (other.CompareTag("Enemy"))
        {
            if (this.GetComponent<Collider2D>().bounds.center.y > other.GetComponent<Collider2D>().bounds.center.y)
            {
                //GameManager.instance.AddPoints(50);
                GameManager.instance.IncreaseEnemiesKilled();
            }
            else
            {
                respawn();
            }
        }
        if (other.CompareTag("Key1") || other.CompareTag("Key2") || other.CompareTag("Key3") || other.CompareTag("Key4"))
        {
            other.gameObject.SetActive(false);
            mainSource.PlayOneShot(KeySound);
            if (other.CompareTag("Key1"))
                GameManager.instance.AddKeys(1);
            else if (other.CompareTag("Key2"))
                GameManager.instance.AddKeys(2);
            else if (other.CompareTag("Key3"))
                GameManager.instance.AddKeys(3);
            else if (other.CompareTag("Key4"))
                GameManager.instance.AddKeys(4);
        }

        else if(other.CompareTag("MovingPlatform"))
        {
            this.transform.SetParent(other.transform);
        }

        else if(other.CompareTag("Checkpoint"))
        {
            Debug.Log("Checkpoint!");
            startPosition = this.transform.position;
        }

        else if (other.CompareTag("Heart"))
        {
            GameManager.instance.AddLife();
            other.gameObject.SetActive(false);
            mainSource.PlayOneShot(LifeSound);
        }

        else if(other.CompareTag("Ladder"))
        {
            isLadder = true;
        }

        else if(other.CompareTag("Void"))
        {
            respawn();
        }

        else if(other.CompareTag("Wind"))
        {
            isTunnel = true;
        }

        else if(other.CompareTag("Finish"))
        {
            mainSource.pitch = originalPitch;
            GameManager.instance.CheckWin(mainSource,FinishSound);
            if (GameManager.instance.currentGameState == GameState.GS_LEVEL_COMPLETED)
            {
                isWalking = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("MovingPlatform"))
        {
            this.transform.SetParent(null);
        }
        if(other.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
        else if (other.CompareTag("Wind"))
        {
            isTunnel = false;
        }
    }

    void Jump()
    {
        if (isGrounded() || isDoubleJumpPossible)
        {
            rigidBody.mass = 1.0f;
            rigidBody.gravityScale = startGravity;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            rigidBody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            Debug.Log("jumping");
        }
        if ((!isGrounded() && isDoubleJumpPossible) || isTunnel)
        {
            isDoubleJumpPossible = false;
            Debug.Log("DOUBLE-jumping");
        }
    }

    bool isFalling()
    {
        if (rigidBody.velocity.y < -0.05)
        {
            return true;
        }
        return false;
    }
    bool enteredSlide()
    {
        if(PlayerPrefs.GetInt("ActiveLevel") == 1)
            if (transform.position.x <= -46.6f) //warunek na  sprawdzanie, czy gracz nie skoczy³
                return true;                    //po lewej na zje¿d¿alnie
        return false;
    }
    private void FixedUpdate()
    {
        if (isClimbing || isLadder)
        {
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, vertical * MoveSpeed);
        }
        else if (isTunnel && !isGrounded())
        {
            rigidBody.gravityScale = 0f;
            float currentYVelocity = rigidBody.velocity.y;
            if (rigidBody.velocity.y < 5f)
            {
                float newYVelocity = Mathf.Lerp(currentYVelocity, 5f, Time.fixedDeltaTime);
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, newYVelocity);
            }
        }
        else if (isFalling())
        {
            rigidBody.gravityScale = gravityFallMultiplier;
        }
        else 
        { 
            rigidBody.gravityScale = startGravity;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            vertical = Input.GetAxis("Vertical");
            if (isLadder)
            {
                if (Mathf.Abs(vertical) > 0.02f)
                    isClimbing = true;
                else
                    isClimbing = false;
            }
            if (isGrounded())
                isDoubleJumpPossible = true;
            isWalking = false;
            //transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World); //przesuwa w prawo
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))//je¿eli gracz jest na zje¿d¿alni to mo¿e tylko jechaæ w dó³, nie wróci do góry
            {
                if (isFacingRight)
                {
                    flip();
                }
                isWalking = true;
                transform.Translate(-MoveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            }
            if (!enteredSlide())//je¿eli nie jest na zje¿d¿alni i jest na obszarze mapy, mo¿e siê ruszaæ w ka¿d¹ stronê
            {
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    if (!isFacingRight)
                    {
                        flip();
                    }
                    isWalking = true;
                    transform.Translate(MoveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                }
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //LPM
                {
                    Jump();
                }
                
                timeSinceLastCoin += Time.deltaTime;
            }
            //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false); //wyœwietlanie promienia do metody is grounded
            //Debug.Log(transform.position.x);
        }
        else if(GameManager.instance.currentGameState == GameState.GS_GAME_OVER)
            this.gameObject.SetActive(false);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isFalling", isFalling());
        animator.SetBool("isGrounded", isGrounded());
        animator.SetBool("isClimbing", isClimbing);
        animator.SetBool("isLadder", isLadder);
    }

}

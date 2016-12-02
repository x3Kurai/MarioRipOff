using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//Known Issues


public class PlayerControls : MonoBehaviour
{

    //movement
    public float moveSpeed;
    public bool isGrounded;
    public float jumpForce;
    public float jumpTime;
    public float jumpTimeCounter;
    public bool stoppedJumping;
    public bool maxSpeed;
    public float xValue;
    public float direction;
    public bool invincible = false;

    public GameObject camera;
    private CameraControls access;
    private float cam_xVal;


    //status
    public int health;
    public float height;
    public bool fireFlower;
    public int lives;

    //grounded
    public Transform groundPoint;
    public Transform groundPointLeft;
    public Transform groundPointRight;
    public float radius;
    public LayerMask groundMask;

    Rigidbody2D rb2D;


    //Start=============================================================

    void Start()
    {
        //Control Values
        moveSpeed = 0.0f;
        jumpTime = 0.25f;
        jumpTimeCounter = jumpTime;

        //status
        health = 1;
        //lives = 3;
        height = 1.0f;
        fireFlower = false;
        direction = 1.0f;

        //ridgedbody
        rb2D = GetComponent<Rigidbody2D>();

        //Others
        access = camera.GetComponent<CameraControls>();
        cam_xVal = access.cam_xValue;
    }

    //Update=============================================================

    void Update()
    {
        checkGrounded();

        if (isGrounded)
            jumpTimeCounter = jumpTime;


        if (transform.position.y < -1.5f)
        {
            //Currently using the scene "Gregg" as the main scene
            SceneManager.LoadScene("Gregg");
            lives--;
        }

        //End of Update()
    }

    //FixedUpdate=============================================================

    void FixedUpdate()
    {
        //Jumping

        //Max Jump Height depending on NotMario's speed.
        isMaxSpeed();

        //Makes NotMario's jump for a longer duration if 'Z' is held down.
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isGrounded)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
                stoppedJumping = false;
            }
        }
        if (Input.GetKey(KeyCode.X) && !stoppedJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }

        //Acceleration, deceleration
        if (!Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow))
                moveSpeed = moveSpeed + 0.2f;
            if (Input.GetKey(KeyCode.LeftArrow))
                moveSpeed = moveSpeed - 0.2f;
        }


        if (!Input.GetKey(KeyCode.RightArrow) && moveSpeed > 0.2f)
            moveSpeed = moveSpeed - 0.2f;
        if (!Input.GetKey(KeyCode.LeftArrow) && moveSpeed < -0.2f)
            moveSpeed = moveSpeed + 0.2f;

        if (!Input.GetKey(KeyCode.RightArrow) && -0.2f < moveSpeed && moveSpeed < 0.2f || !Input.GetKey(KeyCode.LeftArrow) && -0.2f < moveSpeed && moveSpeed < 0.2f)
            moveSpeed = 0.0f;


        //Speed cap
        if (!Input.GetKey(KeyCode.Z))
        {
            if (moveSpeed > 5.0f)
                moveSpeed = 5.0f;
            else if (moveSpeed < -5.0f)
                moveSpeed = -5.0f;
        }
        else
        {
            if (moveSpeed > 10.0f)
                moveSpeed = 10.0f;
            else if (moveSpeed < -10.0f)
                moveSpeed = -10.0f;
        }


        //Locks player to stay on screen
        if ((xValue + 15.0f) <= cam_xVal)
            moveSpeed = 1.0f;

        //Left and right movements
        Vector2 moveDir = new Vector2(moveSpeed, rb2D.velocity.y);
        rb2D.velocity = moveDir;


        //Jumping

        //Flips the character depending on their horizontal movement
        if (health == 2)
            height = 2.0f;

        if (Input.GetKey(KeyCode.RightArrow))
            direction = 1.0f;
        else if (Input.GetKey(KeyCode.LeftArrow))
            direction = -1.0f;
        if (Input.GetKey(KeyCode.DownArrow))
            height = 1.0f;

        transform.localScale = new Vector2(direction, height);

        //if(Input.GetKey(KeyCode.RightArrow))
        //{
        //	transform.localScale = new Vector2(1.0f, height);
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //	transform.localScale = new Vector2(-1.0f, height);
        //}
        //else if(Input.GetKey(KeyCode.DownArrow)
        //{
        //	transform.localScale += new Vector2(0.0f, );
        //}

        //End of FixedUpdate()

        if (invincible)
        {
            GetComponent<Renderer>().material.color = Color.blue;
            transform.gameObject.tag = "Invincible";
            Invoke("resetInvulnerability", 2);
        }
    }

    void LateUpdate()
    {
        xValue = transform.position.x;
        cam_xVal = access.cam_xValue;

        //End of LateUpdate()
    }


    //Misc====================================================================

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemies")
        {
            if (transform.position.y <= col.gameObject.transform.position.y + .7 && !invincible)
            {
                PlayerHit();
            }
        }

        if (col.gameObject.tag == "Mushroom")
        {
            health = 2;
            height = 2.0f;
            col.gameObject.SetActive(false);
        }

        if (col.gameObject.tag == "Fireflower")
        {
            health = 2;
            height = 2.0f;
            fireFlower = true;
            col.gameObject.SetActive(false);
        }
    }

    void PlayerHit()
    {
        if (health == 2)
        {
            PauseWaitResume(1);
            health--;
            height = 1.0f;
            fireFlower = false;
            invincible = true;
        }
        else
        {
            SceneManager.LoadScene("Gregg");
        }
    }

    void isMaxSpeed()
    {
        //Checks if NotMario is running at Max Speed
        if (moveSpeed < 11.0f && moveSpeed > 9.0f || moveSpeed > -11.0f && moveSpeed < -9.0f)
        {
            maxSpeed = true;
            jumpForce = 15.0f;
        }
        else
        {
            maxSpeed = false;
            jumpForce = 14.5f;
        }
    }

    //Displays the Ground Point

    //void OnDrawGizmos()
    //{
    //	Gizmos.color = Color.blue;
    //	Gizmos.DrawWireSphere (groundPoint.position, radius);
    //}

    //Checks if Grounded

    void checkGrounded()
    {
        if (Physics2D.OverlapCircle(groundPoint.position, radius, groundMask) ||
           Physics2D.OverlapCircle(groundPointLeft.position, radius, groundMask) ||
           Physics2D.OverlapCircle(groundPointRight.position, radius, groundMask))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }
    void resetInvulnerability()
    {
        GetComponent<Renderer>().material.color = Color.white;
        transform.gameObject.tag = "Player";
        invincible = false;
    }

    void PauseWaitResume(float pauseDelay)
    {
        Time.timeScale = 0.1f;
        Invoke("resetTime", pauseDelay * Time.timeScale);
    }
    void resetTime()
    {
        Time.timeScale = 1.0f;
    }
}
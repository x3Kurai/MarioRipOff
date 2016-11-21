using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//Known Issues


public class PlayerControls : MonoBehaviour {

	//movement
	public float moveSpeed;
	public bool isGrounded;
	public float jumpForce;
	public float jumpTime;
	public float jumpTimeCounter;
	public bool stoppedJumping;
	public bool maxSpeed;
	public float xValue;

	
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
		
		//ridgedbody
		rb2D = GetComponent<Rigidbody2D>();
		
		//Others
		access = camera.GetComponent<CameraControls>();
		cam_xVal = access.cam_xValue;
	}
	
	//Update=============================================================
	
	void Update()
	{	
		checkGrounded ();
		//Checks if Grounded
		//if(Physics2D.OverlapCircle(groundPoint.position, radius, groundMask) ||
		//	Physics2D.OverlapCircle(groundPointLeft.position, radius, groundMask) ||
		//	Physics2D.OverlapCircle(groundPointRight.position, radius, groundMask))
		//{
		//	isGrounded = true;		
		//}
		
		//if(!Physics2D.OverlapCircle(groundPoint.position, radius, groundMask) &&
		//	!Physics2D.OverlapCircle(groundPointLeft.position, radius, groundMask) &&
		//	!Physics2D.OverlapCircle(groundPointRight.position, radius, groundMask))
		//{
		//	isGrounded = false;		
		//}
		
		if(isGrounded)
			jumpTimeCounter = jumpTime;
		
		
		//Checks if NotMario is running at Max Speed
		//if(moveSpeed == 10.0f || moveSpeed == -10.0f)
		//	maxSpeed = true;
		//if(moveSpeed != 10.0f || moveSpeed != -10.0f)
		//	maxSpeed = false;

		
	if(transform.position.y < -1.5f)
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
		if(Input.GetKeyDown(KeyCode.X))
		{
			if(isGrounded)
			{
				rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
				stoppedJumping = false;
			}
		}
		if(Input.GetKey(KeyCode.X) && !stoppedJumping)
		{
			if(jumpTimeCounter > 0)
			{
				rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
				jumpTimeCounter -= Time.deltaTime;
			}
		}
		if(Input.GetKeyUp(KeyCode.X))
		{
			jumpTimeCounter = 0;
			stoppedJumping = true;
		}
		
		//Acceleration, deceleration

        if(Input.GetKey(KeyCode.RightArrow))
            moveSpeed = moveSpeed + 0.2f;
        else if(Input.GetKey(KeyCode.LeftArrow))
            moveSpeed = moveSpeed - 0.2f;
		
		
        if(!Input.GetKey(KeyCode.RightArrow) && moveSpeed > 0.2f)
            moveSpeed = moveSpeed - 0.2f;
        else if(!Input.GetKey(KeyCode.LeftArrow) && moveSpeed < -0.2f)
            moveSpeed = moveSpeed + 0.2f;
		
        if(!Input.anyKey && -0.2f < moveSpeed && moveSpeed < 0.2f)
            moveSpeed = 0.0f;


        //Speed cap
		
		if(!Input.GetKey(KeyCode.Z))
		{
			if(moveSpeed > 5.0f)
				moveSpeed = 5.0f;
			else if(moveSpeed < -5.0f)
				moveSpeed = -5.0f;
		}
		else
		{
			if(moveSpeed > 10.0f)
				moveSpeed = 10.0f;
			else if(moveSpeed < -10.0f)
				moveSpeed = -10.0f;
		}


		//left and right movement
		
		if((xValue + 15.0f) <= cam_xVal)
			moveSpeed = 1.0f;
	
		Vector2 moveDir = new Vector2(moveSpeed, rb2D.velocity.y);
		rb2D.velocity = moveDir;
		
		
		//Jumping

		//Flips the character depending on their horizontal movement

        if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			//transform.localScale = new Vector3(1.0f, height, 1.0f);
			transform.localScale = new Vector2(1.0f, height);
		}
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			//transform.localScale = new Vector3(-1.0f, height, -1.0f);
			transform.localScale = new Vector3(-1.0f, height);
		}
		
		//End of FixedUpdate()
	}
	
	void LateUpdate()
	{	
		xValue = transform.position.x;
		cam_xVal = access.cam_xValue;
	}
	
	
	//Misc====================================================================
	
	//void OnTriggerEnter(Collider other)
	//{
	//	if(other.gameObject.CompareTag("Mushroom"))
	//	{
	//		other.gameObject.SetActive(false);
	//		if(health == 1)
	//		{
	//			health++;
	//		}
	//	}
	//	if(other.gameObject.CompareTag("FireFlower"))
	//	{
	//		if(health == 1)
	//		{
	//			health++;
	//		}
	//	}
	//}
		
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Enemies")
		{
			PlayerHit();
		}

		if (col.gameObject.tag == "Mushroom") {
			health = 2;
			height = 2.0f;
		}
	}
	
	void PlayerHit()
	{
		if (health == 2) {
			health--;
			height = 1.0f;
		} 
		else
		{
			SceneManager.LoadScene ("Gregg");
		}
	}
	
	void isMaxSpeed()
	{
		//Checks if NotMario is running at Max Speed
		if (moveSpeed == 10.0f || moveSpeed == -10.0f)
		{
			maxSpeed = true;
			jumpForce = 16;
		}
		else
		{
			maxSpeed = false;
			jumpForce = 15;
		}
	}

    //Displays the Ground Point

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (groundPoint.position, radius);
	}

	void checkGrounded()
	{
		//Checks if Grounded
		if (Physics2D.OverlapCircle (groundPoint.position, radius, groundMask) ||
		   Physics2D.OverlapCircle (groundPointLeft.position, radius, groundMask) ||
		   Physics2D.OverlapCircle (groundPointRight.position, radius, groundMask))
		{
			isGrounded = true;		
		} 
		else
			isGrounded = false;
	}
}

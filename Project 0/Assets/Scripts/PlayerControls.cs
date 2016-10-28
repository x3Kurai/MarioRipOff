using UnityEngine;
using System.Collections;

//Known Issues


public class PlayerControls : MonoBehaviour {

	public float moveSpeed;
	public float jumpHeight;
	public bool isGrounded;
	public int health;
	public bool isFF;

	public Transform groundPoint;
	public float radius;
	public LayerMask groundMask;

	Rigidbody2D rb2D;

	
	//Start=============================================================

	void Start() 
	{	
		//Control Values

		moveSpeed = 0.0f;
		jumpHeight = 650.0f;
		health = 1;
		isFF = false;

		rb2D = GetComponent<Rigidbody2D>();
	}
	
	//Update=============================================================
	
	void Update()
	{
		isGrounded = Physics2D.OverlapCircle(groundPoint.position, radius, groundMask);


        //Acceleration, deceleration

        if(Input.GetKey(KeyCode.RightArrow))
            moveSpeed = moveSpeed + 0.2f;
        if(Input.GetKey(KeyCode.LeftArrow))
            moveSpeed = moveSpeed - 0.2f;
		
		
        if(!Input.anyKey && moveSpeed > 0.0f && moveSpeed > 0.2f)
            moveSpeed = moveSpeed - 0.2f;
        if(!Input.anyKey && moveSpeed < 0.0f && moveSpeed < -0.2f)
            moveSpeed = moveSpeed + 0.2f;
		
        if(!Input.anyKey && moveSpeed > 0.0f && moveSpeed < 0.2f)
            moveSpeed = 0.0f;
        if(!Input.anyKey && moveSpeed < 0.0f && moveSpeed > -0.2f)
            moveSpeed = 0.0f;


        //Speed cap

        if(moveSpeed > 5.0f)
            moveSpeed = 5.0f;
        if(moveSpeed < -5.0f)
            moveSpeed = -5.0f;


		//left and right movement
	
		Vector2 moveDir = new Vector2(moveSpeed, rb2D.velocity.y);
	    rb2D.velocity = moveDir;


		//Jumping

		if(isGrounded) 
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) 
			{
				rb2D.AddForce(new Vector2(0, jumpHeight));
			}
		}


		//Flips the character depending on their horizontal movement

        if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			transform.localScale = new Vector3(-1.0f, 1.0f, -1.0f);
		}

        //#End of Update()
	}
	
	//FixedUpdate=============================================================
	
	void FixedUpdate()
	{
		
	}
	
	
	//Misc====================================================================
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Mushroom"))
		{
			other.gameObject.SetActive(false);
			if(health == 1)
			{
				health++;
			}
		}
		if(other.gameObject.CompareTag("FireFlower"))
		{
			if(health == 1)
			{
				health++;
			}
		}
		if(other.gameObject.CompareTag("Enemies"))
		{
			health--;
			other.gameObject.SetActive(false);
			if(health == 0)
			{
				//pause game
				//death
			}
		}
	}

    //Displays the Ground Point

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (groundPoint.position, radius);
	}
}

using UnityEngine;
using System.Collections;

public class GoombaAI : MonoBehaviour {
	
	public float speed;
	Rigidbody2D rbGoomba;


	// Use this for initialization
	void Start ()
	{
		speed = -1.5f;
		rbGoomba = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 moveDir = new Vector2(speed, rbGoomba.velocity.y);
	    rbGoomba.velocity = moveDir;
	}
}

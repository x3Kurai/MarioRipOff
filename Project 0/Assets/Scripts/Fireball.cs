﻿using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	
	public Rigidbody2D rb;
	public Vector2 velocity;

	// Use this for initialization
	void Start ()
	{
		//Destroy(this.gameObject, 10);
		rb = GetComponent<Rigidbody2D> ();
		velocity = rb.velocity;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(rb.velocity.y < velocity.y)
			rb.velocity = velocity;
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		rb.velocity = new Vector2 (velocity.x, -velocity.y);
		
		if(col.collider.tag == "Enemies")
		{
			Destroy(col.gameObject);
			firenull();
		}
		
		if(col.collider.tag != "Player")
		{
			if(col.contacts[0].normal.x != 0)
			{
				firenull();
			}
		}
	}
	
	void firenull()
	{
		Destroy (this.gameObject);
	}
}

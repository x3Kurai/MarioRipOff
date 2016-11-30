using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {
	
	public GameObject projectile;
	public Vector2 velocity;
	bool canShoot = true;
	public Vector2 offset = new Vector2(0.77f,0.3f);
	public float cooldown = 3.0f;
	
	public GameObject player;
	public bool isFire;
	
	private PlayerControls access;

	// Use this for initialization
	void Start ()
	{
		access = player.GetComponent<PlayerControls>();
		isFire = access.fireFlower;
	}
	
	// Update is called once per frame
	void Update ()
	{
		isFire = access.fireFlower;
		
		if(Input.GetKeyDown(KeyCode.Z) && canShoot && isFire)
		{
			GameObject proj = (GameObject) Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.identity);
			proj.GetComponent<Rigidbody2D>().velocity = new Vector2 (velocity.x * transform.localScale.x, velocity.y);
		}
	}
	
	IEnumerator CanShoot()
	{
		canShoot = false;
		yield return new WaitForSeconds(cooldown);
		canShoot = true;
	}
}

using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
	
	public GameObject player;
	public float cam_xValue;
	
	private PlayerControls access;
	private float camSpeed;
	private float player_xValue;
	


	void Start ()
	{
		 access = player.GetComponent<PlayerControls>();
		 cam_xValue = 16.5f;
		 camSpeed = access.moveSpeed;
		 player_xValue = access.xValue;
	}
	
	void Update()
	{
		camSpeed = access.moveSpeed;

		player_xValue = access.xValue;
		cam_xValue = transform.position.x;
		
		
		if(camSpeed >= 0 && cam_xValue < (player_xValue + 6.0f))
		{
			transform.Translate(Vector3.right * Time.deltaTime * camSpeed);
		}
	}
	
	
	void LateUpdate()
	{
		//player_xValue = access.xValue;
		//cam_xValue = transform.position.x;
	}
}

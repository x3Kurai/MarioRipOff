using UnityEngine;
using System.Collections;

public class GoombaAI : MonoBehaviour {
	
	public float speed = -1.5f;
    public Transform sightStart;
    public Transform sightEnd;
    public bool collide;

    public Transform weakness;


    // Use this for initialization
    void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);

        collide = Physics2D.Linecast(sightStart.position, sightEnd.position);

        if (collide)
        {

            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            speed *= -1;

        }
    }

      void OnCollisionEnter2D(Collision2D col)
      {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.transform.position.y >= transform.position.y + .7)
            {
                  col.rigidbody.AddForce(new Vector2(0, 400));
                  transform.gameObject.tag = "DeadEnemies";
                  Dies();
             }


        }


      }

        void Dies()
    {
        Destroy(this.gameObject, 0.2f); //float value for when it dies
    }
}

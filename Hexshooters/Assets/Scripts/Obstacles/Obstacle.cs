using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public int health;
	public int armorWeakness;
	public Vector2 direction;
	public bool MarkedforDeletion;
	public string stat;
	public int damage;
	bool breakImmune; //flag to ensure that every water shotgun spell doesn't endlessly apply break
	public bool canPass;


	// Use this for initialization
	void Start () 
	{
		MarkedforDeletion = false;
		transform.position = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z));
	}

	// Update is called uonce per frame
	public virtual void obstacleUpdate () 
	{
		//Debug.Log (health);
		if (direction != new Vector2 (0, 0))
		{
			move ();
			collide ();
		}
		if (health <= 0)
		{
			MarkedforDeletion = true;
		}
	}
	public virtual void takeDamage(int damage) //created for "break" status
	{
		int multipliers = 1;
		if (this.stat == "break")
		{
			multipliers *= 2;
			stat = "normal";
			breakImmune = true;
		}
		this.health -= damage* multipliers;
	}
	public virtual void move()
	{
		//Debug.Log ("Here");
		//Debug.Log (direction);
		Vector2 target =  new Vector3(transform.position.x +direction.x,transform.position.y +direction.y, 0.0f);
		Vector2 position = Vector2.Lerp (transform.position, target, (Time.deltaTime*8));
		transform.position = position;
	}
	public virtual void collide ()
	{
		Collider2D[] colliders;
		colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x, transform.position.y));
		foreach (Collider2D d in colliders) 
		{
			Player p = d.GetComponent<Player> ();
			//if collides with another obstacle, destroys both
			if (d.gameObject.tag == "Obstacle") 
			{
				//Debug.Log (d.GetComponent<Obstacle> ().gameObject != this.gameObject);
				if (d.GetComponent<Obstacle> ().gameObject != this.gameObject) 
				{
					d.GetComponent<Obstacle> ().MarkedforDeletion = true;
					MarkedforDeletion = true;
				}
			}
				//else means hit a player enemy
			else if (d.gameObject.tag == "Enemy") 
			{
				Enemy e = d.GetComponent<Enemy> ();
				e.takeDamage (damage); //enemy takes dmg
				if (e.transform.position.x != 9) 
				{
					e.transform.position = new Vector3 (e.transform.position.x + 1, e.transform.position.y, e.transform.position.z);
				}
				//move  the piece somewhere, but where?
			} 
			else if (d.gameObject.tag == "Player2") 
			{
				d.GetComponent<Player> ().takeDamage (damage); //player takes dmg 

					if (d.transform.position.x != 9)
					{
						d.transform.position += new Vector3 (1, 0, 0);
					}
				MarkedforDeletion = true;

			} 
			else if(d.gameObject.tag == "Player")
			{
				d.GetComponent<Player> ().takeDamage (damage); //player takes dmg


					if (d.GetComponent<Player> ().transform.position.x != 0) 
					{
						d.transform.position += new Vector3(-1f,0f,0f); 
					}
				MarkedforDeletion = true;
			}
						
		}
	}
}

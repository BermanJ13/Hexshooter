using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : Obstacle {
	public List<GameObject> hitPanels = new List<GameObject>();
	private int flameTimer = 200; //decay from flame timer before poof prox ~8seconds atm
	// Update is called uonce per frame
	public override void obstacleUpdate () 
	{
		collide ();
		flameTimer--;
		//Debug.Log (health);
		if (direction != new Vector2 (0, 0))
		{
			move ();
		}
		if (health <= 0)
		{
			MarkedforDeletion = true;
		}
		if(flameTimer<=0)
		{
			MarkedforDeletion = true;
			flameTimer = 200;
		}
	}
	public override void collide ()
	{
		Collider2D[] colliders;
		colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x, transform.position.y));
		foreach (Collider2D d in colliders) 
		{
			Player p = d.GetComponent<Player> ();
			//if collides with another obstacle, destroys both
			if (d.gameObject.tag == "Obstacle") 
			{
				
			}
			//else means hit a player enemy
			else if (d.gameObject.tag == "Enemy") 
			{
				Enemy e = d.GetComponent<Enemy> ();
				e.takeDamage (damage); //enemy takes dmg
				if (e.transform.position.x != 9) 
				{
					//e.transform.position = new Vector3 (e.transform.position.x + 1, e.transform.position.y, e.transform.position.z);
				}
				MarkedforDeletion = true;
			} 
			else if (d.gameObject.tag == "Player2") 
			{
				d.GetComponent<Player> ().takeDamage (damage); //player takes dmg 

				if (d.transform.position.x != 9)
				{
					//d.transform.position += new Vector3 (1, 0, 0);
				}
				MarkedforDeletion = true;

			} 
			else if(d.gameObject.tag == "Player")
			{
				d.GetComponent<Player> ().takeDamage (damage); //player takes dmg


				if (d.GetComponent<Player> ().transform.position.x != 0) 
				{
					//d.transform.position += new Vector3(-1f,0f,0f); 
				}
				MarkedforDeletion = true;
			}
			if (d.gameObject.tag == "playerZone" || d.gameObject.tag == "enemyZone")
			{
				bool created = false;
				if (direction != new Vector2 (0, 0))
				{
					for (int i = 0; i < hitPanels.Count; i++)
					{
						if(hitPanels[i] == d.gameObject)
							created = true;
					}
					Debug.Log (created);
					if (!created)
					{
						GameObject g = (GameObject)Instantiate (Resources.Load ("Flames"), new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
						g.GetComponent<Flames> ().hitPanels = hitPanels;
					}
					else
						hitPanels.Add (d.gameObject);
				}
			}
		}
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : Obstacle
{
	public List<GameObject> hitPanels = new List<GameObject>();
	private int puddleTimer = 300; //decay from puddle timer before poof prox ~2 seconds atm
	public GameObject spark;
	// Update is called uonce per frame
	public override void obstacleUpdate()
	{
		if (shocked)
		{
			spark.SetActive (true);
			collide ();
		}
		else
		{
			spark.SetActive (false);
		}
		puddleTimer--;
		if (health <= 0)
		{
			MarkedforDeletion = true;
		}
		if (puddleTimer <= 0)
		{
			MarkedforDeletion = true;
			puddleTimer = 50;
		}
	}
	public override void collide()
	{
		Collider2D[] colliders;
		colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
		foreach (Collider2D d in colliders)
		{
			Player p = d.GetComponent<Player>();
			//hit a player enemy
			if (d.gameObject.tag == "Enemy")
			{
				Enemy e = d.GetComponent<Enemy>();
				e.takeDamage(damage, attributes); //enemy takes dmg
				MarkedforDeletion = true;
			}
			else if (d.gameObject.tag == "Player2")
			{
				d.GetComponent<Player>().takeDamage(damage, attributes); //player takes dmg 
				MarkedforDeletion = true;

			}
			else if (d.gameObject.tag == "Player")
			{
				d.GetComponent<Player>().takeDamage(damage, attributes); //player takes dmg
				MarkedforDeletion = true;
			}
		}
	}
}
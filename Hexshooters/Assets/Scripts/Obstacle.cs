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


	// Use this for initialization
	void Start () 
	{
		MarkedforDeletion = false;
		transform.position = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z));
	}

	// Update is called uonce per frame
	public void obstacleUpdate () 
	{
		Debug.Log (health);
		move ();
		collide();
		if (health <= 0)
		{
			MarkedforDeletion = true;
		}
	}
	public void takeDamage(int damage) //created for "break" status
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
	public void move()
	{
		Vector2 target =  new Vector3(transform.position.x +direction.x,transform.position.y +direction.y, 0.0f);
		Vector2 position = Vector2.Lerp (transform.position, target, (Time.deltaTime*8));
		transform.position = position;
	}
	public void collide ()
	{
		Collider2D[] colliders;
		colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x, transform.position.y));
		foreach (Collider2D d in colliders) 
		{
			Player p = d.GetComponent<Player> ();
			//if collides with another obstacle, destroys both
			if (d.gameObject.tag == "Obstacle") 
			{
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
			else if (d.gameObject.tag == "Player 2") 
			{
				//take 15 dmg
				if (p.PlayerNum == 1) 
				{
					if (p.transform.position.x != 0)
					{
						p.transform.position = new Vector3 (p.transform.position.x - 1, p.transform.position.y, p.transform.position.z);
					}
				}
				d.GetComponent<Player> ().takeDamage (damage); //player takes dmg
			} 
			else if(d.gameObject.tag == "Player1")
			{
					if (d.GetComponent<Player> ().transform.position.x != 9) 
					{
						p.transform.position = new Vector3 (p.transform.position.x + 1, p.transform.position.y, p.transform.position.z);
					}
				d.GetComponent<Player> ().takeDamage (damage); //player takes dmg
			}
						
				
				
				//move  the piece somewhere, but where?
		
			/*
				//else would be player 1
				else {
				if (p.PlayerNum == 1) {
					if (p.transform.position.x != 0) {
						p.transform.position = new Vector3 (p.transform.position.x - 1, p.transform.position.y, p.transform.position.z);
					}
				} else {
					if (d.GetComponent<Player> ().transform.position.x != 9) {
						p.transform.position = new Vector3 (p.transform.position.x + 1, p.transform.position.y, p.transform.position.z);
					}
				}
				//take 15 dmg
				d.GetComponent<Player> ().takeDamage (damage); //player takes dmg
				//move  the piece somewhere, but where?
			}
			*/
		}
	}
}

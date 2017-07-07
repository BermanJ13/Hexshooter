using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightningrod : Obstacle {
	private int shockTimer = 200; //decay from shock timer before poof prox ~8seconds atm
	public List<GameObject> hitEnemies= new List<GameObject> (); 
	public override void obstacleUpdate () 
	{
		shockTimer--;
		//Debug.Log (health);
		if (health <= 0)
		{
			MarkedforDeletion = true;
		}
		if(shockTimer<=0)
		{
			MarkedforDeletion = true;
			shockTimer = 200;
		}
	}
	void shock()
	{
		//array of colliders
		Collider2D[] hitColliders;
		hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), 1.25f);
		//goes through the array
		foreach (Collider2D c in hitColliders) {
			bool hit = true;
			foreach (GameObject e in hitEnemies) {
				if (c.gameObject == e) {
					hit = false;
				}
			}

			if (c.gameObject.tag == "Enemy" && hit) {

					c.GetComponent<Enemy> ().takeDamage (damage,attributes);
					hitEnemies.Add (c.gameObject);
			} 
			else if (c.gameObject.tag == "Player"  && hit) {
				c.GetComponent<Player> ().takeDamage (damage,attributes);
				hitEnemies.Add (c.gameObject);
			}

			//show area of affect
			if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone") {
				c.gameObject.gameObject.GetComponent<Panel> ().attacked = true;
			}
		}
		MarkedforDeletion = true;
	}
}

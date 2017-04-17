using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Spell {
	private GameObject[] enemyPanels; 
	public Transform lightning;
	private bool targetNeeded;
	Vector2 target;
	Vector2 position;
	Collider2D[] colliders;	
	// Use this for initialization
	new void Start () {
		base.Start ();
		enemyPanels = GameObject.FindGameObjectsWithTag ("enemyZone");
		targetNeeded = true;
	}

	// Update is called once per frame
	new void spellUpdate() {
		base.spellUpdate ();

		foreach (GameObject p in enemyPanels) 
		{
			//Debug.Log(p.GetComponent<Collider2D> ());
			if (GetComponent<Collider2D> ().IsTouching (p.GetComponent<Collider2D> ())  && weaponUsed == 1) 
			{
				hitBehavior (1);
			}
		}
	}

	public override void movement(int weapon)
	{
		switch (weapon) 
		{
		//Revolver, Rifle, and Gatling Gun
		//Moves Forward Indefinitely
		case 1:
			hitBehavior (1);
			break;
		case 2:
		case 4:
			if (PlayerNum == 1)
			{
				target = new Vector2 (transform.position.x, transform.position.y) + direction;
				position = Vector2.Lerp (transform.position, target, Time.deltaTime * speed);
				transform.position = position;
			} 
			else
			{
				target = new Vector2 (transform.position.x, transform.position.y) - direction;
				position = Vector2.Lerp (transform.position, target, Time.deltaTime * speed);
				transform.position = position;
			}
			break;
		//Shotgun
		case 3:

			if (PlayerNum == 1)
			{
				//Picks a target square to lerp to before activating effect
				if (targetNeeded) {
					target = new Vector2 (transform.position.x+1, transform.position.y);
					targetNeeded= false;
				}
			} 
			else
			{
				//Picks a target square to lerp to before activating effect
				if (targetNeeded) {
					target = new Vector2 (transform.position.x-1, transform.position.y);
					targetNeeded= false;
				}
			}

			position = Vector2.Lerp (transform.position,target, (Time.deltaTime*speed));
			transform.position = position;
			if (transform.position == new Vector3(target.x, target.y,0))
			{
				markedForDeletion = true;
			}
			break;
		//Cane Gun
		//Hits a rectangular Area 3 squares tall in front of the user
		case 5:

			if (PlayerNum == 1)
			{
				colliders = Physics2D.OverlapAreaAll (new Vector2 (transform.position.x+1, transform.position.y - 1.2f), new Vector2 (transform.position.x+1, transform.position.y + 1.2f));
			} 
			else
			{
				colliders = Physics2D.OverlapAreaAll (new Vector2 (transform.position.x-1, transform.position.y - 1.2f), new Vector2 (transform.position.x-1, transform.position.y + 1.2f));
			}
			hitBehavior (5);
			break;
		}
	}

	public override void hitBehavior(int weapon)
	{
		switch (weapon) 
		{
		//Revolver
		case 1:
			// Hits the entire row ahead once it strikes and enemy object or enters the enemy side of the field. 
			if(PlayerNum == 1)
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 10, transform.position.y));
			else
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x - 10, transform.position.y));
			
			foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy")
				{
					c.gameObject.GetComponent<Enemy> ().takeDamage(damageCalc (damageTier, hitNum));
					//Debug.Log (c.gameObject.GetComponent<Enemy> ().health);
				}
				if (c.gameObject.tag == "Obstacle")
				{
					//c.gameObject.GetComponent<Obstacle>().takeDamage(damageCalc (damageTier, hitNum));
				}
				else if(c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;

				}
				else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
				{
					//Debug.Log ("Damage");
					c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;
				}

				if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
				{
					showPanels (c);
				}
				markedForDeletion = true;
			}
			break;
		//Rifle
		case 2:
			//Inflicts Disable if it stirkes the enemy
			colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders)
			{
				if(c.gameObject.tag == "Enemy")
				{
					c.GetComponent<Enemy> ().takeDamage(damageCalc (damageTier, hitNum));
					StatusEffect disabled = new StatusEffect (5);
					disabled.m_type = StatusType.Disabled;
					//c.gameObject.GetComponent<Enemy> ().statMngr.AddEffect (disabled);
					markedForDeletion = true;
				}
				else if(c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle>().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;
				}
				else if(c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
					StatusEffect disabled = new StatusEffect (5);
					disabled.m_type = StatusType.Disabled;
					c.gameObject.GetComponent<Enemy> ().statMngr.AddEffect (disabled);
					markedForDeletion = true;

				}
				else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
				{
					c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
					StatusEffect disabled = new StatusEffect (5);
					disabled.m_type = StatusType.Disabled;
					c.gameObject.GetComponent<Enemy> ().statMngr.AddEffect (disabled);
					markedForDeletion = true;
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}
			break;
		//Shotgun
		case 3:
			//Creates shockwaves originating at each enemy hit by the bullet or shockwave.
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x+1, transform.position.y),1.25f);
			foreach(Collider2D c in hitColliders)
			{
				bool hit = true;
				foreach (GameObject e in hitEnemies)
				{
					if(c.gameObject == e)
					{
						hit = false;
					}
				}
				if (c.gameObject.tag == "Enemy" && hit) 
				{
					//Debug.Log (damageCalc (damageTier, hitNum));
					c.GetComponent<Enemy> ().takeDamage(damageCalc (damageTier, hitNum));
					hitEnemies.Add (c.gameObject);
					markedForDeletion = true;
					GameObject spread = (GameObject)Instantiate (Resources.Load("Lightning"), c.gameObject.transform.position, Quaternion.identity);
					spread.GetComponent<Spell> ().hitEnemies = this.GetComponent<Spell> ().hitEnemies;
				}
				else if(c.gameObject.tag == "Player" && PlayerNum == 2 && hit)
				{
					//Debug.Log (damageCalc (damageTier, hitNum));
					c.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
					hitEnemies.Add (c.gameObject);
					markedForDeletion = true;
					GameObject spread = (GameObject)Instantiate (Resources.Load("Lightning"), c.gameObject.transform.position, Quaternion.identity);
					spread.GetComponent<Spell> ().hitEnemies = this.GetComponent<Spell> ().hitEnemies;

				}
				else if(c.gameObject.tag == "Player2"&& PlayerNum == 1 && hit)
				{
					//Debug.Log (damageCalc (damageTier, hitNum));
					c.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
					hitEnemies.Add (c.gameObject);
					markedForDeletion = true;
					GameObject spread = (GameObject)Instantiate (Resources.Load("Lightning"), c.gameObject.transform.position, Quaternion.identity);
					spread.GetComponent<Spell> ().hitEnemies = this.GetComponent<Spell> ().hitEnemies;
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}
			markedForDeletion = false;
			break;
		//Gatling
		case 4:
			//Changes direction randomly after hitting an opponent
			colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders)
			{

				bool hit = true;
					foreach (GameObject e in hitEnemies)
					{
						if(c.gameObject == e)
						{
							hit = false;
						}
				}
				if (c.gameObject.tag == "Enemy" && hit)
				{
					c.GetComponent<Enemy> ().takeDamage(damageCalc (damageTier, hitNum));
					hitEnemies[0] = c.gameObject;
					int decider = UnityEngine.Random.Range (1,5);
					switch (decider)
					{
					case 1:
						if(direction != new Vector2 (0,1))
							direction = new Vector2 (0,1);
						else
							direction = new Vector2 (0,-1);
						break;
					case 2:
						if(direction != new Vector2 (0,-1))
							direction = new Vector2 (0,-1);
						else
							direction = new Vector2 (0,1);
						break;
					case 3:
						if(direction != new Vector2 (1,0))
							direction = new Vector2 (1,0);
						else
							direction = new Vector2 (-1,0);
						break;
					case 4:
						if(direction != new Vector2 (-1,0))
							direction = new Vector2 (-1,0);
						else
							direction = new Vector2 (1,0);
						break;
					}
				}if (c.gameObject.tag == "Obstacle" && hit)
				{
					c.GetComponent<Obstacle> ().takeDamage(damageCalc (damageTier, hitNum));
					hitEnemies[0] = c.gameObject;
					int decider = UnityEngine.Random.Range (1,5);
					switch (decider)
					{
					case 1:
						if(direction != new Vector2 (0,1))
							direction = new Vector2 (0,1);
						else
							direction = new Vector2 (0,-1);
						break;
					case 2:
						if(direction != new Vector2 (0,-1))
							direction = new Vector2 (0,-1);
						else
							direction = new Vector2 (0,1);
						break;
					case 3:
						if(direction != new Vector2 (1,0))
							direction = new Vector2 (1,0);
						else
							direction = new Vector2 (-1,0);
						break;
					case 4:
						if(direction != new Vector2 (-1,0))
							direction = new Vector2 (-1,0);
						else
							direction = new Vector2 (1,0);
						break;
					}
				}
				if (c.gameObject.tag == "Player" && PlayerNum == 2 && hit)
				{
					c.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
					hitEnemies[0] = c.gameObject;
					int decider = UnityEngine.Random.Range (1,5);
					switch (decider)
					{
					case 1:
						if(direction != new Vector2 (0,1))
							direction = new Vector2 (0,1);
						else
							direction = new Vector2 (0,-1);
						break;
					case 2:
						if(direction != new Vector2 (0,-1))
							direction = new Vector2 (0,-1);
						else
							direction = new Vector2 (0,1);
						break;
					case 3:
						if(direction != new Vector2 (1,0))
							direction = new Vector2 (1,0);
						else
							direction = new Vector2 (-1,0);
						break;
					case 4:
						if(direction != new Vector2 (-1,0))
							direction = new Vector2 (-1,0);
						else
							direction = new Vector2 (1,0);
						break;
					}
				}
				if (c.gameObject.tag == "Player2"&& PlayerNum == 1 && hit)
				{
					c.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
					hitEnemies[0] = c.gameObject;
					int decider = UnityEngine.Random.Range (1,5);
					switch (decider)
					{
					case 1:
						if(direction != new Vector2 (0,1))
							direction = new Vector2 (0,1);
						else
							direction = new Vector2 (0,-1);
						break;
					case 2:
						if(direction != new Vector2 (0,-1))
							direction = new Vector2 (0,-1);
						else
							direction = new Vector2 (0,1);
						break;
					case 3:
						if(direction != new Vector2 (1,0))
							direction = new Vector2 (1,0);
						else
							direction = new Vector2 (-1,0);
						break;
					case 4:
						if(direction != new Vector2 (-1,0))
							direction = new Vector2 (-1,0);
						else
							direction = new Vector2 (1,0);
						break;
					}
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}
			break;
		//Cane Gun
		case 5:
			//Disables enemes that are hit.
			foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy" ) 
				{
					c.gameObject.GetComponent<Enemy> ().takeDamage(damageCalc (damageTier, hitNum));
					c.gameObject.GetComponent<Enemy> ().Status ("disabled");
				}
				if (c.gameObject.tag == "Obstacle" ) 
				{
					c.gameObject.GetComponent<Obstacle>().takeDamage(damageCalc (damageTier, hitNum));
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}
			markedForDeletion = true;
			break;

		}
	}

	public override void setDescription(int weapon)
	{
		switch (weapon)
		{
		//Revolver
		case 1:
			description = "After striking an object hits every panel ahead with a lightning bolt.";
			break;
			//Rifle
		case 2:
			description = "Hits the opponent with a shot that disables their shooting for a time.";
			break;
			//Shotgun
		case 3:
			description = "Sends a shockwave out from each opponent that is hit.";
			break;
			//Gatling
		case 4:
			description = "The spell redirects after striking something.";
			break;
			//Cane Gun
		case 5:
			description = "";
			break;
		}
	}
}
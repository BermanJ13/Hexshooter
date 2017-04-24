using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : Spell {
	private GameObject[] enemyPanels; 
	private GameObject[] playerPanels; 
	private bool targetNeeded;
	bool inBounds;
	public Transform obstacle;
	Vector2 target;
	Vector2 position;
	Collider2D[] colliders;	
	private bool created;
	// Use this for initialization
	new void Start () {
		base.Start ();
		enemyPanels = GameObject.FindGameObjectsWithTag ("enemyZone");
		playerPanels = GameObject.FindGameObjectsWithTag ("playerZone");
		targetNeeded = true;
		inBounds = false;
		created = false;
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
		//Revolver
		case 1:
			if (PlayerNum == 1)
			{
				
				//HIts a square three spaces ahead
				if (targetNeeded)
				{
					target = new Vector2 (transform.position.x + 3, transform.position.y);
					targetNeeded = false;

				}
			}
			else
			{
				//HIts a square three spaces ahead
				if (targetNeeded)
				{
					target = new Vector2 (transform.position.x - 3, transform.position.y);
					targetNeeded = false;
				}
			}
				position = Vector2.Lerp (transform.position, target, (Time.deltaTime * speed));
				transform.position = position;

				if (transform.position == new Vector3 (target.x, target.y, 0))
				{
					hitBehavior (1);
				}
			break;
		//Rifle
		case 2:
			//Moves Forward indefinitely and has an effect if it goes out of bounds
			inBounds = false;
			if (PlayerNum == 1)
			{
				target = new Vector2 (transform.position.x, transform.position.y) + direction;
			} 
			else
			{
				target = new Vector2 (transform.position.x, transform.position.y) - direction;

			}
			position = Vector2.Lerp (transform.position, target, Time.deltaTime * speed);
			transform.position = position;

			Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), 0.2f);
			foreach (Collider2D c in hitColliders)
			{
				//Debug.Log (c);
				if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					inBounds = true;
			}

			//Debug.Log (inBounds);

			if (!inBounds)
				hitBehavior (2);
			break;
		//Gatling
		case 4:
			// Moves FOrward Indefinitely
			if (PlayerNum == 1)
			{
				target = new Vector2 (transform.position.x, transform.position.y) + direction;
			} 
			else
			{
				target = new Vector2 (transform.position.x, transform.position.y) - direction;

			}
			position = Vector2.Lerp (transform.position, target, Time.deltaTime*speed);
			transform.position = position;
			break;
		//Shotgun
		case 3:
			//Targets one space ahead
			if (PlayerNum == 1)
			{
				//HIts a square three spaces ahead
				if (targetNeeded)
				{
					target = new Vector2 (transform.position.x + 1, transform.position.y);
					targetNeeded = false;
				}
			}
			else
			{
				//HIts a square three spaces ahead
				if (targetNeeded)
				{
					target = new Vector2 (transform.position.x - 1, transform.position.y);
					targetNeeded = false;
				}
			}
			position = Vector2.Lerp (transform.position, target, (Time.deltaTime * speed));
			transform.position = position;

			if (transform.position == new Vector3(target.x, target.y,0))
			{
				hitBehavior (3);
			}
			break;
		}
	}

	public override void hitBehavior(int weapon)
	{
		switch (weapon) 
		{
		//Revolver
		case 1:
			bool colided = false;
			colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy")
				{
					if (PlayerNum == 1)
					{
						c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum));
						c.transform.position += new Vector3 (1, 0, 0);
						damage = 0;
						Instantiate (Resources.Load ("TestObstacle"), transform.position, Quaternion.identity);
						markedForDeletion = true;
						colided = true;
					}
				} 
				else if (c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle> ().takeDamage(damageCalc (damageTier, hitNum));
					//c.transform.position += new Vector3 (1, 0, 0);
					//Instantiate (Resources.Load ("TestObstacle"), transform.position, Quaternion.identity);
					markedForDeletion = true;
					colided = true;
				} else if (c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					c.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
					c.transform.position += new Vector3 (-1, 0, 0);
					damage = 0;
					Instantiate (Resources.Load ("TestObstacle"), transform.position, Quaternion.identity);
					markedForDeletion = true;
					colided = true;

				} else if (c.gameObject.tag == "Player2" && PlayerNum == 1)
				{
					c.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
					c.transform.position += new Vector3 (1, 0, 0);
					damage = 0;
					Instantiate (Resources.Load ("TestObstacle"), transform.position, Quaternion.identity);
					markedForDeletion = true;
					colided = true;
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}
			if (!colided && !created)
			{
				//Debug.Log ("Here");
				damage = 0;
				Instantiate (Resources.Load ("TestObstacle"), transform.position, Quaternion.identity);
				created = true;
				markedForDeletion = true;
			}
			break;
		//Rifle
		case 2:
			//Hits the whole back row if it goes out of bounds
			colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders)
			{
				if(c.gameObject.tag == "Enemy")
					{
						if (PlayerNum == 1)
						{
							c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum));
							markedForDeletion = true;
						}
				}
				else if(c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle>().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;
				}
				else if(c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;

				}
				else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
				{
					c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}

			if(!inBounds)
			{
				if(PlayerNum == 1)
					colliders = Physics2D.OverlapAreaAll (new Vector2 (9, -1), new Vector2 (9, 6));
				else
					colliders = Physics2D.OverlapAreaAll (new Vector2 (0, -1), new Vector2 (0, 6));

				foreach (Collider2D c in colliders)
				{
					if(c.gameObject.tag == "Enemy")
						{
							if (PlayerNum == 1)
							{
								c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum));
								markedForDeletion = true;
							}
					}
					else if(c.gameObject.tag == "Obstacle")
					{
						c.GetComponent<Obstacle>().takeDamage(damageCalc (damageTier, hitNum));
						markedForDeletion = true;
					}
					else if(c.gameObject.tag == "Player" && PlayerNum == 2)
					{
						c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
						markedForDeletion = true;

					}
					else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
					{
						c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
						markedForDeletion = true;
					}

						if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
						{
							showPanels (c);
						}
				}
			}
					
			break;
		//Shotgun
		case 3:
			//HIts the whole row ahead of the player
			if(PlayerNum == 1)
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 10, transform.position.y));
			else
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x - 10, transform.position.y));
			foreach (Collider2D c in colliders)
			{
				if(c.gameObject.tag == "Enemy")
					{
						if (PlayerNum == 1)
						{
							c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum));
							markedForDeletion = true;
						}
				}
				else if(c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle>().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;
				}
				else if(c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;

				}
				else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
				{
					c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}
			break;
		//Gatling
		case 4:
			colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy")
					{
						if (PlayerNum == 1)
						{
							c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum));
							c.GetComponent<Enemy> ().health -= c.GetComponent<Enemy> ().armorWeakness;
							c.GetComponent<Enemy> ().armorWeakness++;
							markedForDeletion = true;
						}
				} else if (c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle> ().takeDamage(damageCalc (damageTier, hitNum));
					c.GetComponent<Obstacle> ().health -= c.GetComponent<Enemy> ().armorWeakness;
					c.GetComponent<Obstacle> ().armorWeakness++;
					markedForDeletion = true;
				} else if (c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					c.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
					c.GetComponent<Player> ().health -= c.GetComponent<Enemy> ().armorWeakness;
					c.GetComponent<Player> ().armorWeakness++;
					markedForDeletion = true;

				} else if (c.gameObject.tag == "Player2" && PlayerNum == 1)
				{
					c.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
					c.GetComponent<Player> ().health -= c.GetComponent<Enemy> ().armorWeakness;
					c.GetComponent<Player> ().armorWeakness++;
					markedForDeletion = true;
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}
			break;
		}
	}

	public override void setDescription(int weapon)
	{
		
		switch (weapon)
		{
		//Revolver
		case 1:
			description = "Makes an obstacle 3 panels ahead.";
			break;
			//Rifle
		case 2:
			description = "Shoot past the last row, to strike the entire back row.";
			break;
			//Shotgun
		case 3:
			description = "Creates a fissure striking the row ahead and inflicts stun.";
			break;
			//Gatling
		case 4:
			description = "Stacking Damage";
			break;
			//Cane Gun
		case 5:
			description = "";
			break;
		}
	}
}

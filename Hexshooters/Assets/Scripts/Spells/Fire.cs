using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Spell {

	public Transform fire;
	public bool shotgunMove;
	Vector2 target;
	// Use this for initialization
	new void Start () {
		base.Start ();
		shotgunMove = true;

	}

	// Update is called once per frame
	new void spellUpdate() {
		base.spellUpdate ();
	}

	public override void movement(int weapon)
	{
		
		Vector2 position;
		switch (weapon) 
		{
		//revolver
		case 1:
			target = new Vector2 (transform.position.x, transform.position.y) + direction;
			position = Vector2.Lerp (transform.position, target, (Time.deltaTime*2));
			transform.position = position;
			break;
		//rifle
		case 2:
			target = new Vector2 (transform.position.x, transform.position.y) + direction;
			position = Vector2.Lerp (transform.position, target, (Time.deltaTime*2));
			transform.position = position;
			break;
		//shotgun
		case 3:
			
			if (shotgunMove) {
				target = new Vector2 (transform.position.x+1, transform.position.y);

				shotgunMove = false;
			}

			position = Vector2.Lerp (transform.position,target, (Time.deltaTime));
			transform.position = position;
			break;
			
		}
	}

	public override int damageCalc(int tier, int hits)
	{
		int total=0;
		switch (weaponUsed) 
		{
			case 1:
				hits = 1;
				damage = 10;
				total = hits * tier * damage;
				return total;
				break;
			//rifle
			case 2:
				hits = 1;
				damage = 5;
				total = hits * tier * damage;
				return total;
				break;
			//shotgun
			case 3:
				hits = 1;
				damage = 13;
				total = hits * tier * damage;
				return total;
				break;	
		}
		return total;

	}

	public override void hitBehavior(int weapon)
	{ 
		Collider2D[] colliders;
		switch (weapon) 
		{
			//revolver
			case 1:
			colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders)
			{
				if(c.gameObject.tag == "Enemy")
				{
					c.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
					markedForDeletion = true;
				}
				else if(c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
					markedForDeletion = true;
				}
				else if(c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					c.GetComponent<Player>().health -= damageCalc(damageTier,hitNum);
					markedForDeletion = true;

				}
				else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
				{
					c.GetComponent<Player>().health -= damageCalc(damageTier,hitNum);
					markedForDeletion = true;
				}
			}
				break;
			//rifle
			case 2:
				colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
				foreach (Collider2D c in colliders)
				{
					if(c.gameObject.tag == "Enemy")
					{
						c.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
						c.GetComponent<Enemy>().Status("burn");
						markedForDeletion = true;
					}
					else if(c.gameObject.tag == "Obstacle")
					{
						c.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
						markedForDeletion = true;
					}
					else if(c.gameObject.tag == "Player" && PlayerNum == 2)
					{
						c.GetComponent<Player>().health -= damageCalc(damageTier,hitNum);
						//c.GetComponent<Player>().Status("burn");
						markedForDeletion = true;

					}
					else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
					{
						c.GetComponent<Player>().health -= damageCalc(damageTier,hitNum);
						//c.GetComponent<Player>().Status("burn");
						markedForDeletion = true;
					}
				}
				
				break;
		case 3:
			colliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x + 1, transform.position.y), 1.25f);
			foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy")
				{
					c.GetComponent<Enemy> ().health -= damageCalc (damageTier, damage);
					markedForDeletion = true;
				} else if (c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle> ().health -= damageCalc (damageTier, damage);
					markedForDeletion = true;
				} else if (c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					c.GetComponent<Player> ().health -= damageCalc (damageTier, damage);
					markedForDeletion = true;
				
				} else if (c.gameObject.tag == "Player2" && PlayerNum == 1)
				{
					c.GetComponent<Player> ().health -= damageCalc (damageTier, damage);
					markedForDeletion = true;
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
			description = "Shoots a Fireball Forward";
			break;
			//Rifle
		case 2:
			description = "Inflicts Burn Damage Over a Period of Time.";
			break;
			//Shotgun
		case 3:
			description = "Creates a Small Explosion Ahead";
			break;
			//Gatling
		case 4:
			description = "Shoots a Flame Forward Like a Flamethrower";
			break;
			//Cane Gun
		case 5:
			description = "";
			break;
		}
	}
}

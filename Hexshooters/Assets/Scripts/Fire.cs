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
	new void Update() {
		base.Update ();
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
		
		switch (weapon) 
		{
			//revolver
			case 1:
				foreach(GameObject c in enemies)
				{
					if(c.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
					{
						Debug.Log ("dmg"+damageCalc(damageTier,damage));
						c.GetComponent<Enemy>().health -= damageCalc(damageTier,damage);
						Debug.Log ("hp"+c.GetComponent<Enemy>().health);
						markedForDeletion = true;
					}

				}

				break;
			//rifle
			case 2:
				foreach(GameObject c in enemies)
				{
					if(c.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
						{
							c.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
							Debug.Log ("hp"+c.GetComponent<Enemy>().health);
							c.GetComponent<Enemy>().Status("burn");
							Debug.Log ("hp"+c.GetComponent<Enemy>().health);
							markedForDeletion = true;
						}
					
				}
				
				break;
			case 3:
				Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x+1, transform.position.y),1.25f);
				foreach(Collider2D c in hitColliders)
				{
					if (c.gameObject.tag == "Enemy") 
					{
						c.GetComponent<Enemy> ().health -= damageCalc (damageTier, damage);
						markedForDeletion = true;
					}
				}
				break;
		}
	}

}

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
	new void Update() {
		base.Update ();

		foreach (GameObject p in enemyPanels) 
		{
			Debug.Log(p.GetComponent<Collider2D> ());
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
		case 1:
		case 2:
		case 4:
			target = new Vector2 (transform.position.x, transform.position.y) + direction;
			position = Vector2.Lerp (transform.position, target, Time.deltaTime*8);
			transform.position = position;
			break;
		case 3:
			if (targetNeeded) {
				target = new Vector2 (transform.position.x+1, transform.position.y);
				targetNeeded= false;
			}
			position = Vector2.Lerp (transform.position,target, (Time.deltaTime*8));
			transform.position = position;
			break;
		case 5:
			colliders = Physics2D.OverlapAreaAll (new Vector2 (transform.position.x+1, transform.position.y - 1.2f), new Vector2 (transform.position.x+1, transform.position.y + 1.2f));
			hitBehavior (5);
			break;
		}
	}

	public override void hitBehavior(int weapon)
	{
		switch (weapon) 
		{
		case 1:
			colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 10, transform.position.y));
			foreach (Collider2D c in colliders) 
			{
				if (c.gameObject.tag == "Enemy" ) 
				{
					Debug.Log (c.gameObject.GetComponent<Enemy> ().health);
					c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
					Debug.Log (c.gameObject.GetComponent<Enemy> ().health);
				}
				if (c.gameObject.tag == "Obstacle" ) 
				{
					//c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
				}
				markedForDeletion = true;
			}
			break;
		case 2:
			foreach (GameObject c in enemies)
			{
				if (c.GetComponent<Collider2D> ().IsTouching (GetComponent<Collider2D> ()))
				{
					c.GetComponent<Enemy> ().health -= damageCalc (damageTier, hitNum);
					c.GetComponent<Enemy> ().Status ("disabled");
					Debug.Log (c.gameObject.GetComponent<Enemy> ().stat);
					markedForDeletion = true;
				}
				if (c.gameObject.tag == "Obstacle")
				{
					c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
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
		case 4:
			foreach(GameObject c in enemies)
			{
					if (c.GetComponent<Collider2D> ().IsTouching (GetComponent<Collider2D> ()))
					{
						c.GetComponent<Enemy> ().health -= damageCalc (damageTier, hitNum);
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
			}
			break;
		case 5:
			
			foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy" ) 
				{
					c.gameObject.GetComponent<Enemy> ().health -= damageCalc (damageTier, hitNum);
					c.gameObject.GetComponent<Enemy> ().Status ("disabled");
				}
				if (c.gameObject.tag == "Obstacle" ) 
				{
					c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
				}
			}
			markedForDeletion = true;
			break;

		}
	}
}

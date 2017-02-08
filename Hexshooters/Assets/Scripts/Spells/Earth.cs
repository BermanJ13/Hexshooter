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
	// Use this for initialization
	new void Start () {
		base.Start ();
		enemyPanels = GameObject.FindGameObjectsWithTag ("enemyZone");
		playerPanels = GameObject.FindGameObjectsWithTag ("playerZone");
		targetNeeded = true;
		inBounds = false;
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
		//Revolver
		case 1:
			if (targetNeeded)
			{
				target = new Vector2 (transform.position.x + 3, transform.position.y);
				targetNeeded = false;
			}
			position = Vector2.Lerp (transform.position, target, (Time.deltaTime * 8));
			transform.position = position;

			if (transform.position == new Vector3(target.x,target.y,0))
			{
				hitBehavior (1);
			}
			break;
		//Rifle
		case 2:

			target = new Vector2 (transform.position.x, transform.position.y) + direction;
			position = Vector2.Lerp (transform.position, target, Time.deltaTime * 8);
			transform.position = position;

			Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), 0.2f);
			foreach (Collider2D c in hitColliders)
			{
				Debug.Log (c);
				if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					inBounds = true;
			}

			Debug.Log (inBounds);

			if (!inBounds)
				hitBehavior (2);
			break;
		//Gatling
		case 4:
			target = new Vector2 (transform.position.x, transform.position.y) + direction;
			position = Vector2.Lerp (transform.position, target, Time.deltaTime*8);
			transform.position = position;
			break;
		//Shotgun
		case 3:
			if (targetNeeded)
			{
				target = new Vector2 (transform.position.x + 1, transform.position.y);
				targetNeeded = false;
			}
			position = Vector2.Lerp (transform.position, target, (Time.deltaTime * 8));
			transform.position = position;

			if (transform.position == new Vector3(target.x, target.y,0))
			{
				hitBehavior (3);
			}
			break;
		//Cane Gun
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
		//Revolver
		case 1:
			foreach (GameObject c in enemies)
			{
				if (c.GetComponent<Collider2D> ().IsTouching (GetComponent<Collider2D> ()))
				{
					c.GetComponent<Enemy> ().health -= damageCalc (damageTier, hitNum);
					c.transform.position += new Vector3 (1, 0,0);
					Instantiate(obstacle,  transform.position, Quaternion.identity);
					markedForDeletion = true;
				}
			}
			Instantiate(obstacle,  transform.position, Quaternion.identity);
			markedForDeletion = true;
			break;
		//Rifle
		case 2:
				foreach (GameObject c in enemies)
				{
					if (c.GetComponent<Collider2D> ().IsTouching (GetComponent<Collider2D> ()))
					{
						c.GetComponent<Enemy> ().health -= damageCalc (damageTier, hitNum);
						markedForDeletion = true;
					}
				}
			if(!inBounds)
			{
				colliders = Physics2D.OverlapAreaAll (new Vector2 (9, -1), new Vector2 (9, 6));
				foreach(Collider2D c in colliders)
				{
					c.gameObject.GetComponent<Enemy> ().health -= damageCalc (damageTier, hitNum);
				}
			}
					
			break;
		//Shotgun
		case 3:
			colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 10, transform.position.y));
			foreach (Collider2D c in colliders) 
			{
				if (c.gameObject.tag == "Enemy" ) 
				{
					c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
				}
				if (c.gameObject.tag == "Obstacle" ) 
				{
					//c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
				}
				markedForDeletion = true;
			}
			break;
		//Gatling
		case 4:
			foreach(GameObject c in enemies)
			{
				if(c.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
				{
					c.GetComponent<Enemy>().health -= damageCalc(damageTier,damage);
					c.GetComponent<Enemy>().health -= c.GetComponent<Enemy>().armorWeakness;
					c.GetComponent<Enemy> ().armorWeakness++;
					markedForDeletion = true;
				}

			}
			break;
		}
	}
}

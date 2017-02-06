using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Spell {
	private GameObject[] enemyPanels; 

	// Use this for initialization
	new void Start () {
		base.Start ();
		enemyPanels = GameObject.FindGameObjectsWithTag ("enemyZone");
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
			Vector2 target = new Vector2 (transform.position.x, transform.position.y) + direction;
			Vector2 position = Vector2.Lerp (transform.position, target, Time.deltaTime);
			transform.position = position;
			break;

		}
	}

	public override void hitBehavior(int weapon)
	{
		switch (weapon) 
		{
		case 1:
			Collider2D[] colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 10, transform.position.y));
			foreach (Collider2D c in colliders) 
			{
				if (c.gameObject.tag == "Enemy" ) 
				{
					//c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
				}
				if (c.gameObject.tag == "Obstacle" ) 
				{
					//c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
				}
				markedForDeletion = true;
			}
			break;

		}
	}
}

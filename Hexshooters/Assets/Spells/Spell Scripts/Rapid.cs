using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapid : Spell {

	public LayerMask mask;
	// Use this for initialization
	void spellUpdate () {
		hitBehavior (4);
	}
	
	public override void hitBehavior(int weapon)
	{
		RaycastHit2D hit;
		if(PlayerNum == 1)
			hit = Physics2D.Raycast (new Vector2(transform.position.x + direction.x, transform.position.y + direction.y), direction, 20, LayerMask.NameToLayer ("Panels"));
		else
			hit = Physics2D.Raycast (new Vector2(transform.position.x - direction.x, transform.position.y - direction.y), -direction, 20, LayerMask.NameToLayer ("Panels"));

		if (hit.transform.gameObject != null)
		{
			GameObject c = hit.transform.gameObject;
			Collider2D[] colliders;

			if (c.tag == "Enemy")
			{
				if (PlayerNum == 1)
				{
					c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum), attributes);
					//Debug.Log (c.gameObject.GetComponent<Enemy> ().health);
				}
			}
			if (c.tag == "Obstacle")
			{
				c.GetComponent<Obstacle>().takeDamage (damageCalc(damageTier, hitNum), attributes);
			}
			else
			if (c.tag == "Player" && PlayerNum == 2)
			{
				c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum), attributes);
				markedForDeletion = true;

			}
			else
			if (c.gameObject.tag == "Player2" && PlayerNum == 1)
			{
				//Debug.Log ("Damage");
				c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum), attributes);
				markedForDeletion = true;
			}
		}

			markedForDeletion = true;
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic : Spell {
	protected bool returnShot;
	protected Vector2 rifleOrigin;
	private int spellTimer;
	new void Start()
	{
		base.Start();
		returnShot = false;
		rifleOrigin = transform.position;
		spellTimer = 50;
	}

	// Update is called once per frame
	new void spellUpdate()
	{
		base.spellUpdate();
	}

	public override void movement(int weapon)
	{
		Vector2 target, position;

		if (PlayerNum == 1)
		{
			target = new Vector2 (transform.position.x, transform.position.y) + direction;
		}
		else
		{
			target = new Vector2 (transform.position.x, transform.position.y) - direction;
		}
		position = Vector2.Lerp(transform.position, target, Time.deltaTime*speed);
		transform.position = position;
	}

	public override void hitBehavior(int weapon)
	{
		Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
		foreach (Collider2D c in colliders)
		{
			if (c.gameObject.tag == "Enemy")
			{
				if (PlayerNum == 1)
				{
					c.gameObject.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum));
					markedForDeletion = true;
				}
			} 
			else if (c.gameObject.tag == "Obstacle")
			{
				c.GetComponent<Obstacle> ().takeDamage(damageCalc (damageTier, hitNum));
				markedForDeletion = true;
			} 
			else if (c.gameObject.tag == "Player" && PlayerNum == 2)
			{
				c.gameObject.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
				markedForDeletion = true;

			} 
			else if (c.gameObject.tag == "Player2" && PlayerNum == 1)
			{
				c.gameObject.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
				markedForDeletion = true;
			}

			if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
			{
				showPanels (c);
			}
		}
	}
}

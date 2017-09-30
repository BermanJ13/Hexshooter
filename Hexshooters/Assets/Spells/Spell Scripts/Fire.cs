﻿using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Spell {
	private GameObject[] enemyPanels; 
	private GameObject[] playerPanels; 
	private bool targetNeeded;
	private int spellTimer;
	private int gattlingTimer; //time for how long the gattling gun flamethrower last
	public Transform obstacle;
	Vector2 position;
	Collider2D[] colliders;	
	private bool created;
	// Use this for initialization
	new void Start () {
		base.Start ();
		spellTimer = 50;
		gattlingTimer = 50;
		enemyPanels = GameObject.FindGameObjectsWithTag ("enemyZone");
		playerPanels = GameObject.FindGameObjectsWithTag ("playerZone");
		targetNeeded = true;
		created = false;
	}

	// Update is called once per frame
	new void spellUpdate() {
		base.spellUpdate ();

		foreach (GameObject p in enemyPanels) 
		{
			if (GetComponent<Collider2D> ().IsTouching (p.GetComponent<Collider2D> ())  && weaponUsed == Weapon_Types.Revolver) 
			{
				hitBehavior (Weapon_Types.Revolver);
			}
		}
	}

	public override void movement(Weapon_Types weapon)
	{
		switch (weapon)
		{
		//Revolver = hits 5 spaces ahead
			case Weapon_Types.Revolver:
			//if player 1
				if (PlayerNum == 1)
				{
					if (targetNeeded)
					{
						target = new Vector2 (transform.position.x + 5, transform.position.y);
						targetNeeded = false;

					}
				}
			//if player 2
			else
				{
					if (targetNeeded)
					{
						target = new Vector2 (transform.position.x - 5, transform.position.y);
						targetNeeded = false;
					}
				}
				position = Vector2.Lerp (transform.position, target, (Time.deltaTime * speed));
				transform.position = position;

				if (transform.position == new Vector3 (target.x, target.y, 0) || transform.position.x == 0 || transform.position.x == 9)
				{
					hitBehavior (Weapon_Types.Revolver);
				}
			break;
		//Rifle = moves forward indefinitely 
			case Weapon_Types.Rifle:
			//if player 1
				if (PlayerNum == 1)
				{
					target = new Vector2 (transform.position.x, transform.position.y) + direction;
				} 
			//if player 2
			else
				{
					target = new Vector2 (transform.position.x, transform.position.y) - direction;

				}
				position = Vector2.Lerp (transform.position, target, Time.deltaTime * speed);
				transform.position = position;
			break;
		//Shotgun = moves 2 ahead
			case Weapon_Types.Shotgun:
			//Player one
				if (PlayerNum == 1)
				{
					if (targetNeeded)
					{
						target = new Vector2 (transform.position.x + 2, transform.position.y);
						targetNeeded = false;
					}
				}
			//player 2
			else
				{
					if (targetNeeded)
					{
						target = new Vector2 (transform.position.x - 2, transform.position.y);
						targetNeeded = false;
					}
				}
				position = Vector2.Lerp (transform.position, target, (Time.deltaTime * speed));
				transform.position = position;

				if (transform.position == new Vector3 (target.x, target.y, 0))
				{
					hitBehavior (Weapon_Types.Shotgun);
				}
			break;
		//Gatling = moves for 5 spaces 
			case Weapon_Types.Gatling:
			
				hitBehavior (Weapon_Types.Gatling);
			
			break;

		
			case Weapon_Types.Bow:
				//Player one
				if (PlayerNum == 1)
				{
					if (targetNeeded)
					{
						target = new Vector2 (transform.position.x + 4, transform.position.y);
						targetNeeded = false;
					}
				}
				//player 2
				else
				{
					if (targetNeeded)
					{
						target = new Vector2 (transform.position.x - 4, transform.position.y);
						targetNeeded = false;
					}
				}
				position = Vector2.Lerp (transform.position, target, (Time.deltaTime * speed));
				transform.position = position;

				if (transform.position == new Vector3 (target.x, target.y, 0))
				{
					hitBehavior (Weapon_Types.Bow);
				}
			break;
		}
	}

	public override void hitBehavior(Weapon_Types weapon)
	{
		switch (weapon) 
		{
		//Revolver
			case Weapon_Types.Revolver:
			bool self = false;
			//checks to see if colided with anything for obstacle
			bool colided = false;
			//array of everything it can collide with
			colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x, transform.position.y));
			//goes through the array
			foreach (Collider2D c in colliders)
			{
				//if hits enemy
				if (c.gameObject.tag == "Enemy")
					{
						if (PlayerNum == 1)
						{
							//does damage to the enemy
							c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
							//boundary for pushing them back a panel to make room for the fire obstacle
							//if not against the farthest panel move them back on
							if (c.transform.position.x != 9)
							{
								c.transform.position += new Vector3 (1f, 0f, 0f);
							} 
					//else don't move them or else pushes them off the grid
					else
							{
								c.transform.position += new Vector3 (0f, 0f, 0f); 
							}
							//creates the fire obstacle and deletes the bullet
							damage = 0;
							Instantiate (Resources.Load ("Flames"), transform.position, Quaternion.identity);
							markedForDeletion = true;
							colided = true;
						}
				}
				//if hits an obstacle
				else if (c.gameObject.tag == "Obstacle")
				{
					//does damage to the obstacle and deletes bullet. does not create obstacle
					c.GetComponent<Obstacle> ().takeDamage (damageCalc(damageTier, hitNum), attributes);
					markedForDeletion = true;
					colided = true;
				}
				//if hits player 1
				else if (c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					//does damage to the player
					c.GetComponent<Player> ().takeDamage (damageCalc(damageTier, hitNum), attributes);
					//pushes them back to make room for the fire obstacle
					if (c.transform.position.x != 0) {
						c.transform.position += new Vector3 (-1f, 0f, 0f);
					} 
					//if against the wall don't push them back off the grid
					else
					{
						c.transform.position += new Vector3 (0f, 0f, 0f); 
					}
					//creates the fire flame obstacle and deletes the bullet after creation
					damage = 0;
					Instantiate (Resources.Load ("Flames"), transform.position, Quaternion.identity);
					markedForDeletion = true;
					colided = true;

				}
				//if hit player 2
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1)
				{
					//does damage
					c.GetComponent<Player> ().takeDamage (damageCalc(damageTier, hitNum), attributes);
					//pushes player 2 back a space to make room for fire obstacle
					if (c.transform.position.x != 9) {
						c.transform.position += new Vector3 (1f, 0f, 0f);
					} 
					//if against the wall don't push them off the grid
					else
					{
						c.transform.position += new Vector3 (0f, 0f, 0f); //moves the enemy up a penel
					}
					//creates the fire obstacle and then deletes the bullet
					damage = 0;
					Instantiate (Resources.Load ("Flames"), transform.position, Quaternion.identity);
					markedForDeletion = true;
					colided = true;
				}
				else if (c.gameObject.tag == "Player" && PlayerNum == 1)
				{
					self = true;
				}
				else if (c.gameObject.tag == "Player2" && PlayerNum == 2)
				{
					self = true;
				}
				//shows the area it is affecting
				if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
				{
					showPanels (c);
				}
			}
			//if it hits nothing by the end of the everything then create the fire obstacle and delete the bullet
			if (!colided && !created && !self)
			{
				damage = 0;
				Instantiate (Resources.Load ("Flames"), transform.position, Quaternion.identity);
				created = true;
				markedForDeletion = true;
			}
			break;
		//Rifle = initial strike + burn damage
		case Weapon_Types.Rifle:
			//goes through the array of things can collide with
			colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders) 
			{
				//if attack an enemy
				if (c.gameObject.tag == "Enemy") {

						if (PlayerNum == 1)
						{
							//initial strike
							c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum),attributes); 
							markedForDeletion = true; //used to delete bullet
							//burn them for 5 seconds
							StatusEffect burn = new StatusEffect (500);
							burn.m_type = StatusType.Burn;
							c.GetComponent<Enemy> ().myStatus.AddEffect (burn);
							//while burned do 2 damage 
							if (burn.m_timer % 100 == 0)
							{
								if (c.GetComponent<Enemy> ().myStatus.IsAffected (StatusType.Burn))
								{
									c.GetComponent<Enemy> ().takeDamage (2,attributes);
								}	
							}
							burn.m_timer--;
							if (burn.m_timer-- <= 0)
							{
								markedForDeletion = true;
							}
						}
				}
				//if hit an obstacle does nothing but dmg
				else if (c.gameObject.tag == "Obstacle") 
				{
					//obstacle takes damage
					c.GetComponent<Obstacle> ().takeDamage (damageCalc (damageTier, hitNum),attributes); 
					markedForDeletion = true; //used to delete bullet

				}
				// if hit player 1
				else if (c.gameObject.tag == "Player" && PlayerNum == 2) 
				{
					//initial damage
					c.GetComponent<Player>().takeDamage (damageCalc (damageTier, hitNum),attributes); 
					markedForDeletion = true; //used to delete bullet
					//burn them
					StatusEffect burn = new StatusEffect (500);
					burn.m_type = StatusType.Burn;
					c.GetComponent<Player> ().myStatus.AddEffect (burn);
					//while burned do 2 damage 
					if (burn.m_timer % 100 == 0)
					{
						if(c.GetComponent<Player>().myStatus.IsAffected(StatusType.Burn)){
								c.GetComponent<Player> ().takeDamage (2,attributes);
						}	
					}
					burn.m_timer--;
					if (burn.m_timer-- <= 0)
					{
						markedForDeletion = true;
					}

				}
				// if hit player 2
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1) 
				{
					//initial damage
					c.GetComponent<Player>().takeDamage (damageCalc (damageTier, hitNum),attributes); 
					markedForDeletion = true; //used to delete bullet
					//burn them
					StatusEffect burn = new StatusEffect (500);
					burn.m_type = StatusType.Burn;
					c.GetComponent<Enemy> ().myStatus.AddEffect (burn);
					//while burned do 2 damage 
					if (burn.m_timer % 100 == 0) //modulo ensures that enemy not immediately pushed to back
					{
						if(c.GetComponent<Player>().myStatus.IsAffected(StatusType.Burn)){
								c.GetComponent<Player> ().takeDamage (2,attributes);
						}	
					}
					burn.m_timer--;
					if (burn.m_timer-- <= 0)
					{
						markedForDeletion = true;
					}


				}

				if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
				{
					showPanels (c);
				}
			}
			break;
		//Shotgun =  3x3 aoe damage area
		case Weapon_Types.Shotgun:
			//array of colliders
			Collider2D[] hitColliders;
			//if statement to change the postion of the explosion depending if player 1 or 2
			if(PlayerNum==1)
			{
				hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x+1, transform.position.y), 1.25f);
			}
			else
			{
				hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x-1, transform.position.y), 1.25f);
			}
			//goes through the array
			foreach (Collider2D c in hitColliders) {
				bool hit = true;
				foreach (GameObject e in hitEnemies) {
					if (c.gameObject == e) {
						hit = false;
					}
				}

				if (c.gameObject.tag == "Enemy" && hit) {

						if (PlayerNum == 1)
						{
							c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
							hitEnemies.Add (c.gameObject);
							markedForDeletion = true;
						}
				} else if (c.gameObject.tag == "Player" && PlayerNum == 2 && hit) {
					//Debug.Log (damageCalc (damageTier, hitNum));
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
					hitEnemies.Add (c.gameObject);
					markedForDeletion = true;

				} else if (c.gameObject.tag == "Player2" && PlayerNum == 1 && hit) {
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
					hitEnemies.Add (c.gameObject);
					markedForDeletion = true;
				}
					
				//show area of affect
				if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone") {
					showPanels (c);
				}
			}
			markedForDeletion = true;
			break;
		//Gatling = flamethrower
		case Weapon_Types.Gatling:
			// if it collides with anything and 5 spaces behind it 
			if (PlayerNum == 1) {
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 4, transform.position.y));
			} else {
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x - 4, transform.position.y));
			}
		
			// time the stream of fire is alive
			gattlingTimer--;
			//checks to see if is hitting a spell during this time and if so do damage
			if (gattlingTimer > 0) {
				//checks colliders
				foreach (Collider2D c in colliders) {
					//if attack an enemy
					if (c.gameObject.tag == "Enemy") {

							if (PlayerNum == 1)
							{
						//initial strike
						c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum),attributes); 
						markedForDeletion = true; //used to delete bullet
							}
					}
					//if hit an obstacle does nothing but dmg
					else if (c.gameObject.tag == "Obstacle") {
						//obstacle takes damage
						c.GetComponent<Obstacle> ().takeDamage (damageCalc (damageTier, hitNum),attributes); 
						markedForDeletion = true; //used to delete bullet

					}
					// if hit player 1
					else if (c.gameObject.tag == "Player" && PlayerNum == 2) {
						//initial damage
						c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes); 
						markedForDeletion = true; //used to delete bullet
					}
					// if hit player 2
					else if (c.gameObject.tag == "Player2" && PlayerNum == 1) {
						//initial damage
						c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes); 
						markedForDeletion = true; //used to delete bullet
					}
					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone") {
						showPanels (c);
					}
				}
			}
			//spell is dead
			if (gattlingTimer <= 0) {
				markedForDeletion = true;
				gattlingTimer = 50;
			} 
			break;
			case Weapon_Types.Bow:
				//if statement to change the postion of the explosion depending if player 1 or 2
				if(PlayerNum==1)
				{
					hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x+1, transform.position.y), 1.25f);
				}
				else
				{
					hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x-1, transform.position.y), 1.25f);
				}
				//goes through the array
				foreach (Collider2D c in hitColliders) {
					bool hit = true;
					foreach (GameObject e in hitEnemies) {
						if (c.gameObject == e) {
							hit = false;
						}
					}

					if (c.gameObject.tag == "Enemy" && hit) {

						if (PlayerNum == 1)
						{
							c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
							hitEnemies.Add (c.gameObject);
						}
					} else if (c.gameObject.tag == "Player" && PlayerNum == 2 && hit) {
						//Debug.Log (damageCalc (damageTier, hitNum));
						c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
						hitEnemies.Add (c.gameObject);

					} else if (c.gameObject.tag == "Player2" && PlayerNum == 1 && hit) {
						c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
						hitEnemies.Add (c.gameObject);
					}

					//show area of affect
					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone") {
						showPanels (c);
					}
				}
				markedForDeletion = true;
			break;
		}
	}

	public override void setDescription(Weapon_Types weapon)
	{

		switch (weapon)
		{
		//Revolver
			case Weapon_Types.Revolver:
				description = "Shoots a fireball 5 spaces ahead and creates fire.";
				damage = 15;
			break;
			//Rifle
			case Weapon_Types.Rifle:
				description = "Shoot across a row and burn an enemy if it hits them";
				damage = 10;
			break;
		//Shotgun
			case Weapon_Types.Shotgun:
				description = "A blast area of damage";
				damage = 20;
			break;
		//Gatling
			case Weapon_Types.Gatling:
				description = "Flamethrower";
				damage = 10;
			break;
		//Cane Gun
			case Weapon_Types.Canegun:
				description = "";
			break;
				//Bow
			case Weapon_Types.Bow:
				description = "Shoots an arrow 4 squares ahead that expldes on contact";
			break;
		}
	}
}
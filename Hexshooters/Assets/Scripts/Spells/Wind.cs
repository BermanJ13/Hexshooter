using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Spell {

    // Use this for initialization
	private int spellTimer; //spell timer for rifle spell
	public List<Spell> riflePowered;
    new void Start()
    {
        base.Start();
		riflePowered = new List<Spell>();
		spellTimer = 500;
		setDescription (1);
    }

    // Update is called once per frame
    new void spellUpdate()
    {
        base.spellUpdate();
	
    }

    public override void movement(int weapon)
    {
		Vector2 target, position;
        switch (weapon)
        {
        //rifle
		case 2:
			hitBehavior (2);
			break;
			//revolver
		case 1:
        //shotgun
		case 3:
        //gatling
		case 4:
			//cane gun - not priority
			if (PlayerNum == 1)
			{
				target = new Vector2(transform.position.x, transform.position.y) + direction;
			} 
			else
			{
				target = new Vector2(transform.position.x, transform.position.y) - direction;
			}
			position = Vector2.Lerp (transform.position, target, (Time.deltaTime * 8));
			transform.position = position;
			break;
			//Cane Gun
		case 5:
			if (PlayerNum == 1)
			{
				target = new Vector2(transform.position.x, transform.position.y) + direction;
			} 
			else
			{
				target = new Vector2(transform.position.x, transform.position.y) - direction;
			}
                position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;
        }
    }

	public override void hitBehavior(int weapon)
	{
		//array of everything can collide with
		Collider2D[] colliders;
		switch (weapon) 
		{
		//Revolver, does damage and knocks an enemy up a tile
		case 1:
			colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders) 
			{
				//if attack an enemy
				if (c.gameObject.tag == "Enemy") {

						if (PlayerNum == 1)
						{
							c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum),attributes); //enemy takes dmg
							if (c.transform.position.y != 4) 
							{
								c.transform.position += new Vector3 (0, 1f, 0); //moves the enemy up a penel
							} 
							else
							{
								c.transform.position -= new Vector3 (0f, 1f, 0f); //moves the enemy down a penel
							}
							markedForDeletion = true; //used to delete bullet
						}
				} 
				//if attack an obstacle
				else if (c.gameObject.tag == "Obstacle") {
					//makes a variable depending if bullet is coming from the left or right side
					if (PlayerNum == 1)
					{
						
						c.GetComponent<Obstacle> ().direction = new Vector2(direction.x,direction.y);
						//boundary
						if (c.transform.position.x != 9) {
							c.transform.position += new Vector3 (1f, 0f, 0f);
						} 
						else
						{
							c.transform.position += new Vector3 (0f, 0f, 0f); //moves the enemy up a penel
						}

					} else 
					{
						c.GetComponent<Obstacle> ().direction = direction * -1;
						if (c.transform.position.x != 9) {
							c.transform.position += new Vector3 (-1f, 0f, 0f);
						} 
						else
						{
							c.transform.position += new Vector3 (0f, 0f, 0f); //moves the enemy up a penel
						}
					}
					markedForDeletion = true; //used to delete bullet

					}
				// if hit player 1
				else if (c.gameObject.tag == "Player" && PlayerNum == 2) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes); //player 1 takes dmg
					//checking boundary and then moving position accordingly 
					if (c.transform.position.y != 4) 
					{
						c.transform.position += new Vector3 (0, 1f, 0);
					} 
					else
					{
						c.transform.position -= new Vector3 (0f, 1f, 0f); 
					}
					markedForDeletion = true; //used to delete bullet

				}
				// if hit player 2
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
					//checking boundary and then moving position accordingly
					if (c.transform.position.y != 4) {
						c.transform.position += new Vector3 (0, 1f, 0);
					} 
					else
					{
						c.transform.position -= new Vector3 (0f, 1f, 0f); 
					}

					markedForDeletion = true; //used to delete bullet
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}
			break;


		//Rifle //if spell hits the wind portal == doubles the speed of the spell
		case 2:
			//used to see if already touched the speed
			// Hits the entire row ahead once it strikes and enemy object or enters the enemy side of the field.
			if (PlayerNum == 1) 
			{
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 10, transform.position.y));

				// time the wind tunnel is alive
				spellTimer--;
				//checks to see if is hitting a spell during this time and if so increases the bullet speed
				if (spellTimer >= 0) 
				{
					//checks colliders
					foreach (Collider2D c in colliders) 
					{
						//if touching a spell that isn't itself
						if (c.gameObject.tag == "Spell" && c.gameObject.name != "Wind(Clone)")
						{
							
							Spell s = c.GetComponent<Spell> ();
							//check if bullet is coming from same direction
							if (s.direction == direction) 
							{
								//if the spell hasn't been updated before
								if (!riflePowered.Contains (s)) 
								{
									//multiple speed by 2
									s.speed *= 2; 
									//adds to list of spells that is already powered up
									riflePowered.Add (s);
									//Debug.Log ("post speed" + s.speed);
								}

							}
						}

							if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
							{
								showPanels (c);
							}
					}
				}
				//spell is dead
				if (spellTimer <= 0) 
				{
					markedForDeletion = true;
					spellTimer = 500;
				}
			} 
			else 
			{
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x - 10, transform.position.y));
				direction *= -1;
				// time the wind tunnel is alive
				spellTimer--;
				//checks to see if is hitting a spell during this time and if so increases the bullet speed
				if (spellTimer >= 0) 
				{
					//checks colliders
					//colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
					foreach (Collider2D c in colliders) 
					{
						//if touching a spell
						//if touching a spell
						if (c.gameObject.tag == "Spell" && c.gameObject.name != "Wind(Clone)")
						{

							Spell s = c.GetComponent<Spell> ();
							if (s.direction == direction) 
							{
								s.speed *= 2; 
								//Debug.Log ("post speed" + s.speed);

							}
						}
					}
				}
				if (spellTimer <= 0) 
				{
					markedForDeletion = true;
					spellTimer = 500;
				}
			}
			break;


		//Shotgun:Compressed air blast knockback's enemy by 2 space
		case 3:
			//goes through the array of things can collide with
			colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders) 
			{
				//if attack an enemy
				if (c.gameObject.tag == "Enemy") {

						if (PlayerNum == 1)
						{
					c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum),attributes); //enemy takes dmg
					//checking boundary and then moving accordingly
					if (c.transform.position.x < 8) {
						c.transform.position += new Vector3 (2f, 0f, 0f);
					} 
					else if(c.transform.position.x == 8)
					{
						c.transform.position += new Vector3 (1f, 0f, 0f);
					}

					markedForDeletion = true; //used to delete bullet
						}
				}
				//if hit an obstacle
				else if (c.gameObject.tag == "Obstacle") {
					//makes a variable depending if bullet is coming from the left or right side
					if (PlayerNum == 1) {
						c.GetComponent<Obstacle> ().direction = direction *2;
						//checking boundary and movings accordingly
						if (c.transform.position.x < 8) {
							c.transform.position += new Vector3 (2f, 0f, 0f);
						} 
						else if(c.transform.position.x == 8)
						{
							c.transform.position += new Vector3 (1f, 0f, 0f);
						}
						else
						{
							c.transform.position += new Vector3 (0f, 0f, 0f); //moves the enemy up a penel
						}
					} 
					else 
					{
						c.GetComponent<Obstacle> ().direction = direction * -2;
						//checking boundaries and movings accordlingly
						if (c.transform.position.x > 1) 
						{
							c.transform.position += new Vector3 (-2f, 0f, 0f);
						} 
						else if(c.transform.position.x == 1)
						{
							c.transform.position += new Vector3 (-1f, 0f, 0f);
						}
						else
						{
							c.transform.position += new Vector3 (0f, 0f, 0f); //moves the enemy up a penel
						}
					}
					markedForDeletion = true; //used to delete bullet

				}
				// if hit player 1
				else if (c.gameObject.tag == "Player" && PlayerNum == 2) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes); //player 1 takes dmg

					//checking boundaries and movings accordlingly
					if (c.transform.position.x > 1) 
					{
						c.transform.position += new Vector3 (-2f, 0f, 0f);
					} 
					else if(c.transform.position.x == 1)
					{
						c.transform.position += new Vector3 (-1f, 0f, 0f);
					}
					else
					{
						c.transform.position += new Vector3 (0f, 0f, 0f); //moves the enemy up a penel
					}
					markedForDeletion = true; //used to delete bullet

				}
				// if hit player 2
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
					if (c.transform.position.x < 8) {
						c.transform.position += new Vector3 (2f, 0f, 0f);
					} 
					else if(c.transform.position.x == 8)
					{
						c.transform.position += new Vector3 (1f, 0f, 0f);
					}
					else
					{
						c.transform.position += new Vector3 (0f, 0f, 0f); //moves the enemy up a penel
					}
					markedForDeletion = true; //used to delete bullet
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}
			break;
		//Gatling = vaccum
		case 4:
			//goes through the array of things can collide with
			colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders) 
			{

				//if attack an enemy
					if (c.gameObject.tag == "Enemy") {
						if (PlayerNum == 1)
						{
					c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum),attributes); //enemy takes dmg
					c.transform.position += new Vector3 (-1f, 0f, 0.0f); //moves the enemy two panels back
					markedForDeletion = true; //used to delete bullet
						}
				} 
				else if (c.gameObject.tag == "Obstacle") 
				{
					//first gets direction bullet is coming from
					//makes a variable depending if bullet is coming from the left or right side
					if (PlayerNum == 1) {
						c.GetComponent<Obstacle> ().direction = direction * -1;
						//c.transform.position += new Vector3 (-1f, 0f, 0.0f); 
						markedForDeletion = true; //used to delete bullet
					} else {
						c.GetComponent<Obstacle> ().direction = direction * 1;
						//c.transform.position += new Vector3 (1f, 0f, 0.0f); 
					}
				}
				// if hit player 1
				else if (c.gameObject.tag == "Player" && PlayerNum == 2) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes); //player 1 takes dmg
					//checks boundary and moves accordingly
					if (c.transform.position.x != 4) {
						c.transform.position += new Vector3 (1f, 0f, 0f);
					} 
					else
					{
						c.transform.position += new Vector3 (0f, 0f, 0f); //moves the enemy up a penel
					}
					markedForDeletion = true; //used to delete bullet

				}
				// if hit player 2
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
					if (c.transform.position.x != 5) {
						c.transform.position += new Vector3 (-1f, 0f, 0f);
					} 
					else
					{
						c.transform.position += new Vector3 (0f, 0f, 0f); //moves the enemy up a penel
					}
					markedForDeletion = true; //used to delete bullet
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
				description = "Knocks the enemy up a tile or pushes an obstacle away.";
				damage = 10;
			break;
			//Rifle
			case 2:
				description = "Leaves a gust of wind that increases bullet speed";
				damage = 0;
			break;
			//Shotgun
			case 3:
				description = "Knocks the enemy or obstacle back with a burst of compressed air.";
				damage = 20;
			break;
			//Gatling
			case 4:
				description = "Pulls enemies or obstacles towards you.";
				damage = 15;
			break;
			//Cane Gun
			case 5:
			description = "";
			break;
		}
	}
}

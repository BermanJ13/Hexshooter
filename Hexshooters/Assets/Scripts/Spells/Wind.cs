using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Spell {

    // Use this for initialization
	private int spellTimer; //spell timer for rifle spell
    new void Start()
    {
        base.Start();
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
					c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum)); //enemy takes dmg
					c.transform.position += new Vector3 (0f, 1f, 0f); //moves the enemy up a penel
					markedForDeletion = true; //used to delete bullet
				} 
				//if attack an obstacle
				else if (c.gameObject.tag == "Obstacle") {
					//makes a variable depending if bullet is coming from the left or right side
					if (PlayerNum == 1) {
						c.GetComponent<Obstacle> ().direction = new Vector2(direction.x,direction.y);
						c.transform.position += new Vector3 (1f, 0f, 0.0f);
					} else {
						c.GetComponent<Obstacle> ().direction = direction * -1;
						c.transform.position += new Vector3 (-1f, 0f, 0.0f);
					}
					markedForDeletion = true; //used to delete bullet

					}
				// if hit player 1
				else if (c.gameObject.tag == "Player" && PlayerNum == 2) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum)); //player 1 takes dmg
					c.transform.position += new Vector3 (0f, 1f, 0f); //moves player 1 up a penel
					markedForDeletion = true; //used to delete bullet

				}
				// if hit player 2
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum));
					c.transform.position += new Vector3 (0f, 1f, 0f); //moves the enemy up a penel
					markedForDeletion = true; //used to delete bullet
				}
			}
			break;


		//Rifle //if spell hits the wind portal == doubles the speed of the spell
		case 2:
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
						//if touching a spell
						if (c.gameObject.tag == "Spell" && c.gameObject.name != "Wind(Clone)")
						{
							
							Spell s = c.GetComponent<Spell> ();
							if (s.direction == direction) 
							{
									s.speed *= 2; 
									Debug.Log ("post speed" + s.speed);

							}
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
								Debug.Log ("post speed" + s.speed);

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
					c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum)); //enemy takes dmg
					c.transform.position += new Vector3 (2f, 0f, 0f); //moves the enemy up a penel //moves the enemy two panels back
					markedForDeletion = true; //used to delete bullet
				}
				//if hit an obstacle
				else if (c.gameObject.tag == "Obstacle") {
					//makes a variable depending if bullet is coming from the left or right side
					if (PlayerNum == 1) {
						c.GetComponent<Obstacle> ().direction = direction *2;
						c.transform.position += new Vector3 (2f, 0f, 0.0f);
					} else {
						c.GetComponent<Obstacle> ().direction = direction * -2;
						c.transform.position += new Vector3 (-2f, 0f, 0.0f);
					}
					markedForDeletion = true; //used to delete bullet

				}
				// if hit player 1
				else if (c.gameObject.tag == "Player" && PlayerNum == 2) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum)); //player 1 takes dmg
					c.transform.position += new Vector3 (-2f, 0f, 0f);  //moves the enemy two panels back
					markedForDeletion = true; //used to delete bullet

				}
				// if hit player 2
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum));
					c.transform.position += new Vector3 (2f, 0f, 0f);  //moves the enemy two panels back
					markedForDeletion = true; //used to delete bullet
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
					c.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum)); //enemy takes dmg
					c.transform.position += new Vector3 (-1f, 0f, 0.0f); //moves the enemy two panels back
					markedForDeletion = true; //used to delete bullet
				} 
				else if (c.gameObject.tag == "Obstacle") 
				{
					//first gets direction bullet is coming from
					//makes a variable depending if bullet is coming from the left or right side
					if (PlayerNum == 1) {
						c.GetComponent<Obstacle> ().direction = direction * -1;
						c.transform.position += new Vector3 (-1f, 0f, 0.0f); 
						markedForDeletion = true; //used to delete bullet
					} else {
						c.GetComponent<Obstacle> ().direction = direction * 1;
						c.transform.position += new Vector3 (1f, 0f, 0.0f); 
					}
				}
				// if hit player 1
				else if (c.gameObject.tag == "Player" && PlayerNum == 2) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum)); //player 1 takes dmg
					c.transform.position += new Vector3 (1f, 0f, 0.0f); //moves the enemy two panels back
					markedForDeletion = true; //used to delete bullet

				}
				// if hit player 2
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1) 
				{
					c.GetComponent<Player> ().takeDamage (damageCalc (damageTier, hitNum));
					c.transform.position += new Vector3 (-1f, 0f, 0.0f); //moves the enemy two panels back
					markedForDeletion = true; //used to delete bullet
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
			break;
			//Rifle
			case 2:
			description = "Leaves a gust of wind that increases bullet speed";
			break;
			//Shotgun
			case 3:
			description = "Knocks the enemyor obstacle back with a burst of compressed air.";
			break;
			//Gatling
			case 4:
			description = "Pulls enemies or obstacles toward the you it lands in.";
			break;
			//Cane Gun
			case 5:
			description = "";
			break;
		}
	}
}

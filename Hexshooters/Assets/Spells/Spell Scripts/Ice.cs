using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Spell {

	private int spellTimer;
	private bool targetNeeded;

    // Use this for initialization
    new void Start () {
        base.Start();
		setDescription (weaponUsed);
        spellTimer = 50;
	}
	
	// Update is called once per frame
	new void spellUpdate () {
        base.spellUpdate();
	}

    public override void movement(Weapon_Types weapon)
    {
        Vector2 position;
        switch (weapon)
        {
            //revolver
            case Weapon_Types.Revolver:
                if (PlayerNum == 1)
                {
                    target = new Vector2(transform.position.x, transform.position.y) + direction;
                    
                }
                else
                {
                    target = new Vector2(transform.position.x, transform.position.y) - direction;
                    
                }
			position = Vector2.Lerp(transform.position, target, (Time.deltaTime * speed));
                transform.position = position;
                break;

            //rifle
            case Weapon_Types.Rifle:
                if (PlayerNum == 1)
                {
                    target = new Vector2(transform.position.x, transform.position.y) + direction;

                }
                else
                {
                    target = new Vector2(transform.position.x, transform.position.y) - direction;

                }
			position = Vector2.Lerp(transform.position, target, (Time.deltaTime * speed));
                transform.position = position;
                break;

            //shotgun
            case Weapon_Types.Shotgun:
				if(PlayerNum ==1)
				{
					target = new Vector2(transform.position.x , transform.position.y) + direction;
				}
				else
				{				
					target = new Vector2(transform.position.x , transform.position.y) - direction;
				}
			position = Vector2.Lerp(transform.position, target, (Time.deltaTime * speed));
                transform.position = position;
                break;

            //gatling
            case Weapon_Types.Gatling:
				if(PlayerNum ==1)
				{
					target = new Vector2(transform.position.x, transform.position.y) + direction;
				}
				else
				{				
					target = new Vector2(transform.position.x, transform.position.y) - direction;
				}
			position = Vector2.Lerp(transform.position, target, (Time.deltaTime * speed));
                transform.position = position;
                break;

            //cane gun - not priority
            case Weapon_Types.Canegun:
				if(PlayerNum ==1)
				{
					target = new Vector2(transform.position.x, transform.position.y) + direction;
				}
				else
				{				
					target = new Vector2(transform.position.x, transform.position.y) - direction;
				}
			position = Vector2.Lerp(transform.position, target, (Time.deltaTime * speed));
                transform.position = position;
                break;
			case Weapon_Types.Bow:
				//Player one
				if (PlayerNum == 1)
				{
					if (targetNeeded)
					{
						target = new Vector2 (transform.position.x + 3, transform.position.y);
						targetNeeded = false;
					}
				}
				//player 2
				else
				{
					if (targetNeeded)
					{
						target = new Vector2 (transform.position.x - 3, transform.position.y);
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
		Collider2D[] colliders;
		switch (weapon)
		{
		case Weapon_Types.Revolver: //freeze row
			if(PlayerNum == 1)
			 colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 10, transform.position.y));
			else
				 colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x - 10, transform.position.y));
			
			foreach (Collider2D c in colliders)
			{
				if(c.gameObject.tag == "Enemy")
					{
						if (PlayerNum == 1)
						{
					StatusEffect slow = new StatusEffect (5);
					slow.m_type = StatusType.Slow;
					c.gameObject.GetComponent<Enemy> ().myStatus.AddEffect (slow);

					spellTimer--;
					if (spellTimer <= 0)
					{
						markedForDeletion = true;
						spellTimer = 50;
					}

					//c.gameObject.GetComponent<Enemy>()takeDamage (damageCalc(damageTier, hitNum), attributes);
						}
				}
				else if(c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle>().takeDamage (damageCalc(damageTier, hitNum), attributes);
					markedForDeletion = true;
				}
				else if(c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					c.GetComponent<Player>().takeDamage (damageCalc(damageTier, hitNum), attributes);
					markedForDeletion = true;
					StatusEffect slow = new StatusEffect (5);
					slow.m_type = StatusType.Slow;
					c.gameObject.GetComponent<Player> ().myStatus.AddEffect (slow);

					spellTimer--;
					if (spellTimer <= 0)
					{
						markedForDeletion = true;
						spellTimer = 50;
					}
				}
				else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
				{
					c.GetComponent<Player>().takeDamage (damageCalc(damageTier, hitNum), attributes);
					markedForDeletion = true;
					StatusEffect slow = new StatusEffect (5);
					slow.m_type = StatusType.Slow;
					c.gameObject.GetComponent<Player> ().myStatus.AddEffect (slow);

					spellTimer--;
					if (spellTimer <= 0)
					{
						markedForDeletion = true;
						spellTimer = 50;
					}
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
			}
			break;
		case Weapon_Types.Rifle: //freeze
			colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x, transform.position.y));
			foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy")
					{
						if (PlayerNum == 1)
						{
							StatusEffect freeze = new StatusEffect (8);
							freeze.m_type = StatusType.Freeze;
							c.gameObject.GetComponent<Enemy> ().myStatus.AddEffect (freeze);
							c.gameObject.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
							markedForDeletion = true;
						}
				} else if (c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle> ().takeDamage (damageCalc(damageTier, hitNum), attributes);
					markedForDeletion = true;
				} else if (c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					StatusEffect freeze = new StatusEffect (8);
					freeze.m_type = StatusType.Freeze;
					c.gameObject.GetComponent<Player> ().myStatus.AddEffect (freeze);
					c.gameObject.GetComponent<Player> ().takeDamage (damageCalc(damageTier, hitNum), attributes);
					markedForDeletion = true;

                    }
                    else if (c.gameObject.tag == "Enemy")
                    {

						if (PlayerNum == 1)
						{
                        StatusEffect freeze = new StatusEffect(5);
                        freeze.m_type = StatusType.Freeze;
                        c.gameObject.GetComponent<Enemy>().myStatus.AddEffect(freeze);
                        //c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
						}
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        //c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
                    }

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
                }
                break;
            case Weapon_Types.Shotgun: //Shield
                colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.tag == "Player")
                    {
                        StatusEffect shield = new StatusEffect(5);
                        shield.m_type = StatusType.Shield;
                        c.gameObject.GetComponent<Player>().myStatus.AddEffect(shield);

                        markedForDeletion = true;
                        //c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        markedForDeletion = true;
                        //c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
                    }

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
                }
                break;
            case Weapon_Types.Gatling: //Stacking Damage
			if(PlayerNum == 1)
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 2, transform.position.y));
			else
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x - 2, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.tag == "Player")
                    {

                    }
                    else if (c.gameObject.tag == "Enemy")
                    {

						if (PlayerNum == 1)
						{
                        StatusEffect stack = new StatusEffect(12);
                        stack.m_type = StatusType.Stacking;
                        c.gameObject.GetComponent<Enemy>().myStatus.AddEffect(stack);
                        //c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
						}
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        //c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
                    }

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
                }
                break;
			case Weapon_Types.Bow:
				Instantiate (Resources.Load ("Icicle"), new Vector2 (transform.position.x, transform.position.y-1), Quaternion.identity);
				Instantiate (Resources.Load ("Icicle"), new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
				Instantiate (Resources.Load ("Icicle"), new Vector2 (transform.position.x, transform.position.y+1), Quaternion.identity);
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
				description = "Coats a Row in frost Slowing Enemy Movement";
				damage = 15;
			break;
			//Rifle
		case Weapon_Types.Rifle:
				description = "Freezes the opponent for an Instant.";
				damage = 10;
			break;
			//Shotgun
		case Weapon_Types.Shotgun:
				description = "Sield the player who uses the spell.";
				damage = 0;
			break;
			//Gatling
		case Weapon_Types.Gatling:
				description = "Does damage to the opponent that gets more powerful with each hit.";
				damage = 5;
			break;
			//Cane Gun
		case Weapon_Types.Canegun:
			description = "";
			break;
				//Bow
			case Weapon_Types.Bow:
				description = "Creates 3 Icicles vertically 3 squares ahead.";
			break;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Spell {

    private int spellTimer;

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

    public override void movement(int weapon)
    {
		Vector2 target;
        Vector2 position;
        switch (weapon)
        {
            //revolver
            case 1:
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
            case 2:
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
            case 3:
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
            case 4:
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
            case 5:
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

        }
    }

    public override void hitBehavior(int weapon)
	{
		Collider2D[] colliders;
		switch (weapon)
		{
		case 1: //freeze row
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

					//c.gameObject.GetComponent<Enemy>()takeDamage(damageCalc (damageTier, hitNum));
						}
				}
				else if(c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle>().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;
				}
				else if(c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
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
					c.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum));
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
		case 2: //freeze
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
							c.gameObject.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum));
							markedForDeletion = true;
						}
				} else if (c.gameObject.tag == "Obstacle")
				{
					c.GetComponent<Obstacle> ().takeDamage(damageCalc (damageTier, hitNum));
					markedForDeletion = true;
				} else if (c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					StatusEffect freeze = new StatusEffect (8);
					freeze.m_type = StatusType.Freeze;
					c.gameObject.GetComponent<Player> ().myStatus.AddEffect (freeze);
					c.gameObject.GetComponent<Player> ().takeDamage(damageCalc (damageTier, hitNum));
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
            case 3: //Shield
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
            case 4: //Stacking Damage
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
        }
    }
	public override void setDescription(int weapon)
	{
		switch (weapon)
		{
		//Revolver
		case 1:
			description = "Coats a Row in frost Slowing Enemy Movement";
			break;
			//Rifle
		case 2:
			description = "Freezes the opponent for an Instant.";
			break;
			//Shotgun
		case 3:
			description = "Sield the player who uses the spell.";
			break;
			//Gatling
		case 4:
			description = "Does damage to the opponent that gets more powerful with each hit.";
			break;
			//Cane Gun
		case 5:
			description = "";
			break;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Spell {

    protected bool revolverMove;
    protected Vector2 rifleOrigin;
    private int spellTimer;
    // Use this for initialization
    new void Start()
    {
        base.Start();
        revolverMove = false;
        rifleOrigin = transform.position;
        spellTimer = 50;
    }

    // Update is called once per frame
    new void spellUpdate()
    {
        base.spellUpdate();
		//Debug.Log (speed);
    }

    public override void movement(Weapon_Types weapon)
    {
		Vector2 target, position;
        switch (weapon)
        {
            //revolver
		case Weapon_Types.Revolver:
			if (PlayerNum == 1)
			{
				 target = new Vector2 (transform.position.x , transform.position.y) + direction;
				if (revolverMove)
					target = new Vector2 (transform.position.x , transform.position.y) - direction;
			}
			else
			{
				 target = new Vector2 (transform.position.x, transform.position.y) - direction;
				if (revolverMove)
					target = new Vector2 (transform.position.x , transform.position.y) + direction;
			}

                position = Vector2.Lerp(transform.position, target, Time.deltaTime*speed);
                transform.position = position;
                break;

            //rifle
		case Weapon_Types.Rifle:
			if (PlayerNum == 1)
			{
				if ((transform.position.x - rifleOrigin.x) < 3)
				{
					target = new Vector2 (transform.position.x , transform.position.y) + direction*2;
					position = Vector2.Lerp (transform.position, target, Time.deltaTime*speed);
					transform.position = position;
				} else
				{
						hitBehavior (Weapon_Types.Rifle);
					spellTimer--;
					if (spellTimer <= 0)
					{
						markedForDeletion = true;
						spellTimer = 50;
					}
				}
			} 
			else
			{
				if ((transform.position.x + rifleOrigin.x) < 3)
				{
					target = new Vector2 (transform.position.x , transform.position.y) - direction*2;
					position = Vector2.Lerp (transform.position, target, Time.deltaTime*speed);
					transform.position = position;
				} else
				{
					hitBehavior (Weapon_Types.Rifle);
					spellTimer--;
					if (spellTimer <= 0)
					{
						markedForDeletion = true;
						spellTimer = 50;
					}
				}
			}
                break;

            //shotgun
		case Weapon_Types.Shotgun:
                if (PlayerNum == 1)
                {
                    target = new Vector2(transform.position.x, transform.position.y) + direction;

                }
                else
                {
                    target = new Vector2(transform.position.x, transform.position.y) - direction;

                }
                position = Vector2.Lerp(transform.position, target, Time.deltaTime * speed);
                transform.position = position;
                break;
            //gatling
            case Weapon_Types.Gatling:
                if (PlayerNum == 1)
                {
                    target = new Vector2(transform.position.x, transform.position.y) + direction;

                }
                else
                {
                    target = new Vector2(transform.position.x, transform.position.y) - direction;

                }
                position = Vector2.Lerp(transform.position, target, Time.deltaTime * speed);
                transform.position = position;
                break;
				//cane gun - not priority
			case Weapon_Types.Canegun:
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
			break;

			case Weapon_Types.Bow:
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
			break;
				
        }
    }

    public override void hitBehavior(Weapon_Types weapon)
    {
        switch (weapon)
        {
            case Weapon_Types.Revolver: //shot damages enemy, bounces off and then heals player if it hits them
                Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
                foreach (Collider2D c in colliders)
                {
					if (c.gameObject.tag == "Enemy")
					{
						if (PlayerNum == 1)
						{
							c.gameObject.GetComponent<Enemy> ().takeDamage (damageCalc (damageTier, hitNum),attributes);
							Instantiate (Resources.Load ("Puddle"), new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
							revolverMove = true;
						}
					} 
					else if (c.gameObject.tag == "Obstacle")
					{
						c.GetComponent<Obstacle> ().specialEffects (this);
						Instantiate (Resources.Load ("Puddle"), new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
						revolverMove = true;
					} 
					else if (c.gameObject.tag == "Player" && PlayerNum == 2)
					{
						c.gameObject.GetComponent<Player> ().takeDamage (damageCalc(damageTier, hitNum), attributes);
						revolverMove = true;
						Instantiate (Resources.Load ("Puddle"));
					} 
					else if (c.gameObject.tag == "Player2" && PlayerNum == 1)
					{
						c.gameObject.GetComponent<Player> ().takeDamage (damageCalc(damageTier, hitNum), attributes);
						revolverMove = true;
						Instantiate (Resources.Load ("Puddle"), new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
					}
					else if (c.gameObject.tag == "Player2" && PlayerNum == 2)
					{
						if (revolverMove)
						{
							c.gameObject.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum)*-1, attributes);
							revolverMove = false;
							markedForDeletion = true;
						}
					}
					else if (c.gameObject.tag == "Player" && PlayerNum == 1)
					{
						if (revolverMove)
						{
						c.gameObject.GetComponent<Player>().takeDamage(damageCalc (damageTier, hitNum)*-1, attributes);
							revolverMove = false;
							markedForDeletion = true;
						}
					}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
                }
                break;
            case Weapon_Types.Rifle: //whirlpool shoots 3 squares ahead and drags enemy from adjacent squares
                colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                foreach (Collider2D c in colliders)
                {
                  
                    if (c.gameObject.tag == "Enemy")
					{
						if (PlayerNum == 1)
						{
							//in future call Enemy move to this position so they don't just warp
							c.transform.position = transform.position;
							markedForDeletion = true;
						}
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        //c.gameObject.GetComponent<Obstacle>()takeDamage (damageCalc(damageTier, hitNum), attributes);
                        markedForDeletion = true;
                    }
					else if(c.gameObject.tag == "Player" && PlayerNum == 2)
					{
						c.transform.position = transform.position;
						markedForDeletion = true;
					
					}
					else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
					{
						c.transform.position = transform.position;
						markedForDeletion = true;
					}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}

                }
                break;
            case Weapon_Types.Shotgun: //shotgun makes them vulnerable to next attack
                colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.tag == "Enemy")
					{
						if (PlayerNum == 1)
						{
							c.gameObject.GetComponent<Enemy>().takeDamage(3,attributes);

                        if (c.gameObject.GetComponent<Enemy>().stat == "normal")
                            c.gameObject.GetComponent<Enemy>().Status("break");


                        StatusEffect broken = new StatusEffect(10);
                        broken.m_type = StatusType.Break;
                        //c.gameObject.GetComponent<Enemy>().myStatus.AddEffect(broken);
                        
                        markedForDeletion = true;
                        //c.gameObject.GetComponent<Enemy>().takeDamage (damageCalc(damageTier, hitNum), attributes);
						}
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        markedForDeletion = true;
						c.gameObject.GetComponent<Obstacle> ().specialEffects (this);
                    }
					else if(c.gameObject.tag == "Player" && PlayerNum == 2)
					{
						c.GetComponent<Player>().takeDamage (damageCalc(damageTier, hitNum), attributes);
						markedForDeletion = true;
					
					}
					else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
					{
						c.GetComponent<Player>().takeDamage (damageCalc(damageTier, hitNum), attributes);
						markedForDeletion = true;
					}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}
                }
                break;
            case Weapon_Types.Gatling: //Fire hose
                colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x + 10, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.tag == "Enemy")
					{
						if (PlayerNum == 1)
						{
							if (spellTimer % 10 == 0) //modulo ensures that enemy not immediately pushed to back
							{
								if (c.gameObject.GetComponent<Enemy> ().transform.position.x <= 8)
									c.gameObject.GetComponent<Enemy> ().transform.position = new Vector2 (c.gameObject.GetComponent<Enemy> ().transform.position.x + 1, c.gameObject.GetComponent<Enemy> ().transform.position.y);
							}
							spellTimer--;
							if (spellTimer <= 0)
							{
								markedForDeletion = true;
								spellTimer = 50;
							}
						}
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
						c.gameObject.GetComponent<Obstacle> ().specialEffects (this);
                    }
					else if(c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					if (spellTimer % 10 == 0) //modulo ensures that enemy not immediately pushed to back
					{
						if (c.gameObject.GetComponent<Player>().transform.position.x <= 8)
							c.gameObject.GetComponent<Player>().transform.position = new Vector2(c.gameObject.GetComponent<Enemy>().transform.position.x - 1, c.gameObject.GetComponent<Player>().transform.position.y);
					}
					spellTimer--;
					if (spellTimer <= 0)
					{
						markedForDeletion = true;
						spellTimer = 50;
					}
						markedForDeletion = true;
					
					}
					else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
				{
					if (spellTimer % 10 == 0) //modulo ensures that enemy not immediately pushed to back
					{
						if (c.gameObject.GetComponent<Player>().transform.position.x <= 8)
							c.gameObject.GetComponent<Player>().transform.position = new Vector2(c.gameObject.GetComponent<Enemy>().transform.position.x + 1, c.gameObject.GetComponent<Player>().transform.position.y);
					}
						markedForDeletion = true;
				}

					if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
					{
						showPanels (c);
					}

					spellTimer--;
					if (spellTimer <= 0)
					{
						markedForDeletion = true;
						spellTimer = 50;
					}
                }
                break;
			case Weapon_Types.Bow:
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x, transform.position.y));
				foreach (Collider2D c in colliders)
				{
					if(c.gameObject.tag == "Player" && PlayerNum == 2)
					{
						StatusEffect bubbled = new StatusEffect (5);
						bubbled.m_type = StatusType.Bubbled;
						c.gameObject.GetComponent<Player> ().myStatus.AddEffect (bubbled);
						c.GetComponent<Player>().takeDamage (damageCalc(damageTier, hitNum), attributes);
						markedForDeletion = true;

					}
					else if(c.gameObject.tag == "Player2"&& PlayerNum == 1)
					{
						StatusEffect bubbled = new StatusEffect (5);
						bubbled.m_type = StatusType.Bubbled;
						c.gameObject.GetComponent<Player> ().myStatus.AddEffect (bubbled);
						c.GetComponent<Player>().takeDamage (damageCalc(damageTier, hitNum), attributes);
						markedForDeletion = true;
					}
					else if(c.gameObject.tag == "Enemy")
					{
						StatusEffect bubbled = new StatusEffect (5);
						bubbled.m_type = StatusType.Bubbled;
						c.gameObject.GetComponent<Enemy> ().myStatus.AddEffect (bubbled);
						c.GetComponent<Enemy>().takeDamage (damageCalc(damageTier, hitNum), attributes);
						markedForDeletion = true;
					}
				}
			break;
        }
    }

	public override void setDescription(Weapon_Types weapon)
	{
		switch (weapon)
		{
		//Revolver
		case Weapon_Types.Revolver:
				description = "Bounces off the enemy and heals the player if contact is made with the spell.";
				damage = 5;
			break;
			//Rifle
		case Weapon_Types.Rifle:
				description = "Pulls a nearby enemy toward the panel.";
				damage = 10;
			break;
			//Shotgun
		case Weapon_Types.Shotgun:
				description = "Increases damage for a period of time after striking an enemy.";
				damage = 15;
			break;
			//Gatling
		case Weapon_Types.Gatling:
				description = "Uses a hose to push back the opponent.";
				damage = 10;
			break;
			//Cane Gun
		case Weapon_Types.Canegun:
			description = "";
			break;
				//Bow
			case Weapon_Types.Bow:
				description = "Traps the opponent in a bvubble making them weak to lightning";
			break;
		}
	}
}

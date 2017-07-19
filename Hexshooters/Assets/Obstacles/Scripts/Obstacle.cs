using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attributes {None,Fire,Water,Earth,Wind,Wood,Metal,Darkness,Light,Electric,Ice};
public class Obstacle : MonoBehaviour {
	public Attributes[] weaknesses;
	public Attributes[] strengths;
	public Attributes[] attributes;
	public int health;
	public int armorWeakness;
	public Vector2 direction;
	public bool MarkedforDeletion;
	public string stat;
	public int damage;
	bool breakImmune; //flag to ensure that every water shotgun spell doesn't endlessly apply break
	public bool canPass;
	public bool specialCondition; 
	public bool hittable;
	protected bool burning, shocked, wet, blown, frozen, quaked, overgrown, lit, darkened, metalled;

	// Use this for initialization
	void Start () 
	{
		specialCondition = false;
		MarkedforDeletion = false;
		transform.position = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z));
	}

	// Update is called uonce per frame
	public virtual void obstacleUpdate () 
	{
		//Debug.Log (health);
		if (direction != new Vector2 (0, 0))
		{
			move ();
			collide ();
		}
		if (health <= 0)
		{
			MarkedforDeletion = true;
		}
	}
	public virtual void takeDamage(int damage, Attributes[] effects) //created for "break" status
	{
		int multipliers = 1;
		if (this.stat == "break")
		{
			multipliers *= 2;
			stat = "normal";
			breakImmune = true;
		}
		foreach (Attributes a1 in weaknesses)
		{
			foreach (Attributes b1 in effects)
			{
				if (b1 == a1)
					multipliers *= 2;
			}
		}
		foreach (Attributes c1 in strengths)
		{
			foreach (Attributes d1 in effects)
			{
				if (d1 == c1)
					multipliers /= 2;
			}
		}
		this.health -= damage* multipliers;
	}
	public virtual void move()
	{
		//Debug.Log ("Here");
		//Debug.Log (direction);
		Vector2 target =  new Vector3(transform.position.x +direction.x,transform.position.y +direction.y, 0.0f);
		Vector2 position = Vector2.Lerp (transform.position, target, (Time.deltaTime*8));
		transform.position = position;
	}
	public virtual void collide ()
	{
		Collider2D[] colliders;
		colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x, transform.position.y));
		foreach (Collider2D d in colliders) 
		{
			Player p = d.GetComponent<Player> ();
			//if collides with another obstacle, destroys both
			if (d.gameObject.tag == "Obstacle") 
			{
				//Debug.Log (d.GetComponent<Obstacle> ().gameObject != this.gameObject);
				if (d.GetComponent<Obstacle> ().gameObject != this.gameObject) 
				{
					d.GetComponent<Obstacle> ().MarkedforDeletion = true;
					MarkedforDeletion = true;
				}
			}
				//else means hit a player enemy
			else if (d.gameObject.tag == "Enemy") 
			{
				Enemy e = d.GetComponent<Enemy> ();
				e.takeDamage (damage, attributes); //enemy takes dmg
				if (e.transform.position.x != 9) 
				{
					e.transform.position = new Vector3 (e.transform.position.x + 1, e.transform.position.y, e.transform.position.z);
				}
				//move  the piece somewhere, but where?
			} 
			else if (d.gameObject.tag == "Player2") 
			{
				d.GetComponent<Player> ().takeDamage (damage, attributes); //player takes dmg 

					if (d.transform.position.x != 9)
					{
						d.transform.position += new Vector3 (1, 0, 0);
					}
				MarkedforDeletion = true;

			} 
			else if(d.gameObject.tag == "Player")
			{
				d.GetComponent<Player> ().takeDamage (damage, attributes); //player takes dmg


					if (d.GetComponent<Player> ().transform.position.x != 0) 
					{
						d.transform.position += new Vector3(-1f,0f,0f); 
					}
				MarkedforDeletion = true;
			}
						
		}
	}
	public void specialEffects(Spell s)
	{
		bool damageTaken = false;
		Debug.Log (s.direction);
		//Checks each attribute of the spell it's colliding with
		foreach(Attributes spellAtt in s.attributes)
		{
			//Checks each attribute of the current object - a
			foreach (Attributes ObstAtt in attributes)
			{
				//Picks a set of options depending on the type of spell
				switch (spellAtt)
				{
					case Attributes.Darkness:
						//Picks an option to do based on the attributes of the colliding obstacle.
						switch (ObstAtt)
						{
							default: 
								darkened = true;
								if (!damageTaken && hittable)
								{
									takeDamage (s.damageCalc (s.damageTier, s.hitNum), s.attributes);
									damageTaken = true;
								}
							break;
						}
					break;
					case Attributes.Earth:
						//Picks an option to do based on the colliding obstacle.
						switch (ObstAtt)
						{
							default: 
								quaked = true;
								if (!damageTaken && hittable)
								{
									takeDamage (s.damageCalc (s.damageTier, s.hitNum), s.attributes);
									damageTaken = true;
								}
							break;
						}
					break;
					case Attributes.Electric:
						//Picks an option to do based on the colliding obstacle.
						switch (ObstAtt)
						{
							default: 
								shocked = true;
								if (!damageTaken && hittable)
								{
									takeDamage (s.damageCalc (s.damageTier, s.hitNum), s.attributes);
									damageTaken = true;
								}
							break;
						}
					break;
					case Attributes.Fire:
						//Picks an option to do based on the colliding obstacle.
						switch (ObstAtt)
						{
							default: 
								burning = true;
								if (!damageTaken && hittable)
								{
									takeDamage (s.damageCalc (s.damageTier, s.hitNum), s.attributes);
									damageTaken = true;
								}
							break;
						}
					break;
					case Attributes.Ice:
						//Picks an option to do based on the colliding obstacle.
						switch (ObstAtt)
						{
							default: 
								frozen = true;
								if (!damageTaken && hittable)
								{
									takeDamage (s.damageCalc (s.damageTier, s.hitNum), s.attributes);
									damageTaken = true;
								}
							break;
						}
					break;
					case Attributes.Light:
						//Picks an option to do based on the colliding obstacle.
						switch (ObstAtt)
						{
							default: 
								lit = true;
								if (!damageTaken && hittable)
								{
									takeDamage (s.damageCalc (s.damageTier, s.hitNum), s.attributes);
									damageTaken = true;
								}
							break;
						}
					break;
					case Attributes.Metal:
						//Picks an option to do based on the colliding obstacle.
						switch (ObstAtt)
						{
							default: 
								metalled = true;
								if (!damageTaken && hittable)
								{
									takeDamage (s.damageCalc (s.damageTier, s.hitNum), s.attributes);
									damageTaken = true;
								}
							break;
						}
					break;
					case Attributes.Water:
						//Picks an option to do based on the colliding obstacle.
						switch (ObstAtt)
						{
							default: 
								wet = true;
								if (!damageTaken && hittable)
								{
									takeDamage (s.damageCalc (s.damageTier, s.hitNum), s.attributes);
									damageTaken = true;
								}							
							break;
						}
					break;
					case Attributes.Wind:
						//Picks an option to do based on the colliding obstacle.
						switch (ObstAtt)
						{
							//Transfers Momentum from spell
							case Attributes.Earth:
							case Attributes.Fire:
								blown = true;
								if (s.PlayerNum == 1)
									direction = s.direction * 2;
								else
									direction = s.direction * -2;
								s.delete ();
							break;
							default: 
								blown = true;
								if (!damageTaken && hittable)
								{
									takeDamage (s.damageCalc (s.damageTier, s.hitNum), s.attributes);
									damageTaken = true;
								}
							break;
						}
					break;
					case Attributes.Wood:
						//Picks an option to do based on the colliding obstacle.
						switch (ObstAtt)
						{
							default: 
								overgrown = true;
								if (!damageTaken && hittable)
								{
									takeDamage (s.damageCalc (s.damageTier, s.hitNum), s.attributes);
									damageTaken = true;
								}
							break;
						}
					break;
				}
			}
		}
	}
}

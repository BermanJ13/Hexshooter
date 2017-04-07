/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Template : Spell {

	protected bool returnShot;
	protected Vector2 rifleOrigin;
	private int spellTimer;
	// Use this for initialization
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
		switch (weapon)
		{
		//revolver
		case 1:
			if (PlayerNum == 1)
			{
				//Place movment code from below changed to fit thee desired effect.
			}
			else
			{
				//Place the same Code from above with the direction replaced
			}
			break;

			//rifle
		case 2:
			if (PlayerNum == 1)
			{
				//Place movment code from below changed to fit thee desired effect.
			}
			else
			{
				//Place the same Code from above with the direction replaced
			}
			break;

			//shotgun
		case 3:
			if (PlayerNum == 1)
			{
				//Place movment code from below changed to fit thee desired effect.
			}
			else
			{
				//Place the same Code from above with the direction replaced
			}
			break;

			//gatling
		case 4:
			if (PlayerNum == 1)
			{
				//Place movment code from below changed to fit thee desired effect.
			}
			else
			{
				//Place the same Code from above with the direction replaced
			}
			break;

			//cane gun - not priority
		case 5:
			if (PlayerNum == 1)
			{
				//Place movment code from below changed to fit thee desired effect.
			}
			else
			{
				//Place the same Code from above with the direction replaced
			}
			break;
		}
	}

	public override void hitBehavior(int weapon)
	{
		Collider2D[] colliders;
		switch (weapon)
		{
		case 1: 
			//replace with an Area Of Effect from below
			//foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy")
				{
					//Effects
				} 
				else if (c.gameObject.tag == "Obstacle")
				{
					//Effects
				} 
				else if (c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					//Effects if moving left to right
				} 
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1)
				{
					//Effects if moving right to left
				} 
				else if (c.gameObject.tag == "Player2" && PlayerNum == 2)
				{
					if (returnShot)
					{
						//Effects for returning to the player
					}
				} 
				else if (c.gameObject.tag == "Player" && PlayerNum == 1)
				{
					if (returnShot)
					{
						//Effects for returning to the player
					}
				}
			}
			break;
		case 2: 
			//replace with an Area Of Effect from below
			foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy")
				{
					//Effects
				} 
				else if (c.gameObject.tag == "Obstacle")
				{
					//Effects
				} 
				else if (c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					//Effects if moving left to right
				} 
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1)
				{
					//Effects if moving right to left
				} 
				else if (c.gameObject.tag == "Player2" && PlayerNum == 2)
				{
					if (returnShot)
					{
						//Effects for returning to the player
					}
				} 
				else if (c.gameObject.tag == "Player" && PlayerNum == 1)
				{
					if (returnShot)
					{
						//Effects for returning to the player
					}
				}
			}
			break;
		case 3: 
			//replace with an Area Of Effect from below
			foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy")
				{
					//Effects
				} 
				else if (c.gameObject.tag == "Obstacle")
				{
					//Effects
				} 
				else if (c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					//Effects if moving left to right
				} 
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1)
				{
					//Effects if moving right to left
				} 
				else if (c.gameObject.tag == "Player2" && PlayerNum == 2)
				{
					if (returnShot)
					{
						//Effects for returning to the player
					}
				} 
				else if (c.gameObject.tag == "Player" && PlayerNum == 1)
				{
					if (returnShot)
					{
						//Effects for returning to the player
					}
				}
			}
			break;
		case 4: 
			//replace with an Area Of Effect from below
			foreach (Collider2D c in colliders)
			{
				if (c.gameObject.tag == "Enemy")
				{
					//Effects
				} 
				else if (c.gameObject.tag == "Obstacle")
				{
					//Effects
				} 
				else if (c.gameObject.tag == "Player" && PlayerNum == 2)
				{
					//Effects if moving left to right
				} 
				else if (c.gameObject.tag == "Player2" && PlayerNum == 1)
				{
					//Effects if moving right to left
				} 
				else if (c.gameObject.tag == "Player2" && PlayerNum == 2)
				{
					if (returnShot)
					{
						//Effects for returning to the player
					}
				} 
				else if (c.gameObject.tag == "Player" && PlayerNum == 1)
				{
					if (returnShot)
					{
						//Effects for returning to the player
					}
				}
			}
		}
	}

	//If the Spell has multiple descriptions, then set the here using the description string variable. If not Set in the inspector.
	public override void setDescription(int weapon)
	{
		switch (weapon)
		{
		//Revolver
		case 1:
			break;
		//Rifle
		case 2:
			break;
		//Shotgun
		case 3:
			break;
		//Gatling
		case 4:
			break;
		//Cane Gun
		case 5:
			break;
		}
	}
}
//Movement 
	-One Square ahead at a time (move continously) - Set direction in the inspector-
	if(PlayerNum == 1)
		target = new Vector2 (transform.position.x, transform.position.y) + direction;
	else
		target = new Vector2 (transform.position.x, transform.position.y) - direction;	
		
	position = Vector2.Lerp (transform.position, target, (Time.deltaTime*2));
	transform.position = position;

	-A specific Square in relation to the stating point ie. 3 squares ahead of where i fired it.-
	if (PlayerNum == 1)
			{
				//HIts a square three spaces ahead
				if (targetNeeded)
				{
					target = new Vector2 (transform.position.x + 3, transform.position.y);
					targetNeeded = false;
				}
			}
			else
			{
				//HIts a square three spaces ahead
				if (targetNeeded)
				{
					target = new Vector2 (transform.position.x - 3, transform.position.y);
					targetNeeded = false;
				}
			}
				position = Vector2.Lerp (transform.position, target, (Time.deltaTime * 8));
				transform.position = position;

				if (transform.position == new Vector3 (target.x, target.y, 0))
				{
					hitBehavior (1);
				}


	-Trigger Immediately-
	if(PlayerNum == 1)
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 10, transform.position.y));
			else
				colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x - 10, transform.position.y));
	

//Area of Effect
    -Just where the bullet is touching-
	colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));

	-Circle around the bullet-
	colliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), 1.25f);

	-Forward from the bullet (instant)-
	if(PlayerNum == 1)
		colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x + 10, transform.position.y));
	else
		colliders = Physics2D.OverlapAreaAll (transform.position, new Vector2 (transform.position.x - 10, transform.position.y));

	-Forawrd from bullet (Timed Reactions)-
	if (spellTimer % 10 == 0) //modulo ensures that enemy not immediately pushed to back
    {
        if (c.gameObject.GetComponent<Enemy>().transform.position.x <= 8)
            c.gameObject.GetComponent<Enemy>().transform.position = new Vector2(c.gameObject.GetComponent<Enemy>().transform.position.x + 1, c.gameObject.GetComponent<Enemy>().transform.position.y);
    }
    spellTimer--;
    if (spellTimer <= 0)
    {
        markedForDeletion = true;
        spellTimer = 50;
    }
*/
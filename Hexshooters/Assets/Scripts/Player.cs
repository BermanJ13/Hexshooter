using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public int health;
	public Transform spell;
	public int weapon;
	public List<Object> Chamber = new List<Object>();
	public FieldManager field;
	public bool reload;
	public int PlayerNum;
	private bool yAxisInUse = false;
	private bool xAxisInUse = false;
	GameObject[] bulletIndicators;
	Text currentBullet;
	Text pHealth;
	public int armorWeakness;
	public StatusManager myStatus;
	protected string atkbutton;
	public string stat;
	bool breakImmune; //flag to ensure that every water shotgun spell doesn't endlessly apply break
	int stackDmg;
	public Sprite[] playerImages;
	public Sprite[] playerPortaits;
	bool buttonPresed;
	bool allowShot =false;
	public bool hit;
	public Image playerDisplay;
    // Use this for initialization
    void Start () 
	{
		if (PlayerNum == 1)
		{
			playerDisplay = GameObject.Find ("PlayerImage").GetComponent<Image> ();
		}                                                 
		else
		{				
			playerDisplay = GameObject.Find ("PlayerImage_2").GetComponent<Image> ();
		}
		hit = false;
		if (GameObject.Find ("CharSelect") != null)
		{
			if (PlayerNum == 1)
			{
				weapon = GameObject.Find ("CharSelect").GetComponent<CharSelect> ().p1;
			}
			else
			{				
				weapon = GameObject.Find ("CharSelect").GetComponent<CharSelect> ().p2;
			}
		}
		 myStatus = GetComponent<StatusManager>();
		if (PlayerNum == 1)
		{
			if (GameObject.FindGameObjectWithTag("Player2") != null && PlayerNum == 1)
			{
				atkbutton = "Fire_P1";
			} 
			else
			{
				atkbutton = "Fire_Solo";
			}

		}
		else
		{
			atkbutton = "Fire_P2";
		}
		if (PlayerNum == 1)
		{
			currentBullet = GameObject.Find("Current Bullet").GetComponent<Text>();
		}
		else
		{
			currentBullet = GameObject.Find("Current Bullet_P2").GetComponent<Text>();
		}

		if (PlayerNum == 1)
		{
			pHealth = GameObject.Find("PlayerHealth").GetComponent<Text>();
		}
		else
		{
			pHealth = GameObject.Find("PlayerHealth_2").GetComponent<Text>();
		}

		bulletIndicators = new GameObject[8];
		if (PlayerNum == 1)
		{
			bulletIndicators [0] = GameObject.Find ("Player 1 Bottle 1");
			bulletIndicators [1] = GameObject.Find ("Player 1 Bottle 2");
			bulletIndicators [2] = GameObject.Find ("Player 1 Bottle 3");
			bulletIndicators [3] = GameObject.Find ("Player 1 Bottle 4");
			bulletIndicators [4] = GameObject.Find ("Player 1 Bottle 5");
			bulletIndicators [5] = GameObject.Find ("Player 1 Bottle 6");
			bulletIndicators [6] = GameObject.Find ("Player 1 Bottle 7");
			bulletIndicators [7] = GameObject.Find ("Player 1 Bottle 8");
		} 
		else
		{
			bulletIndicators [0] = GameObject.Find ("Player 2 Bottle 1");
			bulletIndicators [1] = GameObject.Find ("Player 2 Bottle 2");
			bulletIndicators [2] = GameObject.Find ("Player 2 Bottle 3");
			bulletIndicators [3] = GameObject.Find ("Player 2 Bottle 4");
			bulletIndicators [4] = GameObject.Find ("Player 2 Bottle 5");
			bulletIndicators [5] = GameObject.Find ("Player 2 Bottle 6");
			bulletIndicators [6] = GameObject.Find ("Player 2 Bottle 7");
			bulletIndicators [7] = GameObject.Find ("Player 2 Bottle 8");
		}
		GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
		if (p2 != null || PlayerNum != 1)
		{
			field = GameObject.FindGameObjectWithTag ("FieldManager").GetComponent<FieldManagerPVP> ();
		} 
		else
		{
			field = GameObject.FindGameObjectWithTag ("FieldManager").GetComponent<FieldManager> ();
		}
		reload = true;
        	health = 100;

    }
	
	// Update is called once per frame
	public void playerUpdate () 
	{
		bool canMove = true;
		hideEmpty ();
		updateCurrentSpell ();
		pHealth.text = health.ToString();
		buttonPresed = false;
		if (!myStatus.IsAffected( StatusType.Bound))
		{
			movement ();
		}
		//if (Input.GetButtonDown (atkbutton) && Chamber.Count > 0)
		//{
		//	if (!myStatus.IsAffected (StatusType.Disabled))
		//	{
		//		initiateSpell ();
		//	}
		//}
		if (allowShot)
		{
			if (Input.GetAxisRaw (atkbutton) > 0 && Chamber.Count > 0)
			{
				if (!buttonPresed)
				{
					buttonPresed = true;
					if (!myStatus.IsAffected (StatusType.Disabled))
					{
						initiateSpell ();
					}
					allowShot = false;
				}
			}
		}
		if (Input.GetAxisRaw (atkbutton) == 0)
			allowShot = true;
		if (Chamber.Count == 0 && field.Handful.Count > 0)
		{
			reload = true;
		}

		if (hit)
		{
			GetComponent<SpriteRenderer>().color = Color.red;
			hit = false;
		}
		else
		{
			GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

		
	
	void movement()
	{
		bool inboundsX = false;
		bool inboundsY = false;
		bool moveRight = true;
		bool moveLeft  = true;
		bool moveUp  = true;
		bool moveDown = true;

		float Horizontal = 0.0f;
		float vertical = 0.0f;
		string playerArea = "";
		string enemyArea = "";

		if (PlayerNum == 1)
		{
			if (GameObject.FindGameObjectWithTag("Player2") != null && PlayerNum == 1)
			{
				Horizontal = Input.GetAxisRaw ("Horizontal_P1");
				vertical = Input.GetAxisRaw ("Vertical_P1");
			} 
			else
			{
				Horizontal = Input.GetAxisRaw ("Horizontal_Solo");
				vertical = Input.GetAxisRaw ("Vertical_Solo");
			}
			playerArea = "playerZone";
			enemyArea = "enemyZone";
		} else if (PlayerNum == 2)
		{
			Horizontal = Input.GetAxisRaw ("Horizontal_P2");
			vertical = Input.GetAxisRaw ("Vertical_P2");
			playerArea = "enemyZone";
			enemyArea = "playerZone";
		} 
		else
		{
			Horizontal = Input.GetAxisRaw ("Horizontal_P1");
			vertical = Input.GetAxisRaw ("Vertical_P1");
			playerArea = "playerZone";
			enemyArea = "enemyZone";
		}
		//Checks for Left and RIght Movement
		if (Horizontal > 0) 
		{
			if (!xAxisInUse)
			{
				xAxisInUse = true;
				Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x + 1, transform.position.y), 0.2f);
				//Checks whether or not something is in the way or if the desired spot is within the player area.
				foreach (Collider2D c in hitColliders)
				{
					if (c.gameObject.tag == playerArea)
					{
						inboundsX = true;
					}
					if (c.gameObject.tag == "Obstacle")
					{
						if(!c.gameObject.GetComponent<Obstacle>().canPass)
							moveRight = false;
					}
				
					if (c.gameObject.tag == enemyArea)
					{
						moveRight = false;
					}
				
					if (c.gameObject.tag == "enemy")
					{
						moveRight = false;
					}
				}
				//Performs the movement if possible
				if (inboundsX)
				if (moveRight)
				{
					transform.position = new Vector2 (transform.position.x + 1, transform.position.y);
				}
			}
		} 
		else if (Horizontal < 0) 
		{
			if (!xAxisInUse)
			{
				xAxisInUse = true;
				Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x - 1, transform.position.y), 0.2f);
				foreach (Collider2D c in hitColliders)
				{
					//Checks whether or not something is in the way or if the desired spot is within the player.
					if (c.gameObject.tag == playerArea)
					{
						inboundsX = true;
					}
					if (c.gameObject.tag == "Obstacle")
					{
						if(!c.gameObject.GetComponent<Obstacle>().canPass)
						moveLeft = false;
					}
				
					if (c.gameObject.tag == enemyArea)
					{
						moveLeft = false;
					}
				
					if (c.gameObject.tag == "enemy")
					{
						moveLeft = false;
					}
				}
				//Performs the movement if possible
				if (inboundsX)
				if (moveLeft)
					transform.position = new Vector2 (transform.position.x - 1, transform.position.y);
			}
		}
		//Checks for Up and Down Movement
		if (vertical > 0)
		{
			if (!yAxisInUse)
			{
				yAxisInUse = true;
				Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y + 1), 0.2f);
				foreach (Collider2D c in hitColliders)
				{
					//Checks whether or not something is in the way or if the desired spot is within the player.
					if (c.gameObject.tag == playerArea)
					{
						inboundsY = true;
					}
					if (c.gameObject.tag == "Obstacle")
					{
						if(!c.gameObject.GetComponent<Obstacle>().canPass)
							moveUp = false;
					}
				
					if (c.gameObject.tag == enemyArea)
					{
						moveUp = false;
					}
				
					if (c.gameObject.tag == "enemy")
					{
						moveUp = false;
					}
				}
				//Performs the movement if possible
				if (inboundsY)
				if (moveUp)
					transform.position = new Vector2 (transform.position.x, transform.position.y + 1);
			} 
		} else if (vertical < 0)
		{
			if (!yAxisInUse)
			{
				yAxisInUse = true;
				Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y - 1), 0.2f);
				foreach (Collider2D c in hitColliders)
				{
					//Checks whether or not something is in the way or if the desired spot is within the player.
					if (c.gameObject.tag == playerArea)
					{
						inboundsY = true;
					}
					if (c.gameObject.tag == "Obstacle")
					{
						if(!c.gameObject.GetComponent<Obstacle>().canPass)
							moveDown = false;
					}
				
					if (c.gameObject.tag == enemyArea)
					{
						moveDown = false;
					}
				
					if (c.gameObject.tag == "enemy")
					{
						moveDown = false;
					}
				}
				//Performs the movement if possible
				if (inboundsY)
				if (moveDown)
					transform.position = new Vector2 (transform.position.x, transform.position.y - 1);
			}
		}
		if (PlayerNum == 1)
		{
			if (GameObject.FindGameObjectWithTag("Player2") != null && PlayerNum == 1)
			{
				if (Input.GetAxisRaw ("Vertical_P1") == 0)
				{
					yAxisInUse = false;
				}
				if (Input.GetAxisRaw ("Horizontal_P1") == 0)
				{
					xAxisInUse = false;
				}
			} 
			else
			{
				if (Input.GetAxisRaw ("Vertical_Solo") == 0)
				{
					yAxisInUse = false;
				}
				if (Input.GetAxisRaw ("Horizontal_Solo") == 0)
				{
					xAxisInUse = false;
				}
			}

		} 
		else
		{
			if (Input.GetAxisRaw ("Horizontal_P2") == 0)
			{
				xAxisInUse = false;
			}
			if (Input.GetAxisRaw ("Vertical_P2") == 0)
			{
				yAxisInUse = false;
			}
		}
	}
	void initiateSpell()
	{

		GameObject go = (GameObject)Instantiate(Chamber[0],new Vector2(transform.position.x,transform.position.y),Quaternion.identity);

		////get the thing component on your instantiated object
		Spell mything = go.GetComponent<Spell>();

		////set a member variable (must be PUBLIC)
		mything.weaponUsed = weapon; 
		mything.PlayerNum = PlayerNum;
		Chamber.RemoveAt (0);
	}

	void hideEmpty()
	{
		for (int i = bulletIndicators.Length - 1; i >= Chamber.Count; i--)
		{
			bulletIndicators [i].SetActive (false);
		}
	}
	void updateCurrentSpell()
	{
		if (Chamber.Count > 0)
		{
			Spell curSpell = ((GameObject)Resources.Load (Chamber [0].name)).GetComponent<Spell> ();
			currentBullet.text = curSpell.name + ": " + curSpell.damage;
		} 
		else
		{
			currentBullet.text = "";
		}
	}
	public void takeDamage(int damage) //created for "break" status
	{
        AudioSource hitSound = this.gameObject.GetComponent<AudioSource>();
        hitSound.Play();

		int multipliers = 1;
		if (myStatus.IsAffected(StatusType.Break))
		{
			multipliers *= 2;
		}
		if (myStatus.IsAffected(StatusType.Shield))
		{
			multipliers /= 2;
		}
		if (myStatus.IsAffected(StatusType.Stacking))
		{
			this.health -= stackDmg;
			stackDmg++;
		}
		else
		{
			stackDmg = 0;
		}

		this.health -= damage* multipliers + stackDmg;
		hit = true;
	}
	public void updatePlayerImage()
	{
		switch (weapon)
		{
			case 1:
				GetComponent<SpriteRenderer> ().sprite = playerImages [0];
				if (playerDisplay != null)
				{
					playerDisplay.sprite = playerPortaits [0];
					playerDisplay.color = new Color (255, 255, 255, 255);
				}
			break;
			case 2:
			break;
			case 3:
				GetComponent<SpriteRenderer> ().sprite = playerImages [1];
				if (playerDisplay != null)
				{
					playerDisplay.sprite = playerPortaits [1];
					playerDisplay.color = new Color (255, 255, 255, 255);
				}
			break;
			case 4:
			break;
		}
	}
}

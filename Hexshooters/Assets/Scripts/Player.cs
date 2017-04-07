using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    	public int health;
	public Transform spell;
	public int weapon;
	public List<Object> Chamber = new List<Object>();
	public FieldManager field;
	public bool reload;
    public StatusManager myStatus;

    // Use this for initialization
    void Start () 
	{
		field = GameObject.FindGameObjectWithTag ("FieldManager").GetComponent<FieldManager>();
		reload = true;
        	health = 100;

        this.myStatus = this.GetComponent<StatusManager>();
    }
	
	// Update is called once per frame
	public void playerUpdate () 
	{
		//Moves the character
		movement();

		if (Input.GetKeyDown (KeyCode.Space) && Chamber.Count >0) 
		{
			initiateSpell ();

			// Transform earth = Instantiate(variable, position, Identity)
			//Spell earth2 = earth.GetComponent<Spell>();
			//earth2 = weaponNumber;
		}
if (Chamber.Count == 0 && field.Handful.Count > 0)
			reload = true;
	}

		
	
	void movement()
	{
		bool moveRight = false;
		bool moveLeft  = false;
		bool moveUp  = false;
		bool moveDown = false;
		//Checks for Left and RIght Movement
		if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 1, transform.position.y),0.2f);
			//Checks whether or not something is in the way or if the desired spot is within the player area.
			foreach( Collider2D c in hitColliders)
			{
				if (c.gameObject.tag == "playerZone")
				{
					moveRight = true;
				}
				if(c.gameObject.tag == "obstacle")
				{
					moveRight = false;
				}
				
				if(c.gameObject.tag == "enemyZone")
				{
					moveRight = false;
				}
				
				if(c.gameObject.tag == "enemy")
				{
					moveRight = false;
				}
			}
			//Performs the movement if possible
			if (moveRight)
				transform.position = new Vector2 (transform.position.x + 1, transform.position.y);
		} 
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - 1, transform.position.y),0.2f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "playerZone")
				{
					moveLeft = true;
				}
				if(c.gameObject.tag == "obstacle")
				{
					moveLeft = false;
				}
				
				if(c.gameObject.tag == "enemyZone")
				{
					moveLeft = false;
				}
				
				if(c.gameObject.tag == "enemy")
				{
					moveLeft = false;
				}
			}
			//Performs the movement if possible
			if(moveLeft)
				transform.position = new Vector2 (transform.position.x - 1, transform.position.y);
		}
		//Checks for Up and Down Movement
		if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + 1),0.2f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "playerZone")
				{
					moveUp = true;
				}
				if(c.gameObject.tag == "obstacle")
				{
					moveUp = false;
				}
				
				if(c.gameObject.tag == "enemyZone")
				{
					moveUp = false;
				}
				
				if(c.gameObject.tag == "enemy")
				{
					moveUp = false;
				}
			}
			//Performs the movement if possible
			if(moveUp)
				transform.position = new Vector2 (transform.position.x, transform.position.y + 1);
		} 
		else if (Input.GetKeyDown (KeyCode.DownArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 1),0.2f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "playerZone")
				{
					moveDown = true;
				}
				if(c.gameObject.tag == "obstacle")
				{
					moveDown = false;
				}
				
				if(c.gameObject.tag == "enemyZone")
				{
					moveDown = false;
				}
				
				if(c.gameObject.tag == "enemy")
				{
					moveDown = false;
				}
			}
			//Performs the movement if possible
			if(moveDown)
				transform.position = new Vector2 (transform.position.x, transform.position.y - 1);
		}
		//Debug.Log (moveRight);
	}
	void initiateSpell()
	{

		GameObject go = (GameObject)Instantiate(Chamber[0],new Vector2(transform.position.x,transform.position.y),Quaternion.identity);

		////get the thing component on your instantiated object
		Spell mything = go.GetComponent<Spell>();

		////set a member variable (must be PUBLIC)
		mything.weaponUsed = 1; 

		Chamber.RemoveAt (0);

	}


}

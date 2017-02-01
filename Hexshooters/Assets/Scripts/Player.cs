﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public LayerMask mask = LayerMask.NameToLayer("PlayerPanel");
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void FixedUpdate() 
	{
		//Moves the character
		movement();


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
			if(moveRight)
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
	}
}
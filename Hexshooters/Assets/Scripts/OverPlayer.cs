using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		movement ();
		interaction ();
	}
	void movement()
	{
		bool moveRight = false;
		bool moveLeft  = false;
		bool moveUp  = false;
		bool moveDown = false;
		//Checks for Left and RIght Movement
		if (Input.GetKey (KeyCode.RightArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 0.25f, transform.position.y),0.2f);
			//Checks whether or not something is in the way or if the desired spot is within the player area.
			foreach( Collider2D c in hitColliders)
			{
				if (c.gameObject.tag == "Map")
				{
					moveRight = true;
				}
			}
			//Performs the movement if possible
			if (moveRight)
				transform.position = new Vector2 (transform.position.x + 0.25f, transform.position.y);
		} 
		else if (Input.GetKey (KeyCode.LeftArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - 0.25f, transform.position.y),0.2f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					moveLeft = true;
				}
			}
			//Performs the movement if possible
			if(moveLeft)
				transform.position = new Vector2 (transform.position.x - 0.25f, transform.position.y);
		}
		//Checks for Up and Down Movement
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + 0.25f),0.2f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					moveUp = true;
				}
			}
			//Performs the movement if possible
			if(moveUp)
				transform.position = new Vector2 (transform.position.x, transform.position.y + 0.25f);
		} 
		else if (Input.GetKey (KeyCode.DownArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 0.25f),0.2f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					moveDown = true;
				}
			}
			//Performs the movement if possible
			if(moveDown)
				transform.position = new Vector2 (transform.position.x, transform.position.y - 0.25f);
		}
	}
	void interaction()
	{
		
	if (Input.GetKey (KeyCode.A)) 
	{
		Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y),1f);
		if (hitCollider.gameObject.tag == "Building")
		{
			//Load Building Scene
		}
		if (hitCollider.gameObject.tag == "Object")
		{
			//Load Building Scene
		}
		if (hitCollider.gameObject.tag == "NPC")
		{
			//Load Building Scene
		}
	}
}
}

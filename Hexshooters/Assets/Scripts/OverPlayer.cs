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
		bool inboundsX = false;
		bool inboundsY = false;
		bool moveRight = true;
		bool moveLeft  = true;
		bool moveUp  =   true;
		bool moveDown =  true;
		//Checks for Left and RIght Movement
		if (Input.GetKey (KeyCode.RightArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 0.1f, transform.position.y),0.1f);
			//Checks whether or not something is in the way or if the desired spot is within the player area.
			foreach( Collider2D c in hitColliders)
			{
				if (c.gameObject.tag == "Map")
				{
					inboundsX = true;
				}

				if (c.gameObject.tag == "Boundary")
				{
					moveRight = false;
				}
			}
			//Performs the movement if possible
			if (inboundsX)
			{
				if (moveRight)
					transform.position = new Vector2 (transform.position.x + 0.15f, transform.position.y);
			}
		} 
		else if (Input.GetKey (KeyCode.LeftArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - 0.1f, transform.position.y),0.1f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					Debug.Log ("Damn");
					inboundsX = true;
				}
				if (c.gameObject.tag == "Boundary")
				{
					Debug.Log ("Dammit");
					moveLeft = false;
				}

			}
			//Performs the movement if possible
			if (inboundsX)
			{
				if (moveLeft)
					transform.position = new Vector2 (transform.position.x - 0.15f, transform.position.y);
			}
		}
		//Checks for Up and Down Movement
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + 0.1f),0.1f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					inboundsY = true;
				}
				if (c.gameObject.tag == "Boundary")
				{
					moveUp = false;
				}
			}
			//Performs the movement if possible
			if (inboundsY)
			{
				if (moveUp)
					transform.position = new Vector2 (transform.position.x, transform.position.y + 0.15f);
			}
		} 
		else if (Input.GetKey (KeyCode.DownArrow)) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 0.1f),0.1f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					inboundsY = true;
				}
				if (c.gameObject.tag == "Boundary")
				{
					moveDown = false;
				}
			}
			//Performs the movement if possible
			if (inboundsY)
			{
				if (moveDown)
					transform.position = new Vector2 (transform.position.x, transform.position.y - 0.15f);
			}
		}
	}
	void interaction()
	{

        Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 1f);
        if (hitCollider.gameObject.tag=="Cutscene")
        {
            Debug.Log("Cutscene");
        }
        else if (Input.GetKey (KeyCode.Return)) 
	    {
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
                 Debug.Log("NPC");
		    }
	    }
    }
}

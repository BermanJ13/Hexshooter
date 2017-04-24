using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverPlayer : MonoBehaviour {

    List<string> script;

    public GameObject dialog;
    [SerializeField]

    void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	// Use this for initialization
	void Start () {
        dialog = GameObject.FindGameObjectWithTag("DialogMngr");
        script = new List<string>();
        script.Add("Assets/Dialogue/Text/TestCutscene.txt");
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
		if (Input.GetAxisRaw ("Horizontal_Solo") > 0) 
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
		else if (Input.GetAxisRaw ("Horizontal_Solo") < 0) 
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
		if (Input.GetAxisRaw ("Vertical_Solo") > 0) 
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
		else if (Input.GetAxisRaw ("Vertical_Solo") < 0) 
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
            dialog.GetComponent<DialogueManager>().Load(script[0]);
            hitCollider.gameObject.SetActive(false);
        }
		else if (Input.GetButtonDown ("Submit_Solo")) 
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

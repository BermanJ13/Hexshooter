using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverPlayer : MonoBehaviour {

	List<string> script;
	public bool cutscene;
	public Sprite[] playerImages;

	public int style = 0; 
	public DialogueManager dialog;
	[SerializeField]
	public bool battle;
	public bool charUnlock;

	public int weapon;
	UniversalSettings us;
	public Trigger currentTrig; 
	public Trigger[] triggers; 
	public List<string> activatedTriggers = new List<string>();
	public bool returnFromBattle;
	public bool once = false;
    void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	// Use this for initialization
	void Start () {
		weapon = 6;
		dialog = GameObject.FindGameObjectWithTag("DialogMngr").GetComponent<DialogueManager>();
		us = GameObject.Find("__app").GetComponent<UniversalSettings> ();
		style = us.style;
    }
	
	// Update is called once per frame
	void Update () 
	{	
		if (once)
		{
			triggers = FindObjectsOfType<Trigger> ();
			once = true;
		}
		
		if (SceneManager.GetActiveScene().name == "Battle" || SceneManager.GetActiveScene().name == "Active Battle")
			battle = true;
		else
			battle = false;

		if (!battle)
		{
			if (dialog.dialogueLines.Count > 0)
			{
				cutscene = true;
			}
			if (returnFromBattle)
			{
				foreach (string t2 in activatedTriggers)
				{
					GameObject.Find (t2).GetComponent<Trigger> ().interacted = true;
				}
				interactTrigger ();
				if(currentTrig !=null)
				{
					if (currentTrig.postBattle || currentTrig.postRewardActivator)
					{
						currentTrig.battleResults ();
					}
				}
				returnFromBattle = false;
			}
			if (weapon == 1)
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255, 1);
			else
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 255, 1);
			
			if (charUnlock)
			{
				changePlayer ();
			}

			if (!cutscene)
			{
				movement ();
				interactTrigger ();
			}
			else
			{
				if (!currentTrig.passable)
				{
					currentTrig.enforceBoundary ();
				}
				if (currentTrig.preRewardActivator)
				{
					currentTrig.preBattle ();
				}
				if (Input.GetButtonDown ("Submit_Solo"))
				{
					dialog.nextLine ();
				}
				if (dialog.dialogueLines.Count == 0)
				{
					cutscene = false;
				}

				if (currentTrig != null && !cutscene)
				{
					if (currentTrig.battle && !currentTrig.interacted)
					{
						us.mapfile = currentTrig.scenario;
						currentTrig.interacted = true;
						activatedTriggers.Add(currentTrig.name);
						SceneManager.LoadScene ("Battle");
					}

					if (currentTrig.exit)
						SceneManager.LoadScene ("Win");
				}
			}
		}
		else
		{
			this.GetComponent<SpriteRenderer> ().color = new Color (255, 0, 0, 0);
			currentTrig.interacted = true;
		}
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
				{
					transform.position = new Vector2 (transform.position.x + 0.15f, transform.position.y);
					this.gameObject.GetComponent<SpriteRenderer> ().sprite = playerImages[0];
				}
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
					//Debug.Log ("Damn");
					inboundsX = true;
				}
				if (c.gameObject.tag == "Boundary")
				{
					//Debug.Log ("Dammit");
					moveLeft = false;
				}

			}
			//Performs the movement if possible
			if (inboundsX)
			{
				if (moveLeft)
				{
					transform.position = new Vector2 (transform.position.x - 0.15f, transform.position.y);
					this.gameObject.GetComponent<SpriteRenderer> ().sprite = playerImages[1];
				}
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
				{
					transform.position = new Vector2 (transform.position.x, transform.position.y + 0.15f);
					this.gameObject.GetComponent<SpriteRenderer> ().sprite = playerImages[2];
				}
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
				{
					transform.position = new Vector2 (transform.position.x, transform.position.y - 0.15f);
					this.gameObject.GetComponent<SpriteRenderer> ().sprite = playerImages[3];
				}
			}
		}
	}
	void changePlayer()
	{
		if (Input.GetButtonDown ("SwitchButton"))
		{
			weapon++;
			if (weapon > 3)
				weapon = 1;
			if (weapon == 2)
				weapon = 3;
			if (weapon < 0)
				weapon = 3;
			
		}
	}
	void interactTrigger()
	{
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), .2f);
		foreach (Collider2D hitCollider in hitColliders)
		{
			if (hitCollider.gameObject.tag == "Trigger")
			{
				currentTrig = hitCollider.GetComponent<Trigger> ();
				if (currentTrig.touch)
				{
					if (currentTrig.interacted == false || (currentTrig.repeatable == true && currentTrig.interacted == true))
					{
						dialog.Load (currentTrig.script);
						if (!currentTrig.battle)
						{
							currentTrig.interacted = true;
							activatedTriggers.Add(currentTrig.name);
						}
						//cutscene = true;
						Debug.Log ("made it");
					}
				}
			}
		}
	}
}

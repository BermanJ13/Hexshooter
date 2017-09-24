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

	public Weapon_Types weapon;
	public int currentCharacter;
	UniversalSettings us;
	public GameObject pause,deck;
	public Trigger currentTrig; 
	public Trigger[] triggers; 
	public List<string> activatedTriggers = new List<string>();
	public List<Weapon_Types> availableWeapons = new List<Weapon_Types>();
	public bool returnFromBattle;
	public bool once = false;
    void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	// Use this for initialization
	void Start () {
		//Sets the players original weapon 
		availableWeapons.Add (Weapon_Types.Revolver);
		weapon = Weapon_Types.Revolver;
		currentCharacter = 1;

		//FInds the Dialouge Manager, Pause Menu and Settings Files
		dialog = GameObject.FindGameObjectWithTag("DialogMngr").GetComponent<DialogueManager>();
		us = GameObject.Find("__app").GetComponent<UniversalSettings> ();
		//Sets the playstyle of the mtches according to the Settings file setting
		style = us.style;

		//Turn off the pause menu
		pause.SetActive(false);
		deck.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () 
	{	
		//If this is the first time booting up the map Find all the triggers then prevent this from running again
		if (once)
		{
			triggers = FindObjectsOfType<Trigger> ();
			once = true;

		}

		//If we are currently in battle set battle to true
		if (SceneManager.GetActiveScene().name == "Battle" || SceneManager.GetActiveScene().name == "Active Battle")
			battle = true;
		else
			battle = false;

		//If we arent in battle perform all overworld functions
		if (!battle)
		{
			//If dialogue is loaded set it to cutscene mode
			if (dialog.dialogueLines.Count > 0)
			{
				cutscene = true;
			}
			//If you are just getting out of battle 
			if (returnFromBattle)
			{
				//Deactivatre all previously activated triggers
				foreach (string t2 in activatedTriggers)
				{
					GameObject.Find (t2).GetComponent<Trigger> ().interacted = true;
				}

				//Cheks to see if the trigger you are overalpping with a trigger
				interactTrigger ();

				//If there are things to do after a battle has concluded it performs that function.
				if(currentTrig !=null)
				{
					if (currentTrig.postBattle || currentTrig.postRewardActivator)
					{
						currentTrig.battleResults ();
					}
				}
				//Sets return from battle false so this wont be repeated
				returnFromBattle = false;
			}

			//Changes sprite according to weapon - WIll change to a swithc when more chars are available
			if (weapon == Weapon_Types.Revolver)
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255, 1);
			else
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 255, 1);

			//If you have more than one character unlock allow you to change - This implementation will likely change.
			if (charUnlock)
			{
				changePlayer ();
			}

			//If a cutscene is not active then allow movement and trigger interaction
			if (!cutscene)
			{
				movement ();
				interactTrigger ();
				if (Input.GetButtonDown ("PauseOver"))
				{
					if(pause.activeSelf)
						pause.SetActive (false);
					else
						pause.SetActive (true);
				}
			}
			else
			{
				//If it's a boundary move the player off of it to a specific spot and then give a cutscene teling them not to go that way.
				if (currentTrig.passable)
				{
					currentTrig.enforceBoundary ();
				}
				//GIve a reward before a battle such a sa character joining you.
				if (currentTrig.preRewardActivator)
				{
					currentTrig.preBattle ();
				}
				//Advance through dialouge
				if (Input.GetButtonDown ("Submit_Solo"))
				{
					dialog.nextLine ();
				}
				//End a cutscene when a dialogue is finished
				if (dialog.dialogueLines.Count == 0)
				{
					cutscene = false;
				}
				//Checks for things to do after the end of a cutscene such as loading a battle or exiting the single player game.
				if (currentTrig != null && !cutscene)
				{
					//Sets up the battle as designated in a trigger and then launches the battle scene
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
			//Sets you invisble during a battle and make ssure the trigger you interacted with is not set as such
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
					transform.position = new Vector2 (transform.position.x + 0.03f, transform.position.y);
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
					transform.position = new Vector2 (transform.position.x - 0.03f, transform.position.y);
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
					transform.position = new Vector2 (transform.position.x, transform.position.y + 0.03f);
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
					transform.position = new Vector2 (transform.position.x, transform.position.y - 0.03f);
					this.gameObject.GetComponent<SpriteRenderer> ().sprite = playerImages[3];
				}
			}
		}
	}
	void changePlayer()
	{
		if (Input.GetButtonDown ("SwitchButton"))
		{
			currentCharacter++;
			if (currentCharacter >= availableWeapons.Count)
			{
				currentCharacter = 0;
			}
			else if (currentCharacter < 0)
			{
				currentCharacter = availableWeapons.Count-1;
			}
			weapon = availableWeapons[currentCharacter];
		}
	}
	void interactTrigger()
	{
		//Checks whether or not you're overlapping with a trigger
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), .2f);
		foreach (Collider2D hitCollider in hitColliders)
		{
			if (hitCollider.gameObject.tag == "Trigger")
			{
				//If the trigger you're overlapping with is touch activated
				currentTrig = hitCollider.GetComponent<Trigger> ();
				//Checks to see if the trigger is active and ready to be interacted with
				if (currentTrig.active)
				{
					if (currentTrig.touch)
					{
						//Checks to see if it's already been interacted with or if it has, but is repeatable
						if (currentTrig.interacted == false || (currentTrig.repeatable == true && currentTrig.interacted == true))
						{
							//Loads dialouge and sets it to interacted if it doesnt involve a battle.
							dialog.Load (currentTrig.script);
							if (!currentTrig.battle)
							{
								currentTrig.interacted = true;
								activatedTriggers.Add (currentTrig.name);
							}
							//cutscene = true;
							Debug.Log ("made it");
						}
					}
				}
			}
		}
	}
}

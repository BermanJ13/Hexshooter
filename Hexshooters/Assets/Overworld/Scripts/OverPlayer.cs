using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;

public enum Over_States { Cutscene, Roam, Menu, Controls, Battle};
public enum Face_Dir { Forward, Backward, Left, Right};
public class OverPlayer : MonoBehaviour {

	List<string> script;
	public bool cutscene;
	public string[] playerImages;

	public int style = 0; 
	public DialogueManager dialog;
	[SerializeField]
	public bool battle, charUnlock, switchChar;

	public Weapon_Types weapon;
	public int weaponMax, currentCharacter;
	UniversalSettings us;
	public GameObject pause,deck,quest;
	public Trigger currentTrig; 
	public Trigger[] triggers; 
	public List<string> activatedTriggers = new List<string>();
	public List<Weapon_Types> availableWeapons = new List<Weapon_Types>();
	public bool returnFromBattle, once = false;
	public Over_States currentState = Over_States.Roam, lastState;
	SpriteRenderer spriteR;
	List<GameObject> SlotsDeckA = new List<GameObject>(), SlotsPack = new List<GameObject>(), SlotsDeckB = new List<GameObject>();
	public List<Object> groupPack = new List<Object>();
	List<Image> ImagesDeckA = new List<Image>(), ImagesPack = new List<Image>(), ImagesDeckB = new List<Image>();
	List<Text> NamesDeckA = new List<Text>(), NamesPack = new List<Text>(), NamesDeckB = new List<Text>();
	Canvas DeckA, DeckB, Pack;   
	int currentList;
	public Sprite defaultSlot;
	GameObject runeDisplay;
	Text runeDamage, runeName, runeDesc, CharNAme, WeapNAme;
	int deckAIndex = 0, deckBIndex = 0, packIndex = 0;
	bool inPack = false;
	int selectedSpell = 100, originalMenu = 0;
	Image charImage;
	List<Object>[] properLists;
	public Dictionary <Weapon_Types,Character> characters;
	public Animator playerAnimator;
	public List<Quest> quests;
	public Face_Dir direction = Face_Dir.Forward;
	GameObject boulder;
    Attributes[] blastAttributes;

    int idleTimer = 0;
	public List<Quest> questList;
	public void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	// Use this for initialization
	void Start () {
		//Creates the characters for use in the game and stores them in a dictionary
		characters= new Dictionary<Weapon_Types,Character>();
		createCharacters ();

		//Initalizes the Array that will hold references to the lists that are currently being used
		properLists = new List<Object>[3];

		//Saves this object spriterenderer for later usage
		spriteR = this.gameObject.GetComponent<SpriteRenderer> ();

		//Sets the players original weapon 
		availableWeapons.Add (Weapon_Types.Revolver);
		weapon = Weapon_Types.Revolver;
		currentCharacter = 0;
		weaponMax = 30;
		cutscene = false;
		switchChar = true;
        blastAttributes = new Attributes[1];
        blastAttributes[0] = Attributes.Fire;

		//FInds the Dialouge Manager, Pause Menu and Settings Files
		dialog = GameObject.FindGameObjectWithTag("DialogMngr").GetComponent<DialogueManager>();
		us = GameObject.Find("__app").GetComponent<UniversalSettings> ();
		//Sets the playstyle of the mtches according to the Settings file setting
		style = us.style;

		//Turn off the pause menu
		pause.SetActive(false);
		deck.SetActive(false);
		quest.SetActive (false);

		//Fill the Decks Initially
		for (int i = 0; i < 15; i++)
		{
			characters[Weapon_Types.Revolver].DeckA.Add (Resources.Load ("Fire"));
			characters[Weapon_Types.Revolver].DeckA.Add (Resources.Load ("Wind"));
			characters[Weapon_Types.Revolver].DeckB.Add (Resources.Load ("Wind"));
			characters[Weapon_Types.Revolver].DeckB.Add (Resources.Load ("Wind"));

			characters[Weapon_Types.Shotgun].DeckA.Add (Resources.Load ("Earth"));
			characters[Weapon_Types.Shotgun].DeckA.Add (Resources.Load ("Water"));
			characters[Weapon_Types.Shotgun].DeckB.Add (Resources.Load ("Earth"));
			characters[Weapon_Types.Shotgun].DeckB.Add (Resources.Load ("Earth"));
		}
		//Fill the Decks Initially
		for (int i = 0; i < 5; i++)
		{
			groupPack.Add (Resources.Load ("Lightning"));
		}
		playerAnimator = gameObject.GetComponent<Animator> ();
    }
	
	// Update is called once per frame
	void Update () 
	{	
		//If this is the first time booting up the map Find all the triggers then prevent this from running again
        //also reset boulders so when you leave a map you leave no trace
		if (once)
		{
            if (boulder != null)
            {
                Destroy(boulder);
            }
			triggers = FindObjectsOfType<Trigger> ();
			once = true;

		}

		//If we are currently in battle set battle to true
		if (SceneManager.GetActiveScene ().name == "Battle" || SceneManager.GetActiveScene ().name == "Active Battle")
		{
			if(lastState != currentState && currentState != Over_States.Battle)
				lastState = currentState;
			currentState = Over_States.Battle;
		}


		//If dialogue is loaded set it to cutscene mode
		if (dialog.dialogueLines.Count > 0)
		{
			if(lastState != currentState && currentState != Over_States.Cutscene)
				lastState = currentState;
			currentState = Over_States.Cutscene;
		}

		//If you are just getting out of battle
		if (returnFromBattle)
		{
			if(lastState != currentState && currentState != Over_States.Cutscene)
				lastState = currentState;
			currentState = Over_States.Roam; 
			spriteR.color = new Color (255, 255, 255, 1);

			//Deactivatre all previously activated triggers
			foreach (string t2 in activatedTriggers)
			{
				GameObject.Find (t2).GetComponent<Trigger> ().interacted = true;
			}

			//Checks to see if the trigger you are overalpping with a trigger
			interactTrigger ();

			//If here are things to do after a battle has concluded it performs that function.
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

		//If a character switch has been made then update accordinggliy
		if(switchChar)
		{
			//Changes sprites, and references to the appropriate settings
			switch (weapon)
			{
				case Weapon_Types.Revolver:
					spriteR.color = new Color (255, 255, 255, 1);
					playerImages [0] = "Matt_Right";
					playerImages [1] = "Matt_Left";
					playerImages [2] = "Matt_Up";
					playerImages [3] = "Matt_Down";
					playerImages [4] = "Matt_Idle";

					if (currentState == Over_States.Menu)
					{
						charImage.sprite = Resources.Load<Sprite> ("MattHead");
						WeapNAme.text = "Revolver";
						CharNAme.text = "Buckeye";
						properLists [0] = characters[Weapon_Types.Revolver].DeckA;
						properLists [1] = characters[Weapon_Types.Revolver].DeckB;
						properLists [2] = groupPack;
						changeDeckDisplay (characters[Weapon_Types.Revolver].activeDeck);
					}
				break;
				case Weapon_Types.Shotgun:
					spriteR.color = new Color (255, 255, 255, 1);
					playerImages [0] = "John_Right";
					playerImages [1] = "John_Left";
					playerImages [2] = "John_Up";
					playerImages [3] = "John_Down";
					playerImages [4] = "John_Idle";

					if (currentState == Over_States.Menu)
					{
						charImage.sprite = Resources.Load<Sprite> ("JohnHead");
						WeapNAme.text = "Shotgun";
						CharNAme.text = "John";
						properLists [0] = characters[Weapon_Types.Shotgun].DeckA;
						properLists [1] = characters[Weapon_Types.Shotgun].DeckB;
						properLists [2] = groupPack;
						changeDeckDisplay (characters[Weapon_Types.Shotgun].activeDeck);
					}
				break;
				default:
					spriteR.color = new Color (0, 0, 255, 1);
				break;
			}
			playerAnimator.Play(playerImages[3]);

			switchChar = false;
		}

		//Checks for things to do after the end of a cutscene such as loading a battle or exiting the single player game.
		if (currentTrig != null && currentState != Over_States.Cutscene)
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

		//Handles the updating based on what state the game is in
		switch (currentState)
		{
			case Over_States.Roam:

				//If you have more than one character currently unlocked allow you to change
				if (availableWeapons.Count > 1)
				{
					if (Input.GetButtonDown ("SwitchButton"))
					{
						changePlayer ();
					}
				}

				movement ();
				interactTrigger ();
				interactButton ();
				overAbility ();	

				//Opens the Deck MEnu when the o button is pressed
				if (Input.GetButtonDown ("PauseOver"))
				{
					lastState = currentState;
					currentState = Over_States.Menu;
					pause.SetActive (true);
					EventSystem.current.SetSelectedGameObject (GameObject.Find ("DeckMenuButton"));
					
				}
			break;
				
			case Over_States.Menu:
			
				if (Input.GetButtonDown ("PauseOver"))
				{
					deck.SetActive (false);
					currentState = lastState;
				}
				if (EventSystem.current.currentSelectedGameObject != null)
				{
					if (EventSystem.current.currentSelectedGameObject.GetComponent<RuneInfo> () != null)
					{
						runeName.text = EventSystem.current.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
						runeDamage.text = "Damage: " + EventSystem.current.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
						runeDesc.text = EventSystem.current.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
						runeDisplay.GetComponent<Image> ().sprite = EventSystem.current.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
					}
				}
				scrollDeck ();

				if (Input.GetButtonDown ("PauseOver"))
				{
					pause.SetActive (false);
					currentState = lastState;
					EventSystem.current.SetSelectedGameObject (GameObject.Find("DeckMenuButton"));
				}

				if (deck.activeSelf || quest.activeSelf)
				{
					if(Input.GetButtonDown("Cancel_Solo"))
					{
						ToPause ();
						EventSystem.current.SetSelectedGameObject (GameObject.Find("DeckMenuButton"));
					}
				}
				break;
				
			case Over_States.Controls:
			break;
				
			case Over_States.Cutscene:

				//If you have more than one character unlock allow you to change - This implementation will likely change.
				if (availableWeapons.Count > 1)
				{
					if (Input.GetButtonDown ("SwitchButton"))
					{
						changePlayer ();
					}
				}
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
					currentState = lastState;
				}
			break;
			case Over_States.Battle:
				//Sets you invisble during a battle and make ssure the trigger you interacted with is set as such
				this.GetComponent<SpriteRenderer> ().color = new Color (255, 0, 0, 0);
			break;
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
			direction = Face_Dir.Right;
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 0.1f, transform.position.y),0.1f);
			//Checks whether or not something is in the way or if the desired spot is within the player area.
			foreach( Collider2D c in hitColliders)
			{
				if (c.gameObject.tag == "Map")
				{
					inboundsX = true;
				}

				if (c.gameObject.tag == "Boundary" || c.gameObject.tag == "Obstacle")
				{
					moveRight = false;
				}
                if (c.gameObject.tag == "Trigger")
                {
                    if (!c.gameObject.GetComponent<Trigger>().interacted || (c.gameObject.GetComponent<Trigger>().interacted && c.gameObject.GetComponent<Trigger>().repeatable == true))
                        moveRight = false;
                }
            }
			//Performs the movement if possible
			if (inboundsX)
			{
				if (moveRight)
				{
					transform.position = new Vector2 (transform.position.x + 0.15f, transform.position.y);
					playerAnimator.Play(playerImages[0]);
				}
			}
		} 
		else if (Input.GetAxisRaw ("Horizontal_Solo") < 0) 
		{
			direction = Face_Dir.Left;
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - 0.1f, transform.position.y),0.1f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					//Debug.Log ("Damn");
					inboundsX = true;
				}
				if (c.gameObject.tag == "Boundary" || c.gameObject.tag == "Obstacle")
				{
					//Debug.Log ("Dammit");
					moveLeft = false;
				}
                if (c.gameObject.tag == "Trigger")
                {
                    if (!c.gameObject.GetComponent<Trigger>().interacted || (c.gameObject.GetComponent<Trigger>().interacted && c.gameObject.GetComponent<Trigger>().repeatable == true))
                        moveLeft = false;
                }

			}
			//Performs the movement if possible
			if (inboundsX)
			{
				if (moveLeft)
				{
					transform.position = new Vector2 (transform.position.x - 0.15f, transform.position.y);
					playerAnimator.Play(playerImages[1]);
				}
			}
		}
		//Checks for Up and Down Movement
		if (Input.GetAxisRaw ("Vertical_Solo") > 0)
		{
			direction = Face_Dir.Backward;
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y + 0.1f), 0.1f);
			foreach (Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					inboundsY = true;
				}
				if (c.gameObject.tag == "Boundary" || c.gameObject.tag == "Obstacle")
				{
					moveUp = false;
				}
                if (c.gameObject.tag == "Trigger")
                {
                    if (!c.gameObject.GetComponent<Trigger>().interacted || (c.gameObject.GetComponent<Trigger>().interacted && c.gameObject.GetComponent<Trigger>().repeatable == true))
                        moveUp = false;
                }
            }
			//Performs the movement if possible
			if (inboundsY)
			{
				if (moveUp)
				{
					transform.position = new Vector2 (transform.position.x, transform.position.y + 0.15f);
					playerAnimator.Play (playerImages [2]);
				}
			}
		}
		else
		if (Input.GetAxisRaw ("Vertical_Solo") < 0)
		{
			direction = Face_Dir.Forward;
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y - 0.1f), 0.1f);
			foreach (Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					inboundsY = true;
				}
				if (c.gameObject.tag == "Boundary" || c.gameObject.tag == "Obstacle")
				{
					moveDown = false;
				}
                if (c.gameObject.tag == "Trigger")
                {
                    if (!c.gameObject.GetComponent<Trigger>().interacted || (c.gameObject.GetComponent<Trigger>().interacted && c.gameObject.GetComponent<Trigger>().repeatable == true))
                        moveDown = false;
                }
            }
			//Performs the movement if possible
			if (inboundsY)
			{
				if (moveDown)
				{
					transform.position = new Vector2 (transform.position.x, transform.position.y - 0.15f);
					playerAnimator.Play (playerImages [3]);
				}
			}
		}
		
		if (Input.GetAxisRaw ("Vertical_Solo") == 0 && Input.GetAxisRaw ("Horizontal_Solo") == 0)
		{
			idleTimer++;
			if (idleTimer > 500)
			{
				idleTimer = 0;
				playerAnimator.Play (playerImages [4]);
			}
			else
			{
				//playerAnimator.Stop ();
			}
		}

		print (direction);
	}

	//Changes the CHaracter to the next in the available list
	public void changePlayer()
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
			switchChar = true;
	}

	//Changes the CHaracter to the previous in the available list
	public void changePlayerDown()
	{
			currentCharacter--;
			if (currentCharacter < 0)
			{
				currentCharacter = availableWeapons.Count-1;
			}
			else if (currentCharacter >= availableWeapons.Count)
			{
				currentCharacter = 0;
			}
			weapon = availableWeapons[currentCharacter];
			switchChar = true;
	}

	void interactTrigger()
	{
		//Checks whether or not you're overlapping with a trigger
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), 0.3f);
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
							if(currentTrig.repeatable == true && currentTrig.interacted == true && currentTrig.repeatScript != "")
								dialog.Load (currentTrig.repeatScript);
							else
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
		
	public void changeDeckDisplay(int menu)
	{
		currentList = menu;

		//Sets buttons on each layer so they cant be pressed or navigated to
		foreach (GameObject g in SlotsDeckA)
		{
			g.GetComponent<Button> ().interactable = false;
		}
		foreach (GameObject g in SlotsDeckB)
		{
			g.GetComponent<Button> ().interactable = false;
		}
		foreach (GameObject g in SlotsPack)
		{
			g.GetComponent<Button> ().interactable = false;
		}

		//Makes all three equal in sort order so that one can be changed and placed above the others
		DeckA.sortingOrder = 2;
		DeckB.sortingOrder = 2;
		Pack.sortingOrder = 2;

		//Baseed on the variable that was put in. Changes which list is displayed on top and which buttons are made interactable
		switch (menu)
		{
			case 0:
				DeckA.sortingOrder = 3;
				characters[weapon].activeDeck = 0;
				inPack = false;
				foreach (GameObject g in SlotsDeckA)
				{
					g.GetComponent<Button> ().interactable = true;
				}
			break;
			case 1:
				DeckB.sortingOrder = 3;
				characters[weapon].activeDeck = 1;
				inPack = false;
				foreach (GameObject g in SlotsDeckB)
				{
					g.GetComponent<Button> ().interactable = true;
				}
			break;
			case 2:
				Pack.sortingOrder = 3;
				inPack = true;
				foreach (GameObject g in SlotsPack)
				{
					g.GetComponent<Button> ().interactable = true;
				}
			break;
		}
		//refreshes the list to show from the top
		refreshList (0, menu);
	}

	//Retrieves the Active UI Elements in the scene that will be used by mathods and save rweferneces to them. - Objects must be active to be found by GameObject.Find
	void getDeckUI()
	{
		DeckA = GameObject.Find ("Deck A Canvas").GetComponent<Canvas>();
		DeckB = GameObject.Find ("Deck B Canvas").GetComponent<Canvas>();
		Pack = GameObject.Find ("Pack Canvas").GetComponent<Canvas>();

		SlotsDeckA.Add(GameObject.Find("A Slot 1"));
		SlotsDeckA.Add(GameObject.Find("A Slot 2"));
		SlotsDeckA.Add(GameObject.Find("A Slot 3"));
		SlotsDeckA.Add(GameObject.Find("A Slot 4"));
		SlotsDeckA.Add(GameObject.Find("A Slot 5"));
		SlotsDeckA.Add(GameObject.Find("A Slot 6"));
		SlotsDeckA.Add(GameObject.Find("A Slot 7"));
		SlotsDeckA.Add(GameObject.Find("A Slot 8"));
		SlotsDeckA.Add(GameObject.Find("A Slot 9"));

		SlotsDeckB.Add(GameObject.Find("B Slot 1"));
		SlotsDeckB.Add(GameObject.Find("B Slot 2"));
		SlotsDeckB.Add(GameObject.Find("B Slot 3"));
		SlotsDeckB.Add(GameObject.Find("B Slot 4"));
		SlotsDeckB.Add(GameObject.Find("B Slot 5"));
		SlotsDeckB.Add(GameObject.Find("B Slot 6"));
		SlotsDeckB.Add(GameObject.Find("B Slot 7"));
		SlotsDeckB.Add(GameObject.Find("B Slot 8"));
		SlotsDeckB.Add(GameObject.Find("B Slot 9"));

		SlotsPack.Add(GameObject.Find("Pack Slot 1"));
		SlotsPack.Add(GameObject.Find("Pack Slot 2"));
		SlotsPack.Add(GameObject.Find("Pack Slot 3"));
		SlotsPack.Add(GameObject.Find("Pack Slot 4"));
		SlotsPack.Add(GameObject.Find("Pack Slot 5"));
		SlotsPack.Add(GameObject.Find("Pack Slot 6"));
		SlotsPack.Add(GameObject.Find("Pack Slot 7"));
		SlotsPack.Add(GameObject.Find("Pack Slot 8"));
		SlotsPack.Add(GameObject.Find("Pack Slot 9"));

		ImagesDeckA.Add(GameObject.Find("A Slot Image 1").GetComponent<Image>());
		ImagesDeckA.Add(GameObject.Find("A Slot Image 2").GetComponent<Image>());
		ImagesDeckA.Add(GameObject.Find("A Slot Image 3").GetComponent<Image>());
		ImagesDeckA.Add(GameObject.Find("A Slot Image 4").GetComponent<Image>());
		ImagesDeckA.Add(GameObject.Find("A Slot Image 5").GetComponent<Image>());
		ImagesDeckA.Add(GameObject.Find("A Slot Image 6").GetComponent<Image>());
		ImagesDeckA.Add(GameObject.Find("A Slot Image 7").GetComponent<Image>());
		ImagesDeckA.Add(GameObject.Find("A Slot Image 8").GetComponent<Image>());
		ImagesDeckA.Add(GameObject.Find("A Slot Image 9").GetComponent<Image>());

		ImagesDeckB.Add(GameObject.Find("B Slot Image 1").GetComponent<Image>());
		ImagesDeckB.Add(GameObject.Find("B Slot Image 2").GetComponent<Image>());
		ImagesDeckB.Add(GameObject.Find("B Slot Image 3").GetComponent<Image>());
		ImagesDeckB.Add(GameObject.Find("B Slot Image 4").GetComponent<Image>());
		ImagesDeckB.Add(GameObject.Find("B Slot Image 5").GetComponent<Image>());
		ImagesDeckB.Add(GameObject.Find("B Slot Image 6").GetComponent<Image>());
		ImagesDeckB.Add(GameObject.Find("B Slot Image 7").GetComponent<Image>());
		ImagesDeckB.Add(GameObject.Find("B Slot Image 8").GetComponent<Image>());
		ImagesDeckB.Add(GameObject.Find("B Slot Image 9").GetComponent<Image>());

		ImagesPack.Add(GameObject.Find("Pack Slot Image 1").GetComponent<Image>());
		ImagesPack.Add(GameObject.Find("Pack Slot Image 2").GetComponent<Image>());
		ImagesPack.Add(GameObject.Find("Pack Slot Image 3").GetComponent<Image>());
		ImagesPack.Add(GameObject.Find("Pack Slot Image 4").GetComponent<Image>());
		ImagesPack.Add(GameObject.Find("Pack Slot Image 5").GetComponent<Image>());
		ImagesPack.Add(GameObject.Find("Pack Slot Image 6").GetComponent<Image>());
		ImagesPack.Add(GameObject.Find("Pack Slot Image 7").GetComponent<Image>());
		ImagesPack.Add(GameObject.Find("Pack Slot Image 8").GetComponent<Image>());
		ImagesPack.Add(GameObject.Find("Pack Slot Image 9").GetComponent<Image>());

		NamesDeckA.Add(GameObject.Find("A Slot Name 1").GetComponent<Text>());
		NamesDeckA.Add(GameObject.Find("A Slot Name 2").GetComponent<Text>());
		NamesDeckA.Add(GameObject.Find("A Slot Name 3").GetComponent<Text>());
		NamesDeckA.Add(GameObject.Find("A Slot Name 4").GetComponent<Text>());
		NamesDeckA.Add(GameObject.Find("A Slot Name 5").GetComponent<Text>());
		NamesDeckA.Add(GameObject.Find("A Slot Name 6").GetComponent<Text>());
		NamesDeckA.Add(GameObject.Find("A Slot Name 7").GetComponent<Text>());
		NamesDeckA.Add(GameObject.Find("A Slot Name 8").GetComponent<Text>());
		NamesDeckA.Add(GameObject.Find("A Slot Name 9").GetComponent<Text>());

		NamesDeckB.Add(GameObject.Find("B Slot Name 1").GetComponent<Text>());
		NamesDeckB.Add(GameObject.Find("B Slot Name 2").GetComponent<Text>());
		NamesDeckB.Add(GameObject.Find("B Slot Name 3").GetComponent<Text>());
		NamesDeckB.Add(GameObject.Find("B Slot Name 4").GetComponent<Text>());
		NamesDeckB.Add(GameObject.Find("B Slot Name 5").GetComponent<Text>());
		NamesDeckB.Add(GameObject.Find("B Slot Name 6").GetComponent<Text>());
		NamesDeckB.Add(GameObject.Find("B Slot Name 7").GetComponent<Text>());
		NamesDeckB.Add(GameObject.Find("B Slot Name 8").GetComponent<Text>());
		NamesDeckB.Add(GameObject.Find("B Slot Name 9").GetComponent<Text>());

		NamesPack.Add(GameObject.Find("Pack Slot Name 1").GetComponent<Text>());
		NamesPack.Add(GameObject.Find("Pack Slot Name 2").GetComponent<Text>());
		NamesPack.Add(GameObject.Find("Pack Slot Name 3").GetComponent<Text>());
		NamesPack.Add(GameObject.Find("Pack Slot Name 4").GetComponent<Text>());
		NamesPack.Add(GameObject.Find("Pack Slot Name 5").GetComponent<Text>());
		NamesPack.Add(GameObject.Find("Pack Slot Name 6").GetComponent<Text>());
		NamesPack.Add(GameObject.Find("Pack Slot Name 7").GetComponent<Text>());
		NamesPack.Add(GameObject.Find("Pack Slot Name 8").GetComponent<Text>());
		NamesPack.Add(GameObject.Find("Pack Slot Name 9").GetComponent<Text>());

		runeDisplay = GameObject.Find ("Rune Image");
		runeDamage = GameObject.Find ("Spell Damage").GetComponent<Text>();
		runeName = GameObject.Find ("Spell Name ").GetComponent<Text>();
		runeDesc = GameObject.Find ("Spell Decription").GetComponent<Text>();

		charImage = GameObject.Find ("Char Image").GetComponent<Image>();
		CharNAme =  GameObject.Find ("Char Name Text").GetComponent<Text>();
		WeapNAme = GameObject.Find ("Weapon Text").GetComponent<Text>();


		switchChar = true;
	}


	public void refreshList(int startIndex, int menu)
	{
		List<GameObject> tempButtons = new List<GameObject> ();
		List<Image> tempImages = new List<Image> ();
		List<Text> tempNames = new List<Text> ();
		List<Object> targetList = new List<Object> ();
		//Sets the temporary references to the correct Lists for later usage
		switch (menu)
		{
			case 0:
				tempButtons = SlotsDeckA;
				tempImages = ImagesDeckA;
				tempNames = NamesDeckA;
				targetList = properLists [0];
			break;
				
			case 1:
				tempButtons = SlotsDeckB;
				tempImages = ImagesDeckB;
				tempNames = NamesDeckB;
				targetList = properLists [1];
			break;
				
			case 2:
				tempButtons = SlotsPack;
				tempImages = ImagesPack;
				tempNames = NamesPack;
				targetList = properLists [2];
			break;
		}
		//Sets the Image and Information for each of the 9 buttons
		for (int i = 0; i < 9; i++)
		{
			int spellNum = i + startIndex;
			if(targetList != null)
			{
				//Checks to see if the current spell slot in this deck is not full
				//if it is...
				if (targetList.Count > spellNum)
				{
					GameObject curSpell = ((GameObject)Resources.Load (targetList [spellNum].name));
					curSpell.GetComponent<Spell> ().setDescription (weapon);
					RuneInfo r = tempButtons [i].GetComponent<RuneInfo> ();
					r.runeName = curSpell.GetComponent<Spell> ().name;
					r.runeImage = curSpell.GetComponent<Spell> ().runeImage;
					r.runeDesc = curSpell.GetComponent<Spell> ().description;
					r.runeDamage = curSpell.GetComponent<Spell> ().damage.ToString ();
					
					tempNames [i].text = r.runeName;
					tempImages [i].sprite = curSpell.GetComponent<Spell> ().bulletImage;
				
					Button b = tempButtons [i].gameObject.GetComponent<Button> ();
					b.onClick.RemoveAllListeners ();
					//Adds either a Method To select a Bullet for replacement  
					if (selectedSpell >= 100)
					{
						b.onClick.AddListener (delegate
						{
							selectBullet (spellNum, menu);
						});
					}
					else
					{
						List<Object> properList = new List<Object> ();
						switch (menu)
						{
							case 0:
								properList = properLists [0];
							break;
							case 1:
								properList = properLists [1];
							break;
							case 2:
								properList = properLists [2];
							break;
						}
						//Checks to see if te spot in the desired list is occupied if so it swaps the two bulltets
						if (properList.Count >= selectedSpell)
						{
							b.onClick.AddListener (delegate
							{
								replaceBullet (spellNum, menu, selectedSpell, originalMenu);
							});
						}
						//if it isnt it simply Adds the new bullet to the other menu and removes it from the original
						else
						{
							b.onClick.AddListener (delegate
							{
								addBullettoEmpty (spellNum, menu, selectedSpell, originalMenu);
							});
						}
					}
				}

				//if it isn't full, cecks to see if there is space left
				else if (spellNum < weaponMax)
				{
					//if So sets the info to reflect an Empty slot
					RuneInfo r = tempButtons [i].GetComponent<RuneInfo> ();
					r.runeName = "Empty Slot";
					r.runeImage = defaultSlot;
					r.runeDesc = "Select this slot to replace it with a spell from the pack.";
					r.runeDamage = "N/A";
				
					tempNames [i].text = r.runeName;
					tempImages [i].sprite = defaultSlot;

					//sets this slot to have a bullet added to it.
					Button b = tempButtons [i].gameObject.GetComponent<Button> ();
					b.onClick.RemoveAllListeners ();
					if (selectedSpell >= 100)
					{
						b.onClick.AddListener (delegate
						{
							addBullet (spellNum, menu);
						});
					}
					else
					{
						List<Object> properList = new List<Object> ();
						switch (menu)
						{
							case 0:
								properList = properLists [0];
							break;
							case 1:
								properList = properLists [1];
							break;
							case 2:
								properList = properLists [2];
							break;
						}
						//Adds the bullet from the new slot
						if (properList.Count >= selectedSpell)
						{
							b.onClick.AddListener (delegate
							{
								addBullet (spellNum, menu, selectedSpell, originalMenu);
							});
						}
						//Does nothing since both are empty slots
						else
						{
							b.onClick.AddListener (delegate
							{
								doubleEmpty (spellNum, menu, selectedSpell, originalMenu);
							});
						}
					}
				}
			}
		}
	}
	//Selects a bullet to be moved somewhere
	protected void selectBullet(int spellNum, int menu)
	{
		Debug.Log ("SB");
		selectedSpell = spellNum;
		originalMenu = menu;
		changeDeckDisplay (2);
		EventSystem.current.SetSelectedGameObject (GameObject.Find("Pack Button"));
	}

	//Swaps the positions of two sleected bullets
	protected void replaceBullet(int spellNum, int menu, int spelltoreplacewith, int menutomovefrom)
	{
		Debug.Log ("RB");
		List<Object> givingMenu = new List<Object> ();
		List<Object> recievingMenu = new List<Object> ();
		int properIndex = 0;
		GameObject properButton = GameObject.Find ("Pack Button");
		switch (menutomovefrom)
		{
			case 0:
				givingMenu = properLists [0] ;
				properIndex = deckAIndex;
				properButton = GameObject.Find ("Deck A Button");
			break;
			case 1:
				givingMenu = properLists [1] ;
				properIndex = deckBIndex;
				properButton = GameObject.Find ("Deck B Button");
			break;
			case 2:
				givingMenu = properLists [2] ;
				properIndex = packIndex;
				properButton = GameObject.Find ("Pack Button");
			break;
		}
		switch (menu)
		{
			case 0:
				recievingMenu = properLists [0] ;
			break;
			case 1:
				recievingMenu = properLists [1] ;
			break;
			case 2:
				recievingMenu = properLists [2] ;
			break;
		}
		Object replacedObject = recievingMenu [spellNum];
		recievingMenu.RemoveAt (spellNum);
		recievingMenu.Insert(spellNum, givingMenu[spelltoreplacewith]);
		givingMenu.RemoveAt (spelltoreplacewith);
		givingMenu.Insert (spelltoreplacewith, replacedObject);
		selectedSpell = 100;
		changeDeckDisplay (menutomovefrom);
		EventSystem.current.SetSelectedGameObject (properButton);
	}

	//Selects an empty slot for a bullet to be added to
	protected void addBullet(int spellNum, int menu)
	{
		Debug.Log ("ATB");
		selectedSpell = spellNum;
		originalMenu = menu;
		changeDeckDisplay (2);
		EventSystem.current.SetSelectedGameObject (GameObject.Find("Pack Button"));
	}

	//Adds the newly selected bullet to the previously selected empty slot
	protected void addBullet(int spellNum, int menu, int spelltoreplacewith, int menutomovefrom)
	{
		Debug.Log ("ATB2");
		List<Object> givingMenu = new List<Object> ();
		List<Object> recievingMenu = new List<Object> ();
		int properIndex = 0;
		GameObject properButton = GameObject.Find ("Pack Button");
		switch (menutomovefrom)
		{
			case 0:
				givingMenu = properLists [0] ;
				properIndex = deckAIndex;
				properButton = GameObject.Find ("Deck A Button");
			break;
			case 1:
				givingMenu = properLists [1] ;
				properIndex = deckBIndex;
				properButton = GameObject.Find ("Deck B Button");
			break;
			case 2:
				givingMenu = properLists [2] ;
				properIndex = packIndex;
				properButton = GameObject.Find ("Pack Button");
			break;
		}
		switch (menu)
		{
			case 0:
				recievingMenu = properLists [0] ;
			break;
			case 1:
				recievingMenu = properLists [1] ;
			break;
			case 2:
				recievingMenu = properLists [2] ;
			break;
		}
		recievingMenu.Add (givingMenu[spelltoreplacewith]);
		givingMenu.RemoveAt (spelltoreplacewith);
		changeDeckDisplay (menutomovefrom);
		EventSystem.current.SetSelectedGameObject (properButton);
		selectedSpell = 100;
	}

	//Just deselects both empty slots
	protected void doubleEmpty(int spellNum, int menu, int spelltoreplacewith, int menutomovefrom)
	{
		Debug.Log ("DE");
		GameObject properButton = GameObject.Find ("Pack Button");
		switch (menutomovefrom)
		{
			case 0:
				properButton = GameObject.Find ("Deck A Button");
			break;
			case 1:
				properButton = GameObject.Find ("Deck B Button");
			break;
			case 2:
				properButton = GameObject.Find ("Pack Button");
			break;
		}
		changeDeckDisplay (menutomovefrom);
		EventSystem.current.SetSelectedGameObject (properButton);
		selectedSpell = 100;
	}

	//Adds a previously sleecte dbullet to a newly selected empty slot
	protected void addBullettoEmpty(int spellNum, int menu, int spelltoreplacewith, int menutomovefrom)
	{
		Debug.Log ("ATBE");
		List<Object> givingMenu = new List<Object> ();
		List<Object> recievingMenu = new List<Object> ();
		int properIndex = 0;
		GameObject properButton = GameObject.Find ("Pack Button");
		switch (menutomovefrom)
		{
			case 0:
				givingMenu = properLists [0] ;
				properIndex = deckAIndex;
				properButton = GameObject.Find ("Deck A Button");
			break;
			case 1:
				givingMenu = properLists [1] ;
				properIndex = deckBIndex;
				properButton = GameObject.Find ("Deck B Button");
			break;
			case 2:
				givingMenu = properLists [2] ;
				properIndex = packIndex;
				properButton = GameObject.Find ("Pack Button");
			break;
		}
		switch (menu)
		{
			case 0:
				recievingMenu = properLists [0] ;
			break;
			case 1:
				recievingMenu = properLists [1] ;
			break;
			case 2:
				recievingMenu = properLists [2] ;
			break;
		}
		givingMenu.Add (recievingMenu[spellNum]);
		recievingMenu.RemoveAt (spellNum);
		selectedSpell = 100;
		changeDeckDisplay (menutomovefrom);
		EventSystem.current.SetSelectedGameObject (properButton);
	}

	//Scrolls the DeckMenu up or down using the arrow keys
	void scrollDeck()
	{
		bool scollinuse = false;
		if (Input.GetAxisRaw ("Vertical_P1") > 0)
		{
			if (!scollinuse)
			{
				scollinuse = true;
				if (!inPack)
				{
					switch (characters[weapon].activeDeck)
					{
						case 0:
							if (deckAIndex > 0)
							{
								deckAIndex--;
								refreshList (deckAIndex, 0);
							}
						break;
						case 1:
							if (deckBIndex > 0)
							{
								deckBIndex--;
								refreshList (deckBIndex, 1);
							}
						break;
					}
				}
				else
				{
					if (packIndex > 0)
					{
						packIndex--;
						refreshList (packIndex, 2);
					}
				}
			}
		}
		else if (Input.GetAxisRaw ("Vertical_P1") < 0)
		{
			if (!scollinuse)
			{
				scollinuse = true;
				if (!inPack)
				{
					switch (characters[weapon].activeDeck)
					{
						case 0:
							if (deckAIndex < weaponMax - 9)
							{
								deckAIndex++;
								refreshList (deckAIndex, 0);
							}
						break;
						case 1:
							if (deckBIndex < weaponMax - 9)
							{
								deckBIndex++;
								refreshList (deckBIndex, 1);
							}
						break;
					}
				}
				else
				{
					if (packIndex < weaponMax - 9)
					{
						packIndex++;
						refreshList (packIndex, 2);
					}
				}
			}
		}
		if (Input.GetAxisRaw ("Vertical_P1") == 0)
		{
			scollinuse = false;
		}
	}

	//Scrolls the deck menu for use with UI Buttons
	public void menuScrollUp()
	{
		if (!inPack)
		{
			switch (characters[weapon].activeDeck)
			{
				case 0:
					if (deckAIndex > 0)
					{
						deckAIndex--;
						refreshList (deckAIndex, 0);
					}
				break;
				case 1:
					if (deckBIndex > 0)
					{
						deckBIndex--;
						refreshList (deckBIndex, 1);
					}
				break;
			}
		}
		else
		{
			if (packIndex > 0)
			{
				packIndex--;
				refreshList (packIndex, 2);
			}
		}
	}

	//Scrolls the deck menu for use with UI Buttons
	public void menuScrollDown()
	{
		if (!inPack)
		{
			switch (characters[weapon].activeDeck)
			{
				case 0:
					if (deckAIndex < weaponMax - 9)
					{
						deckAIndex++;
						refreshList (deckAIndex, 0);
					}
				break;
				case 1:
					if (deckBIndex < weaponMax - 9)
					{
						deckBIndex++;
						refreshList (deckBIndex, 1);
					}
				break;
			}
		}
		else
		{
			if (packIndex < weaponMax - 9)
			{
				packIndex++;
				refreshList (packIndex, 2);
			}
		}
	}

	//Creates character classes for later use
	void createCharacters()
	{
		Character c = new Character();
		Character c2 = new Character();

		c.name = "Matt";
		c.health = 100;
		c.weapon = Weapon_Types.Revolver;
		characters.Add (c.weapon,c);

		c2.name = "John";
		c2.health = 100;
		c2.weapon = Weapon_Types.Shotgun;
		characters.Add (c2.weapon,c2);
	}



	//Parameters: None
	//Purpose: Takes the player to the Deck Building Section of the Menu
	//Known Errors: None
	public void ToDeck()
	{
		//In Theory only need to disable the main pause menu and active the deck Menu
		//turns off title canvas and turns on level select canvas
		pause.SetActive(false);
		quest.SetActive(false);
		deck.SetActive(true);
		EventSystem.current.SetSelectedGameObject (GameObject.Find("Right Image Button"));
		getDeckUI ();
		changeDeckDisplay (characters[weapon].activeDeck);
	}


	/*
    * Purpose of this section of code is for the quest log.
    */

	//Paramters: None
	//Purpose: Takes the player to the list of Quest Section of the Menu
	//Known Errors: None
	public void ToQuests()
	{
		//In Theory only need to disable the main pause menu and active the deck Menu
		//turns off title canvas and turns on level select canvas
		pause.SetActive(false);
		deck.SetActive(false);
		quest.SetActive(true);

	}

	//Parameters: None
	//Purpose: Generic Return function to main pause menu no matter where in the journal you are
	//Known Errors: None
	public void ToPause()
	{
		deck.SetActive(false);
		quest.SetActive(false);
		pause.SetActive(true);
	}

	//Parameters: None
	//Purpose: Shoot an Earth Bullet Obstacle 
	//Known Errors: 
	public void overAbility()
	{
		//might be just an overworld ability function in general instead of earth. and check which character is being played

		//the button is pressed to use ability
		if(Input.GetKeyDown(KeyCode.Space))
		{
            //depending on the direction shoot a bullet 3 spaces in front
            switch (direction)
            {
                case Face_Dir.Forward:
                    print("shoot forward");
                    playerAnimator.Play(playerImages[3]);
                    switch (weapon)
                    {
                        case Weapon_Types.Revolver:
                            if (boulder != null)
                            {
                                Destroy(boulder);
                                
                            }
                            boulder = (GameObject)Instantiate(Resources.Load("overEarth"), new Vector2(transform.position.x, transform.position.y - 1.30f), Quaternion.identity);
                            
                            break;
                        case Weapon_Types.Shotgun:
							int mapMask = ~(1 << LayerMask.NameToLayer("Map"));
							createFire(new Vector3 (transform.position.x, transform.position.y - 0.5f, transform.position.z));
							
                            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(0, -1), 4.0f, mapMask);
                            if (hit.collider != null)
                            {
                                if (hit.collider.gameObject.tag == "Obstacle")
                                {
                                    hit.collider.gameObject.GetComponent<Obstacle>().takeDamage(5, blastAttributes);
                                    if (hit.collider.gameObject.GetComponent<Obstacle>().MarkedforDeletion)
                                        Destroy(hit.collider.gameObject);

                                    hit.collider.gameObject.GetComponent<Obstacle>().direction = new Vector2(0, -1.5f);
                                    
                                }
                            }
                            break;
                    }
                    break;
				case Face_Dir.Backward:
					print ("shoot back");
                    playerAnimator.Play(playerImages[2]);
                    switch (weapon)
                    {
                        case Weapon_Types.Revolver:
                            if (boulder != null)
                            {
                                Destroy(boulder);
                            }
                            boulder = (GameObject)Instantiate(Resources.Load("overEarth"), new Vector2(transform.position.x, transform.position.y + 1.30f), Quaternion.identity);
                            
                            break;
                        case Weapon_Types.Shotgun:
							int mapMask = ~(1 << LayerMask.NameToLayer("Map"));
							createFire(new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z));
							
                            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(0, +1), 4.0f, mapMask);
                            if (hit.collider != null)
                            {
                                if (hit.collider.gameObject.tag == "Obstacle")
                                {
                                    hit.collider.gameObject.GetComponent<Obstacle>().takeDamage(5, blastAttributes);
                                    if (hit.collider.gameObject.GetComponent<Obstacle>().MarkedforDeletion)
                                        Destroy(hit.collider.gameObject);

                                    hit.collider.gameObject.GetComponent<Obstacle>().direction = new Vector2(0, +1.5f);

                                }
                            }
                            break;
                    }
                    break;
				case Face_Dir.Left:
					print ("shoot left");
                    playerAnimator.Play(playerImages[1]);
                    switch (weapon)
                    {
                        case Weapon_Types.Revolver:
                            if (boulder != null)
                            {
                                Destroy(boulder);
                            }
                            boulder = (GameObject)Instantiate(Resources.Load("overEarth"), new Vector2(transform.position.x - 1.30f, transform.position.y), Quaternion.identity);
                            
                            break;
                        case Weapon_Types.Shotgun:
							int mapMask = ~(1 << LayerMask.NameToLayer("Map"));
							createFire(new Vector3 (transform.position.x-0.5f, transform.position.y, transform.position.z));
							
                            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(-1, 0), 4.0f, mapMask);
                            if (hit.collider != null)
                            {
                                if (hit.collider.gameObject.tag == "Obstacle")
                                {
                                    hit.collider.gameObject.GetComponent<Obstacle>().takeDamage(5, blastAttributes);
                                    if (hit.collider.gameObject.GetComponent<Obstacle>().MarkedforDeletion)
                                        Destroy(hit.collider.gameObject);

                                    hit.collider.gameObject.GetComponent<Obstacle>().direction = new Vector2(-1.5f, 0);

                                }
                            }
                            break;
                    }
                    break;
				case Face_Dir.Right:
					print ("shoot right");
                    playerAnimator.Play(playerImages[0]);
                    switch (weapon)
                    {
                        case Weapon_Types.Revolver:
                            if (boulder != null)
                            {
                                Destroy(boulder);
                            }
                            boulder = (GameObject)Instantiate(Resources.Load("overEarth"), new Vector2(transform.position.x + 1.30f, transform.position.y), Quaternion.identity);
                            
                            break;
						case Weapon_Types.Shotgun:
							createFire(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z));
							
                            int mapMask = ~(1 << LayerMask.NameToLayer("Map"));
                            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(+1, 0), 4.0f, mapMask);
                            if (hit.collider != null)
                            {
                                if (hit.collider.gameObject.tag == "Obstacle")
                                {
                                    hit.collider.gameObject.GetComponent<Obstacle>().takeDamage(5, blastAttributes);
                                    if (hit.collider.gameObject.GetComponent<Obstacle>().MarkedforDeletion)
                                        Destroy(hit.collider.gameObject);

                                    hit.collider.gameObject.GetComponent<Obstacle>().direction = new Vector2(+1.5f, 0);
                                }
                            }
                            break;
                    }
                    break;
			}
			
		}
	}
		

	public void interactButton()
	{
		if (Input.GetButtonDown ("Submit_Solo"))
		{
			Collider2D[] hitColliders;
			switch (direction)
			{
				case Face_Dir.Forward:
					hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y + 0.1f), 0.3f);
				break;
				case Face_Dir.Backward:
					hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y - 0.1f), 0.3f);
				break;
				case Face_Dir.Left:
					hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x + 0.1f, transform.position.y), 0.3f);
				break;
				case Face_Dir.Right:
					hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x - 0.1f, transform.position.y), 0.3f);
				break;
				default:
					hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x - 0.1f, transform.position.y), 0.3f);
				break;
			}

			if (hitColliders.Length != 0)
			{
				foreach (Collider2D hitCollider in hitColliders)
				{
					if (hitCollider.gameObject.tag == "Trigger")
					{
						//If the trigger you're overlapping with is touch activated
						currentTrig = hitCollider.GetComponent<Trigger> ();
						//Checks to see if the trigger is active and ready to be interacted with
						if (currentTrig.active)
						{
							//Checks to see if it's already been interacted with or if it has, but is repeatable
							if (currentTrig.interacted == false || (currentTrig.repeatable == true && currentTrig.interacted == true))
							{
								//Loads dialouge and sets it to interacted if it doesnt involve a battle.
								if (currentTrig.repeatable == true && currentTrig.interacted == true && currentTrig.repeatScript != "")
									dialog.Load (currentTrig.repeatScript);
								else
									dialog.Load (currentTrig.script);

								if (!currentTrig.battle)
								{
									currentTrig.interacted = true;
									activatedTriggers.Add (currentTrig.name);
								}
								//cutscene = true;
							}
						}
					}
				}
			}
		}
	}
	void createFire(Vector3 pos)
	{
		GameObject oldFIre = GameObject.FindWithTag ("FireEffect");
		if (oldFIre != null)
		{
			GameObject.Destroy (oldFIre);
		}
		Instantiate (Resources.Load ("Fireball2"), pos, Quaternion.identity);
	}
}

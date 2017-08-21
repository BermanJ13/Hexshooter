using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum Weapon_Types { Revolver, Rifle, Gatling, Shotgun, Bow, Canegun};
public class FieldManager : MonoBehaviour
{
    public string mapFile;
    [SerializeField]public Transform[] gamePieces;
    public Dictionary<string, Transform> things = new Dictionary<string, Transform>();
	protected TextAsset reader;
    public List<string> rows = new List<string>();

	protected int weaponMax;
	public bool firstPause;
	protected bool updateStopper;
	protected Transform playerPanel;
	protected Transform enemyPanel;
	protected Transform Testdummy;
	protected Spell[] spells;
	protected Enemy[] enemies;
	protected Obstacle[] obstacles;
	protected Player player;
	public bool pause = false;
	public List<Object> Handful = new List<Object>();
	protected List<Object> Temp = new List<Object>();
	public List<int> TempNum = new List<int>();
	protected  static System.Random rand = new System.Random();  
	protected static GameObject[] pauseObjects;
	protected static GameObject[] pauseUI;
	protected SpellHolder spellHold;
	protected GameObject[] spellSlots = new GameObject[8];
	protected List<GameObject> displaySlots = new List<GameObject>();
	public Sprite defaultSlot;
	protected GameObject runeDisplay;
	protected GameObject selector;
	protected Text runeDamage;
	protected Text runeName;
	protected Text runeDesc;
	protected GameObject can;
	protected GameObject p1Gun;
	public EventSystem ES_P1;
	protected GameObject[] battleObjects;
	public GameObject[] weapons;
	public bool once;
	protected UniversalSettings us;
	public int style = 3;

	// Use this for initialization
	public void Start () 
	{
		us = GameObject.Find("__app").GetComponent<UniversalSettings> ();
		mapFile = us.mapfile;
		style = us.style;
		once =true;
		firstPause = true;
		if (style == 1)
			pause = true;
		else
			pause = false;
		weapons = new GameObject[5];
		ES_P1 = EventSystem.current;

		//Hnadful= Deck
		//Pass Deck In from Overworld Scene
		//Placeholder Fils Deck with Lighnin and Eart Spells
		buildDeck();

        //creates the map
        instantiateMap();


        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

		// Retrieves the references to the alkl ofthe UI elements.
		getUI ();

		//Updates the lists of enemies, spells, and obstacles to be used in the battle. 
		updateEnemyList ();
		updateSpellList ();
		updateObstacleList ();

		if (GameObject.Find ("OverPlayer") != null)
			player.weapon = GameObject.Find ("OverPlayer").GetComponent<OverPlayer>().weapon;
		
		if(player.weapon == Weapon_Types.Bow)
			player.Chamber = Handful;
		else
			Shuffle(Handful);

		//Selects and enables the cooresponding UI based on the character
		chooseGun (player.weapon, false);
	}

	void Update()
	{
		switch(style)
		{
			case 0:
				stationaryUpdate ();
			break;
				
			case 1:
				activeUpdate ();
			break;
		}
	}

	// Use this for initialization
	void activeUpdate () 
	{
		if (once)
		{
			chooseGun (player.weapon, false);
			showReloadScreen ();
			once = false;
		}
		if (player.health <= 0 )
		{
			SceneManager.LoadScene ("Game Over");
		}
		updateEnemyList ();
		if(enemies.Length == 0)
		{
			GameObject.Find ("OverPlayer").GetComponent<OverPlayer> ().returnFromBattle = true;
			SceneManager.LoadScene ("Overworld");
		}
		//updateHealth ();
		if (ES_P1.currentSelectedGameObject != null)
		{
			if (ES_P1.currentSelectedGameObject.tag == "SpellHolder")
			{
				runeName.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
				runeDamage.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
				//runeDesc.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
				runeDisplay.GetComponent<Image> ().sprite = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
				runeDisplay.GetComponent<Image> ().color = new Color (0, 0, 0, 255);
				selector.transform.position = ES_P1.currentSelectedGameObject.transform.position;
			}
		}

		if (pause)
		{
			if (player.weapon != Weapon_Types.Bow)
			{
				if (Input.GetButtonDown ("Cancel_Solo"))
				{
					if (Temp.Count > 0)
					{
						removeBullet ();
					}
				}
			}

			//Pauses both sides until ready for the first pus eof the game
			if (firstPause)
			{

			} 
			//COntinues the battle during all subsequent reloads
			else
			{
				player.activeUpdate ();
				bool enemyReload = true;
				foreach (Spell spell in spells)
				{
					if (spell != null)
						spell.spellUpdate ();
				}
				foreach (Enemy enemy in enemies)
				{
					if (enemy != null)
					{
						enemy.enemyUpdate ();
					}
				}
				foreach (Obstacle ob in obstacles)
				{
					if (ob != null)
						ob.obstacleUpdate ();
				}
				updateSpellList ();
				deleteSpells ();
				updateObstacleList ();
				deleteObstacles ();
				updateEnemyList ();
				deleteEnemies ();
			}
		} 
		else
		{
			player.playerUpdate ();
			foreach (Spell spell in spells)
			{
				if (spell != null)
					spell.spellUpdate ();
			}
			foreach (Enemy enemy in enemies)
			{
				if (enemy != null)
				{
					enemy.enemyUpdate ();
				}
			}
			foreach (Obstacle ob in obstacles)
			{
				if (ob != null)
					ob.obstacleUpdate ();
			}
			updateSpellList ();
			deleteSpells ();
			updateObstacleList ();
			deleteObstacles ();
			updateEnemyList ();
			deleteEnemies ();

			//Pulls up the reload screen on a button press
			if ( player.reload && (Input.GetButtonDown("Submit_Solo")||Input.GetButtonDown("Start_Solo")))
			{
				showReloadScreen ();
			}
		}
	}

	// Update is called once per frame
	void stationaryUpdate () 
	{
		//Checks to see if tis battle originate in story mode, and if so it sets the playe rin battle to match the charcter being used in te overworld.
		if (GameObject.Find ("OverPlayer") != null)
			player.weapon = GameObject.Find ("OverPlayer").GetComponent<OverPlayer>().weapon;

		//Ensures the correct gun UI is bveing used in the battle
		if (once)
		{
			chooseGun (player.weapon, false);
			once = false;
		}

		//Checks to see if the player is dead and if so shows the Game Over Screen
		if (player.health <= 0 )
		{
			SceneManager.LoadScene ("Game Over");
		}

		//Checks to see if there are any living enemies and if there arent ends the battle
		updateEnemyList ();
		if(enemies.Length == 0)
		{
			GameObject.Find ("OverPlayer").GetComponent<OverPlayer> ().returnFromBattle = true;
			SceneManager.LoadScene ("Overworld");
		}

		//ALters the display elements to match the rune that is currently being hovered over.
		if (ES_P1.currentSelectedGameObject != null)
		{
			if (ES_P1.currentSelectedGameObject.tag == "SpellHolder")
			{
				runeName.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
				runeDamage.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
				//runeDesc.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
				runeDisplay.GetComponent<Image> ().sprite = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
				runeDisplay.GetComponent<Image> ().color = new Color (0, 0, 0, 255);
				selector.transform.position = ES_P1.currentSelectedGameObject.transform.position;
			}
		}

		if (player.weapon != Weapon_Types.Bow)
		{
			//Removes the last bullet from the camber and places it back in he selection screen.
			if (pause && Input.GetButtonDown ("Cancel_Solo"))
			{
				if (Temp.Count > 0)
				{
					removeBullet ();
				}
			}
		}

		//When not in reload mode calls the update functions of all the elements on the board. 
		if (!pause)
		{
			player.playerUpdate ();
			bool enemyReload = true;
			foreach(Spell spell in spells)
			{
				if(spell != null)
					spell.spellUpdate ();
			}
			foreach(Enemy enemy in enemies)
			{
				if (enemy != null)
				{
					enemy.enemyUpdate ();
					if(enemy.reload == false)
					{
						enemyReload = false;
					}
				}
			}
			foreach(Obstacle ob in obstacles)
			{
				if(ob != null)
					ob.obstacleUpdate ();
			}

			//Updates the lists of elemnts on the board and deltes the ones marked for deletion
			updateSpellList ();
			deleteSpells ();
			updateEnemyList ();
			deleteEnemies ();
			updateObstacleList ();
			deleteObstacles ();

			//Checks whether the player is ready for reload, the enemy is ready for reload, and whether the spells on the board have resolved.
			if (player.reload && enemyReload && spells.Length == 0)
			{
				showReloadScreen ();
			}
		}
	}

	//Creates the map by loading in the text file
    public void instantiateMap()
    {
		
        foreach (Transform trns in gamePieces)
        {
            if(trns.name != "Enemy_Panel" && trns.name != "Player_Panel")
            {
                things.Add(trns.name, trns);
            }
            else if(trns.name == "Enemy_Panel")
            {
                things.Add("e", trns);
            }
            else if (trns.name == "Player_Panel")
            {
                things.Add("p", trns);
            }
        }

        //open ups the streamreader then reads every line and adds it to the rows list
		reader = Resources.Load<TextAsset>(mapFile);
		string[] lines = reader.text.Split ('\n');
		foreach (string s in lines)
		{
			string temp;
			string replaceWith = "";
			temp = s.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
			rows.Add (temp);
		}

        //use this in the foreach loop to hold the first two values
        //which is the position of the objects
        Vector2 place;
        //for each string in rows split the string into tiles
        foreach (string a in rows)
        {
            string[] tiles = a.Split(' ');
            //foreach string in tiles split it into entries
            foreach (string b in tiles)
            {
                string[] entry = b.Split(',');
                //set the place vector from before from the first two enties in the tile
                place = new Vector2(float.Parse(entry[0]), float.Parse(entry[1]));

                //put anything else on the tile that belongs there
                for (int i = 0; i < entry.Length - 2; i++)
                {
                    Instantiate(things[entry[i + 2]], new Vector3(place.x, place.y, 0), Quaternion.identity);
                }
            }
        }
    }
	//DEletes spells that have been marked for deletion
	public void deleteSpells()
	{
		if (spells != null) 
		{
			foreach (Spell spell in spells) 
			{
				////Debug.Log (spell.GetComponent<Spell> ().MarkedForDeletion);
				if (spell != null)
				{
					if (spell.MarkedForDeletion)
					{
						Destroy (spell.gameObject);
					}
				}
			}
		}
		updateSpellList ();
	}
	//DEletes enemies that have been marked for deletion
	public void deleteEnemies()
	{
		if (enemies != null) 
		{
			foreach (Enemy e in enemies) 
			{
				////Debug.Log (spell.GetComponent<Spell> ().MarkedForDeletion);
				if (e != null)
				{
					if (e.MarkedForDeletion)
					{
						Destroy (e.gameObject);
					}
				}
			}
		}
		updateEnemyList ();
	}
	//DEletes obstacles that have been marked for deletion
	public void deleteObstacles()
	{
		if (obstacles != null) 
		{
			foreach (Obstacle obs in obstacles) 
			{
				////Debug.Log (spell.GetComponent<Spell> ().MarkedForDeletion);
				if (obs != null)
				{
					if (obs.MarkedforDeletion)
					{
						Destroy (obs.gameObject);
					}
				}
			}
		}
		updateObstacleList ();
	}
	//updates the list of enemies by finding out whta is currently on the board
	public void updateEnemyList()
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag ("Enemy");
		enemies = new Enemy[temp.Length];
		int count = 0;
		foreach(GameObject t in temp)
		{
			enemies [count] = t.GetComponent<Enemy> ();
			count++;
		}
	}
	//updates the list of spells by finding out whta is currently on the board
	public void updateSpellList()
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag ("Spell");
		spells = new Spell[temp.Length];
		int count = 0;
		foreach(GameObject t in temp)
		{
			spells [count] = t.GetComponent<Spell> ();
			count++;
		}
	}
	//updates the list of obstacles by finding out whta is currently on the board
	public new void  updateObstacleList()
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag ("Obstacle");
		obstacles = new Obstacle[temp.Length];
		int count = 0;
		foreach(GameObject t in temp)
		{
			obstacles [count] = t.GetComponent<Obstacle> ();
			count++;
		}
	}
	//REnables the UI for the reload menu
	public void showReloadScreen()
	{
		//resets the default staus to the rune holding ui
		for (int i = 0; i < weaponMax; i++)
		{
			displaySlots[i].GetComponent<Image>().sprite = defaultSlot;
			displaySlots[i].GetComponent<Image>().color = Color.white;

			spellSlots[i].GetComponent<Image>().sprite = defaultSlot;
			spellSlots[i].GetComponent<Image>().color = Color.white;
		}
		if (player.weapon != Weapon_Types.Bow)
		{
			//removes the bullets used in the last round from the deck
			for (int i = Temp.Count - 1; i > -1; i--)
			{
				if (Temp [i] != null)
				{
					Temp.RemoveAt (i);
				}
				if (TempNum [i] != null)
				{
					Handful.RemoveAt (TempNum [i]);
					TempNum.RemoveAt (i);
				}
			}
		}
		else
		{
			Handful = player.Chamber;
		}
		//reenables the UI for the reload screen
		for (int i = 0; i< pauseObjects.Length;i++)
		{
			if (i < Handful.Count)
			{
				pauseObjects [i].SetActive (true);
			} 
			else
			{
				pauseObjects [pauseObjects.Length-1].SetActive (true);
			}
		}

		//DEactivates the UI for the battle screen
		for (int i = 0; i< battleObjects.Length;i++)
		{
			if(battleObjects[i] != null)
				battleObjects [i].SetActive (false);
		}

		//reenables the UI for the reload screen
		for (int i = 0; i< pauseUI.Length;i++)
		{
				pauseUI [i].SetActive (true);
		}
		foreach (GameObject d in displaySlots)
		{
			d.SetActive (false);
		}
		for (int i = 0; i < weaponMax; i++)
		{
			displaySlots [i].SetActive (true);
		}

		//Selects the proper button
		selectButton ();
		pause = true;

		//Sets each spell holder to a match the spell from the deck.
		for (int i = 0; i < spellHold.children.Count; i++)
		{
			Button b = spellHold.children [i].gameObject.GetComponent<Button> ();
			int currentHolder = i;
			b.onClick.RemoveAllListeners ();
			b.onClick.AddListener (delegate{addBullet(currentHolder);});
			if (Handful.Count > i)
			{
				GameObject curSpell = ((GameObject)Resources.Load (Handful [i].name));
				curSpell.GetComponent<Spell> ().setDescription (player.weapon);
				b.GetComponent<Image> ().sprite = curSpell.GetComponent<Spell> ().bulletImage;
				if (b.GetComponent<Image> ().sprite.name == "Knob")
					b.GetComponent<Image> ().color = curSpell.GetComponent<SpriteRenderer> ().color;
				else
					b.GetComponent<Image> ().color = Color.white;
				RuneInfo r = spellHold.children [i].gameObject.GetComponent<RuneInfo> ();
				r.runeName = curSpell.GetComponent<Spell>().name;
				r.runeImage = curSpell.GetComponent<Spell> ().runeImage;
				//r.runeDesc = curSpell.GetComponent<Spell> ().description;
				r.runeDamage = curSpell.GetComponent<Spell>().damage.ToString();
			}
			if (player.weapon == Weapon_Types.Bow)
			{
				b.interactable = false;
				EventSystem.current.SetSelectedGameObject (GameObject.Find("BattleButton"));
			}
		}

		//Sets the player's reload value to false preventing it from reactivating the reload screen immediately
		player.reload = false;
	}
	public void showBattleScreen()
	{
		//Adds some shot lag to prevent an immediate firing after selection of bullets
		StatusEffect shotLag = new StatusEffect (0.5f);
		shotLag.m_type = StatusType.ShotLag;
		player.myStatus.AddEffect (shotLag);
		firstPause = false;

		if (player.weapon != Weapon_Types.Bow)
		{
			//Adds the selected bullets to the chamber
			for (int i = 0; i < Temp.Count; i++)
			{
				player.Chamber.Add (Temp [i]);
			}
		}

		//DIsbales the pause UI
		foreach (GameObject g in pauseObjects)
		{
			g.SetActive (false);
		}

		//Enables the battle UI
		for (int i = 0; i< battleObjects.Length;i++)
		{
			if(battleObjects[i] != null)
				battleObjects [i].SetActive (true);
		}

		//DIsbales the pause UI
		for (int i = 0; i< pauseUI.Length;i++)
		{
			pauseUI [i].SetActive (false);
		}

		//Pevents the reload creen from reactivating until the poper time
		pause = false;
		player.reload = false;
	}

	//Shuffle sthe order of a List- Used for Deck sHuffling
	public void Shuffle(List<Object> list) 
	{
		for(int i = list.Count -1; i > 1; i--)
		{
			int k = (rand.Next(0, i));
			Object value = list[k];
			list[k] = list[i];
			list[i] = value;
		}
	}

	//Adds a bullet ot te selected list and prevents it from being s;lected again
	protected void addBullet(int num)
	{
		Debug.Log (p1Gun);
		if (Temp.Count < weaponMax)
		{
			Temp.Add (Handful [num]);
			Image slot = spellSlots [Temp.Count - 1].GetComponent<Image> ();
			Image rune = spellHold.children [num].gameObject.GetComponent<Image> ();
			Image slot2 = displaySlots [Temp.Count - 1].GetComponent<Image> ();
			slot.sprite = rune.sprite;
			slot.color = rune.color;
			slot2.sprite = rune.sprite;
			slot2.color = rune.color;
			spellHold.deactivateSpell ("Spell " + num + "");
			TempNum.Add (num);

			//SELects the next possible Button And turns adjusts the UI accordingly
			selectButton ();
			//if(player.weapon == 1)
			//	p1Gun.transform.Rotate (new Vector3 (0.0f,0.0f,60.0f));
			//else if (player.weapon == 2 || player.weapon == 4)
			//	p1Gun.transform.Rotate (new Vector3 (0.0f,0.0f,45.0f));
				

			if(Temp.Count == weaponMax)
				ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));
			//Debug.Log (num);
		} 
		else
		{
			ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));
		}
	}

	//removes te bullet from the prospective selection
	protected void removeBullet()
	{
		if (TempNum [TempNum.Count - 1] != null || TempNum [TempNum.Count - 1] != 100)
		{
			spellHold.activateSpell ("Spell " + TempNum [TempNum.Count - 1] + "");
			spellSlots [Temp.Count - 1].GetComponent<Image> ().sprite = defaultSlot;
			spellSlots [Temp.Count - 1].GetComponent<Image> ().color = Color.white;
			Temp.RemoveAt (Temp.Count - 1);
			TempNum.RemoveAt (TempNum.Count - 1);
			//if(player.weapon == 1)
			//	p1Gun.transform.Rotate (new Vector3 (0.0f, 0.0f, -60.0f));
			//else if (player.weapon == 2 || player.weapon == 4)
			//	p1Gun.transform.Rotate (new Vector3 (0.0f,0.0f,-45.0f));
		}
	}

	//Selects the first button that has not been chosen when the count is under the max. 
	protected void selectButton ()
	{
		bool found = false;
		for (int i = 0; i < 9; i++)
		{
			bool used = false;
			foreach(int j in TempNum)
			{
				if (j == i)
					used = true;
			}
			if(GameObject.Find("Spell " +i+ "") != null && !found && !used)
			{
				ES_P1.SetSelectedGameObject(GameObject.Find("Spell " +i+ ""));
				found = true;
			}
			else if(!found)
				ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));
		}
	}

	//Gets the references to the all of the UI
	public void getUI()
	{
		can = GameObject.Find ("Canvas");
		chooseGun (player.weapon, true);
		spellHold = GameObject.Find ("SpellHolder").GetComponent<SpellHolder>();
		pauseObjects = new GameObject[11];
		pauseObjects[0] = GameObject.Find("Spell 0");
		pauseObjects[1] = GameObject.Find("Spell 1");
		pauseObjects[2] = GameObject.Find("Spell 2");
		pauseObjects[3] = GameObject.Find("Spell 3");
		pauseObjects[4] = GameObject.Find("Spell 4");
		pauseObjects[5] = GameObject.Find("Spell 5");
		pauseObjects[6] = GameObject.Find("Spell 6");
		pauseObjects[7] = GameObject.Find("Spell 7");
		pauseObjects[8] = GameObject.Find("Spell 8");
		pauseObjects[9] = GameObject.Find("Spell 9");
		pauseObjects[10] = GameObject.Find("BattleButton");
		pauseUI = GameObject.FindGameObjectsWithTag ("PauseUI");



		runeDisplay = GameObject.Find ("RuneHolder");
		runeDamage = GameObject.Find ("RuneDamage").GetComponent<Text>();
		runeName = GameObject.Find ("Rune Name").GetComponent<Text>();
		selector = GameObject.Find ("Selected Spell");
		//runeDesc = GameObject.Find ("Rune Description").GetComponent<Text>();
		battleObjects = new GameObject[1];

		displaySlots.Add (GameObject.Find ("SpellSlot1"));
		displaySlots.Add (GameObject.Find ("SpellSlot2"));
		displaySlots.Add (GameObject.Find ("SpellSlot3"));
		displaySlots.Add (GameObject.Find ("SpellSlot4"));
		displaySlots.Add (GameObject.Find ("SpellSlot5"));
		displaySlots.Add (GameObject.Find ("SpellSlot6"));
		displaySlots.Add (GameObject.Find ("SpellSlot7"));
		displaySlots.Add (GameObject.Find ("SpellSlot8"));
	}

	//Builds the deck
	public void buildDeck()
	{
		for (int i = 0; i < 10; i++)
		{
			Handful.Add(Resources.Load ("Fire"));
			Handful.Add(Resources.Load ("Earth"));
			Handful.Add(Resources.Load ("Water"));
			Handful.Add(Resources.Load ("Wind"));
		}
	}

	//Update the spell images
	public void updateChamberImages(int num)
	{
		for (int i = 0; i < weaponMax; i++)
		{
			if (player.Chamber.Count > i)
			{
				Image slot = spellSlots [i].GetComponent<Image> ();
				Sprite rune = ((GameObject)Resources.Load (player.Chamber [i].name)).GetComponent<Spell> ().bulletImage;
				if (rune != null)
				{
					slot.sprite = rune;
				}
			}
			else
			{
				Image slot = spellSlots [i].GetComponent<Image> ();
				slot.sprite = defaultSlot;
			}
		}
	}

	//Chooses the Proper gun for the character and activates the cooresponding UI
	public void chooseGun(Weapon_Types weapon, bool first)
	{
		if (first)
		{
			weapons [0] = GameObject.Find ("UI_GunCylinder");
			weapons [1] = GameObject.Find ("8 Rifle");
			weapons [2] = GameObject.Find ("4 Shot Gun");
			weapons [3] = GameObject.Find ("2 Shot Gun");
			weapons [4] = GameObject.Find ("Bow");
		}
		else
		{
			pauseUI = GameObject.FindGameObjectsWithTag ("PauseUI");
		}

		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons [i] != null)
			{
				weapons [i].SetActive (false);
			}
		}
		switch (weapon)
		{
			case Weapon_Types.Revolver:
				weapons [0].SetActive (true);
				spellSlots[0] = GameObject.Find ("ChamberSlot1");
				spellSlots[1] = GameObject.Find ("ChamberSlot2");
				spellSlots[2] = GameObject.Find ("ChamberSlot3");
				spellSlots[3] = GameObject.Find ("ChamberSlot4");
				spellSlots[4] = GameObject.Find ("ChamberSlot5");
				spellSlots[5] = GameObject.Find ("ChamberSlot6");

				p1Gun = weapons [0];
				weaponMax = 6;
			break;
			case Weapon_Types.Rifle:
				weapons[1].SetActive (true);
				spellSlots[0] = GameObject.Find ("ChamberSlot1");
				spellSlots[1] = GameObject.Find ("ChamberSlot2");
				spellSlots[2] = GameObject.Find ("ChamberSlot3");
				spellSlots[3] = GameObject.Find ("ChamberSlot4");
				spellSlots[4] = GameObject.Find ("ChamberSlot5");
				spellSlots[5] = GameObject.Find ("ChamberSlot6");
				spellSlots[6] = GameObject.Find ("ChamberSlot7");
				spellSlots[7] = GameObject.Find ("ChamberSlot8");
				p1Gun = weapons[1];
				weaponMax = 8;
			break;
			case Weapon_Types.Shotgun:
				weapons [2].SetActive (true);
				spellSlots [0] = GameObject.Find ("ChamberSlot1");
				spellSlots [1] = GameObject.Find ("ChamberSlot2");
				spellSlots [2] = GameObject.Find ("ChamberSlot3");
				spellSlots [3] = GameObject.Find ("ChamberSlot4");
				p1Gun = weapons[2];
				weaponMax = 4;
			break;
			case Weapon_Types.Gatling:
				weapons[1].SetActive (true);
				spellSlots[0] = GameObject.Find ("ChamberSlot1");
				spellSlots[1] = GameObject.Find ("ChamberSlot2");
				spellSlots[2] = GameObject.Find ("ChamberSlot3");
				spellSlots[3] = GameObject.Find ("ChamberSlot4");
				spellSlots[4] = GameObject.Find ("ChamberSlot5");
				spellSlots[5] = GameObject.Find ("ChamberSlot6");
				spellSlots[6] = GameObject.Find ("ChamberSlot7");
				spellSlots[7] = GameObject.Find ("ChamberSlot8");
				p1Gun = weapons[1];
				weaponMax = 8;

			break;
			case Weapon_Types.Canegun:

			break;
			case Weapon_Types.Bow:
				weapons[4].SetActive (true);
				p1Gun = weapons[4];
			break;
				
		}
		player.updatePlayerImage ();
	}

}

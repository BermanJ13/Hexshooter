using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FieldManagerPVP : FieldManager
{
	protected Player player2;
	public List<Object> Handful_2 = new List<Object>();
	protected List<Object> Temp_2 = new List<Object>();
	protected List<int> TempNum_2 = new List<int>();
	protected static System.Random rand = new System.Random();  
	protected static GameObject[] pauseObjects_p2;
	protected SpellHolder spellHold_2;
	protected List<GameObject> spellSlots_2 = new List<GameObject>();
	protected GameObject runeDisplay_2;
	protected Text runeDamage_2;
	protected Text runeName_2;
	protected Text runeDesc_2;
	public EventSystem ES_P2;
	public bool p1Ready,p2Ready;
	protected GameObject p2Gun;
	protected Text curBullet_2;
	public GameObject[] weapons_2;
	public int weaponMax_2;
	
	// Use this for initialization
	void Start () 
	{

		once = true;
		weapons = new GameObject[4];
		weapons_2 = new GameObject[4];
		battleObjects = new GameObject[2];
		//Debug.Log (pauseObjects[0]);
		//Hnadful= Deck
		//Pass Deck In from Overworld Scene
		//Placeholder Fils Deck with Lighnin and Eart Spells
		buildDeck();

		createGrid ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		player2 = GameObject.FindGameObjectWithTag ("Player2").GetComponent<Player> ();
		chooseGun_2 (player2.weapon, true);
		getUI ();
		updateEnemyList ();
		updateSpellList ();
		updateObstacleList ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (once)
		{
			chooseGun (player.weapon, false);
			chooseGun_2 (player2.weapon, false);
			once = false;
		}
		//updateHealth ();
		if (player.health <= 0 || player2.health <=0)
		{
			SceneManager.LoadScene (3);
		}		if (pause)
		{
			if (ES_P1.currentSelectedGameObject != null)
			{
				if (ES_P1.currentSelectedGameObject.tag != null)
				{
					if (ES_P1.currentSelectedGameObject.tag == "SpellHolder")
					{
						runeName.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
						runeDamage.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
						runeDesc.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
						runeDisplay.GetComponent<Image> ().sprite = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
						runeDisplay.GetComponent<Image> ().color = new Color(0,0,0,255);
					}
				}
				if (ES_P2.currentSelectedGameObject.tag == "SpellHolder")
				{
					runeName_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
					runeDamage_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
					runeDesc_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
					runeDisplay_2.GetComponent<Image> ().sprite = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
					runeDisplay_2.GetComponent<Image> ().color = new Color(0,0,0,255);
				}
			}
		}
		if (pause && Input.GetButtonDown("Cancel_P1"))
		{
			if (Temp.Count > 0)
			{
				removeBullet ();
			}
		}
		if (pause && Input.GetButtonDown("Cancel_P2"))
		{
			if (Temp_2.Count > 0)
			{
				removeBullet_P2 ();
			}
		}
		if (pause)
		{
			if(Handful.Count == 0 && Handful_2.Count ==0)
				SceneManager.LoadScene (3);

			if (p1Ready && p2Ready)
			{
				showBattleScreen ();
			}
			if (p1Ready)
			{
				ES_P1.SetSelectedGameObject(null);
			}
			if (p2Ready)
			{
				ES_P2.SetSelectedGameObject(null);
			}
		}
		if (!pause)
		{
			player.playerUpdate ();
			player2.playerUpdate ();
			bool enemyReload = true;
			foreach(Spell spell in spells)
			{
				if(spell != null)
					spell.spellUpdate ();
			}

			foreach(Obstacle ob in obstacles)
			{
				if(ob != null)
					ob.obstacleUpdate ();
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
			updateSpellList ();
			deleteSpells ();
			updateObstacleList ();
			deleteObstacles ();
			if (player.reload && player2.reload && spells.Length == 0)
			{
				showReloadScreen ();
			}

		}
	}

	void showReloadScreen()
	{
		p1Ready = false;
		p2Ready = false;
		foreach(GameObject g in bulletIndicators)
		{
			g.SetActive (true);
		}
		for (int i = 0; i < spellSlots.Count; i++)
		{
			spellSlots[i].GetComponent<Image>().sprite = defaultSlot;
			spellSlots[i].GetComponent<Image>().color = Color.white;
		}
		for (int i = 0; i < spellSlots_2.Count; i++)
		{
			spellSlots_2[i].GetComponent<Image>().sprite = defaultSlot;
			spellSlots_2[i].GetComponent<Image>().color = Color.white;
		}
		for(int i=Temp.Count-1;i>-1;i--)
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
		for(int i=Temp_2.Count-1;i>-1;i--)
		{
			if (Temp_2 [i] != null)
			{
				Temp_2.RemoveAt (i);
			}
			if (TempNum_2 [i] != null)
			{
				Handful_2.RemoveAt (TempNum_2 [i]);
				TempNum_2.RemoveAt (i);
			}
		}
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
		for (int i = 0; i< pauseObjects_p2.Length;i++)
		{
			if (i < Handful_2.Count)
			{
				pauseObjects_p2 [i].SetActive (true);
			} 
			else
			{
				pauseObjects_p2 [pauseObjects.Length-1].SetActive (true);
			}
		}
		for (int i = 0; i< battleObjects.Length;i++)
		{
			if(battleObjects[i] != null)
			battleObjects [i].SetActive (false);
		}
		for (int i = 0; i< pauseUI.Length;i++)
		{
			pauseUI [i].SetActive (true);
		}
		selectButton ();
		selectButton_2 ();
		pause = true;
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
				r.runeDamage = curSpell.GetComponent<Spell>().damage.ToString();
				r.runeDesc = curSpell.GetComponent<Spell> ().description;
			}
		}
		for (int i = 0; i < spellHold_2.children.Count; i++)
		{
			Button b = spellHold_2.children [i].gameObject.GetComponent<Button> ();
			int currentHolder_2 = i;
			b.onClick.RemoveAllListeners ();
			b.onClick.AddListener (delegate{addBullet_2(currentHolder_2);});
			if (Handful_2.Count > i)
			{
				GameObject curSpell = ((GameObject)Resources.Load (Handful_2 [i].name));
				curSpell.GetComponent<Spell> ().setDescription (player2.weapon);
				b.GetComponent<Image> ().sprite = curSpell.GetComponent<Spell> ().bulletImage;

				if (b.GetComponent<Image> ().sprite.name == "Knob")
					b.GetComponent<Image> ().color = curSpell.GetComponent<SpriteRenderer> ().color;
				else
				{b.GetComponent<Image> ().color = Color.white;}

				RuneInfo r = spellHold_2.children [i].gameObject.GetComponent<RuneInfo> ();
				r.runeName = curSpell.GetComponent<Spell>().name;
				r.runeImage = curSpell.GetComponent<Spell> ().runeImage;
				r.runeDamage = curSpell.GetComponent<Spell>().damage.ToString();
				r.runeDesc = curSpell.GetComponent<Spell> ().description;
			}
		}
		pause = true;

		player.reload = false;
		player2.reload = false;
	}
	public void showBattleScreen()
	{
		for (int i = 0; i < Temp.Count; i++)
		{
			player.Chamber.Add(Temp [i]);
		}
		foreach (GameObject g in pauseObjects)
		{
			g.SetActive (false);
		}
		for (int i = 0; i < Temp_2.Count; i++)
		{
			player2.Chamber.Add(Temp_2 [i]);
		}
		foreach (GameObject g in pauseObjects_p2)
		{
			g.SetActive (false);
		}
		for (int i = 0; i< battleObjects.Length;i++)
		{
			if(battleObjects[i] != null)
			battleObjects [i].SetActive (true);
		}
		for (int i = 0; i< pauseUI.Length;i++)
		{
			pauseUI [i].SetActive (false);
		}
		pause = false;
		player.reload = false;
	}


	protected void addBullet_2(int num)
	{

		if (Temp_2.Count < weaponMax_2)
		{
			Temp_2.Add (Handful_2 [num]);
			Image slot = spellSlots_2 [Temp_2.Count - 1].GetComponent<Image> ();
			Image rune = spellHold_2.children [num].gameObject.GetComponent<Image> ();
			slot.sprite = rune.sprite;
			slot.color = rune.color;
			spellHold_2.deactivateSpell ("Spell " + num + "_2");
			TempNum_2.Add (num);
			selectButton_2 ();
			if(player2.weapon == 1)
				p2Gun.transform.Rotate (new Vector3 (0.0f, 0.0f, 60.0f));
			else if (player2.weapon == 2 || player2.weapon == 4)
				p2Gun.transform.Rotate (new Vector3 (0.0f,0.0f,45.0f));
			
			//p2Gun.transform.rotation = Quaternion.Lerp(transform.rotation,  Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z + 60), Time.time*0.1f);
			if(Temp_2.Count == weaponMax_2)
				ES_P2.SetSelectedGameObject(GameObject.Find("BattleButton_2"));
			//Debug.Log (num);
		} 
		else
		{
			ES_P2.SetSelectedGameObject(GameObject.Find("BattleButton_2"));
		}
	}
	protected void removeBullet_P2()
	{
		spellHold_2.activateSpell ("Spell " +TempNum_2[TempNum_2.Count-1]+ "_2");
		spellSlots_2[Temp_2.Count-1].GetComponent<Image>().sprite = defaultSlot;
		spellSlots_2[Temp_2.Count-1].GetComponent<Image>().color = Color.white;
		Temp_2.RemoveAt (Temp_2.Count - 1);
		TempNum_2.RemoveAt (TempNum_2.Count - 1);
		if(player2.weapon == 1)
			p2Gun.transform.Rotate (new Vector3 (0.0f, 0.0f, -60.0f));
		else if (player2.weapon == 2 || player2.weapon == 4)
			p2Gun.transform.Rotate (new Vector3 (0.0f,0.0f,-45.0f));
		//p2Gun.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z - 60), Time.time*0.1f);

	}
	protected void selectButton_2 ()
	{
		bool found = false;
		for (int i = 0; i < 9; i++)
		{
			bool used = false;
			foreach(int j in TempNum_2)
			{
				if (j == i)
					used = true;
			}
			if(GameObject.Find("Spell " +i+ "_2") != null && !found && !used)
			{
				ES_P2.SetSelectedGameObject(GameObject.Find("Spell " +i+ "_2"));
				found = true;
			}
			else if(!found)
				ES_P2.SetSelectedGameObject(GameObject.Find("BattleButton_2"));
		}
	}
	public void readyP1()
	{
		p1Ready = true;
		if (p2Ready)
			showBattleScreen ();
	}
	public void readyP2()
	{
		p2Ready = true;
		if (p1Ready)
			showBattleScreen ();
	}
	public void getUI()
	{
		base.getUI ();
		spellHold_2 = GameObject.Find ("SpellHolder_2").GetComponent<SpellHolder>();

		pauseObjects_p2 = new GameObject[11];
		pauseObjects_p2[0] = GameObject.Find("Spell 0_2");
		pauseObjects_p2[1] = GameObject.Find("Spell 1_2");
		pauseObjects_p2[2] = GameObject.Find("Spell 2_2");
		pauseObjects_p2[3] = GameObject.Find("Spell 3_2");
		pauseObjects_p2[4] = GameObject.Find("Spell 4_2");
		pauseObjects_p2[5] = GameObject.Find("Spell 5_2");
		pauseObjects_p2[6] = GameObject.Find("Spell 6_2");
		pauseObjects_p2[7] = GameObject.Find("Spell 7_2");
		pauseObjects_p2[8] = GameObject.Find("Spell 8_2");
		pauseObjects_p2[9] = GameObject.Find("Spell 9_2");
		pauseObjects_p2[10] = GameObject.Find("BattleButton_2");
		pauseUI = GameObject.FindGameObjectsWithTag ("PauseUI");


		bulletIndicators = new GameObject[16];
		bulletIndicators [0] = GameObject.Find ("Player 1 Bottle 1");
		bulletIndicators [1] = GameObject.Find ("Player 1 Bottle 2");
		bulletIndicators [2] = GameObject.Find ("Player 1 Bottle 3");
		bulletIndicators [3] = GameObject.Find ("Player 1 Bottle 4");
		bulletIndicators [4] = GameObject.Find ("Player 1 Bottle 5");
		bulletIndicators [5] = GameObject.Find ("Player 1 Bottle 6");
		bulletIndicators [6] = GameObject.Find ("Player 1 Bottle 7");
		bulletIndicators [7] = GameObject.Find ("Player 1 Bottle 8");
		bulletIndicators [8] = GameObject.Find ("Player 2 Bottle 1");
		bulletIndicators [9] = GameObject.Find ("Player 2 Bottle 2");
		bulletIndicators [10] = GameObject.Find ("Player 2 Bottle 3");
		bulletIndicators [11] = GameObject.Find ("Player 2 Bottle 4");
		bulletIndicators [12] = GameObject.Find ("Player 2 Bottle 5");
		bulletIndicators [13] = GameObject.Find ("Player 2 Bottle 6");
		bulletIndicators [14] = GameObject.Find ("Player 2 Bottle 7");
		bulletIndicators [15] = GameObject.Find ("Player 2 Bottle 8");

		runeDisplay_2 = GameObject.Find ("RuneHolder_2");
		runeDamage_2 = GameObject.Find ("RuneDamage_2").GetComponent<Text>();
		runeName_2 = GameObject.Find ("Rune Name_2").GetComponent<Text>();
		runeDesc_2 = GameObject.Find ("Rune Description_2").GetComponent<Text>();

		battleObjects = new GameObject[2];
		battleObjects[1] = GameObject.Find("Current Bullet_2");
	}
	protected void createGrid()
	{

		//Creates the Grid
		for (int y = 0; y < 5; y++) 
		{
			for (int x = 0; x < 10; x++) 
			{
				//Checks whether the current panel is for the enmy or player side
				if(x<5)
				{
					Instantiate(Resources.Load("Player_Panel"), new Vector3(x, y, 0), Quaternion.identity);
					//sPawns the Player
					if(y==2 && x==0)
						Instantiate(Resources.Load("Player"), new Vector3(x, y, 0), Quaternion.identity);
				}
				else
					Instantiate(Resources.Load("Enemy_Panel"), new Vector3(x, y, 0), Quaternion.identity);
				if(y==2 && x==9)
					Instantiate(Resources.Load("Player2"), new Vector3(x, y, 0), Quaternion.identity);
			}
		}
	}
	protected void buildDeck()
	{
		for (int i = 0; i < 6; i++)
		{
			//Handful.Add(Resources.Load ("Chains"));
			Handful.Add(Resources.Load ("Fire"));
			Handful.Add(Resources.Load ("Water"));
			Handful.Add(Resources.Load ("Earth"));
			Handful.Add(Resources.Load ("Fire"));
			Handful.Add(Resources.Load ("Boomerang"));
		}
		Shuffle(Handful);

		//Placeholder Fils Deck with Lighnin and Eart Spells
		for (int i = 0; i < 6; i++)
		{
			//Handful_2.Add(Resources.Load ("Chains"));
			Handful_2.Add(Resources.Load ("Wind"));
			Handful_2.Add(Resources.Load ("Water"));
			Handful_2.Add(Resources.Load ("Earth"));
			Handful_2.Add(Resources.Load ("Fire"));
			Handful_2.Add(Resources.Load ("Fire"));
		}
		Shuffle(Handful_2);
	}
	public void chooseGun_2(int weapon, bool first)
	{
		Debug.Log (weapon);
		if (first)
		{
			weapons_2 [0] = GameObject.Find ("UI_GunCylinder_2");
			weapons_2 [1] = GameObject.Find ("8 Rifle_2");
			weapons_2 [2] = GameObject.Find ("4 Shot Gun_2");
			weapons_2 [3] = GameObject.Find ("2 Shot Gun_2");
		}

		for (int i = 0; i < weapons.Length; i++)
		{
			if(weapons_2[i] != null)
				weapons_2 [i].SetActive (false);
		}
		switch (weapon)
		{
			case 1:
				weapons_2 [0].SetActive (true);
				spellSlots_2.Add (GameObject.Find ("SpellSlot1_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot2_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot3_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot4_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot5_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot6_2"));

				p2Gun = weapons_2 [0];
				weaponMax_2 = 6;
			break;
			case 2:
				weapons_2 [1].SetActive (true);
				spellSlots_2.Add (GameObject.Find ("SpellSlot1_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot2_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot3_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot4_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot5_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot6_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot7_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot8_2"));
				p2Gun = weapons_2 [1];
				weaponMax_2 = 8;
			break;
			case 3:
				weapons_2 [3].SetActive (true);
				spellSlots_2.Add (GameObject.Find ("SpellSlot1_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot2_2"));
				p2Gun = weapons_2 [3];
				weaponMax_2 = 2;

				//weapons_2[2].SetActive (true);
				//spellSlots.Add (GameObject.Find("SpellSlot1_2"));
				//spellSlots.Add (GameObject.Find("SpellSlot2_2"));
				//spellSlots.Add (GameObject.Find("SpellSlot3_2"));
				//spellSlots.Add (GameObject.Find("SpellSlot4_2"));
				//p1Gun = weapons_2[2];
				//weaponMax = 4;
			break;
			case 4:
				weapons_2 [1].SetActive (true);
				spellSlots_2.Add (GameObject.Find ("SpellSlot1_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot2_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot3_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot4_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot5_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot6_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot7_2"));
				spellSlots_2.Add (GameObject.Find ("SpellSlot8_2"));
				p2Gun = weapons_2 [1];
				weaponMax_2 = 8;

			break;
			case 5:

			break;

		}

	}
}

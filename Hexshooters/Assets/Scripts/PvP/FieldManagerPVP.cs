using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class FieldManagerPVP : FieldManager
{
	public Player player2;
	public List<Object> Handful_2 = new List<Object>();
	public List<Object> Temp_2 = new List<Object>();
	public List<int> TempNum_2 = new List<int>();
	private static System.Random rand = new System.Random();  
	static GameObject[] pauseObjects;
	static GameObject[] pauseObjects_p2;
	static GameObject[] pauseUI;
	SpellHolder spellHold_2;
	public List<GameObject> spellSlots_2 = new List<GameObject>();
	public GameObject runeDisplay_2;
	public Text runeDamage_2;
	public Text runeName_2;
	public EventSystem ES_P1;
	public EventSystem ES_P2;
	public bool p1Ready,p2Ready;
	GameObject p1Gun;
	GameObject p2Gun;
	GameObject[] bulletIndicators;
	
	// Use this for initialization
	void Start () 
	{

		getUI ();

		//Debug.Log (pauseObjects[0]);
		//Hnadful= Deck
		//Pass Deck In from Overworld Scene
		//Placeholder Fils Deck with Lighnin and Eart Spells
		for (int i = 0; i < 10; i++)
		{
			Handful.Add(Resources.Load ("Lightning"));
			Handful.Add(Resources.Load ("Earth"));
			Handful.Add(Resources.Load ("Water"));
		}
		Shuffle(Handful);

		//Placeholder Fils Deck with Lighnin and Eart Spells
		for (int i = 0; i < 10; i++)
		{
			Handful_2.Add(Resources.Load ("Lightning"));
			Handful_2.Add(Resources.Load ("Earth"));
			Handful_2.Add(Resources.Load ("Water"));
		}
		Shuffle(Handful_2);

		createGrid ();

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		player2 = GameObject.FindGameObjectWithTag ("Player2").GetComponent<Player> ();
		updateEnemyList ();
		updateSpellList ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//updateHealth ();
		if (pause)
		{
			if(ES_P1.currentSelectedGameObject.tag != null)
			{
				if (ES_P1.currentSelectedGameObject.tag == "SpellHolder")
				{
					runeName.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
					runeDamage.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
					runeDisplay.GetComponent<Image> ().sprite = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
				}
			}
			if (ES_P2.currentSelectedGameObject.tag == "SpellHolder")
			{
				runeName_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
				runeDamage_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
				runeDisplay_2.GetComponent<Image> ().sprite = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
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
			Debug.Log ("P1 Ready " + p1Ready + " P2 Ready " + p2Ready);
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
			if (player.reload && player2.reload)
			{
				showReloadScreen ();
			}

		}
	}
	
	void deleteSpells()
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
	}
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
				b.GetComponent<Image> ().sprite = curSpell.GetComponent<Spell> ().bulletImage;
				if (b.GetComponent<Image> ().sprite.name == "Knob")
					b.GetComponent<Image> ().color = curSpell.GetComponent<SpriteRenderer> ().color;
				else
					b.GetComponent<Image> ().color = Color.white;
				RuneInfo r = spellHold.children [i].gameObject.GetComponent<RuneInfo> ();
				r.runeName = curSpell.GetComponent<Spell>().name;
				r.runeImage = curSpell.GetComponent<Spell> ().runeImage;
				r.runeDamage = curSpell.GetComponent<Spell>().damage.ToString();
			}
		}
		for (int i = 0; i < spellHold_2.children.Count; i++)
		{
			Button b = spellHold_2.children [i].gameObject.GetComponent<Button> ();
			int currentHolder = i;
			b.onClick.RemoveAllListeners ();
			b.onClick.AddListener (delegate{addBullet_2(currentHolder);});
			if (Handful_2.Count > i)
			{
				GameObject curSpell = ((GameObject)Resources.Load (Handful_2 [i].name));
				b.GetComponent<Image> ().sprite = curSpell.GetComponent<Spell> ().bulletImage;
				if (b.GetComponent<Image> ().sprite.name == "Knob")
					b.GetComponent<Image> ().color = curSpell.GetComponent<SpriteRenderer> ().color;
				else
					b.GetComponent<Image> ().color = Color.white;
				RuneInfo r = spellHold_2.children [i].gameObject.GetComponent<RuneInfo> ();
				r.runeName = curSpell.GetComponent<Spell>().name;
				r.runeImage = curSpell.GetComponent<Spell> ().runeImage;
				r.runeDamage = curSpell.GetComponent<Spell>().damage.ToString();
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
		for (int i = 0; i< pauseUI.Length;i++)
		{
			pauseUI [i].SetActive (false);
		}
		pause = false;
	}
	
	public void Shuffle(List<Object> list) 
	{
		for(int i = Handful.Count -1; i > 1; i--)
		{
			int k = (rand.Next(0, i));
			Object value = Handful[k];
			Handful[k] = Handful[i];
			Handful[i] = value;
		}
	}
	void addBullet(int num)
	{

		if (Temp.Count < 6)
		{
			Temp.Add (Handful [num]);
			Image slot = spellSlots [Temp.Count - 1].GetComponent<Image> ();
			Image rune = spellHold.children [num].gameObject.GetComponent<Image> ();
			slot.sprite = rune.sprite;
			slot.color = rune.color;
			spellHold.deactivateSpell ("Spell " + num + "");
			TempNum.Add (num);
			selectButton ();
			p1Gun.transform.Rotate (new Vector3 (0.0f,0.0f,60.0f));
			//p1Gun.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z + 60), Time.time*0.1f);
			if(Temp.Count == 6)
				ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));
			//Debug.Log (num);
		} 
		else
		{
			ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));
		}
	}
	void addBullet_2(int num)
	{

		if (Temp_2.Count < 6)
		{
			Temp_2.Add (Handful_2 [num]);
			Image slot = spellSlots_2 [Temp_2.Count - 1].GetComponent<Image> ();
			Image rune = spellHold_2.children [num].gameObject.GetComponent<Image> ();
			slot.sprite = rune.sprite;
			slot.color = rune.color;
			spellHold_2.deactivateSpell ("Spell " + num + "_2");
			TempNum_2.Add (num);
			selectButton_2 ();
			p2Gun.transform.Rotate (new Vector3 (0.0f,0.0f,60.0f));
			//p2Gun.transform.rotation = Quaternion.Lerp(transform.rotation,  Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z + 60), Time.time*0.1f);
			if(Temp_2.Count == 6)
				ES_P2.SetSelectedGameObject(GameObject.Find("BattleButton_2"));
			//Debug.Log (num);
		} 
		else
		{
			ES_P2.SetSelectedGameObject(GameObject.Find("BattleButton_2"));
		}
	}
	void removeBullet()
	{
		spellHold.activateSpell ("Spell " +TempNum[TempNum.Count-1]+ "");
		spellSlots[Temp.Count-1].GetComponent<Image>().sprite = defaultSlot;
		spellSlots[Temp.Count-1].GetComponent<Image>().color = Color.white;
		Temp.RemoveAt (Temp.Count - 1);
		TempNum.RemoveAt (TempNum.Count - 1);
		p1Gun.transform.Rotate (new Vector3 (0.0f,0.0f,-60.0f));
		//p1Gun.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z - 60), Time.time*0.1f);
	}
	void removeBullet_P2()
	{
		spellHold_2.activateSpell ("Spell " +TempNum_2[TempNum_2.Count-1]+ "_2");
		spellSlots_2[Temp_2.Count-1].GetComponent<Image>().sprite = defaultSlot;
		spellSlots_2[Temp_2.Count-1].GetComponent<Image>().color = Color.white;
		Temp_2.RemoveAt (Temp_2.Count - 1);
		TempNum_2.RemoveAt (TempNum_2.Count - 1);
		p2Gun.transform.Rotate (new Vector3 (0.0f,0.0f,-60.0f));
		//p2Gun.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z - 60), Time.time*0.1f);

	}
	void selectButton ()
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
	void selectButton_2 ()
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
	}
	public void readyP2()
	{
		p2Ready = true;
	}
	public void getUI()
	{

		can = GameObject.Find ("Canvas");
		spellHold = GameObject.Find ("SpellHolder").GetComponent<SpellHolder>();
		spellHold_2 = GameObject.Find ("SpellHolder_2").GetComponent<SpellHolder>();
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

		spellSlots.Add (GameObject.Find("SpellSlot1"));
		spellSlots.Add (GameObject.Find("SpellSlot2"));
		spellSlots.Add (GameObject.Find("SpellSlot3"));
		spellSlots.Add (GameObject.Find("SpellSlot4"));
		spellSlots.Add (GameObject.Find("SpellSlot5"));
		spellSlots.Add (GameObject.Find("SpellSlot6"));

		spellSlots_2.Add (GameObject.Find("SpellSlot1_2"));
		spellSlots_2.Add (GameObject.Find("SpellSlot2_2"));
		spellSlots_2.Add (GameObject.Find("SpellSlot3_2"));
		spellSlots_2.Add (GameObject.Find("SpellSlot4_2"));
		spellSlots_2.Add (GameObject.Find("SpellSlot5_2"));
		spellSlots_2.Add (GameObject.Find("SpellSlot6_2"));


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

		runeDisplay = GameObject.Find ("RuneHolder");
		runeDamage = GameObject.Find ("RuneDamage").GetComponent<Text>();
		runeName = GameObject.Find ("Rune Name").GetComponent<Text>();

		runeDisplay_2 = GameObject.Find ("RuneHolder_2");
		runeDamage_2 = GameObject.Find ("RuneDamage_2").GetComponent<Text>();
		runeName_2 = GameObject.Find ("Rune Name_2").GetComponent<Text>();

		p1Gun = GameObject.Find ("UI_GunCylinder");
		p2Gun = GameObject.Find ("UI_GunCylinder_2");
	}
	void createGrid()
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
}

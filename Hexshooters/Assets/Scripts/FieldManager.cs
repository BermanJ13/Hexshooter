using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class FieldManager : MonoBehaviour 
{
	public Spell[] spells;
	public Enemy[] enemies;
	public Player player;
	public bool pause = false;
	public List<Object> Handful = new List<Object>();
	public List<Object> Temp = new List<Object>();
	public List<int> TempNum = new List<int>();
	private static System.Random rand = new System.Random();  
	static GameObject[] pauseObjects;
	static GameObject[] pauseUI;
	SpellHolder spellHold;
	public List<GameObject> spellSlots = new List<GameObject>();
	public Sprite defaultSlot;
	public GameObject runeDisplay;
	public Text runeDamage;
	public Text runeName;
	public GameObject can;

	// Use this for initialization
	void Start () 
	{
		can = GameObject.Find ("Canvas");
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

		spellSlots.Add (GameObject.Find("SpellSlot1"));
		spellSlots.Add (GameObject.Find("SpellSlot2"));
		spellSlots.Add (GameObject.Find("SpellSlot3"));
		spellSlots.Add (GameObject.Find("SpellSlot4"));
		spellSlots.Add (GameObject.Find("SpellSlot5"));
		spellSlots.Add (GameObject.Find("SpellSlot6"));

		runeDisplay = GameObject.Find ("RuneHolder");
		runeDamage = GameObject.Find ("RuneDamage").GetComponent<Text>();
		runeName = GameObject.Find ("Rune Name").GetComponent<Text>();

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
			}
		}

		//test Dummy
		Instantiate (Resources.Load("TestDummy"),new Vector3(6,3,0),Quaternion.identity);

		Instantiate (Resources.Load("TestDummy"),new Vector3(5,4,0),Quaternion.identity);

		Instantiate (Resources.Load("TestDummy"),new Vector3(7,2,0),Quaternion.identity);

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		updateEnemyList ();
		updateSpellList ();
		//showHealth ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//updateHealth ();
		if(EventSystem.current.currentSelectedGameObject.tag == "SpellHolder")
		{
			runeName.text = EventSystem.current.currentSelectedGameObject.GetComponent<RuneInfo>().runeName;
			runeDamage.text = EventSystem.current.currentSelectedGameObject.GetComponent<RuneInfo>().runeDamage;
			runeDisplay.GetComponent<Image>().sprite = EventSystem.current.currentSelectedGameObject.GetComponent<RuneInfo>().runeImage;
		}
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			if (Temp.Count > 0)
			{
				removeBullet ();
			}
		}
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
			updateSpellList ();
			deleteSpells ();
			if (player.reload && enemyReload)
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
		for (int i = 0; i < spellSlots.Count; i++)
		{
			spellSlots[i].GetComponent<Image>().sprite = defaultSlot;
			spellSlots[i].GetComponent<Image>().color = Color.white;
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
		foreach (int i  in TempNum)
		{
			Debug.Log (i);
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
		for (int i = 0; i< pauseUI.Length;i++)
		{
				pauseUI [i].SetActive (true);
		}
		selectButton ();
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
		player.reload = false;
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
		for (int i = 0; i< pauseUI.Length;i++)
		{
			pauseUI [i].SetActive (false);
		}
		pause = false;
		player.reload = false;
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
			if(Temp.Count == 6)
				EventSystem.current.SetSelectedGameObject(GameObject.Find("BattleButton"));
			//Debug.Log (num);
		} 
		else
		{
			EventSystem.current.SetSelectedGameObject(GameObject.Find("BattleButton"));
		}
	}
	void removeBullet()
	{
		spellHold.activateSpell ("Spell " +TempNum[TempNum.Count-1]+ "");
		spellSlots[Temp.Count-1].GetComponent<Image>().sprite = defaultSlot;
		spellSlots[Temp.Count-1].GetComponent<Image>().color = Color.white;
		Temp.RemoveAt (Temp.Count - 1);
		TempNum.RemoveAt (TempNum.Count - 1);
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
				EventSystem.current.SetSelectedGameObject(GameObject.Find("Spell " +i+ ""));
				found = true;
			}
			else if(!found)
				EventSystem.current.SetSelectedGameObject(GameObject.Find("BattleButton"));
		}
	}
	void showHealth()
	{
		for (int i =0;i<enemies.Length;i++)
		{
			GameObject enemyText = new GameObject ("eText" + i);
			Text ehealth = enemyText.AddComponent<Text> ();
			ehealth.transform.position = new Vector3 (enemies[i].transform.position.x, enemies[i].transform.position.y + 10, enemies[i].transform.position.z);
			ehealth.text = enemies[i].health.ToString();
		}
	}
	void updateHealth()
	{
		for (int i =0; i<enemies.Length;i++)
		{
			GameObject enemyText = GameObject.Find("eText" + i);
			Text ehealth = enemyText.GetComponent<Text> ();
			ehealth.text = enemies[i].health.ToString();
		}
	}
}

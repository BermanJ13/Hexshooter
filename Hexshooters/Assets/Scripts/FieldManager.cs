using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;

public class FieldManager : MonoBehaviour
{
    public string mapFile;
    [SerializeField]public Transform[] gamePieces;
    public Dictionary<string, Transform> things = new Dictionary<string, Transform>();
    private StreamReader reader;
    private List<string> rows = new List<string>();

	protected Transform playerPanel;
	protected Transform enemyPanel;
	protected Transform Testdummy;
	protected Spell[] spells;
	protected Enemy[] enemies;
	protected Obstacle[] obstacles;
	protected GameObject[] bulletIndicators;
	protected Player player;
	protected bool pause = false;
	public List<Object> Handful = new List<Object>();
	protected List<Object> Temp = new List<Object>();
	protected List<int> TempNum = new List<int>();
	protected  static System.Random rand = new System.Random();  
	protected static GameObject[] pauseObjects;
	protected static GameObject[] pauseUI;
	protected SpellHolder spellHold;
	protected List<GameObject> spellSlots = new List<GameObject>();
	public Sprite defaultSlot;
	protected GameObject runeDisplay;
	protected Text runeDamage;
	protected Text runeName;
	protected Text runeDesc;
	protected GameObject can;
	protected GameObject p1Gun;
	public EventSystem ES_P1;
	protected GameObject[] battleObjects;

	// Use this for initialization
	void Start () 
	{
		ES_P1 = EventSystem.current;
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
        //creates a dictionary out of the list of objects made in the inspector
        foreach (Transform trns in gamePieces)
        {
            things.Add(trns.name, trns);
        }
        things.Add("p", gamePieces[0]);
        things.Add("e", gamePieces[1]);

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
		updateObstacleList ();
		//showHealth ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//updateHealth ();
		if(ES_P1.currentSelectedGameObject.tag == "SpellHolder")
		{
			runeName.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo>().runeName;
			runeDamage.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo>().runeDamage;
			runeDesc.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
			runeDisplay.GetComponent<Image>().sprite = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo>().runeImage;
		}
		if (pause && Input.GetKeyDown (KeyCode.Escape))
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
			updateObstacleList ();
			deleteObstacles ();
			if (player.reload && enemyReload)
			{
				showReloadScreen ();
			}
		}
	}

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
	public void updateObstacleList()
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
				r.runeDesc = curSpell.GetComponent<Spell> ().description;
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
	protected void addBullet(int num)
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
			if(Temp.Count == 6)
				ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));
			//Debug.Log (num);
		} 
		else
		{
			ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));
		}
	}
	protected void removeBullet()
	{
		spellHold.activateSpell ("Spell " +TempNum[TempNum.Count-1]+ "");
		spellSlots[Temp.Count-1].GetComponent<Image>().sprite = defaultSlot;
		spellSlots[Temp.Count-1].GetComponent<Image>().color = Color.white;
		Temp.RemoveAt (Temp.Count - 1);
		TempNum.RemoveAt (TempNum.Count - 1);

		p1Gun.transform.Rotate (new Vector3 (0.0f,0.0f,-60.0f));
	}
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
	public void getUI()
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

		bulletIndicators = new GameObject[8];
		bulletIndicators [0] = GameObject.Find ("Player 1 Bottle 1");
		bulletIndicators [1] = GameObject.Find ("Player 1 Bottle 2");
		bulletIndicators [2] = GameObject.Find ("Player 1 Bottle 3");
		bulletIndicators [3] = GameObject.Find ("Player 1 Bottle 4");
		bulletIndicators [4] = GameObject.Find ("Player 1 Bottle 5");
		bulletIndicators [5] = GameObject.Find ("Player 1 Bottle 6");
		bulletIndicators [6] = GameObject.Find ("Player 1 Bottle 7");
		bulletIndicators [7] = GameObject.Find ("Player 1 Bottle 8");

		runeDisplay = GameObject.Find ("RuneHolder");
		runeDamage = GameObject.Find ("RuneDamage").GetComponent<Text>();
		runeName = GameObject.Find ("Rune Name").GetComponent<Text>();
		runeDesc = GameObject.Find ("Rune Description").GetComponent<Text>();
		battleObjects = GameObject.FindGameObjectsWithTag("battleUI");

		p1Gun = GameObject.Find ("UI_GunCylinder");
	}

}

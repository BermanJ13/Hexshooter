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
	SpellHolder spellHold;

	// Use this for initialization
	void Start () 
	{
		spellHold = GameObject.Find ("SpellHolder").GetComponent<SpellHolder>();
		pauseObjects = GameObject.FindGameObjectsWithTag ("ShowOnPause");

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
	}
	
	// Update is called once per frame
	void Update () 
	{
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
		foreach (int i  in TempNum)
		{
			Debug.Log (i);
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
			if(i <= Handful.Count)
			{
				pauseObjects[i].SetActive (true);
			}
		}
		selectButton ();
		pause = true;
		for (int i = 0; i < spellHold.children.Count; i++)
		{
			Button b = spellHold.children [i].gameObject.GetComponent<Button> ();
			int currentHolder = i;
			b.onClick.RemoveAllListeners ();
			b.onClick.AddListener (delegate{addBullet(currentHolder);});
			Debug.Log (Handful [i].name);
			b.GetComponent<Image>().color = ((GameObject)Resources.Load ( Handful [i].name)).GetComponent<SpriteRenderer>().color;
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
}

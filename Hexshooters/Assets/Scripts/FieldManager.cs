using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldManager : MonoBehaviour 
{
	public Spell[] spells;
	public Enemy[] enemies;
	public Player player;
	public bool pause = false;
	public List<Object> Handful = new List<Object>();
	private static System.Random rand = new System.Random();  
	static GameObject[] pauseObjects;

	// Use this for initialization
	void Start () 
	{
		pauseObjects = GameObject.FindGameObjectsWithTag ("ShowOnPause");
		Debug.Log (pauseObjects[0]);
		//Hnadful= Deck
		//Pass Deck In from Overworld Scene
		//Placeholder Fils Deck with Lighnin and Eart Spells
		for (int i = 0; i < 30; i++)
		{
			if (i % 2 == 0)
			{
				Handful.Add(Resources.Load ("Lightning"));
			} 
			else
			{
				Handful.Add(Resources.Load ("Earth"));
			}
		}

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
		if (Input.GetKeyDown (KeyCode.P))
		{

			if(!pause)
			{
				showReloadScreen ();
				pause = true;
			}
			else if(pause)
			{

				showBattleScreen ();
				pause = false;
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
		foreach (GameObject g in pauseObjects)
		{
			g.SetActive (true);
		}
		pause = false;
		//Placholder - Reload the chamber
		Shuffle(Handful);
		while (player.Chamber.Count < 6 || Handful.Count >0)
		{
			player.Chamber.Add (Handful [0]);
			Handful.RemoveAt (0);
		}

		player.reload = false;
	}
	public void showBattleScreen()
	{
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

}

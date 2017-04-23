﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ActiveFieldManagerPvP : FieldManagerPVP {
	protected static GameObject[] pauseUI_2;
	protected GameObject[] bulletIndicators_2;
	protected GameObject[] battleObjects_2;
	public bool p1reload;
	public bool p2reload;
	void Start()
	{

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
		p1reload = true;
		p2reload = true;
		firstPause = true;
		pause = true;
		once = true;
	}

	// Use this for initialization
	void Update () 
	{
		if (once)
		{
			chooseGun (player.weapon, false);
			chooseGun_2 (player2.weapon, false);
			showReloadScreen (1);
			showReloadScreen (2);

			once = false;
		}
		if (player.health <= 0 )
		{
			SceneManager.LoadScene ("Game Over");
		}
		if(enemies.Length == 0)
		{
			SceneManager.LoadScene ("Overworld");
		}
		//updateHealth ();
		if(ES_P1.currentSelectedGameObject.tag == "SpellHolder")
		{
			runeName.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
			runeDamage.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
			runeDesc.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
			runeDisplay.GetComponent<Image> ().sprite = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
			runeDisplay.GetComponent<Image> ().color = new Color(0,0,0,255);
		}
		if (ES_P2.currentSelectedGameObject.tag == "SpellHolder")
		{
			runeName_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
			runeDamage_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
			runeDesc_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
			runeDisplay_2.GetComponent<Image> ().sprite = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
			runeDisplay_2.GetComponent<Image> ().color = new Color(0,0,0,255);
		}

		if (pause)
		{
			if (Input.GetButtonDown ("Cancel_P1"))
			{
				if (Temp.Count > 0)
				{
					removeBullet ();
				}
			}
			if (Input.GetButtonDown ("Cancel_P2"))
			{
				if (Temp_2.Count > 0)
				{
					removeBullet_P2 ();
				}
			}
			if (p1Ready)
			{
				ES_P1.SetSelectedGameObject(null);
			}
			if (p2Ready)
			{
				ES_P2.SetSelectedGameObject(null);
			}
			if (firstPause)
			{

				if (p1Ready && p2Ready)
				{
					//firstPause = false;
					showBattleScreen (1);
					showBattleScreen (2);
				}
			} 
			else
			{
				if(!p1reload)
					player.playerUpdate ();
				if(!p2reload)
					player2.playerUpdate ();
				
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
				if (p1Ready)
				{
					showBattleScreen (1);
				}
				if (p2Ready)
				{
					showBattleScreen (2);
				}

				if (!p1reload && Input.GetButtonDown("Start_P1") && player.reload)
					showReloadScreen (1);
				if(!p2reload && Input.GetButtonDown("Start_P2") && player2.reload)
					showReloadScreen (2);
			}
		} 
		else
		{
			if(!p1reload)
				player.playerUpdate ();
			if(!p2reload)
				player2.playerUpdate ();
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

			if (player.reload && Input.GetButtonDown("Start_P1"))
			{
				showReloadScreen (1);
			}
			if ( player2.reload && Input.GetButtonDown("Start_P2"))
			{
				showReloadScreen (2);
			}
		}
		if (p1reload || p2reload)
			pause = true;
	}
	public void showReloadScreen(int num)
	{
		pause = true;
		if (num == 1)
		{
			p1reload = true;
			foreach (GameObject g in bulletIndicators)
			{
				g.SetActive (true);
			}
			for (int i = 0; i < spellSlots.Count; i++)
			{
				spellSlots [i].GetComponent<Image> ().sprite = defaultSlot;
				spellSlots [i].GetComponent<Image> ().color = Color.white;
			}
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
		 
			for (int i = 0; i < pauseObjects.Length; i++)
			{
				if (i < Handful.Count)
				{
					pauseObjects [i].SetActive (true);
				}
				else
				{
					pauseObjects [pauseObjects.Length - 1].SetActive (true);
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
		}
		else
		{
			p2reload = true;
			foreach(GameObject g in bulletIndicators)
			{
				g.SetActive (true);
			}
			for (int i = 0; i < spellSlots_2.Count; i++)
			{
				spellSlots_2[i].GetComponent<Image>().sprite = defaultSlot;
				spellSlots_2[i].GetComponent<Image>().color = Color.white;
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
			for (int i = 0; i< battleObjects_2.Length;i++)
			{
				if(battleObjects_2[i] != null)
					battleObjects_2 [i].SetActive (false);
			}
			for (int i = 0; i< pauseUI_2.Length;i++)
			{
				pauseUI_2 [i].SetActive (true);
			}
			selectButton_2 ();
			for (int i = 0; i < spellHold_2.children.Count; i++)
			{
				Button b = spellHold_2.children [i].gameObject.GetComponent<Button> ();
				int currentHolder = i;
				b.onClick.RemoveAllListeners ();
				b.onClick.AddListener (delegate{addBullet_2(currentHolder);});
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
		}
	}
	public void showBattleScreen( int num)
	{
		if (num == 1)
		{
			p1Ready = false;
			p1reload = false;
			player.reload = false;
			player2.reload = false;
			for (int i = 0; i < Temp.Count; i++)
			{
				player.Chamber.Add(Temp [i]);
			}
			foreach (GameObject g in pauseObjects)
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
			firstPause = false;
		}
		else
		{
			p2Ready = false;
			p2reload = false;
			player.reload = false;
			player2.reload = false;
			for (int i = 0; i < Temp_2.Count; i++)
			{
				player2.Chamber.Add(Temp_2 [i]);
			}
			foreach (GameObject g in pauseObjects_p2)
			{
				g.SetActive (false);
			}
			for (int i = 0; i< battleObjects_2.Length;i++)
			{
				if(battleObjects_2[i] != null)
					battleObjects_2 [i].SetActive (true);
			}
			for (int i = 0; i< pauseUI_2.Length;i++)
			{
				pauseUI_2 [i].SetActive (false);
			}
			pause = false;
			firstPause = false;
		}
	}
	public void getUI()
	{
		base.getUI ();
		bulletIndicators = new GameObject[8];
		bulletIndicators_2  = new GameObject[8];
		bulletIndicators [0] = GameObject.Find ("Player 1 Bottle 1");
		bulletIndicators [1] = GameObject.Find ("Player 1 Bottle 2");
		bulletIndicators [2] = GameObject.Find ("Player 1 Bottle 3");
		bulletIndicators [3] = GameObject.Find ("Player 1 Bottle 4");
		bulletIndicators [4] = GameObject.Find ("Player 1 Bottle 5");
		bulletIndicators [5] = GameObject.Find ("Player 1 Bottle 6");
		bulletIndicators [6] = GameObject.Find ("Player 1 Bottle 7");
		bulletIndicators [7] = GameObject.Find ("Player 1 Bottle 8");
		bulletIndicators_2 [0] = GameObject.Find ("Player 2 Bottle 1");
		bulletIndicators_2 [1] = GameObject.Find ("Player 2 Bottle 2");
		bulletIndicators_2 [2] = GameObject.Find ("Player 2 Bottle 3");
		bulletIndicators_2 [3] = GameObject.Find ("Player 2 Bottle 4");
		bulletIndicators_2 [4] = GameObject.Find ("Player 2 Bottle 5");
		bulletIndicators_2 [5] = GameObject.Find ("Player 2 Bottle 6");
		bulletIndicators_2 [6] = GameObject.Find ("Player 2 Bottle 7");
		bulletIndicators_2 [7] = GameObject.Find ("Player 2 Bottle 8");

		pauseUI_2 = GameObject.FindGameObjectsWithTag ("PauseUI_P2");

		//battleObjects[1] = null;
		battleObjects = new GameObject[1];
		battleObjects[0] = GameObject.Find("Current Bullet");
		battleObjects_2 = new GameObject[1];
		battleObjects_2[0] = GameObject.Find("Current Bullet_2");
	}
	protected void createGrid()
	{

		//Creates the Grid
		for (int y = 0; y < 5; y++)
		{
			for (int x = 0; x < 10; x++)
			{
				//Checks whether the current panel is for the enmy or player side
				if (x < 5)
				{
					Instantiate (Resources.Load ("Player_Panel"), new Vector3 (x, y, 0), Quaternion.identity);
					//sPawns the Player
					if (y == 2 && x == 0)
						Instantiate (Resources.Load ("Player"), new Vector3 (x, y, 0), Quaternion.identity);
				}
				else
					Instantiate (Resources.Load ("Enemy_Panel"), new Vector3 (x, y, 0), Quaternion.identity);
				if (y == 2 && x == 9)
					Instantiate (Resources.Load ("Player2"), new Vector3 (x, y, 0), Quaternion.identity);
			}
		}
	}

	//public void readyP1()
	//{
	//	p1Ready = true;
	//}
	//public void readyP2()
	//{
	//	p2Ready = true;
	//}
}

﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FieldManagerPVP : FieldManager
{
    [Header("PVP")]
    [SerializeField]public string[] maps;
    public List<Object> Handful_2 = new List<Object>();
	protected List<Object> Temp_2 = new List<Object>();
	protected List<int> TempNum_2 = new List<int>();
	protected static System.Random rand = new System.Random();  
	protected static GameObject[] pauseObjects_p2;
	protected SpellHolder spellHold_2;
	protected List<GameObject> displaySlots_2 = new List<GameObject>();
	protected GameObject runeDisplay_2;
	protected Text runeDamage_2;
	protected Text runeName_2;
	protected Text runeDesc_2;
	public EventSystem ES_P2;
	public bool p1Ready,p2Ready;
	protected GameObject p2Gun;
	protected Text curBullet_2;
	public GameObject[] weapons_2;
	protected static GameObject[] pauseUI_2;
	protected GameObject[] battleObjects_2;
	public bool p1reload;
	public bool p2reload;
	protected GameObject selector_2;

    
    // Use this for initialization
    void Start () 
	{
        mapFile = maps[Random.Range(0, maps.Length)];
        Debug.Log(mapFile);
		once = true;
		weapons = new GameObject[5];
		weapons_2 = new GameObject[5];
		battleObjects = new GameObject[2];
		//Debug.Log (pauseObjects[0]);
		//Hnadful= Deck
		//Pass Deck In from Overworld Scene
		//Placeholder Fils Deck with Lighnin and Eart Spells
		buildDeck();

        instantiateMap();

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		player2 = GameObject.FindGameObjectWithTag ("Player2").GetComponent<Player> ();
		player.basic = true;
		player2.basic = true;

		if(player.weapon == Weapon_Types.Bow)
			player.Chamber = Handful;
		else
			Shuffle(Handful);

		if(player2.weapon == Weapon_Types.Bow)
			player2.Chamber = Handful_2;
		else
			Shuffle(Handful_2);
		
		getUI ();
		updateEnemyList ();
		updateSpellList ();
		updateObstacleList ();

		us = GameObject.Find("__app").GetComponent<UniversalSettings> ();
		mapFile = us.mapfile;
		style = us.pvpStyle;
		if (style == 0)
		{
			pause = false;
			firstPause = false;
		}
		else
		{
			pause = true;
			firstPause = true;
		}

	}

	void Update()
	{
		if (style == 0)
		{
			stationaryUpdate ();
		}
		else
		{
			activeUpdate ();
		}
	}
	// Use this for initialization
	void activeUpdate () 
	{
		//Cooses the proper guns and starts the reload screen simultaneously for the first time
		if (once)
		{
			chooseGun (player.weapon, false);
			chooseGun_2 (player2.weapon, false);
			showReloadScreen (1);
			showReloadScreen (2);
			once = false;
		}

		if (player.health <= 0 || player2.health <=0)
		{
			SceneManager.LoadScene ("Results");
		}

		//Updates the Reload screen UI to reflect the currentl selected bullet
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
		if (ES_P2.currentSelectedGameObject != null)
		{
			if (ES_P2.currentSelectedGameObject.tag == "SpellHolder")
			{
				runeName_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
				runeDamage_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
				//runeDesc_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
				runeDisplay_2.GetComponent<Image> ().sprite = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
				runeDisplay_2.GetComponent<Image> ().color = new Color (0, 0, 0, 255);
				selector_2.transform.position = ES_P2.currentSelectedGameObject.transform.position;
			}
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
				//Shows the battle screen for both players
				if (p1Ready && p2Ready)
				{
					//firstPause = false;
					showBattleScreen (1);
					showBattleScreen (2);
				}
			} 
			else
			{
				//Allows the player to move and shoot when not reloading
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

				//Allows the player back into the reload screen 
				if (!p1reload && Input.GetButtonDown("Start_P1") && player.reload)
					showReloadScreen (1);
				if(!p2reload && Input.GetButtonDown("Start_P2") && player2.reload)
					showReloadScreen (2);
			}

			if(Temp.Count == weaponMax)
				ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));
			if(Temp_2.Count == weaponMax_2)
				ES_P2.SetSelectedGameObject(GameObject.Find("BattleButton_2"));
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
			for (int i = 0; i < weaponMax; i++)
			{
				displaySlots[i].GetComponent<Image>().sprite = defaultSlot;
				displaySlots[i].GetComponent<Image>().color = Color.white;

				spellSlots[i].GetComponent<Image>().sprite = defaultSlot;
				spellSlots[i].GetComponent<Image>().color = Color.white;
			}
			if (player.weapon != Weapon_Types.Bow)
			{
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
			foreach (GameObject d in displaySlots)
			{
				d.SetActive (false);
			}
			for (int i = 0; i < weaponMax; i++)
			{
				displaySlots [i].SetActive (true);
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
					//r.runeDesc = curSpell.GetComponent<Spell> ().description;
				}
				if (player.weapon == Weapon_Types.Bow)
				{
					b.interactable = false;
					ES_P1.SetSelectedGameObject (GameObject.Find("BattleButton"));
				}
			}
		}
		else
		{
			p2reload = true;
			for (int i = 0; i < weaponMax_2; i++)
			{
				displaySlots_2[i].GetComponent<Image>().sprite = defaultSlot;
				displaySlots_2[i].GetComponent<Image>().color = Color.white;

				spellSlots_2[i].GetComponent<Image>().sprite = defaultSlot;
				spellSlots_2[i].GetComponent<Image>().color = Color.white;
			}
			if (player2.weapon != Weapon_Types.Bow)
			{
				for (int i = Temp_2.Count - 1; i > -1; i--)
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
			foreach (GameObject d in displaySlots_2)
			{
				d.SetActive (false);
			}
			for (int i = 0; i < weaponMax_2; i++)
			{
				displaySlots_2 [i].SetActive (true);
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
					//r.runeDesc = curSpell.GetComponent<Spell> ().description;
				}
				if (player2.weapon == Weapon_Types.Bow)
				{
					b.interactable = false;
					ES_P2.SetSelectedGameObject (GameObject.Find("BattleButton"));
				}
			}
		}
	}
	// Update is called once per frame
	void stationaryUpdate () 
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
			SceneManager.LoadScene ("Results");
		}		
		if (pause)
		{
			if (ES_P1.currentSelectedGameObject != null)
			{
				if (ES_P1.currentSelectedGameObject.tag == "SpellHolder")
				{
					runeName.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
					runeDamage.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
					//runeDesc.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
					runeDisplay.GetComponent<Image> ().sprite = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
					runeDisplay.GetComponent<Image> ().color = new Color(0,0,0,255);
					selector.transform.position = ES_P1.currentSelectedGameObject.transform.position;
				}
			}
			if (ES_P2.currentSelectedGameObject != null)
			{
				if (ES_P2.currentSelectedGameObject.tag == "SpellHolder")
				{
					runeName_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
					runeDamage_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
					//runeDesc_2.text = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
					runeDisplay_2.GetComponent<Image> ().sprite = ES_P2.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
					runeDisplay_2.GetComponent<Image> ().color = new Color(0,0,0,255);
					selector_2.transform.position = ES_P2.currentSelectedGameObject.transform.position;
				}
			}
		}
		if (player.weapon != Weapon_Types.Bow)
		{
			if (pause && Input.GetButtonDown ("Cancel_P1"))
			{
				if (Temp.Count > 0)
				{
					removeBullet ();
				}
			}
		}
		if (player2.weapon != Weapon_Types.Bow)
		{
			if (pause && Input.GetButtonDown ("Cancel_P2"))
			{
				if (Temp_2.Count > 0)
				{
					removeBullet_P2 ();
				}
			}
		}
		if (pause)
		{
			if(Handful.Count == 0 && Handful_2.Count ==0)
				SceneManager.LoadScene ("Results");

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
			if(Temp.Count == weaponMax)
				ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));
			if(Temp_2.Count == weaponMax_2)
				ES_P2.SetSelectedGameObject(GameObject.Find("BattleButton_2"));
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
		for (int i = 0; i < weaponMax; i++)
		{
			displaySlots[i].GetComponent<Image>().sprite = defaultSlot;
			displaySlots[i].GetComponent<Image>().color = Color.white;

			spellSlots[i].GetComponent<Image>().sprite = defaultSlot;
			spellSlots[i].GetComponent<Image>().color = Color.white;
		}
		for (int i = 0; i < weaponMax_2; i++)
		{
			displaySlots_2[i].GetComponent<Image>().sprite = defaultSlot;
			displaySlots_2[i].GetComponent<Image>().color = Color.white;

			spellSlots_2[i].GetComponent<Image>().sprite = defaultSlot;
			spellSlots_2[i].GetComponent<Image>().color = Color.white;
		}
		if (player.weapon != Weapon_Types.Bow)
		{
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
		if (player2.weapon != Weapon_Types.Bow)
		{
			for (int i = Temp_2.Count - 1; i > -1; i--)
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
		for (int i = 0; i< battleObjects_2.Length;i++)
		{
			if(battleObjects_2[i] != null)
				battleObjects_2 [i].SetActive (false);
		}
		for (int i = 0; i< pauseUI.Length;i++)
		{
			pauseUI [i].SetActive (true);
		}
		for (int i = 0; i< pauseUI_2.Length;i++)
		{
			pauseUI_2 [i].SetActive (true);
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
				//r.runeDesc = curSpell.GetComponent<Spell> ().description;
			}
			if (player.weapon == Weapon_Types.Bow)
			{
				b.interactable = false;
				ES_P1.SetSelectedGameObject (GameObject.Find("BattleButton"));
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
				//r.runeDesc = curSpell.GetComponent<Spell> ().description;
			}
			if (player2.weapon == Weapon_Types.Bow)
			{
				b.interactable = false;
				ES_P2.SetSelectedGameObject (GameObject.Find("BattleButton"));
			}
		}
		pause = true;

		player.reload = false;
		player2.reload = false;
	}
	public void showBattleScreen()
	{
		if (player.weapon != Weapon_Types.Bow)
		{
			for (int i = 0; i < Temp.Count; i++)
			{
				player.Chamber.Add (Temp [i]);
			}
		}
		foreach (GameObject g in pauseObjects)
		{
			g.SetActive (false);
		}
		if (player2.weapon != Weapon_Types.Bow)
		{
			for (int i = 0; i < Temp_2.Count; i++)
			{
				player2.Chamber.Add (Temp_2 [i]);
			}
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
		for (int i = 0; i< pauseUI_2.Length;i++)
		{
			pauseUI_2 [i].SetActive (false);
		}
		pause = false;
		player.reload = false;
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
			if (player.Chamber.Count > weaponMax)
			{
				for (int i = player.Chamber.Count - 1; i >= weaponMax; i--)
				{
					player.Chamber.RemoveAt (i);
				}
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
			if (player2.Chamber.Count > weaponMax_2)
			{
				for (int i = player2.Chamber.Count - 1; i >= weaponMax_2; i--)
				{
					player2.Chamber.RemoveAt (i);
				}
			}
			pause = false;
			firstPause = false;
		}
	}

	//Update the spell images
	public void updateChamberImages(int num)
	{
		if (num == 1)
		{
			Debug.Log ("lp");
			base.updateChamberImages (num);
		}
		else
		{
			Debug.Log ("elp");
			for (int i = 0; i < weaponMax_2; i++)
			{
				if (player2.Chamber.Count > i)
				{
					Image slot = spellSlots_2 [i].GetComponent<Image> ();
					Sprite rune = ((GameObject)Resources.Load (player2.Chamber [i].name)).GetComponent<Spell> ().bulletImage;
					if (rune != null)
					{
						slot.sprite = rune;
					}
				}
				else
				{
					Image slot = spellSlots_2 [i].GetComponent<Image> ();
					slot.sprite = defaultSlot;
				}
			}
		}
	}

	//Adds a bullet ot te selected list and prevents it from being s;lected again -P2
	protected void addBullet_2(int num)
	{

		if (Temp_2.Count < weaponMax_2)
		{
			Temp_2.Add (Handful_2 [num]);
			Image slot = spellSlots_2 [Temp_2.Count - 1].GetComponent<Image> ();
			Image rune = spellHold_2.children [num].gameObject.GetComponent<Image> ();
			Image slot2 = displaySlots_2 [Temp_2.Count - 1].GetComponent<Image> ();
			slot.sprite = rune.sprite;
			slot.color = rune.color;
			slot2.sprite = rune.sprite;
			slot2.color = rune.color;
			spellHold_2.deactivateSpell ("Spell " + num + "_2");
			TempNum_2.Add (num);
			selectButton_2 ();
			//if(player2.weapon == 1)
			//	p2Gun.transform.Rotate (new Vector3 (0.0f, 0.0f, 60.0f));
			//else if (player2.weapon == 2 || player2.weapon == 4)
			//	p2Gun.transform.Rotate (new Vector3 (0.0f,0.0f,45.0f));
			
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

	//removes te bullet from the prospective selection -P2
	protected void removeBullet_P2()
	{
		spellHold_2.activateSpell ("Spell " +TempNum_2[TempNum_2.Count-1]+ "_2");
		spellSlots_2[Temp_2.Count-1].GetComponent<Image>().sprite = defaultSlot;
		spellSlots_2[Temp_2.Count-1].GetComponent<Image>().color = Color.white;
		Temp_2.RemoveAt (Temp_2.Count - 1);
		TempNum_2.RemoveAt (TempNum_2.Count - 1);
		//if(player2.weapon == 1)
		//	p2Gun.transform.Rotate (new Vector3 (0.0f, 0.0f, -60.0f));
		//else if (player2.weapon == 2 || player2.weapon == 4)
		//	p2Gun.transform.Rotate (new Vector3 (0.0f,0.0f,-45.0f));
		//p2Gun.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z - 60), Time.time*0.1f);

	}

	//Selects the first button that has not been chosen when the count is under the max. -P2
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
	//Sets the player to ready and starts it if the other player is also ready.
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

	//Chooses the Proper guns for the character and activates the cooresponding UI
	public void getUI()
	{
		base.getUI ();
		chooseGun_2 (player2.weapon, true);
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
		pauseUI_2 = GameObject.FindGameObjectsWithTag ("PauseUI_P2");

		runeDisplay_2 = GameObject.Find ("RuneHolder_2");
		runeDamage_2 = GameObject.Find ("RuneDamage_2").GetComponent<Text>();
		runeName_2 = GameObject.Find ("Rune Name_2").GetComponent<Text>();
		selector_2 = GameObject.Find ("Selected Spell_2");

		//battleObjects[1] = null;
		battleObjects = new GameObject[1];
		battleObjects[0] = GameObject.Find("Current Bullet");
		battleObjects_2 = new GameObject[1];
		battleObjects_2[0] = GameObject.Find("Current Bullet_2");


		displaySlots_2.Add (GameObject.Find ("SpellSlot1_2"));
		displaySlots_2.Add (GameObject.Find ("SpellSlot2_2"));
		displaySlots_2.Add (GameObject.Find ("SpellSlot3_2"));
		displaySlots_2.Add (GameObject.Find ("SpellSlot4_2"));
		displaySlots_2.Add (GameObject.Find ("SpellSlot5_2"));
		displaySlots_2.Add (GameObject.Find ("SpellSlot6_2"));
		displaySlots_2.Add (GameObject.Find ("SpellSlot7_2"));
		displaySlots_2.Add (GameObject.Find ("SpellSlot8_2"));
	}
	protected void buildDeck()
	{
		for (int i = 0; i < 10; i++)
		{
			Handful.Add(Resources.Load ("Chains"));
			Handful.Add(Resources.Load ("Fire"));
			Handful.Add(Resources.Load ("Water"));
			Handful.Add(Resources.Load ("Earth"));
			Handful.Add(Resources.Load ("Fire"));
			Handful.Add(Resources.Load ("Boomerang"));
			Handful.Add(Resources.Load ("Lightning"));
		}

		//Placeholder Fils Deck with Lighnin and Eart Spells
		for (int i = 0; i < 10; i++)
		{
			Handful_2.Add(Resources.Load ("Chains"));
			Handful_2.Add(Resources.Load ("Wind"));
			Handful_2.Add(Resources.Load ("Water"));
			Handful_2.Add(Resources.Load ("Earth"));
			Handful_2.Add(Resources.Load ("Fire"));
			Handful.Add(Resources.Load ("Boomerang"));
			Handful.Add(Resources.Load ("Lightning"));
		}
	}
	public void chooseGun_2 (Weapon_Types weapon, bool first)
	{
		if (first)
		{
			weapons_2 [0] = GameObject.Find ("UI_GunCylinder_2");
			weapons_2 [1] = GameObject.Find ("8 Rifle_2");
			weapons_2 [2] = GameObject.Find ("4 Shot Gun_2");
			weapons_2 [3] = GameObject.Find ("2 Shot Gun_2");
			weapons_2 [4] = GameObject.Find ("Bow_2");
		}
		else
		{
			pauseUI = GameObject.FindGameObjectsWithTag ("PauseUI");
			pauseUI_2 = GameObject.FindGameObjectsWithTag ("PauseUI_P2");
		}

		for (int i = 0; i < weapons_2.Length; i++)
		{
			if(weapons_2[i] != null)
				weapons_2 [i].SetActive (false);
		}
		Debug.Log(weapons_2[2]);
		switch (weapon)
		{
			case Weapon_Types.Revolver:
				weapons_2 [0].SetActive (true);
				spellSlots_2[0] = GameObject.Find ("ChamberSlot1_2");
				spellSlots_2[1] = GameObject.Find ("ChamberSlot2_2");
				spellSlots_2[2] = GameObject.Find ("ChamberSlot3_2");
				spellSlots_2[3] = GameObject.Find ("ChamberSlot4_2");
				spellSlots_2[4] = GameObject.Find ("ChamberSlot5_2");
				spellSlots_2[5] = GameObject.Find ("ChamberSlot6_2");
				p2Gun = weapons_2 [0];
				weaponMax_2 = 6;
			break;
			case Weapon_Types.Rifle:
				weapons_2 [1].SetActive (true);
				spellSlots_2[0] = GameObject.Find ("ChamberSlot1_2");
				spellSlots_2[1] = GameObject.Find ("ChamberSlot2_2");
				spellSlots_2[2] = GameObject.Find ("ChamberSlot3_2");
				spellSlots_2[3] = GameObject.Find ("ChamberSlot4_2");
				spellSlots_2[4] = GameObject.Find ("ChamberSlot5_2");
				spellSlots_2[5] = GameObject.Find ("ChamberSlot6_2");
				spellSlots_2[6] = GameObject.Find ("ChamberSlot7_2");
				spellSlots_2[7] = GameObject.Find ("ChamberSlot8_2");
				p2Gun = weapons_2 [1];
				weaponMax_2 = 8;
			break;
			case Weapon_Types.Shotgun:
				weapons_2[2].SetActive (true);
				spellSlots_2[0] = GameObject.Find ("ChamberSlot1_2");
				spellSlots_2[1] = GameObject.Find ("ChamberSlot2_2");
				spellSlots_2 [2] = GameObject.Find ("ChamberSlot3_2");
				spellSlots_2 [3] = GameObject.Find ("ChamberSlot4_2");
				p2Gun = weapons_2[2];
				weaponMax_2 = 4;
			break;
			case Weapon_Types.Gatling:
				weapons_2 [1].SetActive (true);
				spellSlots_2[0] = GameObject.Find ("ChamberSlot1_2");
				spellSlots_2[1] = GameObject.Find ("ChamberSlot2_2");
				spellSlots_2[2] = GameObject.Find ("ChamberSlot3_2");
				spellSlots_2[3] = GameObject.Find ("ChamberSlot4_2");
				spellSlots_2[4] = GameObject.Find ("ChamberSlot5_2");
				spellSlots_2[5] = GameObject.Find ("ChamberSlot6_2");
				spellSlots_2[6] = GameObject.Find ("ChamberSlot7_2");
				spellSlots_2[7] = GameObject.Find ("ChamberSlot8_2");
				p2Gun = weapons_2 [1];
				weaponMax_2 = 8;

			break;
			case Weapon_Types.Canegun:

			break;
			case Weapon_Types.Bow:
				weapons_2[4].SetActive (true);
				p2Gun = weapons_2[4];
			break;

		}
		player2.updatePlayerImage ();

	}
}
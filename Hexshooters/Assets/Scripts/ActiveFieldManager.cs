﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ActiveFieldManager : FieldManager {
	void Start()
	{
		base.Start();
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
			SceneManager.LoadScene ("Overworld");
		}
		//updateHealth ();
		if (ES_P1.currentSelectedGameObject != null)
		{
			if (ES_P1.currentSelectedGameObject.tag == "SpellHolder")
			{
				runeName.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeName;
				runeDamage.text = "Damage:" + ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDamage;
				runeDesc.text = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeDesc;
				runeDisplay.GetComponent<Image> ().sprite = ES_P1.currentSelectedGameObject.GetComponent<RuneInfo> ().runeImage;
				runeDisplay.GetComponent<Image> ().color = new Color (0, 0, 0, 255);
			}
		}

		if (pause)
		{
			if (Input.GetButtonDown ("Cancel_Solo"))
			{
				if (Temp.Count > 0)
				{
					removeBullet ();
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
}

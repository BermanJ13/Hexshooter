using UnityEngine;
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

		if (pause)
		{
			if (Input.GetButtonDown ("Cancel_Solo"))
			{
				if (Temp.Count > 0)
				{
					removeBullet ();
				}
			}
			if (firstPause)
			{
					
			} 
			else
			{
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

			if ( player.reload && Input.GetButtonDown("Start_Solo"))
			{
				showReloadScreen ();
				if (player.Chamber.Count > 0)
				{
					for( int i = 0; i < player.Chamber.Count; i ++ )
					{
						Temp.Add (player.Chamber[i]);
						Image slot = spellSlots [Temp.Count - 1].GetComponent<Image> ();

						slot.sprite = ((GameObject)player.Chamber[i]).GetComponent<Spell> ().bulletImage;
						//slot.color = rune.color;
						TempNum.Add (100);
						selectButton ();
						p1Gun.transform.Rotate (new Vector3 (0.0f,0.0f,60.0f));
						if(Temp.Count == 6)
							ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));

					}
					player.Chamber = new List<Object>();
				}
			}
		}
	}
}

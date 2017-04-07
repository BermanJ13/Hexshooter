using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerRefocus_Multiplayer : MonoBehaviour 
{
	public EventSystem ES_P1;
	public EventSystem ES_P2;

	void Update()
	{
		//Refocus Player 1 Event System 
		if (ES_P1.currentSelectedGameObject == null)
		{
			ES_P1.SetSelectedGameObject(ES_P1.firstSelectedGameObject);
		}

		if (ES_P1.currentSelectedGameObject == GameObject.Find("Spell 0_2"))
		{
			ES_P1.SetSelectedGameObject(GameObject.Find("Spell 4"));
		}
		if (ES_P1.currentSelectedGameObject == GameObject.Find("Spell 5_2"))
		{
			ES_P1.SetSelectedGameObject(GameObject.Find("Spell 9"));
		}
		if (ES_P1.currentSelectedGameObject == GameObject.Find("BattleButton_2"))
		{
			ES_P1.SetSelectedGameObject(GameObject.Find("BattleButton"));
		}

		//Refocus Player 2 Event System 
		if (ES_P2.currentSelectedGameObject == null)
		{
			ES_P2.SetSelectedGameObject(ES_P2.firstSelectedGameObject);
		}

		if (ES_P2.currentSelectedGameObject == GameObject.Find("Spell 4"))
		{
			ES_P2.SetSelectedGameObject(GameObject.Find("Spell 0_2"));
		}
		if (ES_P2.currentSelectedGameObject == GameObject.Find("Spell 9"))
		{
			ES_P2.SetSelectedGameObject(GameObject.Find("Spell 5_2"));
		}
		if (ES_P2.currentSelectedGameObject == GameObject.Find("BattleButton"))
		{
			ES_P2.SetSelectedGameObject(GameObject.Find("BattleButton_2"));
		}
	}
}
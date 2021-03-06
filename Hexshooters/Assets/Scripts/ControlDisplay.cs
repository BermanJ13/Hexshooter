﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ControlDisplay : MonoBehaviour {
	public GameObject [] ControlScreens = new GameObject[4];
	public GameObject ControlBackground;
	// Use this for initialization
	void Start () {
		ControlScreens [0] = GameObject.Find ("C1");
		ControlScreens [1] = GameObject.Find ("C2");
		ControlScreens [2] = GameObject.Find ("C3");
		ControlScreens [3] = GameObject.Find ("C4");
		ControlBackground = GameObject.Find ("CBackground");

		for(int i = 0; i <ControlScreens.Length;i++)
		{
			ControlScreens [i].SetActive (false);
		}
		ControlBackground.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("ControlsButton"))
			showControls ();
		else
			hideControls ();
	}

	void showControls()
	{
		string s = SceneManager.GetActiveScene ().name;

		switch (s)
		{
			case "PVP Character Select":
			case "PvP":
			case "Active PVP":
			case "Active PVP Select":
			case "Results":
				ControlScreens [0].SetActive (true);
				ControlScreens [3].SetActive (true);
			break;
				
			case "Active Battle":
			case "Battle":
			case "Battle1":
			case "Battle2":
			case "Battle3":
			case "Battle4":
			case "Battle5":
			case "Game Over":
			case "Win":
			case "Overworld":
				ControlScreens [1].SetActive (true);
				ControlScreens [3].SetActive (true);
			break;
				
			case "Menu":
				ControlScreens [1].SetActive (true);
				ControlScreens [3].SetActive (true);
			break;
		}
		ControlBackground.SetActive (true);

	}
	void hideControls()
	{
		for(int i = 0; i <ControlScreens.Length;i++)
		{
			if(ControlScreens[i] != null)
				ControlScreens [i].SetActive (false);
		}
		if(ControlBackground != null)
			ControlBackground.SetActive (false);
	}
}

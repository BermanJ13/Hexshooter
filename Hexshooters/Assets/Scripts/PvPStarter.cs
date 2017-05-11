using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;

public class PvPStarter : MonoBehaviour {
	public GameObject[] menuObjects;
	public GameObject[] pvpMenuObjects;
	public bool menu;
	public bool pvpMenu;
	public bool change;
	public EventSystem ES;
	// Use this for initialization
	void Start () 
	{
		ES = EventSystem.current;
		menuObjects = GameObject.FindGameObjectsWithTag ("Menu");
		pvpMenuObjects = GameObject.FindGameObjectsWithTag ("PVP Menu");
		menu = true; 
		pvpMenu = false;
		change = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (menu)
		{
			foreach (GameObject g in menuObjects)
			{
				g.SetActive (true);
			}
			foreach (GameObject g in pvpMenuObjects)
			{
				g.SetActive (false);
			}
			if (change)
			{
				ES.SetSelectedGameObject (GameObject.Find ("Over"));
				change = false;
			}
		}
		if(pvpMenu)
		{
			foreach (GameObject g in menuObjects)
			{
				g.SetActive (false);
			}
			foreach (GameObject g in pvpMenuObjects)
			{
				g.SetActive (true);
			}
			if (change)
			{
				ES.SetSelectedGameObject (GameObject.Find ("Active"));
				change = false;
			}
		}
	}
	public void toCharacterSelect()
	{
		SceneManager.LoadScene ("PvP Character Select");
	}
	public void toActiveCharacterSelect()
	{
		SceneManager.LoadScene ("Active PVP Select");
	}
	public void toPvP()
	{
		SceneManager.LoadScene ("PVP");
	}
	public void toActivePvP()
	{
		SceneManager.LoadScene ("Active PVP");
	}
	public void toActive()
	{
		SceneManager.LoadScene ("Active Battle");
	}
	public void toStationary()
	{
		SceneManager.LoadScene ("Battle");
	}
	public void toMenu()
	{
		SceneManager.LoadScene ("Menu");
	}
	public void toOptions()
	{
		SceneManager.LoadScene ("Options");
	}
	public void toCredits()
	{
		SceneManager.LoadScene ("Credits");
	}
	public void toPreLoad()
	{
		GameObject op = GameObject.Find("OverPlayer");
		if (op != null)
			Destroy (op);
		if (GameObject.Find ("__app") != null)
			Destroy (GameObject.Find ("__app"));
		
		SceneManager.LoadScene ("preload");
	}
	public void toOver()
	{
		
        OverPlayer op = GameObject.FindObjectOfType<OverPlayer>();
        op.enabled = true;
        SceneManager.LoadScene ("Overworld");
	}
	public void quitGame()
	{
		Application.Quit();
	}
	public void toPvPMenu()
	{
		menu = false;
		pvpMenu = true;
		change = true;
	}
	public void toMainMenu()
	{
		pvpMenu = false;
		menu = true;
		change = true;
	}
}

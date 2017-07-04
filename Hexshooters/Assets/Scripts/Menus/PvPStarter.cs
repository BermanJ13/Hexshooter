using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum Menus
{
	Main,
	PvP,
	Options,
	Credits,
	CharSelect,
	Win
}
public class PvPStarter : MonoBehaviour {
	public GameObject[] menuObjects;
	public GameObject[] pvpMenuObjects;
	public GameObject[] opMenuObjects;
	public GameObject[] creditsMenuObjects;
	public GameObject[] charMenuObjects;
	public GameObject[] extraMenuObjects;
	public Menus menu;
	public bool pvpMenu;
	public bool change;
	public EventSystem ES;
	UniversalSettings us;
	Text filterDisplay;
	Text volumeDisplay;
	Text fullScreenDisplay;
	Text playStyleDisplay;
	public bool changeVolume;
	bool firstpress = true;
	private bool xAxisInUse = false;
	bool selectChoice = false;
	OLDTVTube otv;
	// Use this for initialization
	void Start () 
	{
		ES = EventSystem.current;
		menuObjects = GameObject.FindGameObjectsWithTag ("Menu");
		pvpMenuObjects = GameObject.FindGameObjectsWithTag ("PVP Menu");
		opMenuObjects = GameObject.FindGameObjectsWithTag ("Options Menu");
		creditsMenuObjects = GameObject.FindGameObjectsWithTag ("Credits");
		charMenuObjects = GameObject.FindGameObjectsWithTag ("CharSelect");
		extraMenuObjects = GameObject.FindGameObjectsWithTag ("ExtraMenu");
		if(SceneManager.GetActiveScene().name != "Win" &&SceneManager.GetActiveScene().name != "Results" && SceneManager.GetActiveScene().name != "Game Over")
			menu = Menus.Main; 
		pvpMenu = false;
		change = true;
		us = GameObject.Find("__app").GetComponent<UniversalSettings> ();
		filterDisplay = GameObject.Find("Filter Display").GetComponent<Text> ();
		volumeDisplay = GameObject.Find("Volume Display").GetComponent<Text> ();
		fullScreenDisplay = GameObject.Find("Full Screen Display").GetComponent<Text> ();
		playStyleDisplay = GameObject.Find("Style Display").GetComponent<Text> ();
		otv = GameObject.Find ("Main Camera").GetComponent<OLDTVTube> ();
		changeVolume = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//[main,pvp,options,credits,character select, Quit/Options buttons]
		if (menu == Menus.Main)
		{
			bool[] menuSettings = new bool[] {true, false, false, false, false, true};
			menuChanger (menuSettings);
		}
		if (menu == Menus.PvP)
		{
			bool[] menuSettings = new bool[] {false, true, false, false, false, true};
			menuChanger (menuSettings);
		}
		if (menu == Menus.Options)
		{
			bool[] menuSettings = new bool[] {false, false, true, false, false, true};
			menuChanger (menuSettings);
			opUpdate ();
		}
		if (menu == Menus.Credits)
		{
			bool[] menuSettings = new bool[] {false, false, false, true, false, true}; 
			menuChanger (menuSettings);
		}
		if (menu == Menus.CharSelect)
		{
			bool[] menuSettings = new bool[] {false, false, false, false, true, false};
			menuChanger (menuSettings);
		}
		if (menu == Menus.Win)
		{
			ES.SetSelectedGameObject (GameObject.Find ("Home"));
		}
	}
	public void menuChanger(bool[] settings)
	{

		if (change)
		{
			foreach (GameObject g in menuObjects)
			{
				g.SetActive (settings[0]);
				if(settings[0])
					ES.SetSelectedGameObject (GameObject.Find ("Over"));
			}
			foreach (GameObject g in pvpMenuObjects)
			{
				g.SetActive (settings[1]);
				if(settings[1])
					ES.SetSelectedGameObject (GameObject.Find ("Active"));
			}
			foreach (GameObject g in opMenuObjects)
			{
				g.SetActive (settings[2]);
				if(settings[2])
					ES.SetSelectedGameObject (GameObject.Find ("Filter Button"));
			}
			foreach (GameObject g in creditsMenuObjects)
			{
				g.SetActive (settings[3]);
				if(settings[3])
					ES.SetSelectedGameObject (GameObject.Find ("Credits Main"));
			}
			foreach (GameObject g in charMenuObjects)
			{
				g.SetActive (settings[4]);
				if(settings[4])
					ES.SetSelectedGameObject (GameObject.Find ("Continue"));
			}
			foreach (GameObject g in extraMenuObjects)
			{
				g.SetActive (settings[5]);
			}
			change = false;
		}
	}
	public void toCharacterSelect()
	{
		us.pvpStyle = 0;
		menu = Menus.CharSelect;
		change = true;
	}
	public void toActiveCharacterSelect()
	{
		us.pvpStyle = 1;
		menu = Menus.CharSelect;
		change = true;
	}
	public void toPvP()
	{
			SceneManager.LoadScene ("PVP");
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
	public void toCredits()
	{
		menu = Menus.Credits;
		change = true;
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
		menu = Menus.PvP;
		change = true;
	}
	public void toMainMenu()
	{
		menu = Menus.Main;
		change = true;
	}
	public void toOptions()
	{
		menu = Menus.Options;
		change = true;
	}
	public void updatePlayStyle()
	{
		if(us.style == 0)
		{
			us.style = 1;
		}
		else if(us.style == 1)
		{
			us.style = 0;
		}
	}
	public void updateFullScreen()
	{
		if(Screen.fullScreen)
		{
			Screen.fullScreen = false;
		}
		else
		{
			Screen.fullScreen = true;
		}
	}
	public void updateFilter()
	{
		us.filter = !us.filter;
	}
	public void updateVolume()
	{
		changeVolume = true;
	}
	public void volumeChanger()
	{
		EventSystem.current.SetSelectedGameObject(null);

		if (us.volume < 0)
		{   
			us.volume = 0;
		}   
		if (us.volume > 1)
		{   
			us.volume = 1;
		}
		if (Input.GetAxisRaw ("Horizontal_Solo") > 0)
		{
			if (!xAxisInUse)
			{
				xAxisInUse = true;
				us.volume += 0.1f;
			}
		}
		else
			if (Input.GetAxisRaw ("Horizontal_Solo") < 0)
			{
				if (!xAxisInUse)
				{
					xAxisInUse = true;
					us.volume -= 0.1f;
				}
			}
		if (!firstpress)
		{
			if (Input.GetButtonDown ("Submit_Solo"))
			{
				changeVolume = false;
				EventSystem.current.SetSelectedGameObject (GameObject.Find("Filter Button"));
				firstpress = true;
				selectChoice = true;
			}
		}
		if (Input.GetAxisRaw ("Horizontal_Solo") == 0)
		{
			xAxisInUse = false;
		}
		firstpress = false;
	}
	void opUpdate () 
	{
		if (Screen.fullScreen)
			fullScreenDisplay.text = "On";
		else
			fullScreenDisplay.text = "Off";

		volumeDisplay.text = Mathf.Round((AudioListener.volume*10)).ToString();

		if (changeVolume)
		{
			volumeChanger ();
		}
		else
		{
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				EventSystem.current.SetSelectedGameObject (EventSystem.current.firstSelectedGameObject);
			}
		}

		if(us.filter)
			filterDisplay.text = "On";
		else
			filterDisplay.text = "Off";

		if(us.style == 0)
			playStyleDisplay.text = "Stationary";
		else
			playStyleDisplay.text = "Active";

		if (selectChoice)
		{
			firstpress = true;
			selectChoice = false;
		}


	}
}

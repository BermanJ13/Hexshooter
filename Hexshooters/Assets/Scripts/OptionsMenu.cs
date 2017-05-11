using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour {
	Text filterDisplay;
	Text volumeDisplay;
	Text fullScreenDisplay;
	Text playStyleDisplay;
	public bool changeVolume;
	bool firstpress = true;
	private bool xAxisInUse = false;
	bool selectChoice = false;
	OLDTVTube otv;
	UniversalSettings us;
	// Use this for initialization
	void Start () 
	{
		filterDisplay = GameObject.Find("Filter Display").GetComponent<Text> ();
		volumeDisplay = GameObject.Find("Volume Display").GetComponent<Text> ();
		fullScreenDisplay = GameObject.Find("Full Screen Display").GetComponent<Text> ();
		playStyleDisplay = GameObject.Find("Style Display").GetComponent<Text> ();
		otv = GameObject.Find ("Main Camera").GetComponent<OLDTVTube> ();
		us = GameObject.Find("__app").GetComponent<UniversalSettings> ();
		changeVolume = false;
	}
	
	// Update is called once per frame
	void Update () 
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
				EventSystem.current.SetSelectedGameObject (EventSystem.current.firstSelectedGameObject);
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
}

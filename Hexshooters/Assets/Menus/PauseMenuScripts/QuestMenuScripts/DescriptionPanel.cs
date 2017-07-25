using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanel : MonoBehaviour {

	public sampleQuestButton quest; //the quest button
	public Text infomation; //description to be displayed about quest
	//public Image sender; //person after/sent you the quest?
	//background panel image that changes depending on type of quest
	Image myImageCompenent; //this panel's image compenent
	public Sprite help;
	public Sprite wanted;
	public Sprite missing;

	// Use this for initialization
	void Start () 
	{
		myImageCompenent = GetComponent<Image>(); //Our image conponent attached to the description panel
	}
	
	// Update is called once per frame
	/*
	void Update () 
	{
		//checks to see what background to display first
		if(quest.type == "help")
		{
			myImageCompenent.sprite = help;
		}
		if(quest.type == "wanted")
		{
			myImageCompenent.sprite = wanted;
		}
		if(quest.type == "missing")
		{
			myImageCompenent.sprite = missing;
			
		}
		//displays question infomation stored
		infomation = quest.description;
		//image info?
		//idk what to do

	}
	*/
			
}

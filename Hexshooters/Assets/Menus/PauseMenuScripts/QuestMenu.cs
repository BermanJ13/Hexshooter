using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMenu : MonoBehaviour {
	//used for deciding which Pause Menu Screen to Display;
	public GameObject closedCanvas; //the main pause menu panel of when the journal is closed
	public GameObject deckCanvas; //the deck building menu of when the journal is open to deck menu
	public GameObject questCanvas; //the quest menu of the journal where player can see all the quests done/doing
	public GameObject logCanvas; //log  Canvas
	public GameObject invenCanvas; //inventory canvas
	public GameObject checkQuitCanvas; //asking if the player is sure if want to quit
	public Text questNames; //list of quest names displayed
	public Text description; //description of said quest

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}
		
	/*
     * different menu button code 
     * */

	//Paramters: None
	//Purpose: Takes the player to main pause menu
	//Known Errors: None
	public void ToClosedBook()
	{
		//In Theory only need to disable the main pause menu and active the deck Menu
		//turns off title canvas and turns on level select canvas
		deckCanvas.SetActive(false);
		closedCanvas.SetActive(true);
	}
	//Paramters: None
	//Purpose: Takes the player to the list of Quest Section of the Menu
	//Known Errors: None
	public void ToQuests()
	{
		//In Theory only need to disable the main pause menu and active the deck Menu
		//turns off title canvas and turns on level select canvas
		deckCanvas.SetActive(false);
		questCanvas.SetActive(true);
	}

	//Paramters: None
	//Purpose: Takes the player to the list of Log Section of the Menu
	//Known Errors: None
	public void ToLog()
	{
		//In Theory only need to disable the main pause menu and active the deck Menu
		//turns off title canvas and turns on level select canvas
		deckCanvas.SetActive(false);
		questCanvas.SetActive(true);
	}

	//Paramters: None
	//Purpose: Takes the player to the list of inventory Section of the Menu
	//Known Errors: None
	public void ToInven()
	{
		//In Theory only need to disable the main pause menu and active the deck Menu
		//turns off title canvas and turns on level select canvas
		deckCanvas.SetActive(false);
		questCanvas.SetActive(true);
	}


	//Parameters: list of quests
	//Purpose: Display the list of quests on left side of the book
	//where players can then scroll down through
	//Known errors: None
	/*
	public void ListofQuest(List<object>QuestList)
	{
		//list for reorganzation of quest by location 
		List<object> displayList - new List<object>();
		//goes through each quest object by town and adds to displayquest in order
		sortHelper(QuestList,displayList,"townName"); //copy and paste for each town
		//once sorted display the quests, display them
		string temp; //name of quest to be edited black or red depending on complete
		foreach(var quest in displayList)
		{
			questNames = questNames + "quest.title"+ "\n";
		}
		
	}

	//parameters: list for questlist,list for displayList string for town location
	//purpose: goes through each quest and adds to the list by town name
	public void sortHelper(List<object>QuestList, List<object>displayList, string townlocation)
	{
		foreach(var quest in QuestList)
		{
			if(quest.location == townlocation)
			{
				displayList.add(quest);
			}
		}
	}
	*/


}

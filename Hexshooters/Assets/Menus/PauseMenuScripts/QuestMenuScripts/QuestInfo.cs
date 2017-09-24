using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInfo : MonoBehaviour {

	public string title; //title/name of the quest
	public string location; //what town is the quest given from
	public string description; //description of the quest to be displayed in quest menu
	public int typeofQuest; //to help display in quest menu if it's a "help", wanted", or "missing" background
	public bool finished; //if the quest has been completed or not
	public string poster; //NPC Image
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

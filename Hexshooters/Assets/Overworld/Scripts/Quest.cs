using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//to edit in inspector for testing
public class Quest : MonoBehaviour {
	
	public string title; //title/name of the quest
	public string location; //what town is the quest given from
	public string description; //description of the quest to be displayed in quest menu
	public string poster; //NPC Image that requested quest/dude need to hunt
	public int typeofQuest; //to help display in quest menu if it's a "help", wanted", or "missing" background
	public bool rank; //check if it's a rank up/story quest or not
	public bool finished; //if the quest has been completed or not
	public object reward; //reward to be given to the player when done
	public List<Trigger> taskList; //array or list of triggers that need to be done

	// Constructor that takes no arguments or a default quest
	public Quest()
	{
		title= "Finding a Shrubbery";
		location = "Knights that say NI!";
		description = "Find a shruberry to pass the Knoghts that say NI and continue to find the holy grail.";
		typeofQuest = 1;
		rank = false; //not a story quest
		bool finished = true; //you did the task
		reward = null; //no reward
	}

	//Constructor that takes arguements
	public Quest(string name, string place, string describe, string person, int type, bool story, bool done, object present, List<Trigger> toDO)
	{
		title = name;
		location = place;
		description = describe;
		poster = person;
		typeofQuest = type;
		rank = story;
		finished = done;
		reward = present;
		taskList = new List<Trigger> (toDO);
	}

	public string Title
	{
		get{ return this.title;}
	}

	public string Description
	{
		get{ return this.description;}
	}

	public string Location
	{
		get{ return this.location;}
	}

	public string Poster
	{
		get{ return this.poster;}
	}

	public bool Finished
	{
		get{return this.finished; }
	}
	//parameters: string
	//purpose: change name of quest
	//errors: unknonwn atm
	public void changeName(string update)
	{
		title = update;
	}

	//parameters: int
	//purpose: change type of quest if story changes it
	//errors: unknonwn atm
	public void changeType(int type)
	{
		typeofQuest = type;
	}

	//parameters: string
	//purpose: change description of the quest/story so players remember what they are doing
	//errors: unknonwn atm
	public void updateDescription(string update)
	{
		description = update;
	}

	//parameters:none
	//purpose: says the quest is done
	//errors: unknown atm
	public void completed()
	{
		finished = true;	
	}


	public void finishedQuest()
	{
		int count =0;
		int required = taskList.Count;
			
		foreach(Trigger trig in taskList)
		{
			if(trig.interacted)
			{
				count++;
			}
		}
		if(count ==  required)
		{
					Reward();
		}
		//goes through the list of triggers
		//check if all of them are "interacted"
		//if yes change bool finished true
	}



	public void Reward()
	{
		//System.out.println ("Hello World");
		//if finished is true give reward
	}
			
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}	

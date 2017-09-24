using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//to edit in inspector for testing
public class QuestList : MonoBehaviour {

	//location names atm are Idledale, Ragged Bend, Coal Creek atm
	public List<Quest>idledale = new List<Quest>();
	private List<Quest>raggedBend = new List<Quest>();
	private List<Quest>coalCreek = new List<Quest>();
	public List<Quest>all = new List<Quest>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//public access to each of the quests so that the user interface can see them
	public List<Quest> Idledale
	{
		get{ return this.idledale;}
	}

	public List<Quest> RaggedBend
	{
		get{ return this.raggedBend;}
	}

	public List<Quest> CoalCreek
	{
		get{ return this.coalCreek;}
	}

	//Parameters: quest
	//Purpose: adds a quest to the quest list
	//known errors: unknown atm
	public void AddQuest(Quest newQuest)
	{
		//adds the quest into the correct town list
		Quest add = newQuest;
		if (add.Location == "Idledale") 
		{
			Idledale.Add (add);
		}
		if (add.Location == "Ragged Bend")
		{
			raggedBend.Add (add);
		}
		if(add.Location == "Coal Creek")
		{
			coalCreek.Add (add);
		}
	}
}

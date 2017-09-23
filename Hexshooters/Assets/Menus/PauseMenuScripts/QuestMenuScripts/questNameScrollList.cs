using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;


public class questNameScrollList : MonoBehaviour {

	//list of variables needed
	OverPlayer op; //connect it to over player
	public Transform contentPanel; //the panel on left side of the book where buttons are displayed
	public SimpleObjectPool buttonObjectPool; //object pool
	public List<Quest> masterList; //list of quests. *Should probably be private*
	//booleans to update say need to refresh
	public bool questCompleted =false;
	public bool questTaken = false;
	public bool questUpdated = false;
	//things around the quest that needs to be changed depending which one you are hovering
	Text name;
	Text description;
	Image person; //not sure what do to find a specfic image
	GameObject wanted;
	GameObject missing;
	GameObject help;
	//public Quests questList; list of quests for when it is created

	// Use this for initialization
	void Start () {
		//connects the pieces together 
		op = GameObject.Find ("OverPlayer").GetComponent<OverPlayer>();
		name = GameObject.Find ("TownNameText").GetComponent<Text>();
		description = GameObject.Find ("DescriptionText").GetComponent<Text>();
		wanted = GameObject.Find ("Wanted");
		missing = GameObject.Find ("Missing");
		help = GameObject.Find ("Help");
		person = GameObject.Find("PosterNPCImage").GetComponent<Image>();
		SortbyLocation (op.quests);
		RefreshDisplay ();
	}

	void Update()
	{
		
		if(questCompleted || questTaken || questUpdated)
		{
			RefreshDisplay ();
		}
		if(EventSystem.current != null)
		{
			UpdateUI ();
		}

	}

	//Purpose: Refreshes the display
	public void RefreshDisplay()
	{
		questCompleted = false;
		questTaken = false;
		questUpdated = false;
		//AddButtons ();
		AddButtons();
		Transform firstButton = GameObject.Find ("Content").transform;
		EventSystem.current.SetSelectedGameObject (firstButton.GetChild(0).gameObject);
	}

	private void AddButtons()
	{
		Transform buttonHolder = GameObject.Find ("Content").transform;
		foreach (Transform child in buttonHolder.transform) 
		{
			GameObject.Destroy (child.gameObject);
		}

		foreach(Quest quest in masterList)
		{
			GameObject newButton = buttonObjectPool.GetObject ();
			newButton.transform.SetParent (contentPanel);

			sampleQuestButton sampleButton = newButton.GetComponent<sampleQuestButton> ();
			sampleButton.Setup (quest,this);
		}
	}

	private void SortbyLocation(List<Quest> questList)
	{
		int sortCount = 0;

		for (int i = 0; i < questList.Count; i++) 
		{
			if(questList[i].Location=="Ideldale")
			{
				masterList.Add (questList [i]);
			}
		}

		for (int i = 0; i < questList.Count; i++) 
		{
			if(questList[i].Location=="Ragged Bend")
			{
				masterList.Add (questList [i]);
			}
		}

		for (int i = 0; i < questList.Count; i++) 
		{
			if(questList[i].Location=="Coal Creek")
			{
				masterList.Add (questList [i]);
			}
		}
	}

	public void UpdateUI()
	{
		if (EventSystem.current.currentSelectedGameObject != null) 
		{
			name.text = EventSystem.current.currentSelectedGameObject.GetComponent<QuestInfo>().location;
			description.text = EventSystem.current.currentSelectedGameObject.GetComponent<QuestInfo>().description;
			print (EventSystem.current.currentSelectedGameObject.GetComponent<QuestInfo>().poster);
			person.sprite = Resources.Load<Sprite>(EventSystem.current.currentSelectedGameObject.GetComponent<QuestInfo>().poster);

			if (EventSystem.current.currentSelectedGameObject.GetComponent<QuestInfo> ().typeofQuest == 1) 
			{
				wanted.SetActive(true);
				missing.SetActive (false);
				help.SetActive (false);
			}
			else if(EventSystem.current.currentSelectedGameObject.GetComponent<QuestInfo> ().typeofQuest == 2)
			{
				wanted.SetActive(false);
				missing.SetActive (true);
				help.SetActive (false);
			}
			else
			{
				wanted.SetActive(false);
				missing.SetActive (false);
				help.SetActive (true);
			}
		}
	}

}

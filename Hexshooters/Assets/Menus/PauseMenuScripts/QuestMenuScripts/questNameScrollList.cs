using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;


//Purpose: To fill information for the quest buttons
[System.Serializable]//to edit in inspector for testing
public class QuestItemMenu
{
	public string questName; //name of quest
	public bool complete; //completed or not
	public string description; //description of the quest
	public string townName; //town name
	public string type; //type of quest it is "wanted, help, etc"

}

public class questNameScrollList : MonoBehaviour {

	OverPlayer op;
	public List<QuestItemMenu> temptList; 
	public Transform contentPanel;
	public SimpleObjectPool buttonObjectPool;
	//real stuff
	public List<Quest> masterList;
	public bool questCompleted =false;
	public bool questTaken = false;
	public bool questUpdated = false;
	Text name;
	//public Quests questList; list of quests for when it is created

	// Use this for initialization
	void Start () {
		op = GameObject.Find ("OverPlayer").GetComponent<OverPlayer>();
		name = GameObject.Find ("TownNameText").GetComponent<Text>();
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
		RealAddButtons();
		Transform firstButton = GameObject.Find ("Content").transform;
		EventSystem.current.SetSelectedGameObject (firstButton.GetChild(0).gameObject);
	}

	//purpose adds buttons to the questscroll list display on the left hand of the screen
	private void AddButtons()
	{
		//needs to be edited to take the quest list
		//loop through  all items in list and add new quests
		for(int i=0;i<temptList.Count;i++)
		{
			QuestItemMenu quest = temptList [i];
			GameObject newButton = buttonObjectPool.GetObject ();
			newButton.transform.SetParent(contentPanel);

			//tell button to set self up
			sampleQuestButton sampleButton = newButton.GetComponent<sampleQuestButton>();
			sampleButton.Setup (quest, this);
		}
	}

	private void RealAddButtons()
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
			sampleButton.RealSetup (quest,this);
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
		}
	}
}

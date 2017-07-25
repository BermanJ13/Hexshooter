using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



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

	public List<QuestItemMenu> questList; 
	public Transform contentPanel;
	public questNameScrollList description;
	public Text townName;
	public SimpleObjectPool buttonObjectPool;
	//public Quests questList; list of quests for when it is created
	// Use this for initialization
	void Start () {
		RefreshDisplay ();
	}

	//Purpose: Refreshes the display
	public void RefreshDisplay()
	{
		AddButtons ();
	}

	//purpose adds buttons to the questscroll list display on the left hand of the screen
	private void AddButtons()
	{
		//needs to be edited to take the quest list
		//loop through  all items in list and add new quests
		for(int i=0;i<questList.Count;i++)
		{
			QuestItemMenu quest = questList [i];
			GameObject newButton = buttonObjectPool.GetObject ();
			newButton.transform.SetParent(contentPanel);

			//tell button to set self up
			sampleQuestButton sampleButton = newButton.GetComponent<sampleQuestButton>();
			sampleButton.Setup (quest, this);
		}
	}
}

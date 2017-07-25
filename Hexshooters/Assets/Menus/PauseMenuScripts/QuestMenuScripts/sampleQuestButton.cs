using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sampleQuestButton : MonoBehaviour {

	public Button button;
	public Text name;
	public Image compeletion;
	public Sprite done;
	public Sprite notDone;
	public string type;
	public string description;
	public string townName;

	private QuestItemMenu item;
	private questNameScrollList scrollList;
	// Use this for initialization
	void Start () {
		
	}

	public void Setup(QuestItemMenu currentItem, questNameScrollList currentScrollList)
	{
		item = currentItem;
		name.text = item.questName;
		if(item.complete)
		{
			compeletion.sprite = done;
		}
		compeletion.sprite = notDone;
		scrollList = currentScrollList;
	}
}

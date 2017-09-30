﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sampleQuestButton : MonoBehaviour {

	public Button button;
	public Text name;
	public Image compeletion;
	public Sprite done;
	public Sprite notDone;
	public int type;

	private questNameScrollList scrollList;

	//real quest stuff;
	private Quest quest;
	// Use this for initialization
	void Start () {
		
	}

	public void Setup(Quest currentQuest, questNameScrollList currentScrollList)
	{
		quest = currentQuest;
		this.gameObject.AddComponent<QuestInfo> ();
		QuestInfo info = this.gameObject.GetComponent<QuestInfo> ();
		info.title = quest.Title;
		info.finished = quest.Finished;
		info.description = quest.Description;
		info.poster = quest.Poster;
		info.typeofQuest = quest.typeofQuest;
		info.location = quest.Location;

		name.text = currentQuest.Title;
		if (quest.Finished) {
			compeletion.sprite = done;
		} else {
			compeletion.sprite = notDone;
		}
		scrollList = currentScrollList;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

	[Header("Script Settings")]
	public string script;
	public bool postBattle;
	public string postBattleScript;
	public string repeatScript;

	[Header("Battle Settings")]
	public string background;
	public string scenario;
	public bool scriptedLoss;

	[Header("Scene Changing Settings")]
	public bool battle;
	public bool exit;
	public bool postBattleExit;

	[Header("Activation Settings")]
	public bool repeatable;
	public bool touch;
	public bool interacted;
	public bool active;

	[Header("Reward Settings")]
	public RewardType preRewardChoice;
	public RewardType postRewardChoice;
	public bool postRewardActivator;
	public bool postReward;
	public string postRewardedItem;
	public bool preRewardActivator;
	public bool preReward;
	public int preRewardCount;
	public string preRewardedItem;

	[Header("Boundary")]
	public Vector2 distance;
	public bool passable;

	[HideInInspector]
	public OverPlayer op;
	public enum RewardType {None, Character, Spell, Weapon};

	void Start()
	{
		op = GameObject.Find ("OverPlayer").GetComponent<OverPlayer> ();
	}
	public void battleResults()
	{
		if (postBattle)
		{
			op.dialog.Load (postBattleScript);
		}
		if (postReward)
		{
			if (postRewardChoice == RewardType.Character)
			{
				if(postRewardedItem == "John")
				{
					op.charUnlock = true;
				}
			}
		}
	}
	public void preBattle()
	{
		if (preReward)
		{
			if (preRewardChoice == RewardType.Character)
			{
				if(preRewardedItem == "John")
				{
					op.availableWeapons.Add (Weapon_Types.Shotgun);
				}
				else if(preRewardChoice == RewardType.Spell)
				{
					for (int i = 0; i < preRewardCount; i++)
					{
						op.groupPack.Add (Resources.Load (preRewardedItem));
					}
				}
			}
		}
	}
	public void activateOthers(Trigger[] others)
	{
		foreach (Trigger t in others)
		{
			t.active = true;
		}
	}
	public void deactivateOthers(Trigger[] others)
	{
		foreach (Trigger t in others)
		{
			t.active = false;
		}
	}
	public void unblockBoundaries(Trigger[] others)
	{
		foreach (Trigger t in others)
		{
			t.passable = true;
		}
	}
	public void blockBoundaries(Trigger[] others)
	{
		foreach (Trigger t in others)
		{
			t.passable = false;
		}
	}
	public void enforceBoundary()
	{
			op.transform.position = new Vector2 (transform.position.x + distance.x, transform.position.y + distance.y);
	}
}

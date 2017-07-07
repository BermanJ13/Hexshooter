using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystem_Multiplayer : EventSystem {

	// Use this for initialization
	protected override void OnEnable()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		EventSystem originalCurrent = EventSystem.current;
		current = this;
		base.Update ();
		current = originalCurrent;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Multiplayer : Button 
{
	public EventSystem eventSystem;

	protected override void Awake()
	{
		base.Awake ();
		eventSystem = GetComponent<EventSystemholder> ().eventSystem;
	}
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;

		if (IsInteractable () && navigation.mode != Navigation.Mode.None)
			eventSystem.SetSelectedGameObject (gameObject, eventData);
		base.OnPointerDown (eventData);
	}

	public override void Select()
	{
		if (eventSystem.alreadySelecting)
			return;
		eventSystem.SetSelectedGameObject (gameObject);
	}
}

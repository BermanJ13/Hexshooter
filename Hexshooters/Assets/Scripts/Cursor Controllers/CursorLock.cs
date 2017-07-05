using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CursorLock : MonoBehaviour {
	EventSystem_Multiplayer e1;
	EventSystem_Multiplayer e2;
	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {

		if (SceneManager.GetActiveScene().name == "PvP")
		{
			e1 = GameObject.Find ("ES_P1").GetComponent<EventSystem_Multiplayer> ();
			e2 = GameObject.Find ("ES_P2").GetComponent<EventSystem_Multiplayer> ();
		}
			
		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Input.mousePosition;

		List<RaycastResult> raycastResults = new List<RaycastResult>();
		if (SceneManager.GetActiveScene().name != "PvP")
			EventSystem.current.RaycastAll (pointer, raycastResults);
		else
		{
			e1.RaycastAll (pointer, raycastResults);
			e2.RaycastAll (pointer, raycastResults);
		}
			

		if(raycastResults.Count > 0)
		{
			foreach (RaycastResult raycastResult in raycastResults) {
				//Debug.Log(raycastResult.gameObject.name);
				GameObject hoveredObj = raycastResult.gameObject;

				if (hoveredObj.GetComponent<Button>()) {
					hoveredObj.GetComponent<Button>().OnPointerExit(pointer);
				} else {//sometimes in my setup the child image is interactable
					hoveredObj = hoveredObj.transform.parent.gameObject;
					if (hoveredObj.GetComponent<Button>()) {
						hoveredObj.GetComponent<Button>().OnPointerExit(pointer);
					}
				}
			}
		}
	}
}

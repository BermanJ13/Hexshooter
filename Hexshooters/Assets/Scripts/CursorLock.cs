using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorLock : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Input.mousePosition;

		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointer, raycastResults);

		if(raycastResults.Count > 0)
		{
			foreach (RaycastResult raycastResult in raycastResults) {
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

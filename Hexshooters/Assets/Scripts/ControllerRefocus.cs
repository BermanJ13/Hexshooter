using UnityEngine;
 using UnityEngine.EventSystems;
 
 // If there is no selected item, set the selected item to the event system's first selected item
 public class ControllerRefocus : MonoBehaviour
 {
     void Update()
     {
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			EventSystem.current.SetSelectedGameObject(GameObject.Find("BattleButton"));
		}
     }
 }
using UnityEngine;
using UnityEngine.EventSystems;

// If there is no selected item, set the selected item to the event system's first selected item
public class MenuRefocus : MonoBehaviour
{
	public PvPStarter p;
	void Update()
	{
		if (p.menu == Menus.Main)
		{
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				EventSystem.current.SetSelectedGameObject (EventSystem.current.firstSelectedGameObject);
			}
		}
		else if (p.menu == Menus.PvP)
		{
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				EventSystem.current.SetSelectedGameObject (GameObject.Find ("Active"));
			}
		}
		else if (p.menu == Menus.Options && !p.changeVolume)
		{
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				EventSystem.current.SetSelectedGameObject (GameObject.Find ("Filter Button"));
			}
		}
	}
}
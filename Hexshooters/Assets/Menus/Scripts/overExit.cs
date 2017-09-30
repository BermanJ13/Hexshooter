using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class overExit : MonoBehaviour {

	public void toPreLoad()
	{
		GameObject op = GameObject.Find("OverPlayer");
		if (op != null)
			Destroy (op);
		if (GameObject.Find ("__app") != null)
			Destroy (GameObject.Find ("__app"));

		SceneManager.LoadScene ("preload");
	}
	public void quitGame()
	{
		Application.Quit();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PvPStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void toInstructions()
	{
		SceneManager.LoadScene (3);
	}
	public void toCharacterSelect()
	{
		SceneManager.LoadScene (1);
	}
	public void toPvP()
	{
		SceneManager.LoadScene (4);
	}
	public void toOver()
	{
        OverPlayer op = GameObject.FindObjectOfType<OverPlayer>();
        op.enabled = true;
        SceneManager.LoadScene (6);
	}
	public void quitGame()
	{
		Application.Quit();
	}
}

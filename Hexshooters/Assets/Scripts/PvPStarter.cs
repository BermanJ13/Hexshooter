using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PvPStarter : MonoBehaviour {

	void Awake ()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
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
		SceneManager.LoadScene ("PVP");
	}
	public void toOver()
	{
        OverPlayer op = GameObject.FindObjectOfType<OverPlayer>();
        op.enabled = true;
        SceneManager.LoadScene ("Overworld");
	}
	public void quitGame()
	{
		Application.Quit();
	}
}

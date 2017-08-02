using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {
	
    // Use this for initialization
    void Awake()
    {
        OverPlayer op = GameObject.FindObjectOfType<OverPlayer>();
        op.enabled = false;

		GameObject pause = GameObject.Find("ClosedBook");
		GameObject deck = GameObject.Find("DeckMenuObject");
		//Turn off the pause menu
		pause.SetActive(false);
		deck.SetActive (false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}

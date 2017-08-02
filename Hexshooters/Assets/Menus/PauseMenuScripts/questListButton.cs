using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questListButton : MonoBehaviour {

	public Button button;
	public Text questName;
	public Image done;


	//Prevents Destruction upon scene switching
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}

	void Start () {
		
	}

	public void Setup()
	{
		
	}

}

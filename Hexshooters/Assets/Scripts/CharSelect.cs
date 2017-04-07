using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;

public class CharSelect : MonoBehaviour {
	public int p1;
	public int p2;
	GameObject p1select;
	GameObject p2select;

	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	// Use this for initialization
	void Start () {
		p1select = GameObject.Find ("P1_Weapon");
		p2select = GameObject.Find ("P2_Weapon");
		p1 = 1;
		p2 = 1;

	}
	
	// Update is called once per frame
	void Update () {
		if (p1select != null)
		{
			if (p1 == 1)
			{
				p1select.GetComponent<Text> ().text = "Revolver";
			}
			if (p1 == 2)
			{
				p1select.GetComponent<Text> ().text = "Rifle";
			}
			if (p1 == 3)
			{
				p1select.GetComponent<Text> ().text = "Shotgun";
			}
			if (p2 == 1)
			{
				p2select.GetComponent<Text> ().text = "Revolver";
			}
			if (p2 == 2)
			{
				p2select.GetComponent<Text> ().text = "Rifle";
			}
			if (p2 == 3)
			{
				p2select.GetComponent<Text> ().text = "Shotgun";
			}

			if (Input.GetKeyDown (KeyCode.A))
			{
				p2--;
				if (p2 < 1)
					p2 = 3;
			}
			if (Input.GetKeyDown (KeyCode.D))
			{
				p2++;
				if (p2 > 3)
					p2 = 1;
			}

			if (Input.GetKeyDown (KeyCode.LeftArrow))
			{
				p1--;
				if (p1 < 1)
					p1 = 3;
			}
			if (Input.GetKeyDown (KeyCode.RightArrow))
			{
				p1++;
				if (p1 > 3)
					p1 = 1;
			}
		}
	}
}

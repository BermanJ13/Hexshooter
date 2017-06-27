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
	bool horizontal = false;
	bool horizontal2 = false;

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
			if (p1 == 4)
			{
				p1select.GetComponent<Text> ().text = "Gatling";
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
			if (p2 == 4)
			{
				p2select.GetComponent<Text> ().text = "Gatling";
			}
			float Horizontal = Input.GetAxisRaw ("Horizontal_P1");
			float Horizontal2 = Input.GetAxisRaw ("Horizontal_P2");

			if (Horizontal2 < 0)
			{
				if (!horizontal2)
				{
					horizontal2 = true;
					p2--;
					if (p2 < 1)
						p2 = 4;
				}
			}
			if (Horizontal2 > 0)
			{
				if (!horizontal2)
				{
					horizontal2 = true;
					p2++;
					if (p2 > 4)
						p2 = 1;
				}
			}

			if (Horizontal < 0)
			{
				if (!horizontal)
				{
					horizontal = true;
					p1--;
					if (p1 < 1)
						p1 = 4;
				}
			}
			if (Horizontal > 0)
			{
				if (!horizontal)
				{
					horizontal = true;
					p1++;
					if (p1 > 4)
						p1 = 1;
				}	
			}                                                                       
		
			if (Horizontal == 0)
			{
				horizontal = false;
			}

			if (Horizontal2 == 0)
			{
				horizontal2 = false;
			}
		}
	}
}
	

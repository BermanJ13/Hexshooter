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
	public Weapon_Types p1Weap;
	public Weapon_Types p2Weap;

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
		p1Weap = Weapon_Types.Revolver;
		p2Weap = Weapon_Types.Revolver;
	}
	
	// Update is called once per frame
	void Update () {
		if (p1select != null)
		{
			if (p1 == 1)
			{
				p1select.GetComponent<Text> ().text = "Revolver";
				p1Weap = Weapon_Types.Revolver;
			}
			if (p1 == 2)
			{
				p1select.GetComponent<Text> ().text = "Rifle";
				p1Weap = Weapon_Types.Rifle;
			}
			if (p1 == 3)
			{
				p1select.GetComponent<Text> ().text = "Shotgun";
				p1Weap = Weapon_Types.Shotgun;
			}
			if (p1 == 4)
			{
				p1select.GetComponent<Text> ().text = "Gatling";
				p1Weap = Weapon_Types.Gatling;
			}
			if (p1 == 6)
			{
				p1select.GetComponent<Text> ().text = "Bow";
				p1Weap = Weapon_Types.Canegun;
			}
			if (p2 == 1)
			{
				p2select.GetComponent<Text> ().text = "Revolver";
				p2Weap = Weapon_Types.Revolver;
			}
			if (p2 == 2)
			{
				p2select.GetComponent<Text> ().text = "Rifle";
				p2Weap = Weapon_Types.Rifle;
			}
			if (p2 == 3)
			{
				p2select.GetComponent<Text> ().text = "Shotgun";
				p2Weap = Weapon_Types.Shotgun;
			}
			if (p2 == 4)
			{
				p2select.GetComponent<Text> ().text = "Gatling";
				p2Weap = Weapon_Types.Gatling;
			}
			if (p2 == 6)
			{
				p2select.GetComponent<Text> ().text = "Bow";
				p2Weap = Weapon_Types.Bow;
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
					if (p1 == 5)
						p1 = 4;
					if (p1 < 1)
						p1 = 6;
				}
			}
			if (Horizontal > 0)
			{
				if (!horizontal)
				{
					horizontal = true;
					p1++;
					if (p1 == 5)
						p1 = 6;
					if (p1 > 6)
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
	

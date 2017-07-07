using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GameResults : MonoBehaviour {

	GameObject p1_stat, p2_stat, p1_heal,p2_heal;
	public string p1_status, p2_status;
	public int p1_health,p2_health;

	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameObject.Find ("CharSelect") != null)
			Destroy (GameObject.Find ("CharSelect"));
		p1_stat = GameObject.Find ("p1_status");
		p2_stat = GameObject.Find ("p2_status");
		p1_heal= GameObject.Find ("p1_health");
		p2_heal= GameObject.Find ("p2_health");

		if (GameObject.FindGameObjectWithTag ("Player"))
		{
			p1_health = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().health;
		}
		if (GameObject.FindGameObjectWithTag ("Player2"))
		{
			p2_health = GameObject.FindGameObjectWithTag ("Player2").GetComponent<Player> ().health;
		}

		if (p1_health > p2_health)
		{
			p1_status = "Winner!";
			p2_status = "Loser";
		} 
		else if (p1_health == p2_health)
		{
			p1_status = "Draw!";
			p2_status = "Draw!";
		} 
		else
		{
			p2_status = "Winner!";
			p1_status = "Loser";
		}

		if(p1_stat != null)
		{
			p1_stat.GetComponent<Text>().text = p1_status;
		}
		if( p2_stat != null)
		{
			p2_stat.GetComponent<Text>().text = p2_status;
		}
		if( p1_heal!= null)
		{
			p1_heal.GetComponent<Text>().text = p1_health.ToString();
		}
		if( p2_heal!= null)
		{
			p2_heal.GetComponent<Text>().text = p2_health.ToString();
		}


	}
}

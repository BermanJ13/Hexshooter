using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	//health 
	public int health;
	public int armorWeakness;
	System.Timers.Timer timeCount = new System.Timers.Timer ();
	int burnTime =3;
	public string stat;


	// Use this for initialization
	void Start () {
		health = 100;
		Debug.Log (health);
		stat = "normal";
		armorWeakness = 0;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		
		Status (stat);
	}

	//health
	public int Health()
	{
		return health;
	}

	//Dictates bullet beavior on the player
	public void Status(string status)
	{
		if (status == "burn") 
		{
			timeCount.Elapsed += timer_Elapsed;
			int wait = 1 - (System.DateTime.Now.Second % 1);
			timeCount.Interval = wait * 1000;
			timeCount.Start();
			Debug.Log (burnTime);
			if(burnTime>0)
			{
				health -= 3;
				Debug.Log (health);
				burnTime--;
			}
			else if(burnTime<=0)
			{
				status = "normal";
				burnTime =3;
			}

		}

	}

	void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		timeCount.Stop ();
		int wait = System.DateTime.Now.Second % 1;
		timeCount.Interval = wait * 1000;
		timeCount.Start ();
	}
}

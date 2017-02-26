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
	public bool reload;
    	bool breakImmune; //flag to ensure that every water shotgun spell doesn't endlessly apply break

    	public StatusManager statMngr = new StatusManager();
    

	// Use this for initialization
	void Start () {
		health = 100;
		//Debug.Log (health);
		stat = "normal";
		armorWeakness = 0;
		reload = true;
        	breakImmune = false;
	}
	
	// Update is called once per frame
	public void enemyUpdate () 
	{
        Debug.Log(health);
        //foreach (StatusEffect s in statMngr.m_effects)
        //{
        //    if (statMngr.IsAffected(s.m_type))
        //    {
        //        Status(s.m_type);
        //    }
        //}
	}

	//health
	public int Health()
	{
		return health;
	}

	//Dictates bullet beavior on the player
	public void Status(StatusType status)
	{
        if (status == StatusType.Burn)
        {
            timeCount.Elapsed += timer_Elapsed;
            int wait = 1 - (System.DateTime.Now.Second % 1);
            timeCount.Interval = wait * 1000;
            timeCount.Start();
            Debug.Log(burnTime);
            if (burnTime > 0)
            {
                health -= 3;
                Debug.Log(health);
                burnTime--;
            }
            else if (burnTime <= 0)
            {
                //status = "normal";
                burnTime = 3;
            }

        }

        if (status == StatusType.Break)
        {
            if (!breakImmune)
            {
                stat = "break";
            }
            else
            {
                breakImmune = false;
            }
        }

        if (status == StatusType.Slow)
        {

        }

	}

    public void Status(string status)
    {
        if (status == "burn")
        {
            timeCount.Elapsed += timer_Elapsed;
            int wait = 1 - (System.DateTime.Now.Second % 1);
            timeCount.Interval = wait * 1000;
            timeCount.Start();
            Debug.Log(burnTime);
            if (burnTime > 0)
            {
                health -= 3;
                Debug.Log(health);
                burnTime--;
            }
            else if (burnTime <= 0)
            {
                status = "normal";
                burnTime = 3;
            }

        }

        if (status == "break")
        {
            if (!breakImmune)
            {
                stat = "break";
            }
            else
            {
                breakImmune = false;
            }
        }

    }

    public void takeDamage(int damage) //created for "break" status
    {
        if (this.stat != "break")
        {
            this.health -= damage;
        }
        else
        {
            this.health -= (damage * 2);
            stat = "normal";
            breakImmune = true;
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

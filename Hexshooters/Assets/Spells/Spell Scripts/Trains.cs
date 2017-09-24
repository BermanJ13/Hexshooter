using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trains : Spell
{
    private GameObject[] enemyPanels;
    private GameObject[] playerPanels;
    private bool targetNeeded;
    bool inBounds;
    public Transform track;
    Vector2 target;
    Vector2 position;
    Collider2D[] colliders;
    private bool created;

    int powerLvl = 1;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        enemyPanels = GameObject.FindGameObjectsWithTag("enemyZone");
        playerPanels = GameObject.FindGameObjectsWithTag("playerZone");
        targetNeeded = true;
        inBounds = false;
        created = false;


        Instantiate(track, gameObject.transform.position, gameObject.transform.rotation);

        trackBehind(transform.position);
        trackAhead(transform.position);
    }

    void trackBehind(Vector3 curPos)
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(curPos.x - 1, curPos.y), new Vector2(curPos.x - 1, curPos.y));
        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Track")
            {
                powerLvl++;
                trackBehind(curPos);
            }
        }
    }
    void trackAhead(Vector3 curPos)
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(curPos.x + 1, curPos.y), new Vector2(curPos.x + 1, curPos.y));
        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Track")
            {
                powerLvl++;
                trackAhead(curPos);
            }
        }
    }
    // Update is called once per frame
    new void spellUpdate()
    {
        base.spellUpdate();
    }

	public override void movement(Weapon_Types weapon)
    {
    }

	public override void hitBehavior(Weapon_Types weapon)
    {
       
    }

	public override void setDescription(Weapon_Types weapon)
    {

    }
}

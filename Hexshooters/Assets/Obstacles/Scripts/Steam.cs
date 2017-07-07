using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : Obstacle
{
    public List<GameObject> hitPanels = new List<GameObject>();
    private int steamTimer = 50; //decay from steam timer before poof prox ~2 seconds atm
                                  // Update is called uonce per frame
    public override void obstacleUpdate()
    {
        collide();
        steamTimer--;
        //Debug.Log (health);
        if (direction != new Vector2(0, 0))
        {
            move();
        }
        if (health <= 0)
        {
            MarkedforDeletion = true;
        }
        if (steamTimer <= 0)
        {
            MarkedforDeletion = true;
            steamTimer = 50;
        }
    }
    public override void collide()
    {
        Collider2D[] colliders;
        colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
        foreach (Collider2D d in colliders)
        {
            Player p = d.GetComponent<Player>();
            //if collides with another obstacle, destroys both
            if (d.gameObject.tag == "Obstacle")
            {

            }
            //else means hit a player enemy
            else if (d.gameObject.tag == "Enemy")
            {
                Enemy e = d.GetComponent<Enemy>();
                e.takeDamage(damage, attributes); //enemy takes dmg
                if (e.transform.position.x != 9)
                {
                    //e.transform.position = new Vector3 (e.transform.position.x + 1, e.transform.position.y, e.transform.position.z);
                }
                MarkedforDeletion = true;
            }
            else if (d.gameObject.tag == "Player2")
            {
                d.GetComponent<Player>().takeDamage(damage, attributes); //player takes dmg 

                if (d.transform.position.x != 9)
                {
                    //d.transform.position += new Vector3 (1, 0, 0);
                }
                MarkedforDeletion = true;

            }
            else if (d.gameObject.tag == "Player")
            {
                d.GetComponent<Player>().takeDamage(damage, attributes); //player takes dmg


                if (d.GetComponent<Player>().transform.position.x != 0)
                {
                    //d.transform.position += new Vector3(-1f,0f,0f); 
                }
                MarkedforDeletion = true;
            }
            if (d.gameObject.tag == "playerZone" || d.gameObject.tag == "enemyZone")
            {
                bool created = false;
                if (direction != new Vector2(0, 0))
                {
                    for (int i = 0; i < hitPanels.Count; i++)
                    {
                        if (hitPanels[i] == d.gameObject)
                            created = true;
                    }
                    //Debug.Log (created);
                    if (!created)
                    {
                        GameObject g = (GameObject)Instantiate(Resources.Load("Steam"), new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                        g.GetComponent<Steam>().hitPanels = hitPanels;
                    }
                    else
                        hitPanels.Add(d.gameObject);
                }
            }
        }
    }
}
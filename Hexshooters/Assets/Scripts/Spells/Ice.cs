using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Spell {

    private int spellTimer;

    // Use this for initialization
    new void Start () {
        base.Start();

        spellTimer = 50;
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
	}

    public override void movement(int weapon)
    {
        switch (weapon)
        {
            //revolver
            case 1:

                Vector2 target = new Vector2(transform.position.x, transform.position.y) + direction;
                Vector2 position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;

            //rifle
            case 2:
                target = new Vector2(transform.position.x + 1, transform.position.y) + direction;
                position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;

            //shotgun
            case 3:
                target = new Vector2(transform.position.x + 1, transform.position.y) + direction;
                position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;

            //gatling
            case 4:
                target = new Vector2(transform.position.x, transform.position.y) + direction;
                position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;

            //cane gun - not priority
            case 5:
                target = new Vector2(transform.position.x, transform.position.y) + direction;
                position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;

        }
    }

    public override void hitBehavior(int weapon)
    {
        switch (weapon)
        {
            case 1: //freeze row
                Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x+10, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.tag == "Player")
                    {
                       
                    }
                    else if (c.gameObject.tag == "Enemy")
                    {
                        StatusEffect slow = new StatusEffect(5);
                        slow.m_type = StatusType.Slow;
                        c.gameObject.GetComponent<Enemy>().statMngr.AddEffect(slow);

                        spellTimer--;
                        if (spellTimer <= 0)
                        {
                            markedForDeletion = true;
                            spellTimer = 50;
                        }

                        //c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        //c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
                    }
                }
                break;
            case 2: //
                colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.tag == "Player")
                    {

                    }
                    else if (c.gameObject.tag == "Enemy")
                    {

                        //c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        //c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
                    }
                }
                break;
            case 3: //
                colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.tag == "Enemy")
                    {
                       
                        //c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        markedForDeletion = true;
                        //c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
                    }
                }
                break;
            case 4: //
                colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.tag == "Player")
                    {

                    }
                    else if (c.gameObject.tag == "Enemy")
                    {

                        //c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        //c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
                    }
                }
                break;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Spell {

    protected bool revolverMove;
    protected Vector2 rifleOrigin;
    private int spellTimer;
    // Use this for initialization
    new void Start()
    {
        base.Start();
        revolverMove = false;
        rifleOrigin = transform.position;
        spellTimer = 50;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void movement(int weapon)
    {
        switch (weapon)
        {
            //revolver
            case 1:
               
                Vector2 target = new Vector2(transform.position.x+1, transform.position.y) + direction;
                if (revolverMove)
                    target = new Vector2(transform.position.x - 1, transform.position.y) + direction;

                Vector2 position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;

            //rifle
            case 2:
                if ((transform.position.x - rifleOrigin.x) < 3)
                {
                    target = new Vector2(transform.position.x + 2, transform.position.y) + direction;
                    position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                    transform.position = position;
                }
                else
                {
                    hitBehavior(2);
                    spellTimer--;
                    if (spellTimer <= 0)
                    {
                        markedForDeletion = true;
                        spellTimer = 50;
                    }
                }
                break;

            //shotgun
            case 3:
                target = new Vector2(transform.position.x+1, transform.position.y) + direction;
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
            case 1: //shot damages enemy, bounces off and then heals player if it hits them
                Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if(c.gameObject.tag == "Player")
                    {
                        if (revolverMove)
                        {
                            c.gameObject.GetComponent<Player>().health += 2;
                            revolverMove = false;
                            markedForDeletion = true;
                        }
                    }
                    else if (c.gameObject.tag == "Enemy")
                    {
                        c.gameObject.GetComponent<Enemy>().health -= 2;
                        revolverMove = true;
                        //c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        //c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
                    }
                }
                break;
            case 2: //whirlpool shoots 3 squares ahead and drags enemy from adjacent squares
                colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                foreach (Collider2D c in colliders)
                {
                  
                    if (c.gameObject.tag == "Enemy")
                    {
                        //in future call Enemy move to this position so they don't just warp
                        c.transform.position = transform.position;
                        markedForDeletion = true;
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        //c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
                        markedForDeletion = true;
                    }
                }
                break;
            case 3: //shotgun makes them vulnerable to next attack
                colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.tag == "Enemy")
                    {
                        c.gameObject.GetComponent<Enemy>().takeDamage(3);

                        if (c.gameObject.GetComponent<Enemy>().stat == "normal")
                            c.gameObject.GetComponent<Enemy>().Status("break");
                        markedForDeletion = true;
                        //c.gameObject.GetComponent<Enemy>().health -= damageCalc(damageTier,hitNum);
                    }
                    if (c.gameObject.tag == "Obstacle")
                    {
                        markedForDeletion = true;
                        //c.gameObject.GetComponent<Obstacle>().health -= damageCalc(damageTier,hitNum);
                    }
                }
                break;
            case 4: //Fire hose
                colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x + 10, transform.position.y));
                foreach (Collider2D c in colliders)
                {
                    if (c.gameObject.tag == "Enemy")
                    {
                        if (spellTimer % 10 == 0) //modulo ensures that enemy not immediately pushed to back
                        {
                            if (c.gameObject.GetComponent<Enemy>().transform.position.x <= 8)
                                c.gameObject.GetComponent<Enemy>().transform.position = new Vector2(c.gameObject.GetComponent<Enemy>().transform.position.x + 1, c.gameObject.GetComponent<Enemy>().transform.position.y);
                        }
                        spellTimer--;
                        if (spellTimer <= 0)
                        {
                            markedForDeletion = true;
                            spellTimer = 50;
                        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildCatStrike : EnemySpell
{


    [Header("Wildcat Strike Vars")]
    public float basicAttackStayTime = 1;
    float attackCounter = 0;
    [System.NonSerialized]
    public bool attacking = false;
    Vector3 playerPos;
    bool hitPlayer = false;
    public float backFromPlayerDist = 0.2f;




    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void spellUpdate()
    {
        base.spellUpdate();
        Debug.Log(speed);
    }

    public override void movement(int weapon)
    {
        if (!attacking)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            playerPos = new Vector3(playerPos.x + backFromPlayerDist, playerPos.y, playerPos.z);
            attacking = true;
        }
        if (RearBack())
        {
            attackCounter = attackCounter + Time.deltaTime;
            gameObject.transform.position = playerPos;
            if (!hitAlready)
            {
                hitBehavior(0);
            }
            if (attackCounter > basicAttackStayTime)
            {
                hitAlready = false;
                attacking = false;
                attackCounter = 0;
                EnemyScript.isMoving = true;
                hitPlayer = false;
                clearRear();
            }
        }
    }



    public override void hitBehavior(int weapon)
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - backFromPlayerDist, transform.position.y), transform.position);
        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Obstacle")
            {
                c.GetComponent<Obstacle>().takeDamage(damageCalc(damageTier, hitNum));
                hitAlready = true;
            }
            else if (c.gameObject.tag == "Player")
            {
                c.gameObject.GetComponent<Player>().takeDamage(damageCalc(damageTier, hitNum));
                hitAlready = true;
            }
        }
    }
    public override void setDescription(int weapon)
    {
        description = "Go up to player and stike them quickly";
    }

}

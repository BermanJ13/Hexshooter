using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicEnemyAttack : EnemySpell {



    [Header("Basic Attack Vars")]
    public float basicAttackStayTime = 1;
    float attackCounter = 0;
    [System.NonSerialized]public bool attacking = false;
    Vector3 playerPos;
    bool hitPlayer = false;




    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void spellUpdate()
    {
        base.spellUpdate();
        //Debug.Log(speed);
    }
    
    public override void movement(int weapon)
    {
        if (!attacking)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            attacking = true;
        }
        if (RearBack())
        {
            attackCounter = attackCounter + Time.deltaTime;
            gameObject.transform.position = playerPos;
            if(!hitAlready)
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
        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position, transform.position);
        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Obstacle")
            {
				c.GetComponent<Obstacle>().takeDamage(damageCalc(damageTier, hitNum), attributes);
                hitAlready = true;
            }
            else if (c.gameObject.tag == "Player")
            {
				c.gameObject.GetComponent<Player>().takeDamage(damageCalc(damageTier, hitNum), attributes);
                hitAlready = true;
            }
        }
    }
    public override void setDescription(int weapon)
    {
        description = "Go up to guy and smack'em";
    }

}

   

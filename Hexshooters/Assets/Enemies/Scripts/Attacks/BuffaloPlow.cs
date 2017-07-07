using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffaloPlow : EnemySpell
{


    [Header("Buffalo Plow Vars")]
    public float basicAttackStayTime = 0.1f;
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
    }

    public override void movement(int weapon)
    {
        //if the enemy is not yet attacking
        if (!attacking)
        {
            //save the current player pos, and change it a little, so when we zap to it later we can just use the base playerPos
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            playerPos = new Vector3(playerPos.x + backFromPlayerDist, playerPos.y, playerPos.z);
            //set attacking to true
            attacking = true;
            EnemyScript.isMoving = false;
        }
        //runs rearback in the enemy class
        //if true then we do the attack cycle
        if (RearBack())
        {
            
            attackCounter = attackCounter + Time.deltaTime;
            if (attackCounter > basicAttackStayTime)
            {
                if (!hitAlready)
                {
                    hitBehavior(0);
                }
                if (gameObject.transform.position.x>0)
                {
                    attackCounter = 0;
                    hitAlready = false;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x-1,gameObject.transform.position.y,gameObject.transform.position.z);
                }
                else
                {
                    //THIS IS WHERE WE RESET EVERYTHING INCLUDING THE REARBACK VARIABLES IN CLEARREAR()
                    hitAlready = false;
                    attacking = false;
                    attackCounter = 0;
                    EnemyScript.isMoving = true;
                    hitPlayer = false;
                    clearRear();

                }
            }
        }
    }



    public override void hitBehavior(int weapon)
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - backFromPlayerDist, transform.position.y), transform.position);

        foreach (Collider2D c in colliders)
        {
            Debug.Log(c.gameObject.name);
            if (c.gameObject.tag == "Obstacle")
            {
				c.GetComponent<Obstacle>().takeDamage(damageCalc(damageTier, hitNum), attributes);
                hitAlready = true;
            }
            else if (c.gameObject.tag == "Player")
            {
                c.gameObject.GetComponent<Player>().takeDamage (damageCalc (damageTier, hitNum),attributes);
                hitAlready = true;
            }
            if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
            {
                showPanels(c);
            }
        }
    }
    public override void setDescription(int weapon)
    {
        description = "rears back, and then teleports to target position at start of rearing. then attacks forward two spaces";
    }

}

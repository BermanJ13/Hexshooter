using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit1 : AIBase {

    private int attackCounter;
    public const int ATTACK_TIMEOUT = 3;// Waits two frames between attacks minimum.
    private bool canAttack;//If true, the enemy can currently attack, if false, cannot for some reason.

    // Bandit1 moves every second.
    public override float TIME_PER_ACTION
    {
        get
        {
            return 1.0f;
        }
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        attackCounter = 0;
        minX = 5;
        maxX = 7;

        minY = 0;
        maxY = 4;
    }

    public override void AIStep()
    {
        // The decision that affects the behaviour tables.
        float decision = UnityEngine.Random.Range(0, 1.0f);

        // Counts down until the next attack is available.
        if(attackCounter > 0)
        {
            attackCounter--;
        }

        // Checking to make sure nothing is preventing attacking.
        canAttack = (attackCounter == 0) && !myStatus.IsAffected(StatusType.Disabled);//Being disabled prevents attacking

        // This is where the actual action happens.
        if (!myStatus.IsAffected(StatusType.Bound)){

            if (canAttack)
            {
                if (decision < 0.5f)//50%
                {
                    Attack();
                }
                else if (decision < 0.65f)//15%
                {
                    Move(Direction.Up);
                }
                else if (decision < 0.80f)//15%
                {
                    Move(Direction.Down);
                }
                else if (decision < 0.85f)//5%
                {
                    Move(Direction.Left);
                }
                else if (decision < 0.90f)//5%
                {
                    Move(Direction.Right);
                }
                else//10%
                {
                    //Do nothing.
                }
            }
            else
            {
                if(decision < 0.35f) //35%
                {
                    Move(Direction.Up);
                }
                else if(decision < 0.7f)//35%
                {
                    Move(Direction.Down);
                }
                else if(decision < 0.8f)//10%
                {
                    Move(Direction.Left);
                }
                else if (decision < 0.9f)//10%
                {
                    Move(Direction.Right);
                }
                else//10%
                {
                    //Do nothing.
                }
            }

        }
        else// Bound loop, can't move but still tries to attack.
        {
            if(canAttack)
            {
                Attack();
            }
        }


    }

    public virtual void Attack()
    {
        attackCounter = ATTACK_TIMEOUT;
        //Implement weak fire spell cast here.
        Debug.LogError("Fire Spell in Bandit1 Attack() not implemented");

    }

    
}

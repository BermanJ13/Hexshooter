using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{

    private int attackCounter;
    public const int ATTACK_TIMEOUT = 3;// Waits two frames between attacks minimum.
    private bool canAttack;//If true, the enemy can currently attack, if false, cannot for some reason.
    
    [SerializeField]
    public string[] spells;
    private int spellcount = 0;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        attackCounter = 0;
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
                    if (movePossible(Direction.Up) == 2)
                    {
                        Move(Direction.Up);
                    }
                }
                else if (decision < 0.80f)//15%
                {
                    if (movePossible(Direction.Down) == 2)
                    {
                        Move(Direction.Down);
                    }
                }
                else if (decision < 0.85f)//5%
                {
                    if (movePossible(Direction.Left) == 2)
                    {
                        Move(Direction.Left);
                    }
                }
                else if (decision < 0.90f)//5%
                {
                    if (movePossible(Direction.Right) == 2)
                    {
                        Move(Direction.Right);
                    }
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
                    if (movePossible(Direction.Up) == 2)
                    {
                        Move(Direction.Up);
                    }
                }
                else if(decision < 0.7f)//35%
                {
                    if (movePossible(Direction.Down) == 2)
                    {
                        Move(Direction.Down);
                    }
                }
                else if(decision < 0.8f)//10%
                {
                    if (movePossible(Direction.Left) == 2)
                    {
                        Move(Direction.Left);
                    }
                }
                else if (decision < 0.9f)//10%
                {
                    if (movePossible(Direction.Right) == 2)
                    {
                        Move(Direction.Right);
                    }
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

    /// <summary>
    /// casts a spell in sequence to how they are loaded in the inspector
    /// </summary>
    public virtual void Attack()
    {
        attackCounter = ATTACK_TIMEOUT;

		GameObject go = (GameObject)Instantiate(Resources.Load(spells[spellcount]),new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
        spellcount++;
        if (spellcount >= spells.Length)
        {
            spellcount = 0;
        }
        Debug.Log(spellcount);
        ////get the thing component on your instantiated object
        Spell mything = go.GetComponent<Spell>();

		////set a member variable (must be PUBLIC)
		mything.weaponUsed = 1; 
		mything.PlayerNum = 2;

    }

    public virtual void Attack(string spell)
    {
        attackCounter = ATTACK_TIMEOUT;

        GameObject go;

        try
        {
             go= (GameObject)Instantiate(Resources.Load(spell), new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }

        catch (Exception e)
        {
            Console.WriteLine("This Spell '"+spell+"' was not found in the spells array.  Go into the inspector and put it in before trying to use it...  Or something else happened i dunno:", e);
            return;
        }
        

        spellcount++;
        if (spellcount <= spells.Length)
        {
            spellcount = 0;
        }
        ////get the thing component on your instantiated object
        Spell mything = go.GetComponent<Spell>();

        ////set a member variable (must be PUBLIC)
        mything.weaponUsed = 1;
        mything.PlayerNum = 2;

    }


}

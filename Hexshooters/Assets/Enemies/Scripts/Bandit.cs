using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{

    private int attackCounter;
    public const int ATTACK_TIMEOUT = 3;// Waits two frames between attacks minimum.
    private bool canAttack;//If true, the enemy can currently attack, if false, cannot for some reason.

    private Transform playertrans;

	UnityEngine.Random ran = new UnityEngine.Random();
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


        //collect data about the tiles around us
        //2 is clear
        //1 is passable, but an obsticale is still there
        //0 is unpassable
        int down = movePossible(Direction.Down);
        int up = movePossible(Direction.Up);
        int left = movePossible(Direction.Left);
        int right = movePossible(Direction.Right);

        // This is where the actual action happens.
        if (!myStatus.IsAffected(StatusType.Bound)){
            playertrans = GameObject.FindGameObjectWithTag("Player").transform;


			int rnd = UnityEngine.Random.Range(0, 100);

            //if the enemy is above the player
            if (gameObject.transform.position.y > playertrans.position.y)
			{
				//move down, or side to side
				if (rnd < 50) 
				{
					if (down == 2)
					{
						Move(Direction.Down);
					}
				}
                else
                {
                    if (rnd < 75)
                    {
                        //right
                        if (right == 2)
                        {
                            Move(Direction.Right);
                        }
                        else if (left == 2)
                        {
                            //or left
                            Move(Direction.Left);
                        }
                        else if (down == 2)
                        {
                            //move away at last resort
                            Move(Direction.Down);
                        }
                    }
                    else
                    {
                        //left
                        if (left == 2)
                        {
                            Move(Direction.Left);
                        }
                        else if (right == 2)
                        {
                            //or right
                            Move(Direction.Right);
                        }
                        else if (down == 2)
                        {
                            //move away at last resort
                            Move(Direction.Down);
                        }
                    }

                }
            }
            else if (gameObject.transform.position.y < playertrans.position.y)
            {
                //if enemy is below the player
                //move up, or side to side

				if (rnd < 50) 
				{
					if (up == 2)
					{
						Move(Direction.Up);
					}
				}
                else
                {
					if(rnd < 75)
                    {
                        //right
                        if (right == 2)
                        {
                            Move(Direction.Right);
                        }
                        else if (left == 2)
                        {
                            //or left
                            Move(Direction.Left);
                        }
                        else if (down == 2)
                        {
                            //move away at last resort
                            Move(Direction.Up);
                        }
                    }
                    else
                    {
                        //left
                        if (left == 2)
                        {
                            Move(Direction.Left);
                        }
                        else if (right == 2)
                        {
                            //or right
                            Move(Direction.Right);
                        }
                        else if (down == 2)
                        {
                            //move away at last resort
                            Move(Direction.Up);
                        }
                    }
                }
            }
			if (Math.Abs (gameObject.transform.position.y - playertrans.position.y) < 1.5) 
			{
				Attack ();
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

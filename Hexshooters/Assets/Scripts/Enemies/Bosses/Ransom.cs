using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ransom : Enemy {

    private int attackCounter;
    public int ATTACK_TIMEOUT = 3;
    public int recentFireCounter = 0;//If greater than zero, fire spell was recently used.
    public const int RECENT_FIRE_RESET = 4; //what recentFireCounter resets to.

    public const int RAGE_ATTACK_TIMEOUT = 1;

    private bool canAttack;//If true, the enemy can currently attack, if false, cannot for some reason.
    public Rob myBrother = null;
    public bool movedByBrother = false;
    

    public float modify_TIME_PER_ACTION;
    public const float RAGE_TIME_PER_ACTION = 0.5f;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        attackCounter = 0;

        myBrother = GameObject.FindObjectOfType<Rob>();
        //Initialize(7, 2);
        modify_TIME_PER_ACTION = TIME_PER_ACTION;
    }

    public override void enemyUpdate()
    {
        if (myBrother == null)
        {
            if (myStatus.IsAffected(StatusType.Slow) || myStatus.IsAffected(StatusType.Freeze))
            {
                timer -= Time.deltaTime * FROZEN_MULTIPLIER;
            }
            else
            {
                timer -= Time.deltaTime;
            }

            if (timer <= 0.0f)
            {
                timer += modify_TIME_PER_ACTION;
                if (!isInitialized)
                {
                    Debug.LogError(this.gameObject.name + " is an enemy that Initialize() was never called on.  Wherever you created this enemy, call Initialize on it.");
                }
                AIStep();
            }
        }
    }

    public override void AIStep()
    {
        // The decision that affects the behaviour tables.
        float decision = UnityEngine.Random.Range(0, 1.0f);

        
        if (recentFireCounter > 0)
        {
            recentFireCounter--;
        }

        // Checking to make sure nothing is preventing attacking.
        canAttack = (attackCounter == 0) && !myStatus.IsAffected(StatusType.Disabled);//Being disabled prevents attacking

        if (myBrother != null)
        {
            if (!movedByBrother)
            {

                if (myBrother.currentY == currentY && myBrother.currentX + 1 == currentX)
                {
                    if (attackCounter > 0)
                    {
                        attackCounter--;
                    }
                }

                if (!myStatus.IsAffected(StatusType.Bound))
                {
                    if (currentX > myBrother.currentX + 1)
                    {
                        if (movePossible(Direction.Left) == 2)
                        {
                            Move(Direction.Left);
                        }
                    }
                    else if (currentX <= myBrother.currentX)
                    {
                        if (movePossible(Direction.Right) == 2)
                        {
                            Move(Direction.Right);
                        }
                    }
                    else if (myBrother.currentY > currentY)
                    {
                        if (movePossible(Direction.Up) == 2)
                        {
                            Move(Direction.Up);
                        }
                    }
                    else if (myBrother.currentY < currentY)
                    {
                        if (movePossible(Direction.Down) == 2)
                        {
                            Move(Direction.Down);
                        }
                    }
                    else
                    {
                        if (myBrother.currentY == currentY && myBrother.currentX + 1 == currentX)
                        {
                            // Counts down until the next attack is available.  Only counts down behind his brother.
                            if (decision < 0.25f)//25% chance of attack up
                            {
                                if (movePossible(Direction.Up) == 2)
                                {
                                    if (!Move(Direction.Up))
                                    {
                                        if (movePossible(Direction.Down) == 2)
                                        {
                                            Move(Direction.Down);
                                        }
                                    }
                                }

                                Attack();
                            }
                            else if (decision < 0.5f)//25% chance of attack down
                            {
                                if (movePossible(Direction.Down) == 2)
                                {
                                    if (!Move(Direction.Down))
                                    {
                                        if (movePossible(Direction.Up) == 2)
                                        {
                                            Move(Direction.Up);
                                        }
                                    }
                                }

                                Attack();
                            }
                            //50% chance of nothing.
                        }
                    }

                }
                
            }
            

        }
        else //Dead brother time.
        {
            if (attackCounter > 0)
            {
                attackCounter--;
            }

            if (!myStatus.IsAffected(StatusType.Bound))
            {

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
                    if (decision < 0.35f) //35%
                    {
                        if (movePossible(Direction.Up) == 2)
                        {
                            Move(Direction.Up);
                        }
                    }
                    else if (decision < 0.7f)//35%
                    {
                        if (movePossible(Direction.Down) == 2)
                        {
                            Move(Direction.Down);
                        }
                    }
                    else if (decision < 0.8f)//10%
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
                if (canAttack)
                {
                    Attack();
                }
            }
        }

        movedByBrother = false;//reset
    }

    public void Attack()
    {
        if(myBrother && myBrother.recentWaterCounter > 0)
        {
            if (recentFireCounter > 0)
            {
				GameObject go = (GameObject)Instantiate(Resources.Load("Lightning"),new Vector2(transform.position.x,transform.position.y),Quaternion.identity);

				////get the thing component on your instantiated object
				Spell mything = go.GetComponent<Spell>();

				////set a member variable (must be PUBLIC)
				mything.weaponUsed = 1; 
				mything.PlayerNum = 2;
            }
            else
            {
                float rand = UnityEngine.Random.Range(0, 1.0f);

                if (rand < 0.5f) //50%
                {
					GameObject go = (GameObject)Instantiate(Resources.Load("Lightning"),new Vector2(transform.position.x,transform.position.y),Quaternion.identity);

					////get the thing component on your instantiated object
					Spell mything = go.GetComponent<Spell>();

					////set a member variable (must be PUBLIC)
					mything.weaponUsed = 1; 
					mything.PlayerNum = 2;
                }
                else
                {
					GameObject go = (GameObject)Instantiate(Resources.Load("Fire"),new Vector2(transform.position.x,transform.position.y),Quaternion.identity);

					////get the thing component on your instantiated object
					Spell mything = go.GetComponent<Spell>();

					////set a member variable (must be PUBLIC)
					mything.weaponUsed = 1; 
					mything.PlayerNum = 2;
					recentFireCounter = RECENT_FIRE_RESET;
                }
            }
        }
        else
        {
            float rand = UnityEngine.Random.Range(0, 1.0f);

            if (recentFireCounter > 0)
            {
                if (rand < 0.5f) //50%
                {
					GameObject go = (GameObject)Instantiate(Resources.Load("Lightning"),new Vector2(transform.position.x,transform.position.y),Quaternion.identity);

					////get the thing component on your instantiated object
					Spell mything = go.GetComponent<Spell>();

					////set a member variable (must be PUBLIC)
					mything.weaponUsed = 1; 
					mything.PlayerNum = 2;
                }
                else
                {
					GameObject go = (GameObject)Instantiate(Resources.Load("Fire"),new Vector2(transform.position.x,transform.position.y),Quaternion.identity);

					////get the thing component on your instantiated object
					Spell mything = go.GetComponent<Spell>();

					////set a member variable (must be PUBLIC)
					mything.weaponUsed = 1; 
					mything.PlayerNum = 2;
                    recentFireCounter = RECENT_FIRE_RESET;
                }
            }
            else //The case where any spell can be used.
            {

                if (rand < 0.33f) //33%
                {
					GameObject go = (GameObject)Instantiate(Resources.Load("Lightning"),new Vector2(transform.position.x,transform.position.y),Quaternion.identity);

					////get the thing component on your instantiated object
					Spell mything = go.GetComponent<Spell>();

					////set a member variable (must be PUBLIC)
					mything.weaponUsed = 1; 
					mything.PlayerNum = 2;
                }
                else if(rand < 0.66f) //33%
                {
					GameObject go = (GameObject)Instantiate(Resources.Load("Fire"),new Vector2(transform.position.x,transform.position.y),Quaternion.identity);

					////get the thing component on your instantiated object
					Spell mything = go.GetComponent<Spell>();

					////set a member variable (must be PUBLIC)
					mything.weaponUsed = 1; 
					mything.PlayerNum = 2;
                }
                else //34%
                {
					GameObject go = (GameObject)Instantiate(Resources.Load("Fire"),new Vector2(transform.position.x,transform.position.y),Quaternion.identity);

					////get the thing component on your instantiated object
					Spell mything = go.GetComponent<Spell>();

					////set a member variable (must be PUBLIC)
					mything.weaponUsed = 1; 
					mything.PlayerNum = 2;
					recentFireCounter = RECENT_FIRE_RESET;
                    recentFireCounter = RECENT_FIRE_RESET;
                }
            }
        }


        attackCounter = ATTACK_TIMEOUT;

    }

    public void Die()
    {
        if (myBrother)
        {
            myBrother.myBrother = null;
            myBrother.ATTACK_TIMEOUT = Rob.RAGE_ATTACK_TIMEOUT;
            myBrother.modify_TIME_PER_ACTION = Rob.RAGE_TIME_PER_ACTION;
        }
    }

    public void OnDestroy()
    {
        Die();
    }
}

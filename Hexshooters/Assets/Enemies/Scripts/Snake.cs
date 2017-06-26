using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
{


    //interval for attack TEMPORARY
    [Header("Snake Vars")]
    public Vector2 randomAttackTimerRange = new Vector2(3, 5);
    private float attackInterval = 0;
    private float attackTime = 0;
    Random rnd = new Random();
    private ScorpionStrike scorpionStrike;
    private bool afterAttack = false;
    Transform playerPos;

    new void Start()
    {
        base.Start();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        //
        //TEMPORARY RANDOM INTERVAL
        //
        scorpionStrike = gameObject.GetComponent<ScorpionStrike>();
        attackInterval = Random.Range(randomAttackTimerRange.x, randomAttackTimerRange.y);
    }
    public override void enemyUpdate()
    {
        //does whatever is in the base enemy file, this is how they do movement
        base.enemyUpdate();

        attackTime = attackTime + Time.deltaTime;

        if(attackInterval < attackTime/2)
        {
            afterAttack = false;
        }

        if (attackInterval < attackTime || scorpionStrike.attacking)
        {
            scorpionStrike.movement(0);
            if (!scorpionStrike.attacking)
            {
                afterAttack = true;
                attackTime = 0;
                attackInterval = Random.Range(randomAttackTimerRange.x, randomAttackTimerRange.y);
            }

        }
    }



    int tries = 0;
    bool[] these = { false, false, false, false };
    
    public override void AIStep()
    {
        Debug.Log(afterAttack);
        // The decision that affects the behaviour tables.
        if (!afterAttack)
        {
            // This is where the actual action happens.
            if (!myStatus.IsAffected(StatusType.Bound))
            {
                if (isMoving)
                {
                    playerPos = GameObject.FindGameObjectWithTag("Player").transform;

                    if (Mathf.Abs(playerPos.position.y - gameObject.transform.position.y) > Mathf.Abs(playerPos.position.x - gameObject.transform.position.x))
                    {
                        if (playerPos.position.y - gameObject.transform.position.y > 0)
                        {
                            if (movePossible(Direction.Up) == 2)
                            {
                                Move(Direction.Up);
                            }
                            else if (movePossible(Direction.Right) == 2)
                            {
                                Move(Direction.Right);
                            }
                        }
                        else
                        {
                            if (movePossible(Direction.Down) == 2)
                            {
                                Move(Direction.Down);
                            }
                            else if (movePossible(Direction.Right) == 2)
                            {
                                Move(Direction.Right);
                            }
                        }
                    }
                    else
                    {
                        if (movePossible(Direction.Right) == 2)
                        {
                            Move(Direction.Right);
                        }
                        else if (playerPos.position.y - gameObject.transform.position.y > 0)
                        {
                            if (movePossible(Direction.Up) == 2)
                            {
                                Move(Direction.Up);
                            }
                        }
                        else if (playerPos.position.y - gameObject.transform.position.y < 0)
                        {
                            if (movePossible(Direction.Down) == 2)
                            {
                                Move(Direction.Down);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            // This is where the actual action happens.
            if (!myStatus.IsAffected(StatusType.Bound))
            {
                if (isMoving)
                {
                    playerPos = GameObject.FindGameObjectWithTag("Player").transform;

                    if (Mathf.Abs(playerPos.position.x - gameObject.transform.position.x) > Mathf.Abs(playerPos.position.y - gameObject.transform.position.y))
                    {
                        if (playerPos.position.x - gameObject.transform.position.x > 0)
                        {
                            if (movePossible(Direction.Down) == 2)
                            {
                                Move(Direction.Down);
                            }
                            else if (movePossible(Direction.Left) == 2)
                            {
                                Move(Direction.Left);
                            }
                        }
                        else
                        {
                            if (movePossible(Direction.Up) == 2)
                            {
                                Move(Direction.Up);
                            }
                            else if (movePossible(Direction.Left) == 2)
                            {
                                Move(Direction.Left);
                            }
                        }
                    }
                    else
                    {
                        if (movePossible(Direction.Left) == 2)
                        {
                            Move(Direction.Left);
                        }
                        else if (playerPos.position.x - gameObject.transform.position.x > 0)
                        {
                            if (movePossible(Direction.Down) == 2)
                            {
                                Move(Direction.Down);
                            }
                        }
                        else if (playerPos.position.x - gameObject.transform.position.x < 0)
                        {
                            if (movePossible(Direction.Up) == 2)
                            {
                                Move(Direction.Up);
                            }
                        }
                    }
                }
            }
        }
    }
}

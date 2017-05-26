using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuffaloPlow))]
public class Buffalo : Enemy
{


    //interval for attack TEMPORARY
    [Header("Buffalo Vars")]
    public Vector2 randomAttackTimerRange = new Vector2(2, 3);
    private float attackInterval = 0;
    private float attackTime = 0;
    Random rnd = new Random();
    private BuffaloPlow buffaloPlow;



    new void Start()
    {
        base.Start();
        //
        //TEMPORARY RANDOM INTERVAL
        //
        buffaloPlow = gameObject.GetComponent<BuffaloPlow>();
        attackInterval = Random.Range(randomAttackTimerRange.x, randomAttackTimerRange.y);
    }
    public override void enemyUpdate()
    {
        base.enemyUpdate();
        attackTime = attackTime + Time.deltaTime;


        if (attackInterval < attackTime || buffaloPlow.attacking)
        {
            buffaloPlow.movement(0);
            if (!buffaloPlow.attacking)
            {
                attackTime = 0;
                attackInterval = Random.Range(randomAttackTimerRange.x, randomAttackTimerRange.y);
            }

        }
    }


    public override void AIStep()
    {
        // The decision that affects the behaviour tables.


        // This is where the actual action happens.
        if (!myStatus.IsAffected(StatusType.Bound))
        {
            if (isMoving)
            {
                float decision = UnityEngine.Random.Range(0, 1.0f);

                if (decision < 0.25f)//25%
                {
                    if (movePossible(Direction.Up) == 2)
                    {
                        Move(Direction.Up);
                    }
                    else if (movePossible(Direction.Down) == 2)
                    {
                        Move(Direction.Down);
                    }
                    else if (movePossible(Direction.Left) == 2)
                    {
                        Move(Direction.Left);
                    }
                    else if (movePossible(Direction.Right) == 2)
                    {
                        Move(Direction.Right);
                    }
                }
                else if (decision < 0.3)//25%
                {
                    if (movePossible(Direction.Left) == 2)
                    {
                        Move(Direction.Left);
                    }
                    else if (movePossible(Direction.Right) == 2)
                    {
                        Move(Direction.Right);
                    }
                    else if (movePossible(Direction.Down) == 2)
                    {
                        Move(Direction.Down);
                    }
                    else if (movePossible(Direction.Up) == 2)
                    {
                        Move(Direction.Up);
                    }
                }
                else if (decision < 0.75f)//25%
                {
                    if (movePossible(Direction.Right) == 2)
                    {
                        Move(Direction.Right);
                    }
                    else if (movePossible(Direction.Left) == 2)
                    {
                        Move(Direction.Left);
                    }
                    else if (movePossible(Direction.Down) == 2)
                    {
                        Move(Direction.Down);
                    }
                    else if (movePossible(Direction.Up) == 2)
                    {
                        Move(Direction.Up);
                    }
                }
                else if (decision < 1f)//25%
                {
                    if (movePossible(Direction.Down) == 2)
                    {
                        Move(Direction.Down);
                    }
                    else if (movePossible(Direction.Up) == 2)
                    {
                        Move(Direction.Up);
                    }
                    else if (movePossible(Direction.Left) == 2)
                    {
                        Move(Direction.Left);
                    }
                    else if (movePossible(Direction.Right) == 2)
                    {
                        Move(Direction.Right);
                    }
                }
            }

        }


    }

}

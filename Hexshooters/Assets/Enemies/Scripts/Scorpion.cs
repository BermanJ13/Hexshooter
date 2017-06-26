using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScorpionStrike))]
public class Scorpion : Enemy {


    //interval for attack TEMPORARY
    [Header("Scorpion Vars")]
    public Vector2 randomAttackTimerRange = new Vector2(3,5);
    private float attackInterval = 0;
    private float attackTime = 0;
    Random rnd = new Random();
    private ScorpionStrike scorpionStrike;

    

    new void Start()
    {
        base.Start();
        //
        //TEMPORARY RANDOM INTERVAL
        //
        scorpionStrike = gameObject.GetComponent<ScorpionStrike>();
        attackInterval = Random.Range(randomAttackTimerRange.x,randomAttackTimerRange.y);
    }
    public override void enemyUpdate()
    {
        //does whatever is in the base enemy file, this is how they do movement
        base.enemyUpdate();
        attackTime = attackTime + Time.deltaTime;


        if(attackInterval < attackTime || scorpionStrike.attacking)
        {
            scorpionStrike.movement(0);
            if(!scorpionStrike.attacking)
            {
                attackTime = 0;
                attackInterval = Random.Range(randomAttackTimerRange.x, randomAttackTimerRange.y);
            }

        }
    }



    int tries = 0;
    bool[] these = { false, false, false, false };

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
                    if (movePossible(Direction.Up)==2)
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
                else if (decision < 0.5)//25%
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

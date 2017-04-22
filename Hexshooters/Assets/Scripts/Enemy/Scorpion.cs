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
        base.enemyUpdate();
        attackTime = attackTime + Time.deltaTime;


        if(attackInterval < attackTime || scorpionStrike.attacking)
        {
            scorpionStrike.movement(0);
            if(!scorpionStrike.attacking)
            {
                attackTime = 0;
                attackInterval = Random.Range(3, 5);
            }

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WildCatStrike))]
public class WildCat : Enemy
{


    //interval for attack TEMPORARY
    [Header("Scorpion Vars")]
    public Vector2 randomAttackTimerRange = new Vector2(2, 3);
    private float attackInterval = 0;
    private float attackTime = 0;
    Random rnd = new Random();
    private WildCatStrike wildCatStrike;


    new void Start()
    {
        base.Start();
        //
        //TEMPORARY RANDOM INTERVAL
        //
        wildCatStrike = gameObject.GetComponent<WildCatStrike>();
        attackInterval = Random.Range(randomAttackTimerRange.x, randomAttackTimerRange.y);
    }
    public override void enemyUpdate()
    {
        base.enemyUpdate();
        attackTime = attackTime + Time.deltaTime;


        if (attackInterval < attackTime || wildCatStrike.attacking)
        {
            wildCatStrike.movement(0);
            if (!wildCatStrike.attacking)
            {
                attackTime = 0;
                attackInterval = Random.Range(randomAttackTimerRange.x, randomAttackTimerRange.y);
            }

        }
    }

}

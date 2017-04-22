using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class EnemySpell : Spell
{

    public Enemy EnemyScript;
    public void Start()
    {
        EnemyScript = gameObject.GetComponent<Enemy>();
    }
}

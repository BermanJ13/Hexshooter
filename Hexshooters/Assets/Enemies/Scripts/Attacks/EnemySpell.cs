using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class EnemySpell : Spell
{

    //rear back variables
    [Header("Rear Back")]
    public float rearBackTime = 3;
    Vector3 rearedPos;
    bool rearing = false;
    float rearCounter;
    [System.NonSerialized]public bool hitAlready = false;

    [System.NonSerialized]public Vector3 oldPos;

    [System.NonSerialized]public Enemy EnemyScript;
    public void Start()
    {
        EnemyScript = gameObject.GetComponent<Enemy>();
    }

    public bool RearBack()
    {
        rearCounter = rearCounter + Time.deltaTime;
        //if it is not rearing set the oldPos to the current pos, then set rearing to true
        if (!rearing)
        {
            oldPos = gameObject.transform.position;
            rearedPos = new Vector3(oldPos.x + 0.3f, oldPos.y, oldPos.z);
            rearing = true;
        }

        if (rearCounter > rearBackTime)
        {
            return true;
        }
        else
        {
            gameObject.transform.position = rearedPos;
        }
        return false;
    }

    /// <summary>
    /// call this after the attack is over, but before you start the next one
    /// </summary>
    public void clearRear()
    {
        gameObject.transform.position = oldPos;
        rearing = false;
        rearCounter = 0;
    }
}

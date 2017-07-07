using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WanderType
{
    stand,
    patrolHorizontal,
    patrolVertical,
}
public class OverEnemy : MonoBehaviour {

    public OverPlayer target;

    public Vector2 startPosition;

    [Header("Behavior")]
    public WanderType movement;
    public int patrolDistance;
    public bool chase;
    public int chaseDistance;
    public int speed;
    public bool reverse;
    
    // Use this for initialization
	void Start () {
        startPosition = this.transform.position;
		if (!movement.Equals(WanderType.stand))
        {
            if (patrolDistance<=0)
            {
                patrolDistance = 5;
            }
            if (speed<=0)
            {
                speed = 1;
            }
        }
        if (chase == true)
        {
            if (chaseDistance<=0)
            {
                chaseDistance = 5;
            }
            if (speed <= 0)
            {
                speed = 1;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        switch (movement)
        {
            case WanderType.patrolHorizontal:
                
                if (!reverse)
                    this.transform.position = new Vector2(this.transform.position.x + 1, this.transform.position.y);
                else
                    this.transform.position = new Vector2(this.transform.position.x - 1, this.transform.position.y);

                if (this.transform.position.x >= (startPosition.x + (patrolDistance/2)))
                {
                    reverse = true;
                }
                else if (this.transform.position.x <=(startPosition.x - (patrolDistance/2)))
                {
                    reverse = false;
                }
                
                break;
            case WanderType.patrolVertical:
                if (!reverse)
                    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1);
                else
                    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 1);

                if (this.transform.position.y >= (startPosition.y + (patrolDistance / 2)))
                {
                    reverse = true;
                }
                else if (this.transform.position.y <= (startPosition.y - (patrolDistance / 2)))
                {
                    reverse = false;
                }
                break;
            case WanderType.stand:
            default:
                break;
        }
        if (chase)
        {
            stopChase(chaseDistance);
        }
	}
    public void stopChase(int chaseDistance)
    {

    }
}

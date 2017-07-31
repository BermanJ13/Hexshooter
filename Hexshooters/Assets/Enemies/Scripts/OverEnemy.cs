using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WanderType //new types can be added
{
    stand,
    patrolLine,
    patrolZag,
    patrolDiamond, 
    meander, //make random targets
    lurk //stand and then chase
}


public class OverEnemy : MonoBehaviour {

    public OverPlayer player;

    public Vector2 startPosition;

    public GameObject target;

    public int pointTraveled;
    public bool switchFlag;

    [Header("Behavior")]
    public WanderType movement;
    public float horizontalDistance;
    public float verticalDistance;
    public float speed; //between .005 and .3
    
    // Use this for initialization
	void Start () {
        //target = this.GetComponentInChildren<GameObject>();
        player = FindObjectOfType<OverPlayer>();
        startPosition = this.transform.position;
        pointTraveled = 0;
        switchFlag = false;

        if (speed <= 0)
        {
            speed = .005f;
        }
        else if (speed > .3)
        {
            speed = .3f;
        }

        switchTarget(movement);
	}

    // Update is called once per frame
    void Update()
    {
        move();
    }
    void move()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 0.1f);

        if (this.transform.position.x <= target.transform.position.x)
        {
            this.transform.position = new Vector2(this.transform.position.x + speed, this.transform.position.y);
            foreach (Collider2D c in hitColliders)
            {
                if (c.gameObject.tag == "Boundary")
                {
                    this.transform.position = new Vector2(this.transform.position.x - speed,this.transform.position.y);
                }
                else if (c.gameObject.tag == "EnemyTarget")
                {
                    switchFlag = true;
                }
            }
        }
        else
        {
            this.transform.position = new Vector2(this.transform.position.x - speed, this.transform.position.y);
            foreach (Collider2D c in hitColliders)
            {
                if (c.gameObject.tag == "Boundary")
                {
                    this.transform.position = new Vector2(this.transform.position.x + speed, this.transform.position.y);
                }
                else if (c.gameObject.tag == "EnemyTarget")
                {
                    switchFlag = true;
                }
            }
        }

        if (this.transform.position.y <= target.transform.position.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + speed);
            foreach (Collider2D c in hitColliders)
            {
                if (c.gameObject.tag == "Boundary")
                {
                    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - speed);
                }
                else if (c.gameObject.tag == "EnemyTarget")
                {
                    switchFlag = true;
                }
            }
        }
        else
        {
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - speed);
            foreach (Collider2D c in hitColliders)
            {
                if (c.gameObject.tag == "Boundary")
                {
                    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + speed);
                }
                else if (c.gameObject.tag == "EnemyTarget")
                {
                    switchFlag = true;
                }
            }
        }
        if (switchFlag)
        {
            switchFlag = false;
            switchTarget(movement);
        }
    }
    void switchTarget(WanderType wander)
    {
        Vector2 pos = startPosition;
        switch (wander)
        {
            case WanderType.patrolRectangle:
                if (pointTraveled == 0)
                    target.transform.position = new Vector2(pos.x + horizontalDistance, pos.y);
                else if (pointTraveled == 1)
                    target.transform.position = new Vector2(pos.x, pos.y + verticalDistance);
                else if (pointTraveled == 2)
                    target.transform.position = new Vector2(pos.x - horizontalDistance, pos.y);
                else if (pointTraveled == 3)
                    target.transform.position = new Vector2(pos.x, pos.y - verticalDistance);

                if (pointTraveled < 3)
                    pointTraveled++;
                else
                    pointTraveled = 0;
                break;
            case WanderType.patrolElbow:
                if (pointTraveled == 0)
                    target.transform.position = new Vector2(pos.x + horizontalDistance, pos.y);
                else if (pointTraveled == 1)
                    target.transform.position = new Vector2(pos.x, pos.y + verticalDistance);
                else if (pointTraveled == 2)
                    target.transform.position = new Vector2(pos.x, pos.y - verticalDistance);
                else if (pointTraveled == 3)
                    target.transform.position = new Vector2(pos.x - horizontalDistance, pos.y);

                if (pointTraveled < 3)
                    pointTraveled++;
                else
                    pointTraveled = 0;
                break;
            case WanderType.patrolLine:
                if (pointTraveled == 0)
                    target.transform.position = new Vector2(pos.x + horizontalDistance, pos.y + verticalDistance);
                else if (pointTraveled == 1)
                    target.transform.position = new Vector2(pos.x - horizontalDistance, pos.y - verticalDistance);

                if (pointTraveled == 0)
                    pointTraveled++;
                else
                    pointTraveled = 0;
                break;
            case WanderType.meander:
                target.transform.position = new Vector2(startPosition.x+Random.Range(-horizontalDistance, horizontalDistance), startPosition.y+ Random.Range(-verticalDistance, verticalDistance));
                break;
            case WanderType.lurk:
                if (Vector2.Distance(player.transform.position, pos) < Vector2.Distance(new Vector2(horizontalDistance / 2, verticalDistance / 2), pos))
                    target.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
                break;
            case WanderType.stand:
            default:
                break;
        }
    }
        
}

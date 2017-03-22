using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public enum EnemyState
{
    IShouldRunUp,
    IShouldRunDown
}

public class Enemy : MonoBehaviour {

	//health 
	public int health;
	public int armorWeakness;
	System.Timers.Timer timeCount = new System.Timers.Timer ();
	int burnTime =3;
	public string stat;
	public bool reload;
	bool breakImmune; //flag to ensure that every water shotgun spell doesn't endlessly apply break

    public bool isMoving;
    private Direction directionMoving;
    public float speed;
    private float distanceToMove;
    public float frozenModifier;
    public StatusManager myStatus;

    public EnemyState myState;


    public StatusManager statMngr = new StatusManager();
    

	// Use this for initialization
	void Start () {
		health = 100;
		//Debug.Log (health);
		stat = "normal";
		armorWeakness = 0;
		reload = true;
        breakImmune = false;

        isMoving = false;
        speed = 0.03f;
        distanceToMove = 0;
        frozenModifier = 0.3f;
        myState = EnemyState.IShouldRunUp;

        this.myStatus = this.GetComponent<StatusManager>();
	}
	
	// Update is called once per frame
	public void enemyUpdate () 
	{
        Debug.Log(health);
        //foreach (StatusEffect s in statMngr.m_effects)
        //{
        //    if (statMngr.IsAffected(s.m_type))
        //    {
        //        Status(s.m_type);
        //    }
        //}
	}

	//health
	public int Health()
	{
		return health;
	}

	//Dictates bullet beavior on the player
	public void Status(StatusType status)
	{
        if (status == StatusType.Burn)
        {
            timeCount.Elapsed += timer_Elapsed;
            int wait = 1 - (System.DateTime.Now.Second % 1);
            timeCount.Interval = wait * 1000;
            timeCount.Start();
            Debug.Log(burnTime);
            if (burnTime > 0)
            {
                health -= 3;
                Debug.Log(health);
                burnTime--;
            }
            else if (burnTime <= 0)
            {
                //status = "normal";
                burnTime = 3;
            }

        }

        if (status == StatusType.Break)
        {
            if (!breakImmune)
            {
                stat = "break";
            }
            else
            {
                breakImmune = false;
            }
        }

        if (status == StatusType.Slow)
        {

        }

        if (status == StatusType.Freeze)
        {

        }

	}

    public void Status(string status)
    {
        if (status == "burn")
        {
            timeCount.Elapsed += timer_Elapsed;
            int wait = 1 - (System.DateTime.Now.Second % 1);
            timeCount.Interval = wait * 1000;
            timeCount.Start();
            Debug.Log(burnTime);
            if (burnTime > 0)
            {
                health -= 3;
                Debug.Log(health);
                burnTime--;
            }
            else if (burnTime <= 0)
            {
                status = "normal";
                burnTime = 3;
            }

        }

        if (status == "break")
        {
            if (!breakImmune)
            {
                stat = "break";
            }
            else
            {
                breakImmune = false;
            }
        }

    }

    public void takeDamage(int damage) //created for "break" status
    {
        if (this.stat != "break")
        {
            this.health -= damage;
        }
        else
        {
            this.health -= (damage * 2);
            stat = "normal";
            breakImmune = true;
        }
    }

	void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		timeCount.Stop ();
		int wait = System.DateTime.Now.Second % 1;
		timeCount.Interval = wait * 1000;
		timeCount.Start ();
	}

    void FixedUpdate()
    {
        // Handles movement.
        if (isMoving)
        {
            Vector3 movementVector = new Vector3(0, 0, 0);
            float movementMag = speed;

            switch (directionMoving)
            {
                case Direction.Down:
                    movementVector = new Vector3(0, -1, 0);
                    break;
                case Direction.Left:
                    movementVector = new Vector3(-1, 0, 0);
                    break;
                case Direction.Right:
                    movementVector = new Vector3(1, 0, 0);
                    break;
                case Direction.Up:
                    movementVector = new Vector3(0, 1, 0);
                    break;
            }

            if (myStatus.IsAffected(StatusType.Freeze))
            {
                movementMag *= frozenModifier;
            }
            if(movementMag > distanceToMove)
            {
                movementMag = distanceToMove;
            }

            movementVector *= movementMag;

            this.transform.position += movementVector;
            distanceToMove -= movementMag;
            if(distanceToMove < 0.0001f)
            {
                distanceToMove = 0;
                isMoving = false;
            }
        }
        else //Normal actions.
        {
            switch (myState)
            {
                case EnemyState.IShouldRunDown:
                    directionMoving = Direction.Up;
                    isMoving = true;
                    distanceToMove = 1.0f;
                    myState = EnemyState.IShouldRunUp;
                    break;
                case EnemyState.IShouldRunUp:
                    //Finished running up, run down.
                    directionMoving = Direction.Down;
                    isMoving = true;
                    distanceToMove = 1.0f;
                    myState = EnemyState.IShouldRunDown;
                    break;
            }
        }
    }
}

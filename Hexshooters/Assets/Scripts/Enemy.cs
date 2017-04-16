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
    int stackDmg;

    //interval for attack TEMPORARY
    private float attackInterval=0;
    private float attackTime=0;
    Random rnd = new Random();

    //basic attack variables
    [Header("Basic Attack")]
    public int basicAttackDamage  = 10;
    public float basicAttackStayTime = 1;
    float attackCounter = 0;
    bool attacking = false;
    Vector3 playerPos;
    bool hitPlayer = false;

    //rear back variables
    [Header("Rear Back")]
    public float rearBackTime = 3;
    bool rearing= false;
    float rearCounter;
    Vector3 oldPos;

    [Header("Moving")]
    public bool isMoving;
    private Direction directionMoving;
    public float speed;
    private float distanceToMove;
    public float frozenModifier;
    public StatusManager myStatus;

    public EnemyState myState;


    public StatusManager statMngr = new StatusManager();
    
    public bool RearBack()
    {
        rearCounter = rearCounter + Time.deltaTime;
        if(!rearing)
        {
            oldPos = gameObject.transform.position;
            rearing = true;
        }

        if (rearCounter > rearBackTime)
        {
            return true;
        }
        else
        {
            gameObject.transform.position = new Vector3(oldPos.x+0.3f, oldPos.y,oldPos.z);
        }
        return false;
    }

    void BasicAttack()
    {
        if (!attacking)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            attacking = true;
        }
            if(RearBack())
            {
            attackCounter = attackCounter + Time.deltaTime;
            gameObject.transform.position = playerPos;
            if(Vector3.Magnitude(GameObject.FindGameObjectWithTag("Player").transform.position-gameObject.transform.position) < 1 && !hitPlayer)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().takeDamage(basicAttackDamage);
                Debug.Log("Health: " +GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health);
                hitPlayer = true;
            }
                if (attackCounter > basicAttackStayTime)
                {
                    gameObject.transform.position = oldPos;
                     rearing = false;
                     attacking = false;
                     rearCounter = 0;
                     attackCounter = 0;
                        isMoving = true;
                     attackTime = 0;
                     attackInterval = Random.Range(3, 5);
                    hitPlayer = false;
                 }
            }
    }


	// Use this for initialization
	void Start () {
		health = 100;
		//Debug.Log (health);
		stat = "normal";
		armorWeakness = 0;
		reload = true;
        breakImmune = false;
        stackDmg = 0;

        isMoving = false;
        speed = 0.03f;
        distanceToMove = 0;
        frozenModifier = 0.3f;
        myState = EnemyState.IShouldRunUp;

        //
        //TEMPORARY RANDOM INTERVAL
        //
        attackInterval = Random.Range(3,5);

        this.myStatus = this.GetComponent<StatusManager>();
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
		int multipliers = 1;
		if (myStatus.IsAffected(StatusType.Break))
		{
			multipliers *= 2;
		}
		if (myStatus.IsAffected(StatusType.Shield))
		{
			multipliers /= 2;
		}
		if (myStatus.IsAffected(StatusType.Stacking))
		{
			this.health -= stackDmg;
			stackDmg++;
		}
		else
		{
			stackDmg = 0;
		}

		this.health -= damage* multipliers + stackDmg;
	}

	void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		timeCount.Stop ();
		int wait = System.DateTime.Now.Second % 1;
		timeCount.Interval = wait * 1000;
		timeCount.Start ();
	}

    public void enemyUpdate()
    {
        //
        //TEMPORARY ATTACK CALL
        //
        attackTime = attackTime + Time.deltaTime;
        if(attackTime > attackInterval)
        {
            isMoving = false;
            BasicAttack();
        }


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
            else if (myStatus.IsAffected(StatusType.Slow))
            {
                movementMag *= (frozenModifier / 2);
            }
            if (movementMag > distanceToMove)
            {
                movementMag = distanceToMove;
            }
        
            movementVector *= movementMag;
        
            this.transform.position += movementVector;
            distanceToMove -= movementMag;
            if (distanceToMove < 0.0001f)
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

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("asdf");
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().takeDamage(basicAttackDamage);
            Debug.Log(other.GetComponent<Player>().health);
        }
    }
}

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

public abstract class Enemy : MonoBehaviour {

	//health 
	public int health = 100;
	public int armorWeakness;
	System.Timers.Timer timeCount = new System.Timers.Timer ();
	int burnTime =3;
	public string stat;
	public bool reload;
	bool breakImmune; //flag to ensure that every water shotgun spell doesn't endlessly apply break
    int stackDmg;
	public bool MarkedForDeletion;
    Random rnd = new Random();
	public bool hit = false;


    [Header("Moving")]
    public float frozenModifier;
    public float movementBreak = 1;

    [System.NonSerialized]
    public bool isMoving = true;
    private Direction directionMoving;
    private float distanceToMove;
    
    public EnemyState myState;


    [System.NonSerialized]
    public StatusManager statMngr = new StatusManager();


    // The timer for determining when to take an action.
    public virtual float TIME_PER_ACTION
    {
        get
        {
            return movementBreak;
        }
    }



    protected float timer;
    // Slows the characters by this amount when frozen.
    public const float FROZEN_MULTIPLIER = 0.5f;

    // Necessary components information-wise.
    [System.NonSerialized]
    public StatusManager myStatus;
    [System.NonSerialized]
    public Enemy myEnemy;
    

    // The current position of the enemy
    // on the board.
    [System.NonSerialized]
    public int currentX;
    [System.NonSerialized]
    public int currentY;

    // Used to transform the index into a location on the board.
    // Modify as necessary for UI and collision to be correct;
    protected const float STEP_UNIT = 1.0f;
    protected const float X_OFFSET = 0.0f;
    protected const float Y_OFFSET = 0.0f;

    protected bool isInitialized = false;

    // Takes the player's current position and returns where that should be in world space.
    public Vector3 PositionToWorldspace()
    {
        return new Vector3(X_OFFSET + STEP_UNIT * currentX, Y_OFFSET + STEP_UNIT * currentY, 0);
    }


    /// <summary>
    /// Initializes AI specific functions.  MUST BE CALLED AFTER ENEMY CREATION.
    /// </summary>
    /// <param name="initialX"></param>
    /// <param name="initialY"></param>
    public virtual void Initialize(int initialX, int initialY)
    {
        this.isInitialized = true;
        this.currentX = initialX;
        this.currentY = initialY;
        this.transform.position = PositionToWorldspace();
    }

    // Use this for initialization of unity scene properties
    public virtual void Start()
    {
        Initialize((int)gameObject.transform.position.x, (int)gameObject.transform.position.y);
        isMoving = true;
        this.timer = TIME_PER_ACTION;
        this.myStatus = this.GetComponent<StatusManager>();


        //Debug.Log (health);
        stat = "normal";
        armorWeakness = 0;
        reload = true;
        breakImmune = false;
        stackDmg = 0;
        
        distanceToMove = 0;
        frozenModifier = 0.3f;
        myState = EnemyState.IShouldRunUp;

        this.myStatus = this.GetComponent<StatusManager>();

    }

    // Update is called once per frame
    public virtual void enemyUpdate()
    {
        if ((myStatus.IsAffected(StatusType.Slow) || myStatus.IsAffected(StatusType.Freeze))&isMoving)
        {
            timer -= Time.deltaTime * FROZEN_MULTIPLIER;
        }
        else
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0.0f)
        {
            timer += TIME_PER_ACTION;
            if (!isInitialized)
            {
                Debug.LogError(this.gameObject.name + " is an enemy that Initialize() was never called on.  Wherever you created this enemy, call Initialize on it.");
            }
            AIStep();
        }

        if (health <= 0)
        {
            MarkedForDeletion = true;
        }
        
		if (hit)
		{
			GetComponent<SpriteRenderer>().color = Color.red;
			hit = false;
		}
		else
		{
			GetComponent<SpriteRenderer>().color = Color.white;
		}
    }

    /// <summary>
    /// All the movements the enemy takes.
    /// </summary>
    public abstract void AIStep();

    /// <summary>
    /// returns 0 when you cant go through
    /// 1 when you can, but there is some kind of obstacle there
    /// 2 when you can and there is nothing in the way
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public int movePossible(Direction direction)
    {
        Debug.Log(direction);
        Collider2D[] hitColliders;
        switch (direction)
        {
            case Direction.Down:
                hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 1), 0.2f);
                foreach (Collider2D c in hitColliders)
                {
                    Debug.Log(c.gameObject.tag);
                    //Checks whether or not something is in the way or if the desired spot is within the enemy.
                    if (c.gameObject.tag == "Obstacle" && c.gameObject.GetComponent<Obstacle>().canPass)
                    {
                        return 1;
                    }
                    if (c.gameObject.tag == "enemyZone")
                    {
                        return 2;
                    }
                    return 0;
                }

                break;

            case Direction.Up:
                hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + 1), 0.2f);
                foreach (Collider2D c in hitColliders)
                {
                    Debug.Log(c.gameObject.tag);
                    //Checks whether or not something is in the way or if the desired spot is within the enemy.
                    if (c.gameObject.tag == "Obstacle" && c.gameObject.GetComponent<Obstacle>().canPass)
                    {
                        return 1;
                    }
                    if (c.gameObject.tag == "enemyZone")
                    {
                        return 2;
                    }
                    return 0;
                }
                break;

            case Direction.Left:
                hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x-1, transform.position.y), 0.2f);
                foreach (Collider2D c in hitColliders)
                {
                    Debug.Log(c.gameObject.tag);
                    //Checks whether or not something is in the way or if the desired spot is within the enemy.
                    if (c.gameObject.tag == "Obstacle" && c.gameObject.GetComponent<Obstacle>().canPass)
                    {
                        return 1;
                    }
                    if (c.gameObject.tag == "enemyZone")
                    {
                        return 2;
                    }
                    return 0;
                }
                break;

            case Direction.Right:
                hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x+1, transform.position.y), 0.2f);
                foreach (Collider2D c in hitColliders)
                {
                    Debug.Log(c.gameObject.tag);
                    //Checks whether or not something is in the way or if the desired spot is within the enemy.
                    if (c.gameObject.tag == "Obstacle" && c.gameObject.GetComponent<Obstacle>().canPass)
                    {
                        return 1;
                    }
                    if (c.gameObject.tag == "enemyZone")
                    {
                        return 2;
                    }
                    return 0;
                }
                break;
        }
        return 0;
    }

    public virtual bool Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Down:
                    currentY--;
                    transform.position = PositionToWorldspace();
                    return true;

                break;

            case Direction.Up:
                    currentY++;
                    transform.position = PositionToWorldspace();
                    return true;
                break;

            case Direction.Left:
                    currentX--;
                    transform.position = PositionToWorldspace();
                    return true;
                break;

            case Direction.Right:
                    currentX++;
                    transform.position = PositionToWorldspace();
                    return true;
                break;
        }

        return false;
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
		hit = true;
	}

	void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		timeCount.Stop ();
		int wait = System.DateTime.Now.Second % 1;
		timeCount.Interval = wait * 1000;
		timeCount.Start ();
	}
    
}

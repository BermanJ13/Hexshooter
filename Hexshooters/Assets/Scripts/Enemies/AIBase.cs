using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBase : MonoBehaviour {

    // The timer for determining when to take an action.
    public abstract float TIME_PER_ACTION { get; }
    protected float timer;
    // Slows the characters by this amount when frozen.
    public const float FROZEN_MULTIPLIER = 0.5f;

    // Necessary components information-wise.
    public StatusManager myStatus;
    public Enemy myEnemy;

    // The 4 bound variables, the enemy
    // will follow these base rules for
    // movement.  Bounds are inclusive.
    protected int minX;
    protected int minY;
    protected int maxX;
    protected int maxY;

    // The current position of the enemy
    // on the board.
    public int currentX;
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
	public virtual void Start () {
        this.timer = TIME_PER_ACTION;
        this.myStatus = this.GetComponent<StatusManager>();
        this.myEnemy = this.GetComponent<Enemy>();

	}
	
	// Update is called once per frame
	public virtual void enemyUpdate () {
        if (myStatus.IsAffected(StatusType.Slow) || myStatus.IsAffected(StatusType.Freeze))
        {
            timer -= Time.deltaTime * FROZEN_MULTIPLIER;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if(timer <= 0.0f)
        {
            timer += TIME_PER_ACTION;
            if (!isInitialized)
            {
                Debug.LogError(this.gameObject.name + " is an enemy that Initialize() was never called on.  Wherever you created this enemy, call Initialize on it.");
            }
            AIStep();
        }
	}

    /// <summary>
    /// All the movements the enemy takes.
    /// </summary>
    public abstract void AIStep();

    public virtual bool Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Down:
                if(currentY - 1 >= minY)
                {
                    currentY--;
                    transform.position = PositionToWorldspace();
                    return true;
                }

                break;

            case Direction.Up:
                if (currentY + 1 <= maxY)
                {
                    currentY++;
                    transform.position = PositionToWorldspace();
                    return true;
                }
                break;

            case Direction.Left:
                if (currentX - 1 >= minX)
                {
                    currentX--;
                    transform.position = PositionToWorldspace();
                    return true;
                }
                break;

            case Direction.Right:
                if (currentX + 1 <= maxX)
                {
                    currentX++;
                    transform.position = PositionToWorldspace();
                    return true;
                }
                break;
        }

        return false;
    }
}

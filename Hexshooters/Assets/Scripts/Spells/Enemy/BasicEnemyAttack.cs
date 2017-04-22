using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicEnemyAttack : EnemySpell {

    protected bool revolverMove;
    protected Vector2 rifleOrigin;
    private int spellTimer;

    // Use this for initialization
    [Header("Basic Attack")]
    public int basicAttackDamage = 10;
    public float basicAttackStayTime = 1;
    float attackCounter = 0;
    bool attacking = false;
    Vector3 playerPos;
    bool hitPlayer = false;



    //interval for attack TEMPORARY
    private float attackInterval = 0;
    private float attackTime = 0;
    Random rnd = new Random();

    new void Start()
    {
        base.Start();
        revolverMove = false;
        rifleOrigin = transform.position;
        //
        //TEMPORARY RANDOM INTERVAL
        //
        attackInterval = Random.Range(3, 5);
    }

    // Update is called once per frame
    new void spellUpdate()
    {
        base.spellUpdate();
        Debug.Log(speed);
    }
    
    public override void movement(int weapon)
    {
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

    public override void hitBehavior(int weapon)
    {
        if (!attacking)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            attacking = true;
        }
        if (EnemyScript.RearBack())
        {
            attackCounter = attackCounter + Time.deltaTime;
            gameObject.transform.position = playerPos;
            if (Vector3.Magnitude(GameObject.FindGameObjectWithTag("Player").transform.position - gameObject.transform.position) < 1 && !hitPlayer)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().takeDamage(basicAttackDamage);
                Debug.Log("Health: " + GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health);
                hitPlayer = true;
            }
            if (attackCounter > basicAttackStayTime)
            {
                attacking = false;
                attackCounter = 0;
                EnemyScript.isMoving = true;
                attackTime = 0;
                attackInterval = Random.Range(3, 5);
                hitPlayer = false;
            }
        }
    }
    public override void setDescription(int weapon)
    {
        description = "Go up to player and smakem";
    }

}

   

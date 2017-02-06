using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell : MonoBehaviour {

	public string[] attributes;
	public string[] effects;
	public int damage;
	protected GameObject[] enemies; 
	protected List<Collider2D> enemyColliders = new List<Collider2D> ();
	public int damageTier;
	public int weaponUsed;
	public Vector2 direction;
	public int hitNum;
	protected bool markedForDeletion;
	public bool MarkedForDeletion
	{
		get { return markedForDeletion;}
	}

	// Use this for initialization
	public void Start () 
	{
		markedForDeletion = false;
		//we never init the list
		//List<Collider2D> enemyColliders = new List<Collider2D> ();
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		foreach(GameObject enemy in enemies)
		{
			enemyColliders.Add(enemy.GetComponent<Collider2D>());
		}
	}
	
	// Update is called once per frame
	public void Update () 
	{
		movement (weaponUsed);
		if(enemyColliders != null)
		{
			foreach(Collider2D enemy in enemyColliders)
			{
				if(GetComponent<Collider2D>().IsTouching(enemy))
				{
					hitBehavior(weaponUsed);
				}
			}
		}
	}

	//Movement of the bullets through the grid. Split up by weapon if necessary.
	public virtual void movement(int weapon)
	{

	}

	//Calculates the damage based on the bullet type and the spell damage
	public virtual int damageCalc(int tier, int hits)
	{
		Debug.Log ("tier "+tier);
		Debug.Log ("Num"+hits);
		Debug.Log ("dmg"+damage);
		return (damage*tier)*hits;
	}

	//Dictatees any effects that happen wqhen the bullet hits an enemy
	public virtual void hitBehavior(int weapon)
	{

	}

	//Dictates bullet beavior on the player
	public virtual void selfStatus()
	{

	}

	//Destroys the spell when it exits the camera view.
	public void OnBecameInvisible() 
	{
		markedForDeletion = true;

	}
}

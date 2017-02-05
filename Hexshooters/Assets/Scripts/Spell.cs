using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell : MonoBehaviour {

	public string[] attributes;
	public string[] effects;
	public int damage;
	private GameObject[] enemies; 
	private List<Collider2D> enemyColliders;
	public int damageTier;
	public int weaponUsed;
	public Vector2 direction;
	public int hitNum;
	private bool markedForDeletion;
	public bool MarkedForDeletion
	{
		get { return markedForDeletion;}
	}


	// Use this for initialization
	public void Start () 
	{
		markedForDeletion = false;
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
					hitBehavior();
				}
			}
		}
		Debug.Log (markedForDeletion);
	}

	//Movement of the bullets through the grid. Split up by weapon if necessary.
	public virtual void movement(int weapon)
	{

	}

	//Calculates the damage based on the bullet type and the spell damage
	public virtual int damageCalc(int tier, int hitNum)
	{
		return damage*tier*hitNum;
	}

	//Dictatees any effects that happen wqhen the bullet hits an enemy
	public virtual void hitBehavior()
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

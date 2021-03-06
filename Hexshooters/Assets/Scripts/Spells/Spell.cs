﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell : MonoBehaviour {

	public string[] attributes;
	public string[] effects;
	public int damage;
	public int damageTier;
	public int weaponUsed;
	public int speed; //time.deltatime multiplier which is typically 8
	public Vector2 direction;
	public int hitNum;
	protected bool markedForDeletion;
	public List<GameObject> hitEnemies= new List<GameObject> (); 
	public Sprite bulletImage;
	public Sprite runeImage;
	public int PlayerNum;
	public bool MarkedForDeletion
	{
		get { return markedForDeletion;}
	}
	public string description; 

	// Use this for initialization
	public void Start () 
	{
		markedForDeletion = false;
		//we never init the list
		//List<Collider2D> enemyColliders = new List<Collider2D> ();
		if (PlayerNum != 1)
		{
			transform.localEulerAngles = new Vector3(0f,0f,180f);
		}
	}
	// Update is called once per frame
	public void spellUpdate () 
	{
		this.movement (weaponUsed);
		Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x, transform.position.y));
		foreach (Collider2D c in colliders)
		{
			if (c.gameObject.tag == "Player" || c.gameObject.tag == "Enemy" || c.gameObject.tag == "Obstacle"|| c.gameObject.tag == "Player2" )
			{
				hitBehavior(weaponUsed);
			} 
			if (c.gameObject.tag == "playerZone" || c.gameObject.tag == "enemyZone")
			{
				showPanels (c);
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
		//Debug.Log ("tier "+tier);
		//Debug.Log ("Num"+hits);
		//Debug.Log ("dmg"+damage);
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
	//Dictates bullet beavior on the player
	public virtual void setDescription(int weapon)
	{

	}

	//Destroys the spell when it exits the camera view.
	public void OnBecameInvisible() 
	{
		markedForDeletion = true;

	}
	public void showPanels(Collider2D c)
	{
		c.gameObject.gameObject.GetComponent<Panel> ().attacked = true;
	}
}
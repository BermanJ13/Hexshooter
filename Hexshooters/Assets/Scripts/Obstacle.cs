using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public int health;
	public int armorWeakness;
	public bool MarkedforDeletion;
	public string stat;
	bool breakImmune; //flag to ensure that every water shotgun spell doesn't endlessly apply break


	// Use this for initialization
	void Start () {
		MarkedforDeletion = false;
	}

	// Update is called once per frame
	public void Update () 
	{
		transform.position = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z));
		if (health <= 0)
		{
			MarkedforDeletion = true;
		}
	}
	public void takeDamage(int damage) //created for "break" status
	{
		int multipliers = 1;
		if (this.stat == "break")
		{
			multipliers *= 2;
			stat = "normal";
			breakImmune = true;
		}
		this.health -= damage* multipliers;
	}
}

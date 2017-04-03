using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public int health;
	public int armorWeakness;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	public void Update () 
	{
		transform.position = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z));
	}
}

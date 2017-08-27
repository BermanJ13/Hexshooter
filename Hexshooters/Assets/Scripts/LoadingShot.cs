using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingShot : MonoBehaviour {

	//controls the position the particle goes
	public GameObject targetPos;
	public GameObject startPos;
	public GameObject influence;

	//flowing particle
	public GameObject particle;

	//which element to play
	public int index;

	//list of all elemntal particles
	public GameObject[] particleList;

	//affects how long the particle takes best 5 - 15
	public float duration;


	private Vector3 velocity;

	//holds the start time
	private float startTime;

	//whether the it is active
	private bool active = false;
		
	// Update is called once per frame
	void Update () {
		//calls when space is press activates the particles
		if (Input.GetKeyDown (KeyCode.Space)) {
			Activate ();
		}

		//when active
		if (active) {
			//gets the amount of time that passed
			float timeDifference = Time.time - startTime;

			//gets ratio
			float frac = (timeDifference * 10) / duration;

			//lerps between the two points
			velocity = Vector3.Lerp (influence.transform.position - particle.transform.position,
				targetPos.transform.position - particle.transform.position,
				frac);
			//normalizes the vector
			velocity.Normalize ();

			//rotate the object so it faces the direction its going
			particle.transform.LookAt (particle.transform.position + velocity);

			//adds the velocity
			particle.transform.position += velocity / duration;

			//explodes when it hits 
			if (frac > 1) {
				active = false;//turns of the trailing particle
				particle.SetActive (false);
				GameObject tmp = Instantiate (particleList [index], particle.transform.position, Quaternion.identity);
				Destroy (tmp, 2.0f);

			}
		}
	}

	void Activate()
	{
		//set particles to active
		particle.SetActive (true);

		//set time
		startTime = Time.time;
		particle.transform.position = startPos.transform.position;
		active = true;


	}

}

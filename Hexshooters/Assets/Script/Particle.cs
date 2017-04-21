using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

	public string name;
	public float speed = .1f;
	public float distance;
	public float time;





	/*
	// Use this for initialization
	public GameObject player;

	public GameObject mainProjectile;
	public ParticleSystem mainParticleSystem;
	public TrailRenderer mainTrail;

	public float speed = .1f;

	private Vector3 velocity;
	public float distance;
	public float time;
	//public ParticleSystem[] mainParticleSystem;

	private float timeActive;
	void Start () {
		velocity = new Vector3 (speed, 0, 0);
		speed = distance / time;


	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Space) && !mainProjectile.active)
		{
			mainProjectile.transform.position = player.transform.position;
			mainProjectile.SetActive (true);
			timeActive = 0;
		}


		if (timeActive >= time) {
			mainProjectile.SetActive (false);
			mainTrail.Clear ();
		} else {
			mainProjectile.transform.position += velocity;
			timeActive += Time.deltaTime;
		}


	}

	void OnCollisionEnter(Collision col)
	{
		Debug.Log ("working");
		if (col.gameObject.tag == "collide") {
			mainProjectile.SetActive (false);

		}

	}

	void getStatus()
	{
		
	}*/
}

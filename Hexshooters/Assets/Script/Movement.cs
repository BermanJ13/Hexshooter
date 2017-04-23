using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public GameObject player;

	private float speed = .0f;
	private Vector3 velocity;

	private Vector3 acceleration;
	private float maxSpeed;
	// Use this for initialization
	void Start () {
		player.transform.position = new Vector3 (0, 0, 0);	
		acceleration = new Vector3 (0, .01f, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (Input.GetKey (KeyCode.W))
			player.transform.position += acceleration;
		if (Input.GetKey (KeyCode.S))
			player.transform.position -= acceleration;


		//Vector3.ClampMagnitude (velocity, maxSpeed);



	}
}

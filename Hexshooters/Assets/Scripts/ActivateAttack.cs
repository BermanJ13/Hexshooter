using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAttack : MonoBehaviour {

	// Use this for initialization
	public GameObject player;

	public GameObject[] ProjectileList;
	public ParticleSystem[] ParticleSystemList;
	public TrailRenderer[] TrailRenderList;

	public int prev;
	public int num = 0;
	public float speed = .1f;

	private Vector3 velocity;
	public float distance;
	public float time;
	//public ParticleSystem[] mainParticleSystem;

	private float timeActive;
	void Start () {
		
		speed = distance / time;
		velocity = new Vector3 (speed, 0, 0);



	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Space) && !ProjectileList[num].active)
		{
			speed = distance / time;
			velocity = new Vector3 (speed, 0, 0);
			ProjectileList[num].transform.position = player.transform.position;
			ProjectileList[num].SetActive (true);
			timeActive = 0;
		}



		if (timeActive >= time) {
			ProjectileList[num].SetActive (false);
			TrailRenderList[num].Clear ();
		} else {
			ProjectileList[num].transform.position += velocity;
			timeActive += Time.deltaTime;
		}

		if (Input.GetKey (KeyCode.UpArrow) && num != prev) {
			prev = num;
			ProjectileList[num].SetActive (false);
			TrailRenderList[num].Clear ();
			num++;
			if (num >= ParticleSystemList.Length)
				num = 0;
			
			
		}

		if (Input.GetKey (KeyCode.DownArrow) && num != prev) {
			prev = num;
			ProjectileList[num].SetActive (false);
			TrailRenderList[num].Clear ();
			num--;
			if (num < 0)
				num = ParticleSystemList.Length - 1;
			
		}
	
	}




}

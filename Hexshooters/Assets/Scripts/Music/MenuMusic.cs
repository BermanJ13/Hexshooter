using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour {


	// references to audio sources
	public AudioClip FightMusic;
	AudioSource Music;


	// Use this for initialization
	void Start () {
		// start music
		Music = this.GetComponent<AudioSource>();
		Music.loop = true;
		Music.Play();
	}

	// Update is called once per frame
	void Update () {

	}


	
}

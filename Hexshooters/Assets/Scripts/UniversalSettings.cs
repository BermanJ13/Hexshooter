using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalSettings : MonoBehaviour {
	public float volume;
	public int style;
	public bool filter;
	// Use this for initialization
	void Start () {
		volume = 1.0f; 
		style = 0;
		filter = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
}

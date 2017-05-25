using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalSettings : MonoBehaviour {
	public float volume;
	public int style;
	public int pvpStyle;
	public bool filter;
	public string mapfile;
	// Use this for initialization
	void Start () {
		volume = 1.0f; 
		style = 0;
		pvpStyle = 0;
		filter = true;
		mapfile = "";
	}
	
	// Update is called once per frame
	void Update () {
	}
}

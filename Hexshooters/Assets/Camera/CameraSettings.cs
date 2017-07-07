using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour {
	OLDTVTube otv;
	UniversalSettings us;
	// Use this for initialization
	void Start () {
		otv = this.gameObject.GetComponent<OLDTVTube>();
		us = GameObject.Find("__app").GetComponent<UniversalSettings> ();
	}
	
	// Update is called once per frame
	void Update () {
		otv.enabled = us.filter;
		AudioListener.volume = us.volume;
	}
}

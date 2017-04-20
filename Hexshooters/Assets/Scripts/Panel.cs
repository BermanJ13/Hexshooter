using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {
	public bool attacked;
	protected Color defColor;
	// Use this for initialization
	void Start () {
		defColor = GetComponent<SpriteRenderer> ().color;
		attacked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (attacked)
		{
			Color c = new Color (255f, 255f, 0f, .7f);
			GetComponent<SpriteRenderer> ().color = c;
		}
		else
		{
			GetComponent<SpriteRenderer> ().color = defColor;
		}
		attacked = false;
	}
}

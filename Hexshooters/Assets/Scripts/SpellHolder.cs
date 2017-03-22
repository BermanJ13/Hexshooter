using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHolder : MonoBehaviour {
	public List<Transform> children = new List<Transform>();
	// Use this for initialization
	void Start () {
		foreach (Transform child in transform)
		{
			children.Add (child);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void activateSpell(string name)
	{
		for (int i = 0; i < children.Count; i++)
		{
			//Debug.Log (name);
			if (children [i].gameObject.name == name)
			{
				children[i].gameObject.SetActive (true);
			}
		}
	}
	public void deactivateSpell(string name)
	{
		for (int i = 0; i < children.Count; i++)
		{
			if (children [i].gameObject.name == name)
			{
				//Debug.Log ("Hi");
				children[i].gameObject.SetActive (false);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Spell {

	// Use this for initialization
	new void Start () {
		base.Start ();
	}

	// Update is called once per frame
	new void Update() {
		base.Update ();
	}

	public override void movement(int weapon)
	{
		switch (weapon) 
		{
		case 1:
			Vector2 target = new Vector2 (transform.position.x, transform.position.y) + direction;
			Vector2 position = Vector2.Lerp (transform.position, target, Time.deltaTime);
			transform.position = position;
			break;
			
		}
	}

}

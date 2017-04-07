using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Spell {

    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void spellUpdate()
    {
        base.spellUpdate();
    }

    public override void movement(int weapon)
    {
		Vector2 target, position;
        switch (weapon)
        {
            //revolver
		case 1:
            //rifle
		case 2:
            //shotgun
		case 3:
            //gatling
		case 4:
            //cane gun - not priority
		case 5:
			if (PlayerNum == 1)
			{
				target = new Vector2(transform.position.x, transform.position.y) + direction;
			} 
			else
			{
				target = new Vector2(transform.position.x, transform.position.y) - direction;
			}
                position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;
        }
    }
	public override void setDescription(int weapon)
	{
		switch (weapon)
		{
			//Revolver
			case 1:
			description = "Knocks the enmy up or down one tile.";
			break;
			//Rifle
			case 2:
			description = "";
			break;
			//Shotgun
			case 3:
			description = "Knocks the enemy back with a burst of compressed air.";
			break;
			//Gatling
			case 4:
			description = "Pulls enemies toward the column it lands in.";
			break;
			//Cane Gun
			case 5:
			description = "";
			break;
		}
	}
}

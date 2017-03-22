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
        switch (weapon)
        {
            //revolver
            case 1:
                Vector2 target = new Vector2(transform.position.x, transform.position.y) + direction;
                Vector2 position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;

            //rifle
            case 2:
                target = new Vector2(transform.position.x, transform.position.y) + direction;
                position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;

            //shotgun
            case 3:
                target = new Vector2(transform.position.x, transform.position.y) + direction;
                position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;

            //gatling
            case 4:
                target = new Vector2(transform.position.x, transform.position.y) + direction;
                position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;

            //cane gun - not priority
            case 5:
                target = new Vector2(transform.position.x, transform.position.y) + direction;
                position = Vector2.Lerp(transform.position, target, Time.deltaTime);
                transform.position = position;
                break;
        }
    }
}

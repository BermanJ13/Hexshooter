using UnityEngine;
using System.Collections;

public class FieldManager : MonoBehaviour 
{
	public Transform playerPanel;
	public Transform enemyPanel;
	public Transform player;
	
	// Use this for initialization
	void Start () 
	{
		//Creates the Grid
		for (int y = 0; y < 5; y++) 
		{
			for (int x = 0; x < 10; x++) 
			{
				//Checks whether the current panel is for the enmy or player side
				if(x<5)
				{
					Instantiate(playerPanel, new Vector3(x, y, 0), Quaternion.identity);
					//sPawns the Player
					if(y==2 && x==0)
						Instantiate(player, new Vector3(x, y, 0), Quaternion.identity);
				}
				else
					Instantiate(enemyPanel, new Vector3(x, y, 0), Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		deleteSpells ();
	}

	void deleteSpells()
	{

		GameObject[] spells = GameObject.FindGameObjectsWithTag ("Spell");
		if (spells != null) 
		{
			foreach (GameObject spell in spells) 
			{
				//Debug.Log (spell.GetComponent<Spell> ().MarkedForDeletion);
				if (spell.GetComponent<Spell> ().MarkedForDeletion) 
				{
					Debug.Log ("blah");
					Destroy (spell);
				}
			}
		}

	}
}

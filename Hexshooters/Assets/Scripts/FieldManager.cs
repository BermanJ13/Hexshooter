using UnityEngine;
using System.Collections;

public class FieldManager : MonoBehaviour 
{
	public Transform playerPanel;
	public Transform enemyPanel;
	public Transform player;
	public Transform Testdummy;

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
					Instantiate(Resources.Load("Player_Panel"), new Vector3(x, y, 0), Quaternion.identity);
					//sPawns the Player
					if(y==2 && x==0)
						Instantiate(Resources.Load("Player"), new Vector3(x, y, 0), Quaternion.identity);
				}
				else
					Instantiate(Resources.Load("Enemy_Panel"), new Vector3(x, y, 0), Quaternion.identity);
			}
		}

		//test Dummy
		Instantiate (Resources.Load("TestDummy"),new Vector3(6,3,0),Quaternion.identity);

		Instantiate (Resources.Load("TestDummy"),new Vector3(5,4,0),Quaternion.identity);

		Instantiate (Resources.Load("TestDummy"),new Vector3(7,2,0),Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () 
	{
		deleteSpells ();
		GameObject[] spells = GameObject.FindGameObjectsWithTag ("Enemy");
		if (spells != null) 
		{
			foreach (GameObject spell in spells) 
			{
				spell.GetComponent<Enemy> ().Update ();
			}
		}
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
					Destroy (spell);
				}
			}
		}

	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class FieldManager : MonoBehaviour 
{
    public string mapFile;
    [SerializeField]
    public Transform[] gamePieces;
    public Dictionary<string, Transform> things = new Dictionary<string, Transform>();
    private StreamReader reader;
    private List<string> rows = new List<string>();


	// Use this for initialization
	void Start () 
	{
        //creates a dictionary out of the list of objects made in the inspector
        foreach(Transform trns in gamePieces)
        {
            things.Add(trns.name, trns);
        }

        //open ups the streamreader then reads every line and adds it to the rows list
        reader = new StreamReader("Assets/Maps/"+mapFile+".txt");
        string line = null;
        line = reader.ReadLine();
        while (line!=null)
        {
            rows.Add(line);
            line = reader.ReadLine();
        }

        //use this in the foreach loop to hold the first two values
        //which is the position of the objects
        Vector2 place;
        //for each string in rows split the string into tiles
        foreach (string a in rows)
        {
            string[] tiles = a.Split(' ');
            //foreach string in tiles split it into entries
            foreach(string b in tiles)
            {
                string[] entry = b.Split(',');
                //set the place vector from before from the first two enties in the tile
                place = new Vector2(float.Parse(entry[0]), float.Parse(entry[1]));

                //put down either a player or enemy panel based on whether or not 
                //the third data entry is p or e
                if (entry[2]=="p")
                {
                    Instantiate(things["Player_Panel"], new Vector3(place.x, place.y, 0), Quaternion.identity);
                }
                else if (entry[2] == "e")
                {
                    Instantiate(things["Enemy_Panel"], new Vector3(place.x, place.y, 0), Quaternion.identity);
                }
                //put anything else on the tile that belongs there
                for (int i =0; i<entry.Length-3;i++)
                {
                    Instantiate(things[entry[i+3]], new Vector3(place.x, place.y, 0), Quaternion.identity);
                }
            }
        }
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

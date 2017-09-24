using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileSheet
{
    desert,
    wood,
    forest
}

public class OverMapLoader : MonoBehaviour {

    public string overMapFile;
    protected TextAsset reader;
    public List<string> rows = new List<string>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   
}

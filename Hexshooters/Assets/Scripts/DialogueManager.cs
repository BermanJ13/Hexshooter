using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public string dialogueFile;
    private StreamReader reader;
    public List<string> dialogueLines = new List<string>();

    public Canvas dialogueBox;
    [SerializeField]

    public Image characterPic;
    [SerializeField]

    public Text name;
    [SerializeField]

    public Text words;
    [SerializeField]

    private bool Load(string fileName)
    {
        
        try
        {
            string line;
            reader = new StreamReader(fileName, Encoding.Default);
           
            using (reader)
            {
                
                do
                {
                    line = reader.ReadLine();

                    if (line != null)
                    {
                        
                        string[] entries = line.Split('#');
                        if (entries.Length > 0)
                            for (int i =0; i<entries.Length;i++)
                            {
                                dialogueLines.Add(entries[i]);
                            }
                            
                    }
                }
                while (line != null);
                // Done reading, close the reader and return true to broadcast success    
                reader.Close();
                return true;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (dialogueLines.Count > 0)
        {
            name.gameObject.SetActive(true);
            characterPic.gameObject.SetActive(true);
            words.gameObject.SetActive(true);

            string[] dialogue= dialogueLines[0].Split(',');
            name.text = dialogue[0];
            characterPic = (Image) Resources.Load ("../Dialogue/Portrait/"+name.text + dialogue[1] + ".png");
            words.text = dialogue[2];
        }
	}

    void nextLine()
    {
        if (dialogueLines.Count > 0)
            dialogueLines.RemoveAt(0);
    }
}

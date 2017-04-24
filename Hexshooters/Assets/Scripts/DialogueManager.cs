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

    public Canvas dialogueCanvas;
    [SerializeField]

    public Image dialogueBox;
    [SerializeField]

    public Image characterPic;
    [SerializeField]

    public Text name;
    [SerializeField]

    public Text words;
    [SerializeField]

    public GameObject[] UI;

    void Start()
    {
        UI = GameObject.FindGameObjectsWithTag("DialogUI");

        //for (int i = 0; i < UI.Length; i++)
           // UI[i].SetActive(false);

        /*
        characterPic.gameObject.SetActive(false);
        name.gameObject.SetActive(false);
        words.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
        */
    }

    public bool Load(string fileName)
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
                            for (int i =1; i<entries.Length;i++)
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
            for (int i = 0; i < UI.Length; i++)
                UI[i].SetActive(true);

            string[] dialogue= dialogueLines[0].Split(',');
            name.text = dialogue[0];
            string portName = name.text + dialogue[1];
            Sprite newPic = Resources.Load<Sprite>("Portrait/"+portName);
            characterPic.GetComponent<Image>().sprite = newPic;
            words.text = dialogue[2];
        }
        else
        {
            //for (int i = 0; i < UI.Length; i++)
                //UI[i].SetActive(false);
        }
	}

    void nextLine()
    {
        if (dialogueLines.Count > 0)
            dialogueLines.RemoveAt(0);
    }
}

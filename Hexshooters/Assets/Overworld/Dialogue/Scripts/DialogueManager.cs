using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public string dialogueFile;
	private TextAsset reader;
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
          string line;
		reader = Resources.Load(fileName) as TextAsset;
			string[] lines = reader.text.Split('#');
				foreach (string s in lines)
				{
					string [] temp;
					string replaceWith = "";
					//temp = s.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
					temp = s.Split('#');
					foreach (string t in temp)
					{
						dialogueLines.Add (t);
					}
				}
		dialogueLines.RemoveAt(0);
                return true;
            
        
    }
	
	// Update is called once per frame
	void Update () {
		if(dialogueCanvas != null)
			dialogueCanvas.gameObject.SetActive (true);
		if (dialogueLines.Count > 0)
        {
            for (int i = 0; i < UI.Length; i++)
                UI[i].SetActive(true);

            string[] dialogue= dialogueLines[0].Split('/');
            name.text = dialogue[0];
            string portName = name.text + dialogue[1];
            Sprite newPic = Resources.Load<Sprite>("Portrait/"+portName);
            characterPic.GetComponent<Image>().sprite = newPic;
            words.text = dialogue[2];
        }
        else
        {
			if(dialogueCanvas!=false)
			dialogueCanvas.gameObject.SetActive (false);
        }

	}

    public void nextLine()
    {
        if (dialogueLines.Count > 0)
            dialogueLines.RemoveAt(0);
    }
}

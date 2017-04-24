using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverPlayer : MonoBehaviour {

	List<string> script;
	public bool cutscene;
	public bool cut0=true;
	public bool cut1=true;
	public bool cut2=true;
	public bool cut3=true;
	public bool cut6=true;
	public bool cut4=true;
	public bool cut5=true;
	public bool cut7=true;
	public bool cut8=true;
	public bool cut9=true;

    public GameObject dialog;
    [SerializeField]
	public bool battle;

    void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	// Use this for initialization
	void Start () {
        dialog = GameObject.FindGameObjectWithTag("DialogMngr");
        script = new List<string>();
		script.Add("Assets/Dialogue/Text/Cutscene0.txt");
		script.Add("Assets/Dialogue/Text/Cutscene1.txt");
        

        script.Add("Assets/Dialogue/Text/Cutscene2.txt");
        

        script.Add("Assets/Dialogue/Text/Cutscene3.txt");

        script.Add("Assets/Dialogue/Text/Cutscene4.txt");

        script.Add("Assets/Dialogue/Text/Cutscene5.txt");

        script.Add("Assets/Dialogue/Text/Cutscene6.txt");

        script.Add("Assets/Dialogue/Text/Cutscene7.txt");

        script.Add("Assets/Dialogue/Text/Cutscene8.txt");

        script.Add("Assets/Dialogue/Text/Cutscene9.txt");
    }
	
	// Update is called once per frame
	void Update () 
	{
		if (SceneManager.GetActiveScene().name == "Battle1" || SceneManager.GetActiveScene().name == "Battle2" || SceneManager.GetActiveScene().name == "Battle3" || SceneManager.GetActiveScene().name == "Battle4" || SceneManager.GetActiveScene().name == "Battle5")
			battle = true;
		else
			battle = false;
		
		if (!battle)
		{
			this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (255, 0, 0, 1);
			if (!cutscene)
			{
				movement ();
				interaction ();
			}
			else
			{
				if (Input.GetButtonDown ("Submit_Solo"))
				{
					dialog.GetComponent<DialogueManager> ().nextLine ();
				}
				if (dialog.GetComponent<DialogueManager> ().dialogueLines.Count == 0)
				{
					cutscene = false;
				}

				if (!cutscene && !cut1)
					SceneManager.LoadScene ("Battle1");
				if (!cutscene && !cut3)
					SceneManager.LoadScene ("Battle2");
				if (!cutscene && !cut4)
					SceneManager.LoadScene ("Battle3");
				if (!cutscene && !cut6)
					SceneManager.LoadScene ("Battle4");
				if (!cutscene && !cut8)
					SceneManager.LoadScene ("Battle5");
			}
		}
		else
			this.GetComponent<SpriteRenderer> ().color = new Color (255, 0, 0, 0);
	}
	void movement()
	{
		bool inboundsX = false;
		bool inboundsY = false;
		bool moveRight = true;
		bool moveLeft  = true;
		bool moveUp  =   true;
		bool moveDown =  true;
		//Checks for Left and RIght Movement
		if (Input.GetAxisRaw ("Horizontal_Solo") > 0) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 0.1f, transform.position.y),0.1f);
			//Checks whether or not something is in the way or if the desired spot is within the player area.
			foreach( Collider2D c in hitColliders)
			{
				if (c.gameObject.tag == "Map")
				{
					inboundsX = true;
				}

				if (c.gameObject.tag == "Boundary")
				{
					moveRight = false;
				}
			}
			//Performs the movement if possible
			if (inboundsX)
			{
				if (moveRight)
					transform.position = new Vector2 (transform.position.x + 0.15f, transform.position.y);
			}
		} 
		else if (Input.GetAxisRaw ("Horizontal_Solo") < 0) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - 0.1f, transform.position.y),0.1f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					Debug.Log ("Damn");
					inboundsX = true;
				}
				if (c.gameObject.tag == "Boundary")
				{
					Debug.Log ("Dammit");
					moveLeft = false;
				}

			}
			//Performs the movement if possible
			if (inboundsX)
			{
				if (moveLeft)
					transform.position = new Vector2 (transform.position.x - 0.15f, transform.position.y);
			}
		}
		//Checks for Up and Down Movement
		if (Input.GetAxisRaw ("Vertical_Solo") > 0) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + 0.1f),0.1f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					inboundsY = true;
				}
				if (c.gameObject.tag == "Boundary")
				{
					moveUp = false;
				}
			}
			//Performs the movement if possible
			if (inboundsY)
			{
				if (moveUp)
					transform.position = new Vector2 (transform.position.x, transform.position.y + 0.15f);
			}
		} 
		else if (Input.GetAxisRaw ("Vertical_Solo") < 0) 
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 0.1f),0.1f);
			foreach( Collider2D c in hitColliders)
			{
				//Checks whether or not something is in the way or if the desired spot is within the player.
				if (c.gameObject.tag == "Map")
				{
					inboundsY = true;
				}
				if (c.gameObject.tag == "Boundary")
				{
					moveDown = false;
				}
			}
			//Performs the movement if possible
			if (inboundsY)
			{
				if (moveDown)
					transform.position = new Vector2 (transform.position.x, transform.position.y - 0.15f);
			}
		}
	}
	void interaction()
	{

		Collider2D[] hitColliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), .2f);
		foreach (Collider2D hitCollider in hitColliders)
		{
			if (hitCollider.gameObject.tag == "Cut 0")
			{
				if (cut0)
				{
					dialog.GetComponent<DialogueManager> ().Load (script [0]);
					hitCollider.gameObject.SetActive (false);
					cutscene = true;
					cut0 = false;
				}
			}
			if (hitCollider.gameObject.tag == "Cut 1")
			{
				if (cut1)
				{
					dialog.GetComponent<DialogueManager> ().Load (script [1]);
					hitCollider.gameObject.SetActive (false);
					cutscene = true;
					cut1 = false;
				}
			
			}
			if (hitCollider.gameObject.tag == "Cut 2")
			{
				if (cut2)
				{
					dialog.GetComponent<DialogueManager> ().Load (script [2]);
					hitCollider.gameObject.SetActive (false);
					cutscene = true;
					cut2 = false;
				}

			}
			if (hitCollider.gameObject.tag == "Cut 3")
			{
				if (cut3)
				{
					dialog.GetComponent<DialogueManager> ().Load (script [3]);
					hitCollider.gameObject.SetActive (false);
					cutscene = true;
					cut3 = false;
				}
			}
			if (hitCollider.gameObject.tag == "Cut 4")
			{
				if (cut4)
				{
					dialog.GetComponent<DialogueManager> ().Load (script [4]);
					hitCollider.gameObject.SetActive (false);
					cutscene = true;
					cut4 = false;
				}
			}
			if (hitCollider.gameObject.tag == "Cut 5")
			{
				if (cut5)
				{
					dialog.GetComponent<DialogueManager> ().Load (script [5]);
					hitCollider.gameObject.SetActive (false);
					cutscene = true;
					cut5 = false;
				}
			}
			if (hitCollider.gameObject.tag == "Cut 6")
			{
				if (cut6)
				{
					dialog.GetComponent<DialogueManager> ().Load (script [6]);
					hitCollider.gameObject.SetActive (false);
					cutscene = true;
					cut6 = false;
				}
			}
			if (hitCollider.gameObject.tag == "Cut 7")
			{
				if (cut7)
				{
					dialog.GetComponent<DialogueManager> ().Load (script [7]);
					hitCollider.gameObject.SetActive (false);
					cutscene = true;
					cut7 = false;
				}
			}
			if (hitCollider.gameObject.tag == "Cut 8")
			{
				if (cut8)
				{
					dialog.GetComponent<DialogueManager> ().Load (script [8]);
					hitCollider.gameObject.SetActive (false);
					cutscene = true;
					cut8 = false;
				}
			}
			if (hitCollider.gameObject.tag == "Cut 9")
			{
				if (cut9)
				{
					dialog.GetComponent<DialogueManager> ().Load (script [9]);
					hitCollider.gameObject.SetActive (false);
					cutscene = true;
					cut9 = false;
				}
			}
			else
			if (Input.GetButtonDown ("Submit_Solo"))
			{
				if (hitCollider.gameObject.tag == "Building")
				{
					//Load Building Scene
				}
				if (hitCollider.gameObject.tag == "Object")
				{
					//Load Building Scene
				}
				if (hitCollider.gameObject.tag == "NPC")
				{
					Debug.Log ("NPC");
				}
			}
		}
	}
}

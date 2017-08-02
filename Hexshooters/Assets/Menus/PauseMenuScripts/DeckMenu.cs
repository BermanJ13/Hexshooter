using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckMenu : MonoBehaviour {
    //used for deciding which Pause Menu Screen to Display;
    public GameObject closedCanvas; //the main pause menu panel of when the journal is closed
    public GameObject deckCanvas; //the deck building menu of when the journal is open to deck menu
    public GameObject questCanvas; //the quest menu of the journal where player can see all the quests done/doing
    public GameObject logCanvas; //log  Canvas
    public GameObject invenCanvas; //inventory canvas
    public GameObject checkQuitCanvas; //asking if the player is sure if want to quit
    public int charnum = 0; //determins which character talking about
    public Text wepnames; // displays wep name for middle bar on left side of the book
    //profile images for top left image 
    Image myImageComponent;
    public Sprite mattProfile;
    public Sprite johnProfile;
    //public Sprite bridgetProfile; //not made yet
    //public Sprite IyeProfile; //not made yet
    public bool aDeck = true; //checks if going with a or b deck
    //textboxes for inventory and deck
    public Text deck;
    public Text inven;


	//Prevents Destruction upon scene switching
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Parameters: none
    //Purpose: to change between the characters when click the button
    //Known Errors: only does john and matt atm
    public void ChangeCharacter()
    {
        //when click to change characters it will change this int variable to identify which character you are on
        //0= matt, 1 = john, 2= bridget, 3= Iye
        if (charnum < 1)
        {
            charnum++;
        }
        else
        {
            charnum = 0;
        }
    }

    /*
     * Left Side of the Menu
     * */
    //Parameters: charnum
    //Purpose: change the character profile on the top left box of the menu to see which one you are on along with the text of
    //the corresponding weapon they use
    //Known Errors: only does john and matt atm
    public void changeProfile(int charnum)
    {
        //changes the image depending on what character number we are on
        //0=matt, 1=john, 2= bridget, 3=Iye 
        switch (charnum)
        {
            case 0:
                myImageComponent.sprite = mattProfile;
                wepnames.text = "Revolver";
                break;
            case 1:
                myImageComponent.sprite = johnProfile;
                wepnames.text = "Shotgun";
                break;
            default:
                myImageComponent.sprite = mattProfile;
                wepnames.text = "Revolver";
                break;
        }
    }

    //Parameters: charnum
    //Purpose: change the weapon name on the middle box of the menu to see which one you are on
    //along with the profile picture of the character that uses it.
    //Known errors: only does john and matt atm
    public void changeWepName()
    {
        //changes the image depending on what character number we are on
        //0=matt, 1=john, 2= bridget, 3=Iye 
        switch (charnum)
        {
            case 0:
                wepnames.text = "Revolver";
                myImageComponent.sprite = mattProfile;
                break;
            case 1:
                wepnames.text = "Shotgun";
                myImageComponent.sprite = johnProfile;
                break;
            default:
                wepnames.text = "Revolver";
                myImageComponent.sprite = mattProfile;
                break;
        }
    }
    //Parameters: none
    //Purpose: when click on the A deck tab, switches to the A deck 
    //Known errors: unknown 
    public void aDeckButton()
    {
        if(!aDeck)
        {
            aDeck = true;
        }
    }

    //Parameters: None
    //Purpose: when click on the B deck tab, switces to the B deck
    public void bDeckButton()
    {
        if(aDeck)
        {
            aDeck = false;
        }
    }


    /*
     * different menu button code 
     * */

	//Paramters: None
	//Purpose: Takes the player to main pause menu
	//Known Errors: None
	public void ToClosedBook()
	{
		//In Theory only need to disable the main pause menu and active the deck Menu
		//turns off title canvas and turns on level select canvas
		deckCanvas.SetActive(false);
		closedCanvas.SetActive(true);
	}
    //Paramters: None
    //Purpose: Takes the player to the list of Quest Section of the Menu
    //Known Errors: None
    public void ToQuests()
    {
        //In Theory only need to disable the main pause menu and active the deck Menu
        //turns off title canvas and turns on level select canvas
        deckCanvas.SetActive(false);
        questCanvas.SetActive(true);
    }

    //Paramters: None
    //Purpose: Takes the player to the list of Log Section of the Menu
    //Known Errors: None
    public void ToLog()
    {
        //In Theory only need to disable the main pause menu and active the deck Menu
        //turns off title canvas and turns on level select canvas
        deckCanvas.SetActive(false);
        questCanvas.SetActive(true);
    }

    //Paramters: None
    //Purpose: Takes the player to the list of inventory Section of the Menu
    //Known Errors: None
    public void ToInven()
    {
        //In Theory only need to disable the main pause menu and active the deck Menu
        //turns off title canvas and turns on level select canvas
        deckCanvas.SetActive(false);
        questCanvas.SetActive(true);
    }

    /*
     *  editing the deck and inventory 
     * */


    //Parameters: List of the deck you are editing, and list of bullet inventory the player has,and spell object want to check
    //Purpose: To remove the object from the inventory and add it to the player's deck
    //Known Errors: None 
    public void addToDeck(List<object> deck, List<object> inven, object spell)
    {

        //check to see if the spell want to add is in inventory
        if (inven.Contains(spell))
        {
            inven.Remove(spell);
            deck.Add(spell);
        }
    }

    //Parameters: List of the deck you are editing,list of bullet inventory, and spell object want to check
    //Purpose: To remove the spell from the current deck you are editing and adding it to the inventory.
    //Known Errors: None
    public void removeFromDeck(List<object> deck, List<object> inven, object spell)
    {
        //check to see if the spell want to add is in inventory
        if (deck.Contains(spell))
        {
            deck.Remove(spell);
            inven.Add(spell);
        }
    }

    //Parameters: not sure how the multiple decks attached to each character is handled data wise
    //Functions: displays the deck of the character depending if it's their first or second deck on left side of the menu
    //Known errors: I have no idea what the parameters are. 
	public void displayDeck(List<Object> Deck) //assuming the list has spell names already
    {
		//Image displayImage;
		//Text name;
		//Get current selected Weapon andpick out it's maximum
		int currentWeapon = 0; //Replace with getting the current selected weapon
		int weaponDeckSize;
		switch (currentWeapon)
		{
			//Set weapon deck size based on the current weapon
			default:
				weaponDeckSize = 0;
			break;
		}
        //then start displaying it on the apporiate text box
		for(int i =0;i<weaponDeckSize;i++)
        {
			if (Deck [i] != null)
			{
				Spell currentSpell = ((GameObject)Deck [i]).GetComponent<Spell> ();
				currentSpell.setDescription (currentWeapon);
				//Change Display Image and name to the objects that make up a button in the deck list. 
				//displayImage.sprite = currentSpell.bulletImage;
				//name.text = currentSpell.gameObject.name;
			}
			else
			{

			}
        }
     }

	/*
    //Parameters: not sure 
    //Functions: displays the list of spells in the inventory 
    //Known errors: I have no idea what the parameters are. 
    public void displayInventory(List<object>bulletInven) //assuming the list has spell names already
    {
        //list of spells within the deck. to later be displayed
        List<string> spells = new List<string>;
        //new line string to be added between each spell for readability.
        string newline = "\n";
        foreach(var spell in bulletInven)
        {
            spells.add(bulletInven.name?);
            spells.add(newline);
        }
        //then start displaying it on the apporiate text box
        foreach(var name in spells)
        {
            inven.text+= name;
        }
     }
     */

}

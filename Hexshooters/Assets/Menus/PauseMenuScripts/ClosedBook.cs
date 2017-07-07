using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedBook : MonoBehaviour
{

    //used for deciding which Pause Menu Screen to Display;
    public GameObject closedCanvas; //the main pause menu panel of when the journal is closed
    public GameObject deckCanvas; //the deck building menu of when the journal is open to deck menu
    public GameObject questCanvas; //the quest menu of the journal where player can see all the quests done/doing
    public GameObject logCanvas; //log  Canvas
    public GameObject invenCanvas; //inventory canvas
    public GameObject checkQuitCanvas; //asking if the player is sure if want to quit
    //bool to see if the decision the player made was sure
    public bool amSure = false;
    public bool nevermind = false;
    //to tell between the characters
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /*
     * Purpose of this section of code is for the main part of the pause menu where the book is closed.
     */

    //Parameters: None
    //Purpose: Check if the player wants to quit Application when selected and act accordingly.
    //Known Errors: None
    public void QuitToDesktop()
    {
        //when click on QuitToDesktop the popup for "are you sure?" should happen.
        checkQuitCanvas.SetActive(true);
        //if am sure becomes true then quit the application
        if (amSure)
        {
            // Close game
            Application.Quit();
        }
        //else it would turn off the the check for it is return them back to the screen
        if (nevermind)
        {
            checkQuitCanvas.SetActive(false);
            nevermind = false;
        }

    }

    //Parameters: None
    //Purpose: Check if the player wants to return to load  when selected and act accordingly.
    //Known Errors: Doesn't return to LoadScene for do not know what # that is atm
    public void QuitToLoadScreen()
    {
        //when click on QuitToDesktop the popup for "are you sure?" should happen.
        checkQuitCanvas.SetActive(true);
        //if am sure becomes true then quit the application
        if (amSure)
        {
            //returns to SinglePlayer Load Screen
            //error: 2 needs to be changed to whatever is the new/load/save screen for single player
            //SceneManager.LoadScene(2);
        }
        //else it would turn off the the check for it is return them back to the screen
        if (nevermind)
        {
            checkQuitCanvas.SetActive(false);
            nevermind = false;
        }

    }

    //Parameters: None
    //Purpose: When the the player presses Yes when the pop up "Are you sure?" comes on turns the bool to true
    //Known Errors: None
    public void AmSure()
    {
        amSure = true;
    }

    //Parameters: None
    //Purpose: When the disable 
    //Known Errors:
    public void disableQuitCheck()
    {
        nevermind = true;

    }
    //Parameters: None
    //Purpose: Generic Return function to main pause menu no matter where in the journal you are
    //Known Errors: None
    public void CloseBook()
    {
        closedCanvas.SetActive(true);
        deckCanvas.SetActive(false);
        questCanvas.SetActive(false);
        logCanvas.SetActive(false);
        invenCanvas.SetActive(false);
    }


    /*
    * Purpose of this section of code is for the deck building menu.
    */

    //Parameters: None
    //Purpose: Takes the player to the Deck Building Section of the Menu
    //Known Errors: None
    public void ToDeck()
    {
        //In Theory only need to disable the main pause menu and active the deck Menu
        //turns off title canvas and turns on level select canvas
        closedCanvas.SetActive(false);
        deckCanvas.SetActive(true);
    }

    /*
    * ToDoList:
    * Needs to go to display Character's Decks
    * Need to be able to remove bullets from each character's decks
    * Need to be able to add bullets into each character's deck from the list the player owns
    * Save changes when leave?
    */

    /*
    * Purpose of this section of code is for the quest log.
    */

    //Paramters: None
    //Purpose: Takes the player to the list of Quest Section of the Menu
    //Known Errors: None
    public void ToQuests()
    {
        //In Theory only need to disable the main pause menu and active the deck Menu
        //turns off title canvas and turns on level select canvas
        closedCanvas.SetActive(false);
        questCanvas.SetActive(true);
    }

    /*
    * ToDoList:
    * Show a list of quests on the left
    * Shows the description of the quest on right
    * check if the player has access to certian parts of the table
    * Assuming it's a hashtable? 
    *   name of quest -> info on quest
    *   depending on info display apporiate image?
    * If not a table then an array of [town/section of game, quest in said section, display info]
    */

    /*
    * Purpose of this section of code is for the log menu.
    */

    //Paramters: None
    //Purpose: Takes the player to the Character Log Section of the Menu
    //Known Errors: None
    public void ToLog()
    {
        //In Theory only need to disable the main pause menu and active the deck Menu
        //turns off title canvas and turns on level select canvas
        closedCanvas.SetActive(false);
        logCanvas.SetActive(true);
    }

    public void DisplayNames(object[] characters, object[] items, object[] spells, GameObject logCanvas)
    {
        /*
		logCanvas.AddComponent<Text> ();
		Text textComponent = myGO.GetComponent<Text> ();
        text.text = "Characters";

        Font AdventurejournalFONT = (Font)Resources.GetBuiltinResource(typeof(Font), "Adventurejournal.ttf");
        text.font = ArialFont;
        text.material = ArialFont.material;

        return text;
        */
    }

    /*
    * ToDoList:
    * Names on left, information on right
    * Check if player has access to certain info people/item 
    * will this be updated? if so how? (like entry says good guy, but then later twist he is bad. )
    */


    /*
    * Purpose of this section of code is for the inventory menu.
    */

    //Paramters: None
    //Purpose: Takes the player to the Recipe Section of the Menu
    //Known Errors: None
    public void ToInven()
    {
        //In Theory only need to disable the main pause menu and active the deck Menu
        //turns off title canvas and turns on level select canvas
        closedCanvas.SetActive(false);
        invenCanvas.SetActive(true);
    }

    /*
    * ToDoList:
    * Goes through inventory(assuming an array or something) of the player and displays info 
    */


}

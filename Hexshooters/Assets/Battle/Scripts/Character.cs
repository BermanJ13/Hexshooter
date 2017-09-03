using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour 
{
	public string name= "";
	public List<Object> DeckA = new List<Object>();
	public List<Object> DeckB = new List<Object>();
	public int health = 100;
	public int activeDeck = 0;
	public Weapon_Types weapon;
	public int weaponMax = 30;
}

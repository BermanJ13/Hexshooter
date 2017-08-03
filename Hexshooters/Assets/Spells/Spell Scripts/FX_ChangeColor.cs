using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_ChangeColor : MonoBehaviour {

	//holds a list of particles for the prefab
	public ParticleSystem[] particleList;

	//for changing each individual particle colors to different colors 
	public Color[] colorList;

	//changes all the particle color
	public Color color = Color.white;

	//whether the particles a single color and different
	public bool single = true;

	// Use this for initialization
	void Start () {
		colorList = new Color[particleList.Length];//implements the colorList
		//default changes all particles color to white
		for (int i = 0; i < particleList.Length; i++) {
			colorList [i] = Color.white;

			var mainModule = particleList [i].main;
			var tmp = color;

			//if not single changes to sotres individual color
			if (!single)
				tmp = colorList [i];
			
			mainModule.startColor = tmp;//assigns color to the particle
		}

	

			
	}
	
	// Update is called once per frame
	void Update () {
		changeColor ();
			
	}

	//changes color on runtime
	void changeColor()
	{
		//if single and the color got changed sets all particle color
		if (single && color != particleList[0].main.startColor.color) {
			for (int i = 0; i < particleList.Length; i++) {
				var mainModule = particleList [i].main;
				mainModule.startColor = color;
			}
		}

		//if not single runs through the array and changes each individual color to their respected color
		if (!single) {
			for (int i = 0; i < particleList.Length; i++) {
				var mainModule = particleList [i].main;
				mainModule.startColor = colorList[i];
			}
		}
	}

}

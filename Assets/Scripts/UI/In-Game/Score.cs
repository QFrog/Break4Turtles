using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	//score = number of lanes crossed
	public Text scoreText;
	//subtract score if health is lost

	//Next Lane Int minus 1 is how many lanes are on-screen

	public void Start ()
	{
		StaticItems.score = this;
	}

	public void Update ()
	{
		UpdateScore ();
	}

	public void UpdateScore ()
	{
		scoreText.text = "Score: " + StaticItems.scoreValue;
	}


}
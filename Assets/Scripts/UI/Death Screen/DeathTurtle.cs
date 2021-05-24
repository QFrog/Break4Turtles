using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTurtle : MonoBehaviour
{
	public void Start ()
	{
		StaticItems.deathTurtle = this;
		StaticItems.gameOverText = this;
		this.gameObject.SetActive (false);
	}

	public void DeathScreen ()
	{
		StaticItems.Dead = true;
		StaticItems.inGame.gameObject.SetActive (false);
	}

}

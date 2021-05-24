using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	void Start ()
	{
		StaticItems.mainMenu = this;
	}

	public void PlayGame ()
	{
		
		//Loads GameScene
		SceneManager.LoadScene ("GameScene");
		StaticItems.Reset ();
	}

	public void QuitGame ()
	{
		//Quit game
		Application.Quit ();
	}
}

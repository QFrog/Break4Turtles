using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	public void Start ()
	{
		StaticItems.pauseMenu = this;
	}

	public void ShowMainMenu ()
	{
		//Load the main menu
		SceneManager.LoadScene ("UISceneMainMenu");
	}

	public void Update ()
	{
		if (StaticItems.Paused || StaticItems.Dead) {
			Time.timeScale = 0.0001f;
		} else {
			Time.timeScale = 1f;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        StaticItems.inGame = this;
    }

    public void PauseGame()
    {
        //Set TimeScale = 0.001f to slow all FixedUpdate functions so they look like they're all stationary
        StaticItems.Paused = true;
    }
}

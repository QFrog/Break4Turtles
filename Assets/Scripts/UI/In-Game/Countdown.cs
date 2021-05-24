using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public float timeLeft = 4f;
    public Text startText; // used for showing countdown from 3, 2, 1 
    public Button button;
    public bool countingDown;

    public void Update()
    {
        if (countingDown)
        {
            timeLeft -= Time.deltaTime * 10000;
            startText.text = "" + (int)timeLeft;
            Debug.Log(timeLeft);

            if (timeLeft <= 1)
            {
                Debug.Log("Resuming Game should be false");
                timeLeft = 4.0f;
                countingDown = false;
                StaticItems.Paused = false;
                this.gameObject.SetActive(false);
                StaticItems.inGame.gameObject.SetActive(true);
            }
        }
    }

    public void Resuming()
    {
        countingDown = true;
        Debug.Log("countingDown = true");
    }
}

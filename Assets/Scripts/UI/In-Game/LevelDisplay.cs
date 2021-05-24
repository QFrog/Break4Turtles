using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{

	Image myImage;
	public List<Sprite> banners;
	public float Timer = 2f;

	// Use this for initialization
	void Start ()
	{
		myImage = GetComponent<Image> ();
		myImage.sprite = banners [Mathf.FloorToInt (StaticItems.actualLevel * 2)];
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Timer > 0) {
			Timer -= Time.deltaTime;
		}

		if (Timer < 0)
			Timer = 0;

		if (Timer <= 1) {
			myImage.color = new Color (255, 255, 255, Timer);
		}
	}
}

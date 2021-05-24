using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

	public Image HealthSlider;
	public const float maxHealth = 100;
	public float currentHealth = maxHealth;
	public float healthBarLength;

	public void Start ()
	{
		healthBarLength = Screen.width / 2;
		StaticItems.playerHealth = this;
		Camera.main.backgroundColor = StaticItems.Turtle.terrainColors [StaticItems.CurrentLevel];
	}


	public void DamageTaken (int damage)
	{
		currentHealth -= damage;
		if (currentHealth <= 0) {
			StaticItems.Dead = true;
		}

		if (currentHealth >= maxHealth) {
			currentHealth = maxHealth;
		}

		healthBarLength = (Screen.width / 2);
		HealthSlider.fillAmount = (currentHealth / maxHealth);
	}

	public void Update ()
	{
		DamageTaken (0);
	}
}

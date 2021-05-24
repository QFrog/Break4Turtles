using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B4T.Animals;
using B4T.Roads;
using B4T.Vehicles;
using UnityEngine.SceneManagement;

public static class StaticItems
{

	public static int AnimalsHit = 0;
	public static TurtleController Turtle;
	public static bool Dead = false;
	public static MainMenu mainMenu;
	public static PauseMenu pauseMenu;
	public static InGame inGame;
	public static PlayerHealth playerHealth;
	public static DeathTurtle deathTurtle;
	public static DeathTurtle gameOverText;
	public static bool Paused = false;
	public static GameObject PowerUp;
	public static bool animalHit = false;
	public static Score score;

	//stuff that stays between levels
	public static float healthValue = 100;

	static float scoreVal_ = 0;

	public static float scoreValue {
		get {
			return scoreVal_;
		}
		set {
			scoreVal_ = value;
			if (scoreVal_ < 0)
				scoreVal_ = 0;
			
		}
	}

	public static int TrafficConeCount = 1;
	public static int BarricadeCount = 1;
	public static int AirhornCount = 1;

	public static float actualLevel = 0;

	public static int CurrentLevel { get { return Mathf.FloorToInt (actualLevel); } }

	public static int allLanesCrossed = 0;

	public static void Reset ()
	{
		TrafficConeCount = 1;
		BarricadeCount = 1;
		AirhornCount = 1;
		actualLevel = 0;
		allLanesCrossed = 0;
		healthValue = 100;
		AnimalsHit = 0;
		scoreVal_ = 0;
		animalHit = false;
		Paused = false;
		Dead = false;
	}

	public static IEnumerator LevelTransition ()
	{
		allLanesCrossed += RoadGeneration.current.TotalLanes;
		while (Turtle.lanesCrossed < Turtle.maxLanes + 3.5f) {
			yield return null;
		}
		actualLevel += .5f;


		if (actualLevel <= 4) {
			float lerp = 0;
			while (Camera.main.backgroundColor != Turtle.terrainColors [CurrentLevel]) {
				Camera.main.backgroundColor = Color.Lerp (Camera.main.backgroundColor, Turtle.terrainColors [CurrentLevel], lerp);
				lerp += Time.deltaTime / 10;
				yield return null;
			}

			while (Turtle.lanesCrossed < Turtle.maxLanes + 5f) {
				yield return null;
			}

			SceneManager.LoadScene ("GameScene");
		} else {
			//final screen
		}
	}
}

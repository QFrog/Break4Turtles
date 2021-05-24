using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B4T.Animals
{
	public class TurtleController : AnimalController
	{
		//[Header("Turtle")]
		public List<Color> terrainColors;
		public float lanesCrossed = 0;
		public int maxLanes = 5;
		float lastLanesCrossed = 0;

		protected override void Start ()
		{
			base.Start ();
			StaticItems.Turtle = this;
			lanesCrossed = transform.position.y / Roads.RoadGeneration.LaneWidth + .5f;
			lastLanesCrossed = lanesCrossed;
		}

		protected override void FixedUpdate ()
		{
			AnimalSpawningWatchtower.AnimalSpawnCountdown ();
			base.FixedUpdate ();
			lastLanesCrossed = lanesCrossed;
			lanesCrossed = transform.position.y / Roads.RoadGeneration.LaneWidth + .5f;

			if (Mathf.FloorToInt (lastLanesCrossed) < Mathf.FloorToInt (lanesCrossed) && Mathf.FloorToInt (lanesCrossed) >= 1 && Mathf.FloorToInt (lanesCrossed) <= maxLanes) {
				CrossedLane ();
			}

			if (StaticItems.healthValue <= 0) {
				isDead = true;
				StaticItems.deathTurtle.gameObject.SetActive (true);
				StaticItems.deathTurtle.DeathScreen ();
			}
		}

		protected override void Death ()
		{
			base.Death ();
		}

		protected override void OnBecameInvisible ()
		{
			
		}

		public void CrossedLane ()
		{
			StaticItems.scoreValue += 100f;
			if (Mathf.FloorToInt (lanesCrossed) >= maxLanes) {
				StartCoroutine (StaticItems.LevelTransition ());
			}
		}
	}
}
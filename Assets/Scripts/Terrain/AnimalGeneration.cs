using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B4T.Roads;

namespace B4T.Animals
{
	public class AnimalGeneration : MonoBehaviour
	{
		[System.Serializable]
		public class AreaPalette
		{
			public Sprite CrossingSprite;
			public Sprite SideSprite;
			public List<Sprite> ClutterSprites;
			public List<GameObject> Animals;
		}

		[Header ("Setup")]
		public List<AreaPalette> AreaPalettes;
		public int CurrentArea;
		public int ZoneWidth;

		[Header ("Animals")]
		//List of spawned animals
		public List<AnimalController> SpawnedAnimals = new List<AnimalController> ();

		[Header ("Spawning")]
		//List of spawn locations
		public List<GameObject> SpawnLocations = new List<GameObject> ();
		//Maximum animals allowed to be spawned

		//This bush's position
		public int MyPos = 0;
		//Allowed to spawn an animal, checking the watchtower
		public bool CanSpawnAnimals {
			get {
				return AnimalSpawningWatchtower.CanSpawnAnimals;
			}
		}

		void Start ()
		{
			CurrentArea = StaticItems.CurrentLevel;
			bool fluffSpawned = Random.Range (0, 100) > 50;
			for (int i = -ZoneWidth; i <= ZoneWidth; i++) {
				GameObject loc = SpawnSegment (i, fluffSpawned);
				if (loc.tag == "AnimalSpawn") {
					SpawnLocations.Add (loc);
				}
				fluffSpawned = !fluffSpawned;
			}
		}

		public GameObject SpawnSegment (int xpos, bool spawnFluff)
		{
			GameObject g;
			if (xpos == 0 || Mathf.Abs (xpos) == ZoneWidth || Mathf.Abs (xpos) == ZoneWidth - 1) {
				g = new GameObject ("Crossing_" + gameObject.name);
				g.transform.SetParent (this.transform);
				g.transform.localPosition = new Vector2 (xpos * RoadGeneration.LaneWidth, 0);
				SpriteRenderer r = g.AddComponent<SpriteRenderer> ();
				r.sprite = AreaPalettes [CurrentArea].CrossingSprite;
				r.sortingOrder = -6;
			} else {
				g = new GameObject ("AnimalSpawn_" + gameObject.name + "_" + xpos);
				g.transform.SetParent (this.transform);
				g.transform.localPosition = new Vector2 (xpos * RoadGeneration.LaneWidth, 0);
				g.tag = "AnimalSpawn";
				SpriteRenderer r = g.AddComponent<SpriteRenderer> ();
				r.sprite = AreaPalettes [CurrentArea].SideSprite;
				r.sortingOrder = -6;

				//generate a random clutter within the sprite
				if (spawnFluff) {
					float areaLimit = (RoadGeneration.LaneWidth / 2) - .75f * RoadGeneration.LaneWidth;
					for (int i = 0; i < 1; i++) {
						//make the gameobject
						GameObject c = new GameObject (g.name + "_clutter");
						c.transform.SetParent (g.transform);
						//set its sprite
						SpriteRenderer c_r = c.AddComponent<SpriteRenderer> ();
						c_r.sprite = AreaPalettes [CurrentArea].ClutterSprites [Random.Range (0, AreaPalettes [CurrentArea].ClutterSprites.Count - 1)];
						c_r.sortingOrder = 20;
						//place it
						float x_pos = Random.Range (-areaLimit, areaLimit);
						float y_pos = Random.Range (-areaLimit, areaLimit);
						c.transform.localPosition = new Vector2 (x_pos, y_pos);
					}
				}
			}
			return g;
		}

		void Update ()
		{
			if (AnimalSpawningWatchtower.CanSpawnAnimals) {
				float dir;
				Transform pos = SpawnLocations [Random.Range (0, SpawnLocations.Count - 1)].transform;
				if (CheckForNearbyVehicles (pos, out dir)) {
					SpawnAnimal (pos.position, dir);
				}
			}
		}

		//returns true if its a-ok to spawn
		bool CheckForNearbyVehicles (Transform SpawnPoint, out float direction)
		{
			//direction multiplier, -1 = down, 1 = up
			direction = Mathf.Sign (StaticItems.Turtle.transform.position.y - SpawnPoint.position.y);

			if (direction > 0)
				return false;

			//check if next to a road
			if (MyPos + direction < 0) {
				return false;
			}

			if (RoadGeneration.current.PreviousLaneInfo [(int)MyPos + (int)direction] == 0) {
				return false;
			}

			//position for center of box
			Vector2 boxDir = new Vector2 (0, direction) * RoadGeneration.LaneWidth;
			Vector2 boxDimensions = 2 * Vector2.one;
			Collider2D[] hits = Physics2D.OverlapBoxAll ((Vector2)SpawnPoint.transform.position + boxDir, boxDimensions, 0);

			for (int i = 0; i < hits.Length; i++) {
				if (hits [i].gameObject.tag == "Vehicle" || hits [i].gameObject.tag == "Animal") {
					return false;
				}
			}
			return true;
		}

		void SpawnAnimal (Vector3 spawnPosition, float direction)
		{
			GameObject animal = GameObject.Instantiate (AreaPalettes [CurrentArea].Animals [Random.Range (0, AreaPalettes [CurrentArea].Animals.Count - 1)], spawnPosition, Quaternion.identity);
			animal.transform.localScale = new Vector2 (1, direction);
			animal.GetComponent<AnimalController> ().mySpawner = this;

			SpawnedAnimals.Add (animal.GetComponent<AnimalController> ());
			AnimalSpawningWatchtower.AllSpawnedAnimals.Add (animal.GetComponent<AnimalController> ());
			AnimalSpawningWatchtower.AnimalSpawnTimer = AnimalSpawningWatchtower.GetNextSpawnInterval ();
		}

		void OnBecameInvisible ()
		{
			Destroy (this.gameObject);
		}
	}

	public static class AnimalSpawningWatchtower
	{
		//Maximum animals allowed onscreen (excluding the turtle!)
		public static int MaximumAnimals {
			get {
				return (StaticItems.actualLevel > 0) ? 1 : 0;
			}
		}
		//List of all animal spawners
		public static List<AnimalController> AllSpawnedAnimals = new List<AnimalController> ();

		public static bool CanSpawnAnimals {
			get {
				return AllSpawnedAnimals.Count < MaximumAnimals && AnimalSpawnTimer <= 0;
			}
		}

		public static float AnimalSpawnTimer = 0f;

		public static float GetNextSpawnInterval ()
		{
			return Random.Range (15, 60);
		}

		public static void AnimalSpawnCountdown ()
		{
			if (AnimalSpawnTimer > 0)
				AnimalSpawnTimer -= Time.deltaTime;
		}
	}
}
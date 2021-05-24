using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B4T.Vehicles;

namespace B4T.Roads
{
	public class VehicleGeneration : MonoBehaviour
	{
		[System.Serializable]
		public class VehiclePalette
		{
			public List<GameObject> SpawnableVehicles = new List<GameObject> ();
		}

		[Header ("Vehicles")]
		//List of Vehicles that can be spawned
		public List<VehiclePalette> vehiclePalette;
		//List of Vehicles currently on road
		public List<VehicleController> SpawnedVehicles = new List<VehicleController> ();

		[Header ("Spawning")]
		public GameObject Spawnpoint;
		public bool canSpawn = true;
		public float MinimumDistance = .5f;
		public float MaxmimumDistance = 2f;
		public bool isLeftFacing;
		public float timeTillNextCar = 0;
		public int myLaneNumber = 0;

		[SerializeField]
		private float nextDistance = 0;
		[SerializeField]
		private int nextVehicle;

		//Saved position for where the road is
		private Vector2 position;

		void Start ()
		{
			position = transform.position;
			if (isLeftFacing) {
				Vector3 newSpawn = new Vector3 (-Spawnpoint.transform.localPosition.x, Spawnpoint.transform.localPosition.y, Spawnpoint.transform.localPosition.z);
				Spawnpoint.transform.localPosition = newSpawn;
			}
			timeTillNextCar = Random.Range (0f, 3f) + .5f;
			nextVehicle = Random.Range (0, vehiclePalette [StaticItems.CurrentLevel].SpawnableVehicles.Count - 1);
		}

		void FixedUpdate ()
		{
			if (timeTillNextCar > 0)
				timeTillNextCar -= Time.deltaTime;
			else if (canSpawn) {
				if (SpawnedVehicles.Count > 0) {
					if (Vector2.Distance (SpawnedVehicles [SpawnedVehicles.Count - 1].transform.position, Spawnpoint.transform.position) >= nextDistance) {
						SpawnCar ();
					}
				} else {
					SpawnCar ();
				}
			}
		}

		void SpawnCar ()
		{
			GameObject newCar = Instantiate (vehiclePalette [StaticItems.CurrentLevel].SpawnableVehicles [nextVehicle], this.transform) as GameObject;
			newCar.transform.localPosition = Spawnpoint.transform.localPosition;
			newCar.name += "_Spawned";

			newCar.GetComponent<VehicleController> ().OnSpawn ();

			if (SpawnedVehicles.Count > 0) {
				SpawnedVehicles [SpawnedVehicles.Count - 1].behindMe = newCar.GetComponent<VehicleController> ();
				newCar.GetComponent<VehicleController> ().aheadOfMe = SpawnedVehicles [SpawnedVehicles.Count - 1];
			}

			SpawnedVehicles.Add (newCar.GetComponent<VehicleController> ());
			FetchCar ();
			//timeTillNextCar = Random.Range (0f, 3f) + .5f;
		}

		void FetchCar ()
		{
			nextVehicle = Random.Range (0, vehiclePalette [StaticItems.CurrentLevel].SpawnableVehicles.Count);
			nextDistance = Random.Range (MinimumDistance, MaxmimumDistance) + BoundaryGetter (vehiclePalette [StaticItems.CurrentLevel].SpawnableVehicles [nextVehicle]);
			if (SpawnedVehicles.Count > 0)
				nextDistance += 1.25f;
		}

		public static float BoundaryGetter (GameObject a)
		{
			return Mathf.Abs (a.GetComponent<Collider2D> ().bounds.extents.x);
		}

		void OnBecameVisible ()
		{
			GameObject.Find ("TerrainGeneration").SendMessage ("LaneBecameVisible", new float[2] {
				myLaneNumber,
				transform.position.y
			});
		}

		void OnBecameInvisible ()
		{
			Destroy (this.gameObject);
		}
	}
}
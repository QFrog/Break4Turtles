using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B4T.Roads;

public class PowerUpController : MonoBehaviour
{
	public float BarricateTimer = 0;
	public float ConeTimer = 0;
	public B4T.Vehicles.VehicleController[] vehicles;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		vehicles = GameObject.FindObjectsOfType<B4T.Vehicles.VehicleController> ();

		if (BarricateTimer > 0)
			BarricateTimer -= Time.deltaTime;
		if (BarricateTimer < 0)
			BarricateTimer = 0;

		if (ConeTimer > 0)
			ConeTimer -= Time.deltaTime;
		if (ConeTimer < 0)
			ConeTimer = 0;

		//SpeedUp
		if (BarricateTimer > 0)
			StaticItems.Turtle.movementOverride = 2;
		else
			StaticItems.Turtle.movementOverride = -1;

		//vehicles slow
		if (ConeTimer > 0) {
			foreach (B4T.Vehicles.VehicleController vc in vehicles) {
				vc.overrideSpeed = .25f * vc.movementSpeed;
			}
		} else {
			foreach (B4T.Vehicles.VehicleController vc in vehicles) {
				vc.overrideSpeed = -1;
			}
		}
	}

	public void SpeedUp () //speeds up the turtle for 10 seconds
	{
		if (!StaticItems.Paused) {
			if (StaticItems.AirhornCount > 0) {
				StaticItems.AirhornCount--;
				BarricateTimer = 10;
			}
		}
	}

	public void PreventSpawn () //add roadblock to block all lanes on screen for 10 seconds
	{
		if (!StaticItems.Paused) {
			if (StaticItems.BarricadeCount > 0) {
				StaticItems.BarricadeCount--;
				GameObject[] roads = GameObject.FindGameObjectsWithTag ("RoadSegment");
				foreach (GameObject g in roads) {
					VehicleGeneration v = g.GetComponent<VehicleGeneration> ();
					v.canSpawn = false;
					foreach (B4T.Vehicles.VehicleController vc in v.SpawnedVehicles) {
						Destroy (vc.gameObject);
					}
				}
			}
		}
	}

	public void SlowDown () //slows down all cars for 10 seconds
	{
		if (!StaticItems.Paused) {
			if (StaticItems.TrafficConeCount > 0) {
				StaticItems.TrafficConeCount--;
				ConeTimer = 10;
			}
		}
	}
}

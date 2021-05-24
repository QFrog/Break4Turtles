using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B4T.Animals;

namespace B4T.Roads
{
	public class RoadGeneration : MonoBehaviour
	{
		[Header ("Generation Info")]
		public static RoadGeneration current;
		public static int LaneWidth = 2;
		[Range (0, 1)]
		public float ChanceOfNoLane = .25f;

		[Header ("Generation Items")]
		public List<GameObject> Lanes = new List<GameObject> ();
		public List<GameObject> Bushes = new List<GameObject> ();
		public List<int> PreviousLaneInfo = new List<int> ();
		//ID number of upcoming lane
		public int NextLaneInt = 0;
		public int TotalLanes = 0;

		bool stopGen = false;

		// Use this for initialization
		void Start ()
		{
			current = this;
			//CReate Initial Lane offscreen
			for (int i = -LaneWidth * 1; i < 0; i += LaneWidth) {
				SpawnBush (i);
			}
			CreateLane (0, false, true);
		}

		void LaneBecameVisible (float[] dataCatch)
		{
			if (stopGen)
				return;

			//data 0 = lane number, data 1 = position
			if (TotalLanes >= StaticItems.Turtle.maxLanes) {
				float startBush = dataCatch [1] + LaneWidth;
				for (int i = 0; i < 1; i++) {
					SpawnBush (startBush);
					startBush += LaneWidth;
				}
				return;
			}

			bool nextRoadPlaced = true;
			float laneBonus = 0;
			do {
				laneBonus += LaneWidth;

				if (TotalLanes >= StaticItems.Turtle.maxLanes) {
					nextRoadPlaced = CreateLane (dataCatch [1] + laneBonus, nextRoadPlaced, true);
				} else {
					nextRoadPlaced = CreateLane (dataCatch [1] + laneBonus, nextRoadPlaced);
				}
			} while (!nextRoadPlaced);
		}

		bool CreateLane (float yPos, bool previousLaneDirection, bool overrideRNG = false)
		{
			float RNGCalc = Random.value;
			//caluculation for whether or not to make a lane
			bool makelane =
				(RNGCalc >= ChanceOfNoLane && //Lane is within chance of spawning
				!CheckRoadTooThick (5, PreviousLaneInfo.Count - 1) //////////////////////////////////////////////////CHANGE LATER WITH DIFFICULTY SCALING
				) || overrideRNG; //Or if lane is ovveridden if we need to
		
			if (makelane) {
				//Lane generation and placing
				GameObject lane = Instantiate (Lanes [Random.Range (0, Lanes.Count - 1)], this.transform);
				lane.transform.position = new Vector2 (0, yPos);
				//Lane direction calculation
				lane.transform.localScale = (previousLaneDirection) ? 
				new Vector2 (PreviousLaneInfo [PreviousLaneInfo.Count - 1], 1) :
				new Vector2 (
					(Random.value >= .5) ? -1 : 1,
					1);
				//Data storage
				lane.GetComponent <VehicleGeneration> ().myLaneNumber = NextLaneInt;
				PreviousLaneInfo.Add ((int)lane.transform.localScale.x);

				NextLaneInt++;
				TotalLanes++;
				return true;
			} else {
				//Code for spawning forest area
				NextLaneInt++;
				TotalLanes++;
				return SpawnBush (yPos);
			}
		}

		bool SpawnBush (float yPos)
		{
			GameObject bush = Instantiate (Bushes [Random.Range (0, Bushes.Count - 1)], this.transform);
			bush.transform.position = new Vector2 (0, yPos);
			bush.GetComponent<AnimalGeneration> ().MyPos = NextLaneInt;

			PreviousLaneInfo.Add (0);
			return false;
		}

		bool CheckRoadTooThick (int lanesInRow, int currentLane)
		{
			if (PreviousLaneInfo.Count < lanesInRow)
				return false; //not enough roads to be too thick

			for (int i = 0; i < lanesInRow; i++) {
				if (PreviousLaneInfo [PreviousLaneInfo.Count - 1 - i] == 0) {
					return false;
				}
			}

			return true;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B4T.Roads;
using UnityEngine.SceneManagement;

namespace B4T.Vehicles
{
	public abstract class VehicleController : MonoBehaviour
	{
		//collider
		[HideInInspector]
		public Collider2D coll;
		//rigidbody
		[HideInInspector]
		public Rigidbody2D rbody;
		//Road I spawned on
		[HideInInspector]
		public VehicleGeneration vGen;

		[Header ("Generation")]
		public List<GameObject> PowerupList;

		//section for stuff that applies to
		[Header ("Basic Motion")]
		//speed of car
		public float movementSpeed = 3;
		//override speed of car
		public float overrideSpeed = -1;
		//speed calculated based on ahead
		[HideInInspector]
		public float actualMovementSpeed = 3;
		//is the car stopped?
		public bool stopped;
		//calculated with car ahead as well
		[HideInInspector]
		public bool actualStopped;
		//current stop cooldown
		public float stopCooldown = 0;

		//how many times it has been tapped
		public int timesTapped = 0;
		//Countup untuil taps are forgotten
		public float tapForgetTime = 0f;
		//The time it takes for a car to forget its taps
		public const float timeUntilForgetTaps = 3;

		protected Vector2 startPos;

		public VehicleController aheadOfMe;
		public VehicleController behindMe;

		[Header ("Stats")]
		public int tapsRequired = 1;
		public float maxStopDuration = 4;
		public float stopDistance = .75f;

		// Use this for initialization
		public virtual void OnSpawn ()
		{
			coll = GetComponent<Collider2D> ();
			rbody = GetComponent<Rigidbody2D> ();
			vGen = transform.GetComponentInParent<VehicleGeneration> ();
			startPos = transform.localPosition;
		}

		// Update is called once per frame
		protected virtual void FixedUpdate ()
		{
			Movement ();
			HandleTapCooldown ();
			HandleStopping ();
			if (
				(transform.localPosition.x <= -9 && Mathf.Sign (startPos.x) == 1) ||
				(transform.localPosition.x >= 9 && Mathf.Sign (startPos.x) == -1)) {
				OnLeaveRoad ();
			}
			if (Random.Range (0, 10000) == 25) {
				SpawnPowerup ();
			}
		}

		protected void HandleTapCooldown ()
		{
			if (timesTapped > 0) {
				if (tapForgetTime >= timeUntilForgetTaps) {
					timesTapped = 0;
				}
				tapForgetTime += Time.deltaTime;
			} else {
				tapForgetTime = 0;
			}
		}

		protected void HandleStopping ()
		{
			if (stopped) {
				stopCooldown += Time.deltaTime;

				if (stopCooldown >= maxStopDuration) {
					stopped = false;
					stopCooldown = 0;
				}
			}
			GetComponentInChildren<Collider2D> ().isTrigger = stopped;
		}

		//Movement function
		virtual protected void Movement ()
		{
			actualMovementSpeed = ((overrideSpeed >= 0) ? overrideSpeed : movementSpeed) * (1f - ((float)timesTapped / (float)tapsRequired));

			bool aheadStopped = false;
			float distAhead = Mathf.NegativeInfinity;
			float minDist = Mathf.NegativeInfinity;
			if (aheadOfMe != null) {
				minDist = stopDistance + VehicleGeneration.BoundaryGetter (this.gameObject) + VehicleGeneration.BoundaryGetter (aheadOfMe.gameObject);
				distAhead = Vector2.Distance (gameObject.transform.position, aheadOfMe.transform.position);
				aheadStopped = (aheadOfMe.actualStopped && distAhead <= minDist);
			}

			actualStopped = aheadStopped || stopped;

			if (actualStopped) {
				rbody.velocity = Vector2.zero;
			} else {
				if (aheadOfMe != null)
				if (distAhead <= minDist && aheadOfMe.actualMovementSpeed < actualMovementSpeed)
					actualMovementSpeed = aheadOfMe.actualMovementSpeed;
				
				rbody.velocity = Vector2.right * actualMovementSpeed * Mathf.Sign (transform.parent.localScale.x);
			}
		}

		//When colliding with something
		virtual protected void OnCollisionEnter2D (Collision2D coll)
		{
			if (coll.gameObject.tag == "Turtle") {
				coll.gameObject.GetComponent<B4T.Animals.TurtleController> ().isDead = true;
				StaticItems.deathTurtle.gameObject.SetActive (true);
				StaticItems.deathTurtle.DeathScreen ();
			} else if (coll.gameObject.tag == "Animal") {
				coll.gameObject.SendMessage ("Death");
			}
		}

		//When tapped
		virtual protected void OnMouseDown ()
		{
			if (!StaticItems.Paused) {
				if (timesTapped < tapsRequired && !stopped) {
					timesTapped++;

					if (timesTapped >= tapsRequired) {
						stopped = true;
						timesTapped = 0;
					}
				}
			}
		}

		virtual protected void OnLeaveRoad ()
		{
			behindMe.aheadOfMe = null;
			vGen.SpawnedVehicles.Remove (this);
			Destroy (this.gameObject);
		}

		public void SpawnPowerup ()
		{
			int powerup = Random.Range (0, PowerupList.Count - 1);
			GameObject p = Instantiate (PowerupList [powerup]) as GameObject;
			p.transform.position = transform.position + new Vector3 (coll.bounds.extents.x * transform.forward.x, 0, 0);
		}
	}
}
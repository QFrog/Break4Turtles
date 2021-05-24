using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B4T.Animals
{
	public abstract class AnimalController : MonoBehaviour
	{
		public enum MovementType
		{
			StraightLine,
			DipBack,
			PlayDead,
			StopRandom
		}

		//collider
		[HideInInspector]
		public Collider2D coll;
		//rigidbody
		[HideInInspector]
		public Rigidbody2D rbody;
		//Animator
		[HideInInspector]
		public Animator anim;

		//Section for stuff that applies to all animals
		[Header ("Basic Motion")]
		//If true, this animal dying ends the game
		public bool importantAnimal;
		//Vectors used for movement, varies based on animal
		public List<Vector2> movementVectors;
		//Movement speed
		public float movementSpeed = 1;
		public float movementOverride = -1;

		[Header ("States")]
		//Is this animal dead?
		public bool isDead;
		//What type of movement is used
		public MovementType MyMovement;
		//Multi-purpose timer
		protected float Timer = 0;
		//Multi-purpose switch
		protected bool Switch = false;
		//spawner that made me
		[HideInInspector]
		public AnimalGeneration mySpawner;

		// Use this for initialization
		protected virtual void Start ()
		{
			coll = GetComponent<Collider2D> ();
			rbody = GetComponent<Rigidbody2D> ();
			anim = GetComponent<Animator> ();
			Timer = Random.Range (5, 10);
		}
	
		// Update is called once per frame
		protected virtual void FixedUpdate ()
		{

			switch (MyMovement) {
			case MovementType.StraightLine:
				StraightLineMovement ();
				break;
			case MovementType.DipBack:
				DipBackMovement ();
				break;
			case MovementType.PlayDead:
				PlayDeadMovement ();
				break;
			case MovementType.StopRandom:
				StopRandomMovement ();
				break;
			}
		}

		protected virtual void StraightLineMovement (int movementMult = 1)
		{
			rbody.velocity = 
				movementVectors [0] *
			((movementOverride >= 0) ? movementOverride : movementSpeed) *
			transform.localScale.y *
			movementMult;
		}

		protected virtual void DipBackMovement ()
		{
			if (Timer > 0)
				Timer -= Time.deltaTime;

			StraightLineMovement ((Timer > 0) ? 1 : -1);
		}

		protected virtual void PlayDeadMovement ()
		{
			if (Timer > 0) {
				Timer -= Time.deltaTime;
			} else if (!Switch) {
				Switch = true;
				Timer = Random.Range (5, 10);
			}

			StraightLineMovement ((Switch) ? 
				(-1) : 
				((Timer > 0) ? 1 : 0)
			);
		}

		protected virtual void StopRandomMovement ()
		{
			if (Timer > 0) {
				Timer -= Time.deltaTime;
			} else if (!Switch) {
				Switch = true;
				Timer = Random.Range (2, 4);
			}

			StraightLineMovement ((Switch) ?
                (-1) :
                ((Timer > 0) ? 1 : 0)
			);
		}

		virtual protected void Death ()
		{
			anim.SetBool ("isDead", true);
			StaticItems.AnimalsHit++;
			StaticItems.healthValue -= 10;
			movementSpeed = 0;
			coll.enabled = false;
			StaticItems.playerHealth.DamageTaken (1);
			StaticItems.scoreValue -= 1000f;
		}

		virtual protected void OnBecameInvisible ()
		{
			Disappear ();
		}

		void Disappear ()
		{
			if (mySpawner != null)
				mySpawner.SpawnedAnimals.Remove (this);
			if (!isDead)
				StaticItems.scoreValue += 1000f;
			AnimalSpawningWatchtower.AllSpawnedAnimals.Remove (this);
			Destroy (this.gameObject);
		}
	}
}
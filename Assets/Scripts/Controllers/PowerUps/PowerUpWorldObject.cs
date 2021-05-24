using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
	TrafficCone,
	Barricade,
	Airhorn
}

public class PowerUpWorldObject : MonoBehaviour
{

	public PowerUpType type;

	public float Timer = 0;
	Collider2D coll;
	Rigidbody2D rbody;

	//add basic powerup funincatllity (spawning, pickup, show on ui that you have it, ability to use it)
	public void Start ()
	{
		coll = GetComponent<Collider2D> ();
		rbody = GetComponent<Rigidbody2D> ();
	}

	public void Update ()
	{
		if (Timer > 0)
			Timer -= Time.deltaTime;

		if (Timer <= 0) {
			Destroy (this.gameObject);
		}
	}

	//When tapped
	virtual protected void OnMouseDown ()
	{
		if (!StaticItems.Paused) {
			switch (type) {
			case PowerUpType.TrafficCone:
				StaticItems.TrafficConeCount++;
				break;
			case PowerUpType.Barricade:
				StaticItems.BarricadeCount++;
				break;
			case PowerUpType.Airhorn:
				StaticItems.AirhornCount++;
				break;
			}
			//hook for initiating pause
			Destroy (this.gameObject);
		}
	}
}

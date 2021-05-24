using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B4T.Animals
{
	public class Skunk : AnimalController
	{
		protected override void Start ()
		{
			base.Start ();
		}

		protected override void FixedUpdate ()
		{
			base.FixedUpdate ();
		}

		protected override void Death ()
		{
			deathCloud ();
			base.Death ();
		}

		protected override void OnBecameInvisible ()
		{
			base.OnBecameInvisible ();
		}

		void deathCloud ()
		{
            
		}
	}
}
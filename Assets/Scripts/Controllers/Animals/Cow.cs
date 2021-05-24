using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B4T.Animals
{
	public class Cow : AnimalController
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
			base.Death ();
		}

		protected override void OnBecameInvisible ()
		{
			base.OnBecameInvisible ();
		}

		void cowSpecial ()
		{
			//cout << "COW IS HERE AND HERE TO STAY";
			//make screen saw warning cow
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B4T.Vehicles
{
    public class Bus : VehicleController
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            movementSpeed = Random.Range(2, 4);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void Movement()
        {
            base.Movement();
        }

        protected override void OnMouseDown()
        {
            base.OnMouseDown();
        }

        protected override void OnCollisionEnter2D(Collision2D coll)
        {
            base.OnCollisionEnter2D(coll);
        }
    }
}
using UnityEngine;
using System;
using System.Collections;

public class partMove : baseMove
{

	#region Exposed to UI values
		public ParticleSystem MyPartSystem;
		public float RadiusRatioParts;
	#endregion
	
	
		// Update is called once per frame
		void Update ()
		{
				base.Update ();
				
				if (MyPartSystem != null) {
						if (Stop == true) {
								// Stop if out of screen
								MyPartSystem.Stop ();
								Debug.Log (gameObject.name + ": Particles stop");
								//Destroy (gameObject);
						}
					
						if (!MyPartSystem.IsAlive ()) {
								Destroy (gameObject);
								Debug.Log (gameObject.name + ": Particles ended -> Die now");
						}
						
				} else {
						Destroy (gameObject);
				}

				transform.Translate (Velocity * Time.deltaTime);


		}
}

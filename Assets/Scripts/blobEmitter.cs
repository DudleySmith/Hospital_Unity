using UnityEngine;
using System;
using System.Collections;

public class blobEmitter : MonoBehaviour
{

	#region Exposed to UI values
		public ParticleSystem MyPartSystem;
		public float RadiusRatioParts;

		public string comment;
		public double lifeTimeMillis;
		public double life;
	#endregion

	#region Properties
		// Exposes the position
		public Vector3 Position {
				get {
						return transform.position;
				}
				set {
						transform.position = value;
				}
		}
	
		private Vector3 _velocity;
		public Vector3 Velocity {
				get {
						return _velocity;
				}
				set {
						_velocity = value;
				}
		}
	
		// Exposes graphics property
		private float _radius;
		public float Radius {
				get {
						return _radius;
				}
				set {
						if (_radius != value) {
								// Set local value
								_radius = value;
								// Set particle size
								if (MyPartSystem != null) {
										//MyPartSystem..maxSize = Radius * RadiusRatioParts;
								}
						}
				}
		}

		private DateTime _lastMove;
		public DateTime LastMove {
				get {
						return _lastMove;
				}
				set {
						_lastMove = value;
				}
		}

		public bool _stop;
		public bool Stop {
				get {
						return _stop;
				}
				set {
						_stop = value;
				}
		}
		
		private bool _isRandomMove = false;
		public bool IsRandomMove {
				get {
						return _isRandomMove;
				}
				set {
						_isRandomMove = value;
				}
		}
		
	#endregion

		// Use this for initialization
		void Start ()
		{
				//_randomMove = new Vector3 (UnityEngine.Random.Range (-1, 1), 0, UnityEngine.Random.Range (-1, 1));
				//_randomMoveSpeed = 2.5f;
		}
	
		// Update is called once per frame
		void Update ()
		{
				// Calculate and Display lifetime
				life = DateTime.UtcNow.Subtract (LastMove).TotalMilliseconds;
				if (life > lifeTimeMillis) {
						//Stop = true;
				}
				
				// Check the postion relative to resolution and stop it ---
				GameObject sceneCamObj = GameObject.Find ("MainCamera");
				if (sceneCamObj != null) {
						// Should output the real dimensions of scene viewport
						Vector3 screenPosition = sceneCamObj.camera.WorldToScreenPoint (this.Position);
						Vector3 screenLimits = new Vector3 (0, 0, 0);
						screenLimits.x = sceneCamObj.camera.pixelWidth;
						screenLimits.y = sceneCamObj.camera.pixelHeight;
						
						//Debug.Log ("Camera limits:" + screenLimits + " : Position:" + screenPosition);
						// Here we have y check (not z check), because it's screen coordinates
						if (screenPosition.x <= 0 
								|| screenPosition.y <= 0 
								|| screenPosition.x >= screenLimits.x
								|| screenPosition.y >= screenLimits.y) {
								Stop = true;
								Debug.Log (gameObject.name + ": No more visible -> imminent death");
						}
						
				} else {
						Debug.LogError ("Well, no camera...");
				}
				
				
				
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

				transform.Translate (_velocity * Time.deltaTime);


		}
}

using UnityEngine;
using System;
using System.Collections;

public class baseMove : MonoBehaviour
{

	#region Exposed to UI values
		// Just for displaying
		public string comment;
		public double life;
	#endregion
		
		// Private calculation
		public double lifeTimeMillis {
				get;
				set;
		}
	
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
						// TO DO : More efficient smooth velocity (lerp, etc.)
						_velocity = 0.5f * _velocity + 0.5f * value;
				}
		}
		
		public float Angle {
				set {
						Vector3 newAngle = new Vector3 (0, value, 0);
						//Vector3 oldAngle = transform.localEulerAngles;
						
						this.gameObject.transform.localEulerAngles = newAngle;
						
						//this.gameObject.transform.Rotate (new Vector3 (0, value, 0));
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
	
		}
	
		// Update is called once per frame
		protected void Update ()
		{
				// Move more naturally
				this.gameObject.transform.Translate (_velocity * Time.deltaTime);
				
				// Calculate and Display lifetime
				life = DateTime.UtcNow.Subtract (LastMove).TotalMilliseconds;
				if (life > lifeTimeMillis) {
						Stop = true;
						Debug.Log (gameObject.name + ": Time to die");
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
								
								Debug.Log (gameObject.name 
										+ ": Screen position=" + screenPosition.ToString ()
										+ ": Screen limits=" + screenLimits.ToString ()
										+ ": No more visible -> imminent death");
						}
			
				} else {
						Debug.LogError ("Well, no camera...");
				}
		
		
		
		}
}

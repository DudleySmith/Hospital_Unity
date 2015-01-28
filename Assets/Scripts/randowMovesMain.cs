using UnityEngine;
using System;
using System.Collections;

public class randowMovesMain : MonoBehaviour
{

		public partMove randomEmitter;
	
		// Use this for initialization
		void Start ()
		{
				//randomEmitter = new blobEmitter ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyUp (KeyCode.Space)) {
						Debug.Log ("hit and add : " + System.DateTime.Today);
						InstanciateOne ();
				}
		
				if (Input.GetMouseButtonUp (0)) {
						InstanciateOne ();
				}
		}
	
		void InstanciateOne ()
		{
				partMove instance = Instantiate (randomEmitter) as partMove;
				instance.IsRandomMove = true;
				instance.Velocity = new Vector3 (UnityEngine.Random.Range (-1f, 1f), 0f, UnityEngine.Random.Range (-1f, 1f));
				instance.LastMove = DateTime.UtcNow;
				// Check the postion relative to resolution and stop it ---
				GameObject sceneCamObj = GameObject.Find ("MainCamera");
				if (sceneCamObj != null) {
						Vector3 newPos;
						newPos = sceneCamObj.camera.ScreenToWorldPoint (Input.mousePosition);
						newPos.y = 0;
						instance.Position = newPos;
				}
				//Vector3 mouseMove = Input.mousePosition.normalized;
				//mouseMove.y = 0;
				//instance.Move = mouseMove;
				
		}
}

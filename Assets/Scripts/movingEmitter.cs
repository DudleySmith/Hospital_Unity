using UnityEngine;
using System.Collections;

public class MovingEmitter : MonoBehaviour
{

		public float moveSpeed = 10f;
		public float destroyDistance = 10f;
		public ParticleSystem partSystem;

		private Vector3 move = new Vector3 (Random.value, 0, Random.value);
	
		// Use this for initialization
		void Start ()
		{
				Vector3 newPosition = new Vector3 (Random.Range (-5f, 5f), 0, -1f * destroyDistance);
				transform.position = newPosition;


		}
	
		// Update is called once per frame
		void Update ()
		{
				// Destroy if going out
				if (transform.position.z >= destroyDistance) {
						// Stop if out of screen
						partSystem.Stop ();
				}

				if (!partSystem.IsAlive ()) {
						Destroy (gameObject);
				}

				// Move even i
				transform.Translate (move * moveSpeed * Time.deltaTime);
				//Debug.Log("Move : " + move.z.ToString());

		}
}

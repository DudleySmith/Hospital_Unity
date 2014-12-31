using UnityEngine;
using System.Collections;

public class triggers : MonoBehaviour
{

		public photo ExtPhoto;

		void OnTriggerEnter (Collider collider)
		{
				Debug.Log ("Collision, collider name : " + collider.gameObject.name + " My name : " + this.gameObject.name);
				// Instantiate an object
				photo newPhoto = Instantiate (ExtPhoto) as photo;
				newPhoto.transform.position = collider.transform.position;
				
		}
		void OnTriggerStay (Collider collider)
		{
				//Debug.Log ("On Stay, collider name : " + collider.gameObject.name);
		}
		void OnTriggerExit (Collider collider)
		{
				//Debug.Log ("On Exit, collider name : " + collider.gameObject.name);
		}
	
}

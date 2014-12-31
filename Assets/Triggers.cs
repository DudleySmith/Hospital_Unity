using UnityEngine;
using System.Collections;

public class Triggers : MonoBehaviour
{

	void Start ()
	{
		
	}
		
	void Update ()
	{
		
	}
		
	void OnTriggerEnter (Collider collider)
	{
		Debug.Log ("On Enter, collider name : " + collider.gameObject.name);
	}
	void OnTriggerStay (Collider collider)
	{
		Debug.Log ("On Stay, collider name : " + collider.gameObject.name);
	}
	void OnTriggerExit (Collider collider)
	{
		Debug.Log ("On Exit, collider name : " + collider.gameObject.name);
	}
	
}

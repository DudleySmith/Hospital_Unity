using UnityEngine;
using System.Collections;

public class InstantanciateEverybody : MonoBehaviour {

	public MovingEmitter emitterPrefab;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Space)){
  			Debug.Log("hit and add : " + System.DateTime.Today);
			Instantiate(emitterPrefab);
		}
	}
}

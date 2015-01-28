using UnityEngine;
using System.Collections;

public class ropeMove : baseMove
{	
		// Update is called once per frame
		void Update ()
		{
				base.Update ();
				
				if (Stop == true) {
						Destroy (gameObject);
				}
		}
}

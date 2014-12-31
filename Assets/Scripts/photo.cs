using UnityEngine;
using System;
using System.Collections;

public class photo : MonoBehaviour
{

		public float fadingTimeInSec;
		
		private DateTime _birthDate;
		
		
		// Use this for initialization
		void Start ()
		{
				Texture2D tex;
				
				tex = Resources.Load<Texture2D> (GetResourceFullPath (UnityEngine.Random.Range (1, 99)));
				this.renderer.material.mainTexture = tex;
				
				_birthDate = DateTime.UtcNow;
				//StartCoroutine (FadeOut (fadingTimeInSec, renderer.material));
		}
		
		// Update is called once per frame
		void Update ()
		{
				// Ratio life
				float lifeRatio = 0.5f * (float)(DateTime.UtcNow.Subtract (_birthDate).TotalMilliseconds / (double)(fadingTimeInSec * 1000f));
			
				// Destroy after 5s
				if (lifeRatio > 0.5f) {
						Destroy (this.gameObject);
				}
				
				Color fadeColor = new Color (0.5f - lifeRatio, 0.5f - lifeRatio, 0.5f - lifeRatio);
				//fadeColor.a = lifeRatio;
				this.renderer.material.SetColor ("_TintColor", fadeColor);
				
		}

		static string GetResourceFullPath (int index)
		{
				String folder = "HiCulture/";
				String name = "HiCulture_";
				String indexFormat = "00";
				
				String fullPath = folder + name + index.ToString (indexFormat);
				
				return fullPath;
				
		}
}

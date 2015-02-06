//
//	  UnityOSC - Example of usage for OSC receiver
//
//	  Copyright (c) 2012 Jorge Garcia Martin
//
// 	  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// 	  documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// 	  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
// 	  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// 	  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// 	  of the Software.
//
// 	  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// 	  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// 	  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// 	  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// 	  IN THE SOFTWARE.
//

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class oscMain : MonoBehaviour
{
	
		private Dictionary<string, ServerLog> servers;
		private Dictionary<string, baseMove> movingBlobs;

		public baseMove prefabCam1;
		public baseMove prefabCam2;
		public baseMove prefabCam3;
		
		public float 	LimitX = 10;
		public float 	LimitZ = 10;
		public float 	BirthPlaneY;
		public double	WorldLifeTimeMillis;
		
		private long lastTimeStamp = DateTime.UtcNow.Ticks;

		// Script initialization
		protected void Start ()
		{	
				OSCHandler.Instance.Init (); //init OSC
				OSCHandler.Instance.CreateServer ("cam1", 1551);
				OSCHandler.Instance.CreateServer ("cam2", 1552);
				OSCHandler.Instance.CreateServer ("cam3", 1553);
				// Instantiate arrays
				servers = new Dictionary<string, ServerLog> ();
				movingBlobs = new Dictionary<string, baseMove> ();
		}

		// NOTE: The received messages at each server are updated here
		// Hence, this update depends on your application architecture
		// How many frames per second or Update() calls per frame?
		protected void Update ()
		{
		
				OSCHandler.Instance.UpdateLogs ();
				servers = OSCHandler.Instance.Servers;

				// SERVERS --
				foreach (KeyValuePair<string, ServerLog> thisServer in servers) {
						// PACKETS --
						int idxPacket = 0;
						foreach (var thisPacket in thisServer.Value.packets) {
								if (lastTimeStamp < thisPacket.TimeStamp) {

										lastTimeStamp = thisPacket.TimeStamp;

										idxPacket++;
										// DATAS --
										int idxData = 0;
										foreach (var thisPacketData in thisPacket.Data) {
												idxData++;
												OSCMessage m = thisPacketData as OSCMessage;
												// --
												AnalyseBlob (m);
												//LogValue(m);
												// --
										}
								}
						}

						// Empty packets read --
						thisServer.Value.packets.Clear ();
						thisServer.Value.log.Clear ();

				}
	
		}

		// This is a blob
		// We have to analyse coordinates to place graphics
		private void AnalyseBlob (OSCMessage _m)
		{
				// A blob, analyse
				// Splitting, finding blob number
				char[] seps = {'/'};
				String[] addresses = _m.Address.Split (seps, StringSplitOptions.RemoveEmptyEntries);
				if (addresses [1].Equals ("blobs") && _m.Data.Count == 7) {
		
						// Analysing ----------------------------------------------------
						string receivedBlobNumber = _m.Data [0].ToString ();
						
						// TO DO : use map and some UI parameters
						Vector2 recPosition = new Vector2 ((float)_m.Data [1], 1.0f - (float)_m.Data [2]);
						float recRadius = (float)_m.Data [3];
						Vector2 recVelocity = new Vector2 ((float)_m.Data [4], 1.0f - (float)_m.Data [5]);
						float recAngle = (float)_m.Data [6];
						
						// Calculation -------------------------------------------
						string key = addresses [0] + "_" + receivedBlobNumber;
						
						// Position is relative to screen ------------------------
						Vector3 calculatedPosition = new Vector3 ();
						Vector3 calculatedVelocity = new Vector3 ();
						
						// Check the postion relative to resolution and stop it ---
						GameObject sceneCamObj = GameObject.Find ("MainCamera");
						if (sceneCamObj != null) {
								// Transform screen to world coordinates
								Vector3 screenPosition = new Vector3 (
					(float)(recPosition.x * sceneCamObj.camera.pixelWidth),
					(float)(recPosition.y * sceneCamObj.camera.pixelHeight),
					0);
				
								calculatedPosition = sceneCamObj.camera.ScreenToWorldPoint (screenPosition);
								calculatedPosition.y = BirthPlaneY;
								
								// Should output the real dimensions of scene viewport
								/*
								Vector3 screenVelocity = new Vector3 (
					(float)(receivedVelocity.x * sceneCamObj.camera.pixelWidth),
					(float)(receivedVelocity.y * sceneCamObj.camera.pixelHeight),
					0);
				
								calculatedVelocity = sceneCamObj.camera.ScreenToWorldPoint (screenVelocity);
								*/
								calculatedVelocity.Set (recVelocity.x, 0, recVelocity.y);
								
								//calculatedPosition.x = (2 * receivedPosition.x - 1) * LimitX;
								//calculatedPosition.y = BirthPlaneY;
								//calculatedPosition.z = (1 - 2 * receivedPosition.z) * LimitZ;
						}
						
						//float calcRotation = Vector2.Angle (new Vector2 (1, 1), new Vector2 (calculatedVelocity.x, calculatedVelocity.y));
						float calcRotation = recAngle;
						Debug.Log (key + " : Vel=" + calculatedVelocity + " : Ang=" + calcRotation);
						
						// Search and manage which blobs are awake
						//if(blobEmitters.ContainsKey(key)){
						// exists -> update position or destroy
						baseMove blobValue;
						if (movingBlobs.TryGetValue (key, out blobValue)) {
								if (blobValue != null) {	
										// Move it
										blobValue.Position = calculatedPosition;
										blobValue.Velocity = calculatedVelocity;
										blobValue.Radius = recRadius;
										blobValue.LastMove = DateTime.UtcNow;
										blobValue.Angle = calcRotation;
								} else {
										movingBlobs.Remove (key);
								}
						} else {
								// does not exist -> Create
								baseMove newBlobEmit = new partMove ();
								
								// Instantiate any of prefab
								if (addresses [0].Equals ("cam1")) {
										newBlobEmit = Instantiate (prefabCam1) as baseMove;

								} else if (addresses [0].Equals ("cam2")) {
										newBlobEmit = Instantiate (prefabCam2) as baseMove;

								} else if (addresses [0].Equals ("cam3")) {
										newBlobEmit = Instantiate (prefabCam3) as baseMove;

								}

								// If address matches with a cam, we can add
								if (newBlobEmit != null) {
										// Roll it
										//Instantiate(newBlobEmit);
										//newBlobEmit.Position = calculatedPosition;
										newBlobEmit.Velocity = calculatedVelocity;
										newBlobEmit.Angle = calcRotation;
										newBlobEmit.Radius = recRadius;
										newBlobEmit.gameObject.name = "blob_" + key;
										newBlobEmit.comment = "Blob:" + key;
										newBlobEmit.LastMove = DateTime.UtcNow;
										newBlobEmit.lifeTimeMillis = WorldLifeTimeMillis;
										// Add in array
										movingBlobs.Add (key, newBlobEmit);
								}

								//Debug.Log (String.Format ("BLOB. Number={0} X={1} Z={2} Y (constant) ={3} Radius={4}", key, calculatedPosition.x, calculatedPosition.z, calculatedPosition.y, radius));
			

						}


				}

		}

		// Simple log of everything
		private void LogValue (OSCMessage _m)
		{
				// VALUES --
				int idxValue = 0;
				foreach (var thisValue in _m.Data) {
						// -- -- 
						idxValue++;
						Debug.Log (String.Format ("{0} | Address: {1} Type: {2} Value: {3}",
			                          idxValue, 
			                          _m.Address, 
			                          thisValue.GetType (), 
			                          thisValue));
				}
		}
		
}
/* Copyright 2018 and onwards Augumenta Ltd.
*
*  Licensed under the Apache License, Version 2.0 (the "License");
*  you may not use this file except in compliance with the License.
*  You may obtain a copy of the License at
*
*  http://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing,  software
*  distributed under the License is distributed on an "AS IS"  BASIS,
*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either    express or implied.
*  See the License for the specific language governing permissions and
*  limitations under the License.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Augumenta;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PhysicalFingerController : PoseTransformer
{
	public GameObject fingerPrefab;

	public Image buttonImage;
	int buttonPresses = 0;

	GameObject fingerObject;

	public override void Start()
	{
		base.Start();
		// Apriori calibration matrix
		// (unsing Augumenta camera-display-eye calibration)
		calibration = new Matrix4x4(new Vector4(0.92512f, -0.00484f, 0.00281f, 0.00167f),
									new Vector4(0.00545f, 0.76185f, -0.50441f, 0.03845f),
									new Vector4(0.06192f, 0.54036f, 0.76089f, 0.07454f),
									new Vector4(0.00000f, 0.00000f, 0.00000f, 1.00000f)).transpose;
	}

	public override void OnPose(PoseEvent e, bool isNew, Vector3 position, Quaternion rotation)
	{
		// Check that the pose is within visible view
		Vector3 screenPos = Camera.main.WorldToViewportPoint(position);
		// Only detect if the OnPose object is detected where the display is
		if(screenPos.x >= -0.25f && screenPos.x <= 1.25f &&
				screenPos.y >= 0f && screenPos.y <= 1f) {
			CancelInvoke("LosePhysicalPose");

			if(fingerObject == null) {
				// New pose: create the rigidbody object for it
				fingerObject = Instantiate(fingerPrefab, Camera.main.transform);
				fingerObject.transform.position = position;
			} else {
				// Smooth the motion of the kinematic rigidbody to ensure better
				// collision with buttons
				PositionLerp positionLerp=fingerObject.GetComponent<PositionLerp>();
				if(positionLerp != null) {
					positionLerp.SetTarget(position);
				}
			}
		} else {
			// Don't loose it too fast as it might come back...
			if(!IsInvoking("LosePhysicalPose")) {
				Invoke("LosePhysicalPose", 0.5f);
			}
		}
	}

	public override void OnPoseLost(PoseEvent e)
	{
		if(!IsInvoking("LosePhysicalPose")) {
			Invoke("LosePhysicalPose", 0.5f);
		}
	}

	void LosePhysicalPose()
	{
		if(fingerObject) {
			Destroy(fingerObject);
			fingerObject = null;
		}
	}

	public void OnButtonPress(GameObject button)
	{
		// Keep alternating the button image between green/red after each press
		++buttonPresses;
		if((buttonPresses % 2) == 1) {
			buttonImage.color = Color.green;
		} else {
			buttonImage.color = Color.red;
		}
	}
}

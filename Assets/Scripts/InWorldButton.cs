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
using UnityEngine.Events;

[System.Serializable]
public class WorldButtonPressedEvent : UnityEvent<GameObject>
{
}

public class InWorldButton : MonoBehaviour
{

	[Tooltip("How far in metters (Z-direction) to push the button")]
	public float pressDistance = 0.045f;

	bool isPressed = false;
	Vector3 originalPosition;
	Rigidbody body;

	public WorldButtonPressedEvent pressed = new WorldButtonPressedEvent();

	private void Start()
	{
		body = GetComponent<Rigidbody>();
		originalPosition = transform.localPosition;

		body.velocity = Vector3.zero;

		isPressed = false;
	}

	private void FixedUpdate()
	{
		// Keep clamped on the XY plane
		Vector3 posNow = transform.localPosition;
		posNow.x = originalPosition.x;
		posNow.y = originalPosition.y;

		if(posNow.z > originalPosition.z) {
			body.AddRelativeForce(new Vector3(0f, 0f, -(posNow.z - originalPosition.z) * 20f));
		} else {
			posNow.z = originalPosition.z;
			body.velocity = Vector3.zero;
		}

		transform.localPosition = posNow;

		bool wasPressed = isPressed;
		isPressed = (transform.localPosition - originalPosition).z > pressDistance;
		if(isPressed && !wasPressed) {
			pressed.Invoke(gameObject);
		}
	}
}

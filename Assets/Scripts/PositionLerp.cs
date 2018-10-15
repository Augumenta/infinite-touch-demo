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

public class PositionLerp : MonoBehaviour
{
	bool moving = false;
	Vector3 targetPos;

	public void SetTarget(Vector3 newTarget)
	{
		moving = true;
		targetPos = newTarget;
	}

	// Update is called once per frame
	void Update()
	{
		if(moving) {
			transform.position = Vector3.Lerp(transform.position, targetPos, Mathf.Min(1f, 15f * Time.deltaTime));
			if((transform.position - targetPos).magnitude < 0.001f) {
				transform.position = targetPos;
				moving = false;
			}
		}
	}
}

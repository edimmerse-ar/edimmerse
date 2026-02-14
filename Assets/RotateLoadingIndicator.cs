using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLoadingIndicator : MonoBehaviour
{
	// Rotation speed in degrees per second
	public float rotationSpeed = 100f;

	void Update()
	{
		// Rotate the object around its Y-axis
		transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
	}
}

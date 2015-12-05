﻿using UnityEngine;

/// <summary>
/// A GameObject with a SpaceMass attached is just an object floating in space. It has a size, which determines
/// how big it is physically, and can optionally orbit another SpaceMass.
/// </summary>
[ExecuteInEditMode]
public class SpaceMass : MonoBehaviour {

	/// <summary>
	/// Gets the logical size of this SpaceMass.
	/// </summary>
	/// <value>The logical size of this SpaceMass.</value>
	public float Size {
		get { return size; }
	}

	/// <summary>
	/// Gets the radius of this SpaceMass in game distance units.
	/// </summary>
	/// <value>The radius of this SpaceMass.</value>
	public float Radius {
		get { return size / 2; }
	}

	/// <summary>
	/// The size of the space mass. Determines the scale of the sphere object on initialize.
	/// </summary>
	[SerializeField] private float size = 1f;

	/// <summary>
	/// The primary body this mass orbits around. If set to null, this object is stationary.
	/// </summary>
	[SerializeField] private SpaceMass orbitalPrimary = null;

	/// <summary>
	/// The speed at which this body orbits its primary.
	/// </summary>
	[SerializeField] private float orbitSpeed = 1f;

	/// <summary>
	/// If set to true, this body orbits its primary clockwise.
	/// </summary>
	[SerializeField] private bool orbitsClockwise = true;

	/// <summary>
	/// The amount of food on this space mass.
	/// </summary>
	[SerializeField] private int food = 5;

	/// <summary>
	/// The calculated distance between this body and its primary.
	/// </summary>
	private float distanceToPrimary;

	/// <summary>
	/// The current angle between this body and its primary.
	/// </summary>
	private float angleToPrimary;

	/// <summary>
	/// Initialize this component.
	/// </summary>
	private void Start () {

		// Initialize the SpaceMass with initial values.
		Initialize (size, orbitalPrimary, orbitSpeed, orbitsClockwise, distanceToPrimary, angleToPrimary, food);
	}

	/// <summary>
	/// Update this component.
	/// </summary>
	private void Update () {

		// If this body orbits a primary, update the orbit.
		if (orbitalPrimary != null) {

			// Increment the angle to the primary based on direction, speed, and frame time.
			angleToPrimary += (orbitsClockwise ? -1 : 1) * orbitSpeed * Time.deltaTime;

			// Recalculate and update the position of this object's transform.
			transform.position = MathUtil.RadialPosition (
				distanceToPrimary, angleToPrimary,orbitalPrimary.transform.position);
		}
	}

	/// <summary>
	/// Reinitialize this space mass.
	/// </summary>
	/// <param name="size">Size.</param>
	/// <param name="orbitalPrimary">Orbital primary.</param>
	/// <param name="orbitSpeed">Orbit speed.</param>
	/// <param name="orbitsClockwise">If set to <c>true</c> orbits clockwise.</param>
	/// <param name="orbitRadius">Orbit radius.</param>
	/// <param name="startAngle">Start angle.</param>
	public void Initialize (float size, SpaceMass orbitalPrimary, float orbitSpeed,
		bool orbitsClockwise, float orbitRadius, float startAngle,
		int food = 5) {

		// Copy logical data.
		this.size = size;
		this.orbitalPrimary = orbitalPrimary;
		this.orbitSpeed = orbitSpeed;
		this.orbitsClockwise = orbitsClockwise;
		this.food = food;

		// Apply the logical size to the physical body.
		gameObject.GetComponentInChildren<CircleCollider2D> ().radius = size;
		gameObject.GetComponentInChildren<Renderer> ().transform.localScale = Vector3.one * size;

		// If this body orbits a primary, initialize the orbit.
		if (orbitalPrimary != null) {

			// Position in orbit.
			transform.position = MathUtil.RadialPosition (orbitRadius, startAngle, orbitalPrimary.transform.position);

			// Calculate the distance to the primary.
			distanceToPrimary = Vector2.Distance (transform.position, orbitalPrimary.transform.position);

			// Calculate the initial angle to the primary.
			angleToPrimary = MathUtil.AngleBetweenPoints (transform.position, orbitalPrimary.transform.position);
		}
	}

	/// <summary>
	/// Select this SpaceMass.
	/// </summary>
	public void Select () {
		UserInterface.Select (GetComponent<Selectable> ());
	}
}

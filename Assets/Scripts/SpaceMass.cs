using UnityEngine;

/// <summary>
/// A GameObject with a SpaceMass attached is just an object floating in space. It has a size, which determines
/// how big it is physically, and can optionally orbit another SpaceMass.
/// </summary>
public class SpaceMass : MonoBehaviour {

	/// <summary>
	/// Gets the logical size of this SpaceMass.
	/// </summary>
	/// <value>The logical size of this SpaceMass.</value>
	public float Size {
		get { return size; }
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
	void Start () {

		// Apply the logical size to the physical body.
		gameObject.GetComponentInChildren<SphereCollider> ().transform.localScale = Vector3.one * size;

		// If this body orbits a primary, initialize the orbit.
		if (orbitalPrimary != null) {

			// Calculate the distance to the primary.
			distanceToPrimary = Vector2.Distance (transform.position, orbitalPrimary.transform.position);

			// Calculate the initial angle to the primary.
			angleToPrimary = Mathf.Atan2 (
				transform.position.y - orbitalPrimary.transform.position.y,
				transform.position.x - orbitalPrimary.transform.position.x);
		}
	}

	/// <summary>
	/// Update this component.
	/// </summary>
	void Update () {

		// If this body orbits a primary, update the orbit.
		if (orbitalPrimary != null) {

			// Increment the angle to the primary based on direction, speed, and frame time.
			angleToPrimary += (orbitsClockwise ? -1 : 1) * orbitSpeed * Time.deltaTime;

			// Recalculate and update the position of this object's transform.
			float x = orbitalPrimary.transform.position.x + distanceToPrimary * Mathf.Cos (angleToPrimary);
			float y = orbitalPrimary.transform.position.y + distanceToPrimary * Mathf.Sin (angleToPrimary);
			transform.position = new Vector2 (x, y);
		}
	}
}

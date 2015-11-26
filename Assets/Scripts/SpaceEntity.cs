using UnityEngine;

/// <summary>
/// A SpaceEntity is an entity that exists in space and probably has a home on a space mass.
/// </summary>
public class SpaceEntity : MonoBehaviour {

	/// <summary>
	/// A space entity's home is the SpaceMass they are bound to.
	/// </summary>
	[SerializeField] private SpaceMass home = null;

	/// <summary>
	/// The angle to the center of the entity's current home. If home is null, this value is meaningless.
	/// </summary>
	private float angleToHome;

	/// <summary>
	/// Initialize this component.
	/// </summary>
	void Start () {

		// If this entity starts with a home, attach to it.
		if (home != null) {
			AttachToSpaceMass (home); 
		}
	}

	/// <summary>
	/// Attaches this entity to the specified SpaceMass.
	/// </summary>
	/// <param name="spaceMass">New SpaceMass home for this entity.</param>
	public void AttachToSpaceMass (SpaceMass spaceMass) {

		// Attach the entity to the space mass.
		home = spaceMass;
		transform.SetParent (spaceMass.transform);

		// Calculate and cache the current angle to the center of the space mass.
		angleToHome = MathUtil.AngleBetweenPoints (transform.position, home.transform.position);

		// Update the entity's position against its new home mass.
		UpdatePosition ();
	}

	/// <summary>
	/// Updates the position of this entity so that it respects its home dimensions.
	/// </summary>
	private void UpdatePosition () {

		// Reposition the entity so that it lingers on the edge of the mass.
		transform.localPosition = MathUtil.LocalRadialPosition (home.Radius, angleToHome);

		// Make this entity face inwards.
		transform.localRotation = Quaternion.Euler (0, 0, Mathf.Rad2Deg * angleToHome - 90);
	}
}

using UnityEngine;

/// <summary>
/// A SpaceEntity is an entity that exists in space and probably has a home on a space mass.
/// </summary>
public class SpaceEntity : MonoBehaviour {

	/// <summary>
	/// The frequency that this entity updates its AI behaviors.
	/// </summary>
	private const float BEHAVIOR_UPDATE_FREQUENCY = 2f;

	/// <summary>
	/// A space entity's home is the SpaceMass they are bound to.
	/// </summary>
	[SerializeField] private SpaceMass home = null;

	/// <summary>
	/// The species this SpaceEntity is a member of.
	/// </summary>
	private Species species;

	/// <summary>
	/// The angle to the center of the entity's current home. If home is null, this value is meaningless.
	/// </summary>
	private float angleToHome;

	/// <summary>
	/// This entity's idle wandering velocity.
	/// </summary>
	private float wanderingVelocity = 0;

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
	/// Update this component.
	/// </summary>
	void Update () {

		// Test entity walking around on home.
		if (home != null) {
			MoveAlongSurface (wanderingVelocity * Time.deltaTime);
		}
	}

	/// <summary>
	/// Initialize this SpaceEntity given a species.
	/// </summary>
	/// <param name="species">Species this SpaceEntity will be a member of.</param>
	public void Initialize (Species species) {
		this.species = species;
		species.AddMember (this);
	}

	/// <summary>
	/// Attaches this entity to the specified SpaceMass.
	/// </summary>
	/// <param name="spaceMass">New SpaceMass home for this entity.</param>
	public void AttachToSpaceMass (SpaceMass spaceMass) {

		// Cancel idle wandering.
		CancelInvoke ("UpdateIdleWandering");

		// Base case: detachment.
		if (spaceMass == null) {
			home = null;
			transform.SetParent (null);
			return;
		}

		// Attach the entity to the space mass.
		home = spaceMass;
		transform.SetParent (spaceMass.transform);

		// Calculate and cache the current angle to the center of the space mass.
		angleToHome = MathUtil.AngleBetweenPoints (transform.position, home.transform.position);

		// Update the entity's position against its new home mass.
		UpdatePosition ();

		// Reset idle wandering.
		InvokeRepeating ("UpdateIdleWandering", Random.Range (0, BEHAVIOR_UPDATE_FREQUENCY), BEHAVIOR_UPDATE_FREQUENCY);
	}

	/// <summary>
	/// Select this SpaceEntity.
	/// </summary>
	public void Select () {
		UserInterface.Select (GetComponent<WorldSelectable> ());
	}

	/// <summary>
	/// Moves this entity a given distance along their home's surface.
	/// </summary>
	/// <param name="distance">Distance moved clockwise.</param>
	private void MoveAlongSurface (float distance) {

		// Base case: home is null.
		if (home == null) {
			Debug.LogError ("This entity does not have a home to walk on!", this);
			return;
		}

		// Calculate the new angle against the center of the home.
		angleToHome += MathUtil.AngleFromArcLength (-1 * distance, home.Radius);

		// Update the entity's position against its home mass.
		UpdatePosition ();
	}

	/// <summary>
	/// Updates the position of this entity so that it respects its home dimensions.
	/// </summary>
	private void UpdatePosition () {

		// If this entity has a home, attach to it physically.
		if (home != null) {

			// Reposition the entity so that it lingers on the edge of the mass.
			transform.localPosition = MathUtil.LocalRadialPosition (home.Radius, angleToHome);

			// Make this entity face inwards.
			transform.localRotation = Quaternion.Euler (0, 0, Mathf.Rad2Deg * angleToHome - 90);
		}
	}

	/// <summary>
	/// Updates the idle wandering behavior.
	/// </summary>
	private void UpdateIdleWandering () {

		// Base case: home is null.
		if (home == null) {
			Debug.LogError ("This entity does not have a home to wander on!", this);
			return;
		}

		// Re-roll the wandering velocity.
		wanderingVelocity = Random.Range (-1, 2);
	}
}

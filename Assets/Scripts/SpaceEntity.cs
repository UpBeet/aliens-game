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
	/// Describes the current state of a SpaceEntity.
	/// </summary>
	private enum SpaceEntityState {
		Floating,
		Attached,
		Launching,
	};

	/// <summary>
	/// Gets the SpaceEntity's home.
	/// </summary>
	/// <value>The home.</value>
	public SpaceMass Home {
		get {
			return home;
		}
		private set {
			home = value;
		}
	}

	/// <summary>
	/// A space entity's home is the SpaceMass they are bound to.
	/// </summary>
	[SerializeField] private SpaceMass home = null;

	/// <summary>
	/// The current state of this SpaceEntity.
	/// </summary>
	private SpaceEntityState state = SpaceEntityState.Floating;

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
		if (home != null && state == SpaceEntityState.Attached) {
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
	/// Select this SpaceEntity.
	/// </summary>
	public void Select () {
		UserInterface.Select (GetComponent<WorldSelectable> ());
	}

	/// <summary>
	/// Attaches this entity to the specified SpaceMass.
	/// </summary>
	/// <param name="spaceMass">New SpaceMass home for this entity.</param>
	public void AttachToSpaceMass (SpaceMass spaceMass) {

		// Base case: detachment.
		if (spaceMass == null) {
			home = null;
			transform.SetParent (null);
			SetIsWalking (false);
			SetState (SpaceEntityState.Floating);
			return;
		}

		// Attach the entity to the space mass.
		home = spaceMass;
		transform.SetParent (spaceMass.transform);
		SetState (SpaceEntityState.Attached);

		// Calculate and cache the current angle to the center of the space mass.
		angleToHome = MathUtil.AngleBetweenPoints (transform.position, home.transform.position);

		// Update the entity's position against its new home mass.
		UpdatePosition ();

		// Start walking along the home.
		SetIsWalking (true);
	}

	/// <summary>
	/// Prepare this entity for launch.
	/// </summary>
	public void PrepareForLaunch () {
		SetIsWalking (false);
		SetState (SpaceEntityState.Launching);
		Collider2D collider = GetComponent<Collider2D> ();
		transform.localPosition = new Vector2 (
			collider.offset.x,
			collider.offset.y);
		Camera.main.GetComponent<CameraDrag> ().Follow (home.transform);
		Camera.main.GetComponent<CameraZoom> ().Focus (home.Size);
		gameObject.AddComponent<LaunchController> ();
	}

	/// <summary>
	/// Cancels the launch state.
	/// </summary>
	public void CancelLaunch () {
		Destroy (GetComponent<LaunchController> ());
		AttachToSpaceMass (home);
		UserInterface.Select (home.GetComponent<WorldSelectable> ());
	}

	/// <summary>
	/// Sets the entity's state properly.
	/// </summary>
	/// <param name="newState">New state.</param>
	private void SetState (SpaceEntityState newState) {

		// Exit previous state.

		// Copy new state.
		state = newState;

		// Perform state changes.
		Animator anim = GetComponentInChildren<Animator> ();
		Vector2 spriteOffset = Vector2.zero;;
		switch (state) {
		case SpaceEntityState.Floating:
		case SpaceEntityState.Launching:
			anim.Play ("Idle Floating");
			WorldSelectable.Enabled = false;
			UserInterface.Select (null);
			break;
		case SpaceEntityState.Attached:
			anim.Play ("Idle Attached");
			spriteOffset = new Vector2 (0f, 0.5f);
			WorldSelectable.Enabled = true;
			break;
		}

		// Apply offset changes.
		GetComponent<Collider2D> ().offset = spriteOffset;
		GetComponentInChildren<SpriteRenderer> ().transform.localPosition = spriteOffset;
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
			transform.localPosition = MathUtil.Vector2FromMagnitudeAndAngle (home.Radius, angleToHome);

			// Make this entity face inwards.
			transform.localRotation = Quaternion.Euler (0, 0, Mathf.Rad2Deg * angleToHome - 90);
		}
	}

	/// <summary>
	/// Sets whether or not this SpaceEntity is walking along its home planet.
	/// </summary>
	/// <param name="isWalking">If set to <c>true</c> is walking.</param>
	private void SetIsWalking (bool isWalking) {

		// Cancel the invoke to make sure it doesn't get repeated.
		CancelInvoke ("UpdateIdleWandering");

		// Check if we want this entity to walk, and we have a planet to walk on.
		if (isWalking && home != null) {

			// Reset idle wandering.
			InvokeRepeating ("UpdateIdleWandering", Random.Range (0, BEHAVIOR_UPDATE_FREQUENCY), BEHAVIOR_UPDATE_FREQUENCY);
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

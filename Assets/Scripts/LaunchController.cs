using UnityEngine;

/// <summary>
/// This component attaches to a SpaceEntity and manages the controls during launch.
/// </summary>
[RequireComponent (typeof (SpaceEntity))]
public class LaunchController : MonoBehaviour {

	/// <summary>
	/// Describes the current state of the LaunchController.
	/// </summary>
	private enum LaunchState {
		Free,
		Grabbed,
		Released,
	}

	/// <summary>
	/// The current state of the LaunchController.
	/// </summary>
	private LaunchState state = LaunchState.Free;

	/// <summary>
	/// Reference to the SpaceEntity.
	/// </summary>
	private SpaceEntity entity;

	/// <summary>
	/// The maximum distance you can pull an entity while launching them.
	/// </summary>
	private float maxDistance = 10f;

	/// <summary>
	/// The minimum distance you can pull. Releasing before this threshold will cancel the launch.
	/// </summary>
	private float minDistance = 1f;

	/// <summary>
	/// The amount of force per unity of distance pulled.
	/// </summary>
	private float forceCoefficient = 100f;

	/// <summary>
	/// Initialize this component.
	/// </summary>
	void Start () {
		entity = GetComponent<SpaceEntity> ();
		Camera.main.GetComponent<CameraDrag> ().enabled = false;
	}

	/// <summary>
	/// Deinitialize this component.
	/// </summary>
	void OnDestroy () {
		Debug.LogWarning ("destroyed");
		if (Camera.main != null) {
			CameraDrag drag = Camera.main.GetComponent<CameraDrag> ();
			if (drag != null) {
				drag.enabled = true;
			}
		}
	}

	/// <summary>
	/// Update this component.
	/// </summary>
	void Update () {

		// Controls during the free state.
		if (state == LaunchState.Free) {
	
			// The mouse button is pressed down.
			if (Input.GetMouseButtonDown (0)) {

				// Check if we hit the entity with the grab.
				Collider2D col = UnityUtil.GetCollider2DUnderInput ("Entities");
				if (col == GetComponentInChildren<Collider2D> ()) {

					// Begin grabbing.
					state = LaunchState.Grabbed;
				}
			}
		}

		// Controls during the grabbed state.
		else if (state == LaunchState.Grabbed) {

			// Check if we keep the mouse button down.
			if (Input.GetMouseButton (0)) {

				// Create a scoped copy of the entity and planet positions.
				Vector2 entityPos = UnityUtil.GetInputPosition ();
				Vector2 homePos = entity.Home.transform.position;

				// Clamp to max distance from home.
				if (Vector2.Distance (entityPos, homePos) > maxDistance) {
					entityPos = MathUtil.Vector2FromMagnitudeAndAngle (
						maxDistance, MathUtil.AngleBetweenPoints (entityPos, homePos), homePos);
				}

				// Place the entity at the current input position.
				entity.transform.position = entityPos;
			}

			// Check if the mouse is released while grabbed.
			if (Input.GetMouseButtonUp (0)) {
				Release ();
			}
		}

		// Controls during the released state.
		else if (state == LaunchState.Released) {

			// Cache a scoped copy of the entity and planet positions.
			Vector2 entityPos = entity.transform.position;
			Vector2 homePos = entity.Home.transform.position;

			// Check if we should still be applying force.
			float distance = Vector2.Distance (entityPos, homePos);
			if (distance >= minDistance) {

				// Calculate the launch angle.
				float angle = MathUtil.AngleBetweenPoints (entityPos, homePos);

				// Release the entity and apply a force.
				entity.GetComponent<Rigidbody2D> ().AddForce (
					MathUtil.Vector2FromMagnitudeAndAngle (-distance, angle) * forceCoefficient * Time.deltaTime,
					ForceMode2D.Impulse);
			}

			// Otherwise we can free this entity.
			else {
				entity.FreeFromLaunch ();
			}
		}
	}

	/// <summary>
	/// Release the launch.
	/// </summary>
	private void Release () {

		// Cache scoped copies of entity and planet positions.
		Vector2 entityPos = entity.transform.position;
		Vector2 homePos = entity.Home.transform.position;

		// Get the distance that the user pulled the entity.
		float distance = Vector2.Distance (entityPos, homePos);

		// Check if it is below the cancel threshold.
		if (distance < minDistance) {
			entity.CancelLaunch ();
			return;
		}

		// Switch to released state.
		state = LaunchState.Released;
	}
}

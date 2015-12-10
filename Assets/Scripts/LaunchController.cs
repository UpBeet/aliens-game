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
	private float maxDistance = 5f;

	/// <summary>
	/// The minimum distance you can pull. Releasing before this threshold will cancel the launch.
	/// </summary>
	private float minDistance = 1f;

	/// <summary>
	/// The amount of force per unity of distance pulled.
	/// </summary>
	private float forceCoefficient = 10f;

	/// <summary>
	/// The position of the entity last frame. Used after release.
	/// </summary>
	private Vector2 prevEntityPos;

	/// <summary>
	/// The amount of distance the entity has to travel before being freed. Used after release.
	/// </summary>
	private float distanceToTravel = 0f;

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
		if (Camera.main != null && GetComponent<FloatingController> () == null) {
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

			// Record change in distance since last frame.
			float distanceTravelled = Vector2.Distance (entityPos, prevEntityPos);
			distanceToTravel -= distanceTravelled;

			// Check if we should still be applying force.
			if (distanceToTravel > 0) {

				// Calculate remaining distance and launch angle.
				float distance = Vector2.Distance (entityPos, homePos);
				float angle = MathUtil.AngleBetweenPoints (entityPos, homePos);

				// Release the entity and apply a force.
				entity.GetComponent<Rigidbody2D> ().AddForce (
					MathUtil.Vector2FromMagnitudeAndAngle (-distance, angle) * forceCoefficient * Time.deltaTime,
					ForceMode2D.Impulse);

				// Copy position from last frame.
				prevEntityPos = entityPos;
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

		// Record the distance the entity neds to travel.
		distanceToTravel = distance;

		// Instantiate particles.
		ParticleSystem particles = Instantiate (Resources.Load<ParticleSystem> ("Floating Particles"));
		particles.transform.SetParent (entity.transform);
		particles.transform.localPosition = Vector3.zero;
		particles.transform.localRotation = Quaternion.identity;
		particles.transform.localScale = Vector3.zero;

		// Copy current position into previous position.
		prevEntityPos = entityPos;

		// Switch to released state.
		state = LaunchState.Released;
	}
}

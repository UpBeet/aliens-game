using UnityEngine;

/// <summary>
/// This component attaches to a SpaceEntity and manages the controls during launch.
/// </summary>
[RequireComponent (typeof (SpaceEntity))]
public class LaunchController : MonoBehaviour {

	/// <summary>
	/// Reference to the SpaceEntity.
	/// </summary>
	private SpaceEntity entity;

	/// <summary>
	/// If set to true, the entity is in the player's hands.
	/// </summary>
	private bool grabbed = false;

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
	
		// The mouse button is pressed down.
		if (Input.GetMouseButtonDown (0)) {

			// Check if we hit the entity with the grab.
			Collider2D col = UnityUtil.GetCollider2DUnderInput ("Entities");
			if (col == GetComponentInChildren<Collider2D> ()) {
				grabbed = true;
			}
		}

		// Check if we keep the mouse button down.
		if (Input.GetMouseButton (0)) {

			// Check if we are grabbing the entity.
			if (grabbed) {

				// Place the entity at the current input position.
				entity.transform.position = UnityUtil.GetInputPosition ();

				// Clamp to max distance from home.
			}
		}
	}
}

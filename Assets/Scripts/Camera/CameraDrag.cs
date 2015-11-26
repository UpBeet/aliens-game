using UnityEngine;

/// <summary>
/// Attach to a Camera component to enable touch and drag or click and drag. Moves along the XY plane currently
/// (assumed orthographic or vertical view).
/// </summary>
[RequireComponent (typeof(Camera))]
public class CameraDrag : MonoBehaviour {

	/// <summary>
	/// The drag speed coefficient.
	/// </summary>
	public float dragSpeed = 1f;

	/// <summary>
	/// The drag position last frame.
	/// </summary>
	private Vector2 prevDragPosition = Vector2.zero;

	/// <summary>
	/// The current drag velocity.
	/// </summary>
	private Vector2 velocity = Vector2.zero;

	/// <summary>
	/// Update this component.
	/// </summary>
	void Update () {

		#if (UNITY_EDITOR || UNITY_STANDALONE)

		if (Input.GetMouseButtonDown(0)) {
			prevDragPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton (0)) {
			Vector2 currentMousePosition = Input.mousePosition;
			Vector2 deltaMousePosition = currentMousePosition - prevDragPosition;
			velocity = deltaMousePosition * dragSpeed;
			prevDragPosition = currentMousePosition;
		}
		else {
			velocity *= 0.9f;
		}

		#endif

		transform.Translate (-1 * velocity);
	}
}

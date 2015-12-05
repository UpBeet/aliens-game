using UnityEngine;

/// <summary>
/// Global library of static tools used on Unity engine data throughout the project.
/// </summary>
public static class UnityUtil {

	/// <summary>
	/// Destroys all the children attached to this game object.
	/// </summary>
	/// <param name="gameObject">The specified parent game object.</param>
	public static void DestroyAllChildren (this GameObject gameObject) {
		DestroyAllChildren (gameObject.transform);
	}

	/// <summary>
	/// Destroys all the children attached to this transform.
	/// </summary>
	/// <param name="transform">The specified parent transform.</param>
	public static void DestroyAllChildren (this Transform transform) {
		for (int i = 0; i < transform.childCount; i++) {
			Object.Destroy (transform.GetChild (i));
		}
	}

	/// <summary>
	/// Properly gets the Collider2D under the relevant input.
	/// </summary>
	/// <returns>The Collider2D under input.</returns>
	public static Collider2D GetCollider2DUnderInput () {

		// Get the input position.
		Vector2 input = GetInputPosition ();

		// Get the collider under the input and return it.
		return Physics2D.OverlapPoint (input);
	}

	/// <summary>
	/// Gets the platform-relevant input position.
	/// </summary>
	/// <returns>The input position.</returns>
	public static Vector2 GetInputPosition () {

		// Default to mouse position.
		Vector2 input = Input.mousePosition;

		// Use input to determine world position in 2D space.
		Vector3 wp = Camera.main.ScreenToWorldPoint (input);
		Vector2 inputPosition = new Vector2 (wp.x, wp.y);

		// Return the calculated position.
		return inputPosition;
	}
}

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
}

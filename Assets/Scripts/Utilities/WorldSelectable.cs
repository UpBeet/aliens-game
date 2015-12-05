using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Collider selector.
/// </summary>
public class WorldSelectable : MonoBehaviour {

	/// <summary>
	/// If set to false, selection is totally disabled.
	/// </summary>
	public static bool Enabled = true;

	/// <summary>
	/// Fires when this GameObject is selected.
	/// </summary>
	public UnityEvent OnSelect = new UnityEvent ();

	/// <summary>
	/// Update this component.
	/// </summary>
	public static void Update () {

		// Only update if we are enabled.
		if (Enabled) {

			// Listen for left mouse button release.
			if (Input.GetMouseButtonUp (0)) {

				// Get collider.
				Collider2D col = UnityUtil.GetCollider2DUnderInput ();

				// Try to select it.
				WorldSelectable selected = null;
				if (col != null) {
					selected = col.GetComponentInChildren<WorldSelectable> ();
					if (selected != null) {
						selected.OnSelect.Invoke ();

						#if UNITY_EDITOR

						if (selected != null) {
							UnityEditor.Selection.activeGameObject = selected.gameObject;
						}

						#endif
					}
				}
			}
		}
	}
}

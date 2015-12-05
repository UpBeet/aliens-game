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

				// Use mouse position to determine 
				Vector3 wp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector2 touchPos = new Vector2 (wp.x, wp.y);
				SelectAtPos (touchPos);
			}
		}
	}

	/// <summary>
	/// Tries to select something at the specified position.
	/// </summary>
	/// <param name="pos">Position to select at.</param>
	private static void SelectAtPos (Vector2 pos) {
		Collider2D col = Physics2D.OverlapPoint (pos);
		WorldSelectable selected = null;
		if (col != null) {
			selected = col.GetComponentInChildren<WorldSelectable> ();
			if (selected != null) {
				selected.OnSelect.Invoke ();
			}
		}

		#if UNITY_EDITOR

		if (selected != null) {
			UnityEditor.Selection.activeGameObject = selected.gameObject;
		}

		#endif
	}
}

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Collider selector.
/// </summary>
public class WorldSelectable : MonoBehaviour {

	/// <summary>
	/// Fires when this GameObject is selected.
	/// </summary>
	public UnityEvent OnSelect = new UnityEvent ();

	/// <summary>
	/// Update this component.
	/// </summary>
	public static void Update () {

		// Listen for left mouse button release.
		if (Input.GetMouseButtonUp (0)) {

			// Use mouse position to determine 
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 touchPos = new Vector2(wp.x, wp.y);
			Collider2D col = Physics2D.OverlapPoint (touchPos);
			if (col != null) {
				WorldSelectable selected = col.GetComponentInChildren<WorldSelectable> ();
				if (selected != null) {
					selected.OnSelect.Invoke ();
				}
			}
		}
	}
}

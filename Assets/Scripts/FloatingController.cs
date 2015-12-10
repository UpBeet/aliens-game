using UnityEngine;
using System.Collections;

/// <summary>
/// Attaches to a SpaceEntity to manipulate them while they are floating through space after being launched.
/// </summary>
public class FloatingController : MonoBehaviour {

	/// <summary>
	/// The amount of force applied per second while blowing.
	/// </summary>
	private float blowImpulse = 10f;

	/// <summary>
	/// Update this component.
	/// </summary>
	void Update () {

		// Check if the mouse button is held down.
		if (Input.GetMouseButton (0)) {

			// Get the angle between the input position
			Vector2 inputPos = UnityUtil.GetInputPosition ();
			float angle = MathUtil.AngleBetweenPoints (inputPos, transform.position);

			// Apply a force in the opposite direction.
			GetComponent<Rigidbody2D> ().AddForce (
				MathUtil.Vector2FromMagnitudeAndAngle (-blowImpulse, angle) * Time.deltaTime,
				ForceMode2D.Impulse);
		}
	}
}

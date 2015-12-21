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
	/// Max breath an entity can have
	/// </summary>
	[SerializeField] private float maxBreath = 100f;

	/// <summary>
	/// Current remaining breath of the entity
	/// </summary>
	[SerializeField] private float currentBreath = 100f;

	/// <summary>
	/// The breath loss rate.
	/// </summary>
	[SerializeField] private float breathLossRate = 0.1f;

	/// <summary>
	/// The blowing loss rate multiplier.
	/// </summary>
	[SerializeField] private float blowingLossRateMultiplier = 3f;

	/// <summary>
	/// The current breath loss rate.
	/// </summary>
	private float currentBreathLossRate;

	/// <summary>
	/// Initialize this component.
	/// </summary>
	void Start () {
		Camera.main.GetComponent<CameraDrag> ().enabled = false;
	}

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

			// Set blowing loss rate
			currentBreathLossRate = breathLossRate * blowingLossRateMultiplier;
		} else {
			// Use base loss rate when not blowing
			currentBreathLossRate = breathLossRate;
		}

		// Lose breath over time
		currentBreath -= currentBreathLossRate;

		if (currentBreath <= 0) {
			// destroy the alien when it suffocates
		}
	}

	/// <summary>
	/// Destroy this component.
	/// </summary>
	void OnDestroy () {
		if (Camera.main != null) {
			CameraDrag drag = Camera.main.GetComponent<CameraDrag> ();
			if (drag != null) {
				drag.enabled = true;
			}
		}

		// Re-enable selection.
		WorldSelectable.Enabled = true;
	}
}

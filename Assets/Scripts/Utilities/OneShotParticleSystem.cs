using UnityEngine;

/// <summary>
/// A one-shot particle system destroys itself when its particle system dies.
/// </summary>
[RequireComponent (typeof (ParticleSystem))]
public class OneShotParticleSystem : MonoBehaviour {

	/// <summary>
	/// Reference to the particle system we are listening to.
	/// </summary>
	private ParticleSystem system;

	/// <summary>
	/// Initialize this component.
	/// </summary>
	void Start () {
		system = GetComponentInChildren<ParticleSystem> ();
	}

	/// <summary>
	/// Update this component.
	/// </summary>
	void Update () {
		if (!system.IsAlive ()) {
			Destroy (gameObject);
		}
	}
}

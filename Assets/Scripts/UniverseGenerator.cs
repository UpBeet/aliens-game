using UnityEngine;

/// <summary>
/// If you attach a UniverseGenerator component to a GameObject, it will procedurally generate a universe. You
/// probably only need one of those in the scene at a time.
/// </summary>
public class UniverseGenerator : MonoBehaviour {

	/// <summary>
	/// Reference to the basic SpaceMass prefab.
	/// </summary>
	[SerializeField] private SpaceMass spaceMassPrefab;

	/// <summary>
	/// The size of the first mass generated.
	/// </summary>
	[SerializeField] private float startingMassSize = 20f;

	/// <summary>
	/// The size at which planets stop generating children.
	/// </summary>
	[SerializeField] private float minPlanetSize = 1f;

	/// <summary>
	/// Initialize this component.
	/// </summary>
	void Start () {
		GenerateUniverse ();
	}

	/// <summary>
	/// Procedurally generates a new universe, clearing the old one if there is one.
	/// </summary>
	public void GenerateUniverse () {

		// Clear the old universe.
		gameObject.DestroyAllChildren ();

		// Start by instantiating the center of the universe.
		GenerateSpaceMass (0, null);
	}

	/// <summary>
	/// Generates a new space mass and its children. Recursive function.
	/// </summary>
	/// <param name="generation">Generation.</param>
	/// <param name="primary">Primary.</param>
	private void GenerateSpaceMass (int generation, SpaceMass primary) {

		// Base case: start planet.
		if (generation == 0 && primary == null) {
			SpaceMass startingMass = Instantiate (spaceMassPrefab);
			startingMass.Initialize (startingMassSize, null, 0, false, 0, 0);
			startingMass.transform.SetParent (transform);
		}
	}
}

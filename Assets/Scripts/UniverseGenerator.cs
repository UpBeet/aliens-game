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
	/// The minimum relative proportionate scale of a primary's generated satellite.
	/// </summary>
	[SerializeField] private float minSizeOfPrimary = 0.1f;

	/// <summary>
	/// The maximum relative proportionate scale of a primary's generated satellite.
	/// </summary>
	[SerializeField] private float maxSizeOfPrimary = 0.5f;

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
		GenerateSpaceMass ();
	}

	/// <summary>
	/// Generates a new space mass and its children. Recursive function.
	/// </summary>
	/// <param name="generation">Generation.</param>
	/// <param name="primary">Primary.</param>
	private void GenerateSpaceMass (SpaceMass primary = null) {

		// Base case: start planet.
		if (primary == null) {
			SpaceMass startingMass = Instantiate (spaceMassPrefab);
			startingMass.Initialize (startingMassSize, null, 0, false, 0, 0);
			startingMass.transform.SetParent (transform);
			GenerateSpaceMass (startingMass);
			return;
		}

		// Base case: primary is too small to have satellites.
		if (primary.Size < minPlanetSize) {
			return;
		}

		// The size is a randomly selection proportion of the primary.
		float size = primary.Size * Random.Range (minSizeOfPrimary, maxSizeOfPrimary);

		// Orbital speed is based on the size of the mass.
		float orbitalSpeed = Random.Range(0f, 1f) / size;

		// Randomly pick right or left orbit direction.
		bool orbitsClockwise = Random.Range (0, 2) == 0;

		// Randomize distance from primary.
		float orbitRadius = primary.Radius * 2;

		// Randomize initial orbit angle.
		float orbitAngle = Random.Range (0, 2 * Mathf.PI);

		// Initialize space mass with procedural values.
		SpaceMass newSpaceMass = Instantiate (spaceMassPrefab);
		newSpaceMass.name = "" + Random.Range (0, 100);
		newSpaceMass.Initialize (
			size,
			primary,
			orbitalSpeed,
			orbitsClockwise,
			orbitRadius,
			orbitAngle
		);

		// Attach to the universe.
		newSpaceMass.transform.SetParent (transform);

		// Generate satellite.
		GenerateSpaceMass (newSpaceMass);
	}
}

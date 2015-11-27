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
	private void GenerateSpaceMass (int generation = 0, SpaceMass primary = null, int satelliteIndex = 0) {

		// Base case: primary is too small to have satellites.
		if (primary != null && primary.Size < minPlanetSize) {
			return;
		}

		// Create a new space mass to initialize.
		SpaceMass newSpaceMass = Instantiate (spaceMassPrefab);

		// Base case: start planet.
		if (primary == null) {
			newSpaceMass.Initialize (startingMassSize, null, 0, false, 0, 0);
		}

		// Initialize a random satellite planet.
		else {

			// The size is a randomly selection proportion of the primary.
			float size = primary.Size * Random.Range (minSizeOfPrimary, maxSizeOfPrimary);

			// Orbital speed is based on the size of the mass.
			float orbitalSpeed = Random.Range (0f, 1f) / size;

			// Randomly pick right or left orbit direction.
			bool orbitsClockwise = Random.Range (0, 2) == 0;

			// Randomize distance from primary.
			float orbitRadius = primary.Radius * 2 * (satelliteIndex + 1);

			// Randomize initial orbit angle.
			float orbitAngle = Random.Range (0, 2 * Mathf.PI);

			// Initialize space mass with procedural values.
			newSpaceMass.name = "" + Random.Range (0, 100);
			newSpaceMass.Initialize (
				size,
				primary,
				orbitalSpeed,
				orbitsClockwise,
				orbitRadius,
				orbitAngle
			);
		}

		// Attach to the universe.
		newSpaceMass.transform.SetParent (transform);

		// Color the planet according to its generation.
		Color color = Color.grey;
		if (generation == 0) {
			color = Color.white;
		}
		else if (generation == 1) {
			color = Color.yellow;
		}
		else if (generation == 2) {
			color = Color.green;
		}
		else if (generation == 3) {
			color = Color.grey;
		}
		newSpaceMass.GetComponentInChildren<Renderer> ().material.SetColor ("_Color", color);

		// Determine the number of satellites along a normal curve.
		int numSatellites = (int)MathUtil.RandomFromNormalDistribution (6 / (generation + 1), 1, true);

		// Generate satellite.
		for (int i = 0; i < numSatellites; i++) {
			GenerateSpaceMass (generation + 1, newSpaceMass, i);
		}
	}
}

﻿using UnityEngine;

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
	/// Reference to the basic SpaceEntity prefab.
	/// </summary>
	[SerializeField] private SpaceEntity spaceEntityPrefab;

	/// <summary>
	/// The size of the first mass generated.
	/// </summary>
	[SerializeField] private Vector2 starMassSizeRange = new Vector2 (10f, 20f);

	/// <summary>
	/// The minimum relative proportionate scale of a primary's generated satellite.
	/// </summary>
	[SerializeField] private Vector2 satelliteDiminishRange = new Vector2 (0.1f, 0.5f);

	/// <summary>
	/// The size at which planets stop generating children.
	/// </summary>
	[SerializeField] private float minPlanetSize = 1f;

	/// <summary>
	/// The player's home in this universe.
	/// </summary>
	private SpaceMass home = null;

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

		// Reset the player's home.
		home = null;

		// Start by instantiating the center of the universe.
		GenerateSpaceMass ();

		// Configure the home planet.
		if (home != null) {

			// Create a home species.
			Species homeSpecies = new Species ("My Species", home);

			// Spawn a few members.
			for (int i = 0; i < 6; i++) {
				SpawnSpaceEntity (homeSpecies, home);
			}

			// Select the home planet.
			UserInterface.SelectSpaceMass (home);
		}
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
			newSpaceMass.Initialize (Random.Range (starMassSizeRange.x, starMassSizeRange.y), null, 0, false, 0, 0);
		}

		// Initialize a random satellite planet.
		else {

			// The size is a randomly selection proportion of the primary.
			float size = primary.Size * Random.Range (satelliteDiminishRange.x, satelliteDiminishRange.y);

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
			color = Color.yellow;
		}
		else if (generation == 1) {
			color = Color.green;
			color = Color.green;

			// Pick the first generation 1 planet as home for testing.
			if (home == null) {
				home = newSpaceMass;
			}
		}
		newSpaceMass.GetComponentInChildren<Renderer> ().material.SetColor ("_Color", color);

		// Determine the number of satellites along a normal curve.
		int numSatellites = (int)MathUtil.RandomFromNormalDistribution (6 / (generation + 1), 1, true);

		// Generate satellite.
		for (int i = 0; i < numSatellites; i++) {
			GenerateSpaceMass (generation + 1, newSpaceMass, i);
		}
	}

	/// <summary>
	/// Spawns the space entity.
	/// </summary>
	/// <param name="home">Home.</param>
	public void SpawnSpaceEntity (Species species, SpaceMass home) {

		// Instantiate, initialize, and place the entity.
		SpaceEntity newSpaceEntity = Instantiate (spaceEntityPrefab);
		newSpaceEntity.Initialize (species);
		newSpaceEntity.AttachToSpaceMass (home);
	}
}

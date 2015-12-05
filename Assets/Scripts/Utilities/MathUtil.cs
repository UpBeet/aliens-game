using UnityEngine;

/// <summary>
/// Basic math utilities used throughout the project.
/// </summary>
public static class MathUtil {

	/// <summary>
	/// Gets the angle between to points in 2D space.
	/// </summary>
	/// <returns>The angle between the specified points in radians.</returns>
	/// <param name="a">The first 2D point in space.</param>
	/// <param name="b">The second 2D point in space.</param>
	public static float AngleBetweenPoints (Vector2 a, Vector2 b) {
		return Mathf.Atan2 (a.y - b.y, a.x - b.x);
	}

	/// <summary>
	/// Gets a local radial position given a distance from the origin and an angle in radians.
	/// </summary>
	/// <returns>The local position vector from the origin.</returns>
	/// <param name="distance">Distance from the origin.</param>
	/// <param name="angle">Angle from the origin in radians.</param>
	public static Vector2 Vector2FromMagnitudeAndAngle (float distance, float angle) {
		return Vector2FromMagnitudeAndAngle (distance, angle, Vector2.zero);
	}

	/// <summary>
	/// Gets a global radial position given a distance from the origin, an angle in radians, and an origin position.
	/// </summary>
	/// <returns>The local position vector from the origin.</returns>
	/// <param name="distance">Distance from the origin.</param>
	/// <param name="angle">Angle from the origin in radians.</param>
	/// <param name="origin">Origin point in 2D space.</param>
	public static Vector2 Vector2FromMagnitudeAndAngle (float distance, float angle, Vector2 origin) {
		float x = origin.x + distance * Mathf.Cos (angle);
		float y = origin.y + distance * Mathf.Sin (angle);
		return new Vector2 (x, y);
	}

	/// <summary>
	/// Gets an angular translation in radians given an arc length travelled and the arc's radius.
	/// </summary>
	/// <returns>The angular translation travelled in radians.</returns>
	/// <param name="length">Length of the arc.</param>
	/// <param name="radius">Radius of the arc.</param>
	public static float AngleFromArcLength (float length, float radius) {
		return length / radius;
	}

	/// <summary>
	/// Gets a random number along the normal distribution
	/// </summary>
	/// <returns>The from normal distribution.</returns>
	public static float RandomFromNormalDistribution (float mean = 0, float stdDev = 1, bool positiveOnly = false) {

		// Get the random standard deviation.
		float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(Random.value)) * Mathf.Sin(2f * Mathf.PI * Random.value);
		float randNormal = mean + stdDev * randStdNormal;

		// If positive-only, clamp to positives.
		if (positiveOnly) {
			randNormal = Mathf.Abs (randNormal);
		}

		// Return the random normal number.
		return randNormal;
	}
}

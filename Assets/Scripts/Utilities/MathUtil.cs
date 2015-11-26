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
	public static Vector2 LocalRadialPosition (float distance, float angle) {
		return RadialPosition (distance, angle, Vector2.zero);
	}

	/// <summary>
	/// Gets a global radial position given a distance from the origin, an angle in radians, and an origin position.
	/// </summary>
	/// <returns>The local position vector from the origin.</returns>
	/// <param name="distance">Distance from the origin.</param>
	/// <param name="angle">Angle from the origin in radians.</param>
	/// <param name="origin">Origin point in 2D space.</param>
	public static Vector2 RadialPosition (float distance, float angle, Vector2 origin) {
		float x = origin.x + distance * Mathf.Cos (angle);
		float y = origin.y + distance * Mathf.Sin (angle);
		return new Vector2 (x, y);
	}
}

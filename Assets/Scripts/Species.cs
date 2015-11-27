using System.Collections.Generic;

/// <summary>
/// A species defines the properties for a group of SpaceEntities.
/// </summary>
public class Species {

	/// <summary>
	/// The name of the species.
	/// </summary>
	private string name;

	/// <summary>
	/// The home of this species.
	/// </summary>
	private SpaceMass home;

	/// <summary>
	/// SpaceEntities that are of this species.
	/// </summary>
	private List<SpaceEntity> members = new List<SpaceEntity> ();

	/// <summary>
	/// Initializes a new instance of the <see cref="Species"/> class.
	/// </summary>
	/// <param name="name">Name of the species.</param>
	public Species (string name, SpaceMass home) {
		this.name = name;
		this.home = home;
	}

	/// <summary>
	/// Adds a new member to this species.
	/// </summary>
	/// <param name="member">Incoming member.</param>
	public void AddMember (SpaceEntity member) {
		members.Add (member);
	}

	/// <summary>
	/// Removes the specified member from this species.
	/// </summary>
	/// <param name="member">Outoging member.</param>
	public void RemoveMember (SpaceEntity member) {
		members.Remove (member);
	}
}

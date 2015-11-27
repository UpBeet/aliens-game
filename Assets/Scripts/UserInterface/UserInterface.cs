using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// User interface controller that uses a static singleton architecture.
/// </summary>
public class UserInterface : MonoBehaviour {

	/// <summary>
	/// Reference to the singleton instance of the UserInterface.
	/// </summary>
	private static UserInterface singleton;

	/// <summary>
	/// The currently selected SpaceMass.
	/// </summary>
	private static SpaceMass selected;

	/// <summary>
	/// Initialize this component.
	/// </summary>
	void Start () {
		singleton = this;
	}

	/// <summary>
	/// Selects the specified space mass.
	/// </summary>
	/// <param name="spaceMass">Selected space mass.</param>
	public static void SelectSpaceMass (SpaceMass spaceMass) {

		// If there was a previously selected SpaceMass, deselect it.
		if (selected != null) {
			selected.GetComponentInChildren<HighlightingSystem.Highlighter> ().Off ();
		}

		// Record the newly selected mass.
		selected = spaceMass;

		// Get the selected planet panel.
		HideableInterfaceElement selectedPlanetPanel = singleton.transform.Find ("Selected Planet").GetComponent<HideableInterfaceElement> ();

		// Base case: deselection.
		if (selected == null) {
			selectedPlanetPanel.Hide ();
			return;
		}

		// Set the name text.
		Text selectedPlanetNameText = selectedPlanetPanel.transform.Find ("Planet Name").GetComponent<Text> ();
		selectedPlanetNameText.text = selected.name;

		// Show the panel.
		selectedPlanetPanel.Show ();

		// Highlight the selected mass.
		selected.GetComponentInChildren<HighlightingSystem.Highlighter> ().ConstantOn ();

		// Focus the camera on the selected planet.
		Camera.main.GetComponent<CameraDrag> ().Follow (selected.transform);
	}
}

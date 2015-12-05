﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// User interface controller that uses a static singleton architecture.
/// </summary>
public class UserInterface : MonoBehaviour {

	/// <summary>
	/// Reference to the singleton instance of the UserInterface.
	/// </summary>
	private static UserInterface singleton;

	private static Selectable selected;

	/// <summary>
	/// Initialize this component.
	/// </summary>
	void Start () {
		singleton = this;
	}

	/// <summary>
	/// Update this component.
	/// </summary>
	void Update () {
		Selectable.Update ();
	}

	/// <summary>
	/// Selects the specified space mass.
	/// </summary>
	/// <param name="spaceMass">Selected space mass.</param>
	public static void Select (Selectable incomingSelected) {

		// If there was a previous selection, deselect it.
		if (selected != null) {

			// Check if there is a Highlighter component on the previous selection.
			HighlightingSystem.Highlighter highlighter = selected.GetComponentInChildren<HighlightingSystem.Highlighter> ();

			// If so, disable it.
			if (highlighter) {
				highlighter.Off ();
			}
		}

		// Record the new selection
		selected = incomingSelected;

		// Get the selected planet panel.
		HideableInterfaceElement selectedPanel = singleton.transform.Find ("Selected Panel").GetComponent<HideableInterfaceElement> ();

		// Base case: deselection.
		if (selected == null) {
			selectedPanel.Hide ();
			return;
		}

		// Set the name text.
		Text selectedNameText = selectedPanel.transform.Find ("Name Text").GetComponent<Text> ();
		selectedNameText.text = selected.name;

		// Show the panel.
		selectedPanel.Show ();

		// Highlight the selected mass.
		selected.GetComponentInChildren<HighlightingSystem.Highlighter> ().ConstantOn ();

		// Focus the camera on the selected planet.
		Camera.main.GetComponent<CameraDrag> ().Follow (selected.transform);
		Camera.main.GetComponent<CameraZoom> ().Focus (selected.GetComponentInChildren<Renderer> ().bounds.size.magnitude);
	}
}

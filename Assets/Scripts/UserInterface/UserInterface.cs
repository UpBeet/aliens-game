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
	/// Currently selected WorldSelectable.
	/// </summary>
	private static WorldSelectable selected;

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
		WorldSelectable.Update ();
	}

	/// <summary>
	/// Selects the specified space mass.
	/// </summary>
	/// <param name="spaceMass">Selected space mass.</param>
	public static void Select (WorldSelectable incomingSelected) {

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
		SelectedObjectPanelController selectedPanel = singleton.GetComponentInChildren<SelectedObjectPanelController> ();

		// Base case: deselection.
		if (selected == null) {
			selectedPanel.Hide ();
			return;
		}

		// Render information.
		selectedPanel.ShowInformation (selected);

		// Show the panel.
		selectedPanel.Show ();

		// Highlight the selected mass.
		selected.GetComponentInChildren<HighlightingSystem.Highlighter> ().ConstantOn ();

		// Focus the camera on the selected planet.
		Camera.main.GetComponent<CameraDrag> ().Follow (selected.transform);
		Camera.main.GetComponent<CameraZoom> ().Focus (selected.GetComponentInChildren<Renderer> ().bounds.size.magnitude);
	}

	/// <summary>
	/// Select the specified WorldSelectable after time seconds.
	/// </summary>
	/// <param name="incomingSelected">Incoming selected.</param>
	/// <param name="time">Time before selection.</param>
	public static void SelectAfterPause (WorldSelectable incomingSelected, float time) {
		selected = incomingSelected;
		singleton.Invoke ("Reselect", time);
	}

	/// <summary>
	/// Called when the Launch button is pressed.
	/// </summary>
	public void OnLaunchButtonPressed () {

		// If nothing is selected, this won't work. Exit out and print an error.
		if (selected == null) {
			Debug.LogError ("Nothing selected to launch.");
			return;
		}

		// Check if the selection is a SpaceEntity.
		SpaceEntity entity = selected.GetComponent<SpaceEntity> ();
		if (entity != null) {
			entity.PrepareForLaunch ();
		}

		// Otherwise, we can't launch this thing.
		else {
			Debug.LogWarning ("Can't launch this " + selected.name + " thing.");
		}
	}

	/// <summary>
	/// Reselect the currently selected WorldSelectable.
	/// </summary>
	private void Reselect () {
		UserInterface.Select (selected);
	}
}

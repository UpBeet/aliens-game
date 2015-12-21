using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Selected object panel controller.
/// </summary>
public class SelectedObjectPanelController : HideableInterfaceElement {

	/// <summary>
	/// Text component used to display the name of the selectable.
	/// </summary>
	private Text nameText;

	/// <summary>
	/// GameObjects to flag enabled/disabled depending on whether or not they are relevant.
	/// </summary>
	private GameObject foodIndicator, mutrientsIndicator;

	/// <summary>
	/// Text components for displaying food and mutrient amount.
	/// </summary>
	private Text foodText, mutrientsText;

	/// <summary>
	/// Initialize this component.
	/// </summary>
	void Awake () {

		// Cache all the modifiable UI components.
		nameText = transform.Find ("Name Text").GetComponent<Text> ();
		foodIndicator = transform.Find ("Food Indicator").gameObject;
		mutrientsIndicator = transform.Find ("Mutrients Indicator").gameObject;
		foodText = foodIndicator.transform.GetComponentInChildren<Text> ();
		mutrientsText = mutrientsIndicator.transform.GetComponentInChildren<Text> ();
	}

	/// <summary>
	/// Displays the information relevant to the specified WorldSelectable to the UI.
	/// </summary>
	/// <param name="selected">Selectable.</param>
	public void ShowInformation (WorldSelectable selected) {

		// Set the name text.
		nameText.text = selected.name;

		// Check if this WorldSelectable is a SpaceMass.
		SpaceMass spaceMass = selected.GetComponent<SpaceMass> ();
		if (spaceMass != null) {

			// Show resource indicators.
			foodIndicator.SetActive (true);
			mutrientsIndicator.SetActive (true);

			// Display resources.
			foodText.text = "" + spaceMass.Food;
			mutrientsText.text = "" + spaceMass.Mutrients;
		}

		// Check if this WorldSelectable is a SpaceEntity.
		SpaceEntity spaceEntity = selected.GetComponent<SpaceEntity> ();
		if (spaceEntity != null) {

			// Hide resource indicators.
			foodIndicator.SetActive (false);
			mutrientsIndicator.SetActive (false);
		}
	}
}

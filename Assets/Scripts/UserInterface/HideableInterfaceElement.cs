using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HideableEvent : UnityEvent<HideableInterfaceElement> {}

/// <summary>
/// HideableInterfaceElement attaches to a CanvasGroup component to enable hiding
/// and showing the element using basic programmatic
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class HideableInterfaceElement : MonoBehaviour {

	/// <summary>
	/// Semantic for the direction in which this element animates hiding.
	/// </summary>
	public enum HideDirection {
		Up,
		Down,
		Left,
		Right,
		None
	}

	/// <summary>
	/// The action that occurs when this element is finished hiding.
	/// </summary>
	public enum HideAction {
		None,
		Deactivate,
		Destroy
	}

	/// <summary>
	/// The action that occurs when this component is hidden.
	/// </summary>
	public HideAction onHide = HideAction.Deactivate;

	/// <summary>
	/// If set to true, starts this hideable in the hidden state.
	/// </summary>
	public bool startsHidden = false;

	/// <summary>
	/// If set to true, the CanvasGroup will continue to block raycasts while hidden.
	/// </summary>
	public bool blocksRaycastsWhenHidden = true;

	/// <summary>
	/// Event that fires when this hideable is finished hiding itself.
	/// Fires before default assigned HideAction behavior.
	/// </summary>
	public HideableEvent OnBeginHide = new HideableEvent ();
	public HideableEvent OnHidden = new HideableEvent ();
	public HideableEvent OnBeginShow = new HideableEvent ();
	public HideableEvent OnShown = new HideableEvent ();

	/// <summary>
	/// If true, this element fades in and out as it hides.
	/// </summary>
	public bool fade = true;

	/// <summary>
	/// The direction this element floats towards as it hides.
	/// </summary>
	public HideDirection hideDirection = HideDirection.Down;

	/// <summary>
	/// The distance this element moves in the hide direction as it hides.
	/// </summary>
	public float hideDistance = 50.0f;

	/// <summary>
	/// The rate at which this element moves as it hides.
	/// </summary>
	public float hideRate = 20.0f;

	/// <summary>
	/// Shorthand for referencing the RectTransform component attached to this GameObject.
	/// </summary>
	/// <value>The rect.</value>
	public RectTransform Rect { get { return GetComponent<RectTransform> (); } }

	/// <summary>
	/// Shorthand for referencing the CanvasGroup component attached to this GameObject
	/// </summary>
	/// <value>The canvas group.</value>
	public CanvasGroup CanvasGroup { get { return GetComponent<CanvasGroup> (); } }

	/// <summary>
	/// Gets a value indicating whether this instance is shown.
	/// </summary>
	/// <value><c>true</c> if this instance is shown; otherwise, <c>false</c>.</value>
	public bool IsShown { get { return shown; } }

	/// <summary>
	/// If true, this element is currently shown. If false, it is currently hiding.
	/// </summary>
	private bool shown = true;

	/// <summary>
	/// The current alpha of this element's CanvasGroup should render at.
	/// </summary>
	private float a = 1;

	/// <summary>
	/// The position this element should seek when showing.
	/// </summary>
	private Vector2 shownPosition;

	/// <summary>
	/// The position this element should seek when hiding.
	/// </summary>
	private Vector2 hiddenPosition;

	/// <summary>
	/// A vector representation of the hide diretion semantic.
	/// </summary>
	private Vector2 hideDirectionVector;

	/// <summary>
	/// Initialize this HideableInterfaceElement.
	/// </summary>
	public virtual void Awake () {
		SetHideDirection ();

		// Check if this elements starts hidden.
		if (startsHidden) {

			// Immediately hide it.
			HideImmediate ();
		}
	}

	/// <summary>
	/// Moves and resets the position values for this hideable.
	/// </summary>
	/// <param name="anchoredPosition">Anchored position.</param>
	public void Reposition (Vector2 anchoredPosition) {
		GetComponent<RectTransform> ().anchoredPosition = anchoredPosition;
		SetHideDirection ();
	}
	
	/// <summary>
	/// Update this HideableInterfaceElement.
	/// </summary>
	public virtual void Update () {
		float hideIncrement = hideRate * Time.deltaTime;
		if (shown && a < 1) {
			a += hideIncrement;
			if (a >= 1) {
				a = 1;
				OnShow ();
			}
		}
		else if (!shown && a > 0) {
			a -= hideIncrement;
			if (a <= 0) {
				a = 0;
				OnHide ();
			}
		}

		Rect.anchoredPosition = Vector2.Lerp (hiddenPosition, shownPosition, a * a);
		if (fade) { CanvasGroup.alpha = a; }
		else { CanvasGroup.alpha = 1; }
	}

	/// <summary>
	/// Animate hiding this HideableInterfaceElement.
	/// </summary>
	public void Hide () {
		shown = false;
		BeginHide ();

		// If the hideable is already fully hidden, call OnHide by default.
		if (a <= 0) {
			OnHide ();
		}
	}
	
	/// <summary>
	/// Animate showing this HideableInterfaceElement.
	/// </summary>
	public virtual void Show () {
		shown = true;
		BeginShow ();
		
		// If the hideable is already fully shown, call OnShow by default.
		if (a >= 1) {
			OnShow ();
		}
	}
	
	/// <summary>
	/// Immediately hide and deactivate this HideableInterfaceElement.
	/// </summary>
	public void HideImmediate () {
		shown = false;
		a = 0;
		CanvasGroup.alpha = a;

		BeginHide ();
		OnHide ();
	}

	/// <summary>
	/// Immediately show and activate this HideableInterfaceElement.
	/// </summary>
	public void ShowImmediate () {
		shown = true;
		a = 1;

		BeginShow ();
		OnShow ();
	}

	/// <summary>
	/// Toggle this instance.
	/// </summary>
	public void Toggle () {
		if (shown) Hide ();
		else Show ();
	}

	/// <summary>
	/// Shows or hides this element given a boolean value.
	/// </summary>
	/// <param name="shown">If set to <c>true</c> shown.</param>
	public void SetShown (bool shown) {
		if (shown) {
			Show ();
		}
		else {
			Hide ();
		}
	}
	
	/// <summary>
	/// Calculates the hide direction vector based on the hide direction semantic
	/// </summary>
	private void SetHideDirection () {
		// Interpret semantic.
		switch (hideDirection) {
		case HideDirection.Down:
			hideDirectionVector = -Vector2.up;
			break;
		case HideDirection.Left:
			hideDirectionVector = -Vector2.right;
			break;
		case HideDirection.Up:
			hideDirectionVector = Vector2.up;
			break;
		case HideDirection.Right:
			hideDirectionVector = Vector2.right;
			break;
		case HideDirection.None:
			hideDirectionVector = Vector2.zero;
			break;
		}
		
		// Calculate the shown and hidden positions.
		shownPosition = Rect.anchoredPosition;
		hiddenPosition = Rect.anchoredPosition + hideDirectionVector * hideDistance;
	}
	
	/// <summary>
	/// Raises the on begin hide event.
	/// </summary>
	private void BeginHide () {
		
		// Fire event.
		if (OnBeginHide != null) {
			OnBeginHide.Invoke (this);
		}
	}
	
	/// <summary>
	/// Raises the on hide event.
	/// </summary>
	private void OnHide () {
		
		// Fire event.
		if (OnHidden != null) {
			OnHidden.Invoke (this);
		}
		
		// HideAction behavior.
		if (onHide == HideAction.Deactivate) gameObject.SetActive (false);
		else if (onHide == HideAction.Destroy) Destroy (gameObject);
		
		// Don't intercept raycasts.
		CanvasGroup.blocksRaycasts = blocksRaycastsWhenHidden;
	}
	
	/// <summary>
	/// Raises the on begin show event.
	/// </summary>
	private void BeginShow () {
		
		// Fire event.
		if (OnBeginShow != null) {
			OnBeginShow.Invoke (this);
		}
		
		// Make sure the game object is reactivated.
		gameObject.SetActive (true);
		
		// Intercept raycasts.
		CanvasGroup.blocksRaycasts = true;
	}
	
	/// <summary>
	/// Raises the on show event.
	/// </summary>
	private void OnShow () {
		
		// Fire event.
		if (OnShown != null) {
			OnShown.Invoke (this);
		}
	}
}

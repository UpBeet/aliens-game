using UnityEngine;

/// <summary>
/// Attach to a Camera component to enable scroll wheel and
/// touch pinch zooming. Currently only works for orthographic cameras.
/// </summary>
[RequireComponent (typeof (Camera))]
public class CameraZoom : MonoBehaviour {

	/// <summary>
	/// The minimum orthographic size of the camera.
	/// </summary>
	public float minOrthoSize = 4;

	/// <summary>
	/// The maximum orthographic size of the camera.
	/// </summary>
	public float maxOrthoSize = 10;

	/// <summary>
	/// The orthographic zoom rate.
	/// </summary>
	public float orthoZoomRate = 1;

	/// <summary>
	/// Reference to the camera component attached to this game object.
	/// </summary>
	/// <value>The Camera component.</value>
	public Camera Camera { get { return GetComponent<Camera> (); } }

	/// <summary>
	/// Update this component.
	/// </summary>
	void Update () {

		#if (UNITY_EDITOR || UNITY_STANDALONE)

		// ... change the orthographic size based on the change in distance between the touches.
		Camera.orthographicSize += Input.mouseScrollDelta.y * orthoZoomRate * Time.deltaTime;

		// Clamp orthographic size.
		Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, minOrthoSize, maxOrthoSize);

		#else

		// If there are two touches on the device...
		if (Input.touchCount == 2)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);
			
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			
			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			
			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			// ... change the orthographic size based on the change in distance between the touches.
			Camera.orthographicSize += deltaMagnitudeDiff * orthoZoomRate * Time.deltaTime;
			
			// Clamp orthographic size.
			Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, minOrthoSize, maxOrthoSize);
		}

		#endif
	}
}

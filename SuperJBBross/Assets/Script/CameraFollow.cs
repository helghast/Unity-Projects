using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform target = null;
	[SerializeField] private Vector2 offset = new Vector2(0.1f, 0.0f);
	[SerializeField] private float dampTime = 0.3f;
	[SerializeField] private Vector3 velocity = Vector3.zero;

	private Camera cam;

	private static CameraFollow sharedInstance = null;
	public static CameraFollow GetInstance { get => sharedInstance; }

	private void Awake()
	{
		sharedInstance = this;

		Application.targetFrameRate = 60;
		cam = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
    {
		CalculateCameraDestination(out Vector3 destination);

		this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, dampTime);
    }

	public void ResetCameraPosition()
	{
		/** you cannot pass as parameter a property like this.transform.position. 
		instead store this.transform.position in a temp Vector3 and pass it as ref parameter, or pass 
		a uninitialize Vector3 as out parameter, and then assign to this.transform.position:
		Vector3 destination;
		CalculateCameraDestination(out destination); */

		// variable declaration an pass as parameter can be inlined:
		CalculateCameraDestination(out Vector3 destination);

		this.transform.position = destination;
	}

	/** use out to pass a reference uninitialized variables. use ref to pass a reference to a initialized variable */
	private void CalculateCameraDestination(out Vector3 Value)
	{
		Vector3 point = cam.WorldToViewportPoint(target.position);
		Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(offset.x, offset.y, point.z));
		Value = new Vector3(target.position.x, offset.y, cam.transform.position.z);
	}
}

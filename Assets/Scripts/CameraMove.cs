using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	public float zoomAcceleration = 100f;
	public float zoomFriction = 0.9f;
	float zoomVelocity = 0f;	//seperate from other velocity since it isn't on the same axes
	public Vector2 zoomBounds = Vector2.one;
	public float acceleration = 100f;
	public float friction = 0.99f;
	public float stopFriction = 0.9f;
	public Rect panBounds;
	public float rotationAcceleration = 20f;
	public float rotationFriction = 0.9f;
	float rotationVelocity = 0f;
	Vector3 velocity = Vector3.zero;
	Vector3 centerScreen = Vector3.zero;
	Rect fullScreenRect = new Rect(0, 0, Screen.width, Screen.height);

	void Start () {
		velocity = Vector3.zero;
		zoomVelocity = 0f;
		rotationVelocity = 0f;
		centerScreen.x = Screen.width / 2f;
		centerScreen.y = Screen.height / 2f;
	}

	void Update () {
		if ((((Mathf.Abs(Input.mousePosition.x - centerScreen.x) > centerScreen.x - 10) && !Input.GetMouseButton(2)) ||
			(Mathf.Abs(Input.mousePosition.y - centerScreen.y) > centerScreen.y - 10)) &&
            !Input.GetMouseButton(0) && fullScreenRect.Contains(Input.mousePosition)) {

			velocity += (Input.mousePosition - centerScreen).normalized * acceleration * Time.deltaTime;
			velocity *= friction;
		} else {
			velocity *= stopFriction;
		}

		if (Input.GetMouseButton(2))
			rotationVelocity += Input.GetAxis("Mouse X") * rotationAcceleration;

		zoomVelocity += Input.mouseScrollDelta.y * zoomAcceleration * Time.deltaTime;
		zoomVelocity *= zoomFriction;
		rotationVelocity *= rotationFriction;
		/*
		ray = GetComponent<Camera>().ScreenPointToRay(centerScreen);
		Vector3 point = ray.GetPoint(Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.x) * transform.position.y);
		transform.RotateAround(point, Vector3.up, rotationVelocity * Time.deltaTime);
		*/

		Ray ray;
		RaycastHit[] hits;
		ray = GetComponent<Camera>().ScreenPointToRay(centerScreen);
		hits = Physics.RaycastAll(ray, Mathf.Infinity);

		foreach (RaycastHit hit in hits) {
			if (hit.transform.tag == "Ground") {
				transform.RotateAround(hit.point, Vector3.up, rotationVelocity * Time.deltaTime);
			}
		}


		// Movement rotated flat, but not around y yet.
		Vector3 flatMovement = Quaternion.AngleAxis(90, Vector3.right) * velocity * Time.deltaTime;
		transform.position += transform.rotation * Quaternion.AngleAxis(-transform.rotation.eulerAngles.x, Vector3.right) * flatMovement;
		transform.Translate(0, 0, zoomVelocity * Time.deltaTime, Space.Self);
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, panBounds.xMin, panBounds.xMax),
			Mathf.Clamp(transform.position.y, zoomBounds.x, zoomBounds.y),
			Mathf.Clamp(transform.position.z, panBounds.yMin, panBounds.yMax));
	}
}

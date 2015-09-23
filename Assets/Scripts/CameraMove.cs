using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	public float zoomAcceleration = 100f;
	public float zoomFriction = 0.9f;
	float zoomVelocity = 0f;	//seperate from other velocity since it isn't on the same axes
	public Vector2 zooomBounds = Vector2.one;
	public float acceleration = 100f;
	public float friction = 0.99f;
	public float stopFriction = 0.9f;
	Vector3 velocity = Vector3.zero;
	Vector3 centerScreen = Vector3.zero;

	void Start () {
		velocity = Vector3.zero;
		zoomVelocity = 0f;
		centerScreen.x = Screen.width / 2f;
		centerScreen.y = Screen.height / 2f;
	}
	
	void Update () {
		if (!Input.GetMouseButton(0)) {
			if (Mathf.Abs(Input.mousePosition.x - centerScreen.x) > centerScreen.x - 10) {
				velocity += (Input.mousePosition - centerScreen).normalized * acceleration * Time.deltaTime;
				velocity *= friction;
			} else if (Mathf.Abs(Input.mousePosition.y - centerScreen.y) > centerScreen.y - 10) {
				velocity += (Input.mousePosition - centerScreen).normalized * acceleration * Time.deltaTime;
				velocity *= friction;
			} else {
				velocity *= stopFriction;
			}
		}

		zoomVelocity += Input.mouseScrollDelta.y * zoomAcceleration * Time.deltaTime;
		zoomVelocity *= zoomFriction;

		transform.position += Quaternion.AngleAxis(90, Vector3.right) * velocity * Time.deltaTime;
		transform.Translate(0, 0, zoomVelocity * Time.deltaTime, Space.Self);
	}
}

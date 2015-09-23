using UnityEngine;
using System.Collections;

public class SlowTest : MonoBehaviour {
	public float speed = 1;
	bool moving = false;
	Vector3 target = Vector3.zero;
	float dist = 0f;
	float tTime = 0f;
	float startTime = 0f;

	SelectionManager selectionManager;

	void Start () {
		selectionManager = GameObject.Find("PlayerGameManager").GetComponent<SelectionManager>();
		selectionManager.RegisterUnit(gameObject);
		moving = false;
	}


	void Update () {
		if (moving) {	// TODO: Make slow movement actually linear.
			transform.position = Vector3.Lerp(transform.position, target, (Time.time - startTime) / tTime);
			if ((Time.time - startTime) / tTime >= 1f)
				moving = false;
		}
	}


	public void Move (Vector3 newPos) {
		moving = true;
		if (selectionManager.selectedUnits.Count == 1)
			target = newPos;
		else
			target = newPos + (Quaternion.AngleAxis((360f / selectionManager.selectedUnits.Count) *
				selectionManager.selectedUnits.IndexOf(gameObject), Vector3.up) * Vector3.forward * (selectionManager.selectedUnits.Count / 2));
		// Evenly distribute units around target

		dist = Vector3.Distance(transform.position, target);
		tTime = dist / speed;
		startTime = Time.deltaTime;
	}
}
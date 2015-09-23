using UnityEngine;
using System.Collections;

public class FastTest : MonoBehaviour {
	public float speed = 1;
	bool moving = false;
	Vector3 target = Vector3.zero;

	SelectionManager selectionManager;

	void Start () {
		selectionManager = GameObject.Find("PlayerGameManager").GetComponent<SelectionManager>();
		selectionManager.RegisterUnit(gameObject);
		moving = false;
	}


	void Update () {
		if (moving) {
			transform.position = Vector3.Lerp(transform.position, target, speed);
			if (Vector3.Distance(target, transform.position) < 0.1)
				moving = false;
		}
	}


	public void Move (Vector3 newPos) {
		moving = true;
		if (selectionManager.selectedUnits.Count == 1)
			target = newPos;
		else
			target = newPos + (Quaternion.AngleAxis((360f / selectionManager.selectedUnits.Count) *
				selectionManager.selectedUnits.IndexOf(gameObject), Vector3.up) * Vector3.forward * (selectionManager.selectedUnits.Count/2));
		// Evenly distribute units around target
	}
}

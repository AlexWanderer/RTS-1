using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour {
	SelectionManager selectionManager;


	void Start () {
		selectionManager = GetComponent<SelectionManager>();

	}

	void Update () {
		if (Input.GetMouseButtonUp(1)) {
			Vector3 targetPos = Vector3.zero;
			Ray ray;
			RaycastHit[] hits;
			ray = selectionManager.cam.ScreenPointToRay(Input.mousePosition);
			hits = Physics.RaycastAll(ray, Mathf.Infinity);

			bool success = false;
			foreach(RaycastHit hit in hits) {
				if (hit.transform.tag == "Ground") {
					targetPos = hit.point;
					success = true;
				}
			}
			
			if (success) {
				foreach (UnitObject unit in selectionManager.selectedUnits) {
					unit.GameObject.SendMessage("Move", targetPos, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	void OnGUI () {
		bool somethingMoving = false;
		foreach (UnitObject unit in selectionManager.selectedUnits) {
			if (unit.GameObject.GetComponent<SwarmAgent>().moving)
				somethingMoving = true;
		}
		if (somethingMoving) {
			if (GUI.Button(new Rect(130, 20, 100, 20), "Cancel Move")) {
				foreach (UnitObject unit in selectionManager.selectedUnits) {
					unit.GameObject.SendMessage("Stop", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}

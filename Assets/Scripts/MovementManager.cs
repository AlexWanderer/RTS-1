using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour {
	SelectionManager selectionManager;

	void Start () {
		selectionManager = GameObject.Find("PlayerGameManager").GetComponent<SelectionManager>();

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
				if (hit.transform.tag == "NavMesh") {
					targetPos = hit.point;
					success = true;
				}
			}
			
			if (success) {
				foreach (GameObject unit in selectionManager.selectedUnits) {
					unit.SendMessage("Move", targetPos, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}

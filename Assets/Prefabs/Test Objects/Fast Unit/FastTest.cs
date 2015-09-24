using UnityEngine;
using System.Collections;
using System.Linq;

public class FastTest : MonoBehaviour {
	public float speed = 30;
	bool moving = false;
	Vector3 target = Vector3.zero;
	Vector3 startPos = Vector3.zero;
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
		if (moving) {
			transform.position = Vector3.Lerp(startPos, target, (Time.time - startTime) / tTime);
			if ((Time.time - startTime) / tTime >= 1f)
				moving = false;
		}
	}


	public void Move (Vector3 newPos) {
		moving = true;
		if (selectionManager.selectedUnits.Count == 1)
			target = newPos;
		else {
			int count = selectionManager.selectedUnits.Count(u => u.GetComponent<FastTest>() != null);
            target = newPos + (Quaternion.AngleAxis((360f / count) *
				selectionManager.selectedUnits.Where(u => u.GetComponent<FastTest>() != null).ToList().IndexOf(gameObject), Vector3.up) * 
				Vector3.forward * (count / 2));
			// Evenly distribute like units around target
		}

		dist = Vector3.Distance(transform.position, target);
		tTime = dist / speed;
		startTime = Time.time;
		startPos = transform.position;
	}
}

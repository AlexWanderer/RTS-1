using UnityEngine;
using System.Collections;

public class SelectObject : MonoBehaviour {
	public bool selected;
	Vector3 startScale = Vector3.zero;
//	SelectionManager selector;

	// Use this for initialization
	void Start () {
		selected = false;
		//		selector = GameObject.Find("PlayerGameManager").GetComponent<SelectionManager>();
		startScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (selected) {
			transform.localScale = startScale * (0.3f * Mathf.Sin(Time.time * 5) + 1);
		} else {
			transform.localScale = startScale;
		}

	}

	public void Select () {
		selected = true;
	}

	public void Deselect () {
		selected = false;
	}
}

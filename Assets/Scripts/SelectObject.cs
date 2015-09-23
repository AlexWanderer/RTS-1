using UnityEngine;
using System.Collections;

public class SelectObject : MonoBehaviour {
	public bool selected;
//	SelectionManager selector;

	// Use this for initialization
	void Start () {
		selected = false;
//		selector = GameObject.Find("PlayerGameManager").GetComponent<SelectionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (selected) {
			transform.localScale = Vector3.one * (0.3f * Mathf.Sin(Time.time * 5) + 1);
		} else {
			transform.localScale = Vector3.one;
		}

	}

	public void Select () {
		selected = true;
	}

	public void Deselect () {
		selected = false;
	}
}

using UnityEngine;
using System.Collections;

public class SelectionAgent : MonoBehaviour {
	public bool selected = false;   //Read-only
	public Material normalMat;
	public Material selectionMat;
	public UnitClass unitClass;

	SelectionManager selectionManager;


	void Start () {
		selectionManager = GameObject.Find("PlayerGameManager").GetComponent<SelectionManager>();
		selectionManager.RegisterUnit(new UnitObject(gameObject, unitClass, 1));
		
		GetComponent<Renderer>().material = normalMat;
	}

	public void Select () {
		selected = true;
		GetComponent<Renderer>().material = selectionMat;
	}

	public void Deselect () {
		selected = false;
		GetComponent<Renderer>().material = normalMat;
	}
}

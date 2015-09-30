using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SelectionManager : MonoBehaviour {
	public Camera cam;
	public float minimumBoxSize = 5f;		// Any smaller and it's a click.
	public Texture2D boxTexture;

	/// <summary>
	/// All units currently selected.
	/// </summary>
	public List<UnitObject> selectedUnits = new List<UnitObject>();

	/// <summary>
	/// All units the player can interact with.
	/// </summary>
	public List<UnitObject> allFriendlyUnits = new List<UnitObject>();
	Vector3 firstClickPos = Vector2.zero;   // Why is mouse position a Vector3?!?
	Rect selectionBox = new Rect(0, 0, 0, 0);
	Rect GuiSelectionBox = new Rect(0, 0, 0, 0);


	void Start () {
		if (cam == null)
			cam = Camera.main;
		selectedUnits.Clear();
		allFriendlyUnits.Clear();
	}

	void Update () {
		// TODO: ctrl+click will select all of one unit
		if (Input.GetMouseButtonDown(0)) {
			firstClickPos = Input.mousePosition;
		}

		if (Input.GetMouseButton(0)) {
			// make rect of screen coords
			selectionBox.x = Mathf.Min(firstClickPos.x, Input.mousePosition.x);			// upper left x-coord
			selectionBox.y = Mathf.Min(firstClickPos.y, Input.mousePosition.y);			// upper left y-coord
			selectionBox.width = Mathf.Abs(firstClickPos.x - Input.mousePosition.x);	// width
			selectionBox.height = Mathf.Abs(firstClickPos.y - Input.mousePosition.y);	// height
		}

		if (Input.GetMouseButtonUp(0)) {
			if (!Input.GetKey(KeyCode.LeftShift)) // If they're holding shift, select multiple
				ClearSelectedUnits();
			if (Vector3.Distance(Input.mousePosition, firstClickPos) > minimumBoxSize) {
				BoxSelect(selectionBox);
			} else {
				SingleSelect();
			}
		}

	}


	/// <summary>
	/// Makes a marquee selection in screen space.
	/// </summary>
	/// <param name="box">Rect to make selection within.</param>
	public void BoxSelect (Rect box) {
		// TODO: Make better marquee selection
		foreach (UnitObject unit in allFriendlyUnits) {
			if (box.Contains(cam.WorldToScreenPoint(unit.GameObject.transform.position))) {
				SelectUnit(unit);
            }
		}
	}


	/// <summary>
	/// Adds a single unit under the mouse to the selection.
	/// </summary>
	public void SingleSelect () {
		Ray ray;
		RaycastHit hit;

		ray = cam.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
			var matchingUnit = allFriendlyUnits.FirstOrDefault(u => u.GameObject == hit.transform.gameObject);
            if (matchingUnit != null)
				SelectUnit(matchingUnit, true);	// true makes it toggle select
		}
	}


	/// <summary>
	/// Removes all units from the selection.
	/// </summary>
	public void ClearSelectedUnits () {
		foreach (UnitObject unit in selectedUnits) {
			unit.GameObject.SendMessage("Deselect", SendMessageOptions.DontRequireReceiver);
		}
		selectedUnits.Clear();
	}


	/// <summary>
	/// Adds a unit to the selection.
	/// </summary>
	/// <param name="unit">Unit to be selected.</param>
	/// <param name="toggle">Set to true to toggle selection state.</param>
	public void SelectUnit (UnitObject unit, bool toggle = false) {
		if (!selectedUnits.Contains(unit)) {
			unit.GameObject.SendMessage("Select", SendMessageOptions.DontRequireReceiver);
			selectedUnits.Add(unit);
		} else if (toggle) {
			DeselectUnit(unit);
		}
	}


	/// <summary>
	/// Removes a unit from the selection.
	/// </summary>
	/// <param name="unit">Unit to be deselected.</param>
	public void DeselectUnit (UnitObject unit) {
		unit.GameObject.SendMessage("Deselect", SendMessageOptions.DontRequireReceiver);
		selectedUnits.Remove(unit);
	}


	/// <summary>
	/// Adds a unit to the list of active friendly units.  
	/// </summary>
	/// <param name="unit">Unit to be added.</param>
	public void RegisterUnit (UnitObject unit) {
		if (allFriendlyUnits.Contains(unit))
			return;
		allFriendlyUnits.Add(unit);
	}


	/// <summary>
	/// Removes a unit from the list of active friendly units.  This should only happen if it dies.
	/// </summary>
	/// <param name="unit">Unit to be removed.</param>
	public void UnregisterUnit (UnitObject unit) {
		if (!allFriendlyUnits.Contains(unit))
			return;
		allFriendlyUnits.Remove(unit);
//		var types = allFriendlyUnits.Select(t => t.Type).Distinct();
		// TODO: move this


	}

	void OnGUI () {
		if (Input.GetMouseButton(0)) {
			GuiSelectionBox = selectionBox;
			GuiSelectionBox.y = Screen.height - GuiSelectionBox.y - GuiSelectionBox.height;
			GUI.DrawTexture(GuiSelectionBox, boxTexture);
		}
	}

}

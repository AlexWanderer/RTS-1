using UnityEngine;
using System.Collections;
using System.Linq;

public class CommanderTest : MonoBehaviour {
	public GameObject dropBeacon;
	public Texture2D throwRadiusTex;
	public float throwRadius = 100;
	public UnitState state = UnitState.Defend;
	UnitClass spawnType = UnitClass.Fast;

	SelectionManager selectionManager;
	SpawnQueueManager spawnQueueManager;
	SelectionAgent selectionAgent;


	void Start () {
		selectionManager = GameObject.Find("PlayerGameManager").GetComponent<SelectionManager>();
//		selectionManager.RegisterUnit(new UnitObject());
		spawnQueueManager = GameObject.Find("PlayerGameManager").GetComponent<SpawnQueueManager>();
		selectionAgent = GetComponent<SelectionAgent>();
		state = UnitState.Defend;
		transform.GetChild(0).localScale = new Vector3(1, 1, 0) * throwRadius * 1.4f;
		// NOTE: I have no idea why 1.4 works.  It's 2 * 0.7, but I don't know why 70% is special.
	}

	
	void Update () {

		if (state == UnitState.Spawn) {
			transform.GetChild(0).gameObject.SetActive(true);
			if (Input.GetMouseButtonUp(0)) {
				Vector3 spawnPos = Vector3.zero;

				Ray ray;
				RaycastHit[] hits;
				ray = selectionManager.cam.ScreenPointToRay(Input.mousePosition);
				hits = Physics.RaycastAll(ray, Mathf.Infinity);

				bool success = false;
				foreach (RaycastHit hit in hits) {
					float dist = Vector3.Distance(transform.position, hit.point);
					if (hit.transform.tag == "Ground" && dist <= throwRadius) {
						spawnPos = hit.point;
						success = true;
					}
				}

				if (success) {
					PlaceBeacon(spawnPos + (Vector3.up * 0.5f));
				}
			}

			if (Input.GetKeyUp(KeyCode.Escape)) {
				state = UnitState.Defend;
			}
		} else {
			transform.GetChild(0).gameObject.SetActive(false);
		}
	}

	public void PlaceBeacon (Vector3 spawnPos) {
		GameObject newBeacon = Instantiate(dropBeacon, spawnPos, Quaternion.identity) as GameObject;
		newBeacon.transform.GetChild(0).gameObject.SetActive(true);
		newBeacon.SendMessage("Setup", 5f, SendMessageOptions.DontRequireReceiver);
		UnitObject newUnit = new UnitObject(spawnType, 1, newBeacon, Time.time + 5f);
		spawnQueueManager.spawnQueue.Add(newUnit);
	}

	public void Select () {
		//state = UnitState.Spawn;
		state = UnitState.Defend;

	}

	public void Deselect () {
		//state = UnitState.Defend;
	}

	void OnGUI () {
		if (selectionAgent.selected && state != UnitState.Spawn) {
			if (GUI.Button(new Rect(20, 20, 100, 20), "Spawn fast")) {
				state = UnitState.Spawn;
				spawnType = UnitClass.Fast;
			}
			if (GUI.Button(new Rect(20, 50, 100, 20), "Spawn slow")) {
				state = UnitState.Spawn;
				spawnType = UnitClass.Slow;
			}
		}
		if (state == UnitState.Spawn) {
			if (GUI.Button(new Rect(20, 20, 100, 20), "Exit spawning")) {
				state = UnitState.Defend;
			}
		}
	}
}
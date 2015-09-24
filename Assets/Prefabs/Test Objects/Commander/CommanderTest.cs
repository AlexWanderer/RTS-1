using UnityEngine;
using System.Collections;
using System.Linq;

public class CommanderTest : MonoBehaviour {
	public float speed = 15;
	public GameObject dropBeacon;
	public Texture2D throwRadiusTex;
	public float throwRadius = 100;
	UnitState state = UnitState.Defend;
	UnitClass spawnType = UnitClass.Fast;
	bool moving = false;
	Vector3 target = Vector3.zero;
	Vector3 startPos = Vector3.zero;
	float dist = 0f;
	float tTime = 0f;
	float startTime = 0f;
	bool selected = false;

	SelectionManager selectionManager;
	SpawnQueueManager spawnQueueManager;

	void Start () {
		selectionManager = GameObject.Find("PlayerGameManager").GetComponent<SelectionManager>();
		selectionManager.RegisterUnit(gameObject);
		spawnQueueManager = GameObject.Find("PlayerGameManager").GetComponent<SpawnQueueManager>();
		moving = false;
		selected = false;
		state = UnitState.Defend;
		transform.GetChild(0).localScale = new Vector3(1, 1, 0) * throwRadius * 2;
	}


	void Update () {
		if (moving) {
			transform.position = Vector3.Lerp(startPos, target, (Time.time - startTime) / tTime);
			if ((Time.time - startTime) / tTime >= 1f)
				moving = false;
		}
		if (selected) {
			transform.localScale = Vector3.one * (0.3f * Mathf.Sin(Time.time * 5) + 1);
		} else {
			transform.localScale = Vector3.one;
		}

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
					Debug.Log(dist);
					if (hit.transform.tag == "NavMesh" && dist <= throwRadius) {
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


	public void Move (Vector3 newPos) {
		moving = true;
		if(selectionManager.selectedUnits.Count == 1)
            target = newPos;
		else {
			int count = selectionManager.selectedUnits.Count(u => u.GetComponent<CommanderTest>() != null);
			target = newPos + (Quaternion.AngleAxis((360f / count) *
				selectionManager.selectedUnits.Where(u => u.GetComponent<CommanderTest>() != null).ToList().IndexOf(gameObject), Vector3.up) *
				Vector3.forward * (count / 2));
		}

		dist = Vector3.Distance(transform.position, target);
		tTime = dist / speed;
		startTime = Time.time;
		startPos = transform.position;
	}

	public void PlaceBeacon (Vector3 spawnPos) {
		GameObject newBeacon = Instantiate(dropBeacon, spawnPos, Quaternion.identity) as GameObject;
		newBeacon.transform.GetChild(0).gameObject.SetActive(true);
		SpawnQueueObject newUnit = new SpawnQueueObject(spawnType, newBeacon, Time.time + 5f);
		spawnQueueManager.spawnQueue.Add(newUnit);
	}

	public void Select () {
		selected = true;
		if (state == UnitState.Spawn)
			state = UnitState.Defend;
	}

	public void Deselect () {
		selected = false;
	}

	void OnGUI () {
		if (selected && state != UnitState.Spawn) {
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
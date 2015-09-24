using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnQueueManager : MonoBehaviour {
	/// <summary>
	/// All units waiting to spawn.
	/// </summary>
	public List<SpawnQueueObject> spawnQueue = new List<SpawnQueueObject>();
	List<SpawnQueueObject> removeQueue = new List<SpawnQueueObject>();
	public GameObject fastUnit;
	public GameObject slowUnit;



	void Start () {
		spawnQueue.Clear();
		removeQueue.Clear();
	}
	
	void Update () {
		foreach (SpawnQueueObject unit in spawnQueue) {
			if (Time.time >= unit.spawnTime) {
				if (unit.type == UnitClass.Fast)
					Instantiate(fastUnit, unit.beacon.transform.position - (Vector3.up * 0.5f), Quaternion.identity);
				else if (unit.type == UnitClass.Slow)
					Instantiate(slowUnit, unit.beacon.transform.position - (Vector3.up * 0.5f), Quaternion.identity);
				Destroy(unit.beacon);
				removeQueue.Add(unit);
			}
		}
		spawnQueue.RemoveAll(u => Time.time >= u.spawnTime);
	}
}

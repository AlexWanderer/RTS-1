using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnQueueManager : MonoBehaviour {
	/// <summary>
	/// All units waiting to spawn.
	/// </summary>
	public List<UnitObject> spawnQueue = new List<UnitObject>();
	public GameObject fastUnit;
	public GameObject slowUnit;
	

	void Start () {
		spawnQueue.Clear();
	}
	
	void Update () {
		foreach (UnitObject unit in spawnQueue) {
			if (unit.Age > 0f) {
				if (unit.Type == UnitClass.Fast)
					unit.Spawn(fastUnit);
				else if (unit.Type == UnitClass.Slow)
					unit.Spawn(slowUnit);
			}
		}
		spawnQueue.RemoveAll(u => u.Spawned == true);
	}
}

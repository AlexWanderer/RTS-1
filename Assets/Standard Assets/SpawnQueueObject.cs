using UnityEngine;
using System.Collections;

/// <summary>
/// A unit that has yet to spawn.
/// </summary>
public class SpawnQueueObject{
	public SpawnQueueObject (UnitClass type, GameObject beacon, float spawnTime) {
		this.type = type;
		this.beacon = beacon;
		this.spawnTime = spawnTime;
	}

	/// <summary>
	/// The type of unit in the queue.
	/// </summary>
	public UnitClass type { get; set; }

	/// <summary>
	/// The beacon the unit will drop onto.
	/// </summary>
	public GameObject beacon { get; set; }

	/// <summary>
	/// The Time.time when the unit will drop.
	/// </summary>
	public float spawnTime { get; set; }
}

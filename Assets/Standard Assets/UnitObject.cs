using UnityEngine;
using System.Collections;

/// <summary>
/// A unit that has yet to spawn.
/// </summary>
public class UnitObject{
	/// <summary>
	/// This overload is for ordered units which have not spawned yet.
	/// </summary>
	/// <param name="type">The type of unit which will be created.</param>
	/// <param name="level">The upgrade level with which the unit will spawn.</param>
	/// <param name="beacon">The gameObject marking where the unit will spawn.</param>
	/// <param name="spawnTime">The Time.time when the unit is done being built.</param>
	public UnitObject (UnitClass type, int level, GameObject beacon, float spawnTime) {
		this.Type = type;
		this.Level = level;
		this.Beacon = beacon;
		this.SpawnTime = spawnTime;
		this.Spawned = false;
	}

	/// <summary>
	/// This overload is for immediately spawned units.  Note: this still requires calling Spawn().
	/// </summary>
	/// <param name="gameObject">The gameObject that will be called a unit.</param>
	/// <param name="type">The type of unit to create.</param>
	/// <param name="level">The upgrade level with which the unit will spawn.</param>
	public UnitObject (GameObject gameObject, UnitClass type, int level) {
		this.GameObject = gameObject;
		this.Type = type;
		this.Level = level;
		this.SpawnTime = Time.time;
		this.Spawned = true;
	}

	/// <summary>
	/// Make a new unit.  Handles everything involved.
	/// </summary>
	/// <param name="prefab">The prefab to make the unit out of.</param>
	public void Spawn (GameObject prefab) {
		this.GameObject = GameObject.Instantiate(prefab, this.Beacon.transform.position - (Vector3.up * 0.5f) +
			(Vector3.up * prefab.transform.localScale.y * 0.5f), Quaternion.identity) as GameObject;
		GameObject.Destroy(this.Beacon.gameObject);
		this.Beacon = null;
		this.Spawned = true;
	}

	/// <summary>
	/// The unit's gameObject.
	/// </summary>
	public GameObject GameObject { get; set; }

	/// <summary>
	/// The unit's type.
	/// </summary>
	public UnitClass Type { get; set; }

	/// <summary>
	/// The unit's spawn beacon.  None if null.
	/// </summary>
	public GameObject Beacon { get; set; }

	/// <summary>
	/// The unit's Time.time of spawn.
	/// </summary>
	public float SpawnTime { get; set; }

	/// <summary>
	/// The unit's state of existence.
	/// </summary>
	public bool Spawned { get; set; }

	/// <summary>
	/// The upgrade level of the unit.
	/// </summary>
	public int Level { get; set; }

	/// <summary>
	/// The time in seconds since the unit spawned.
	/// </summary>
	public float Age {
		get { return Time.time - this.SpawnTime; }
		set { this.SpawnTime = Time.time - value; }
	}
}

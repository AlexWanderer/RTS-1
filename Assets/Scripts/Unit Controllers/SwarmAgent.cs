using UnityEngine;
using System.Collections;

public class SwarmAgent : MonoBehaviour {
	public int movingPriority = 45;
	int staticPriority = 40;
	bool moving = false;
	float moveCommandTime = 0f;
	NavMeshAgent navAgent;

	void Start () {

		navAgent = GetComponent<NavMeshAgent>();
		staticPriority = navAgent.avoidancePriority;

		moving = false;
	}

	void Update () {
		if (moving) {
			if ((navAgent.velocity.magnitude <= 0.1f) && (Time.time > moveCommandTime + 0.5f)) {
				moving = false;
				navAgent.avoidancePriority = staticPriority;
				navAgent.Stop();
			}
		}
	}

	void Move (Vector3 target) {
		moving = true;
		navAgent.destination = target;
		navAgent.avoidancePriority = movingPriority;
		navAgent.Resume();
		moveCommandTime = Time.time;
	}

	void Stop () {
		moving = false;
		navAgent.avoidancePriority = staticPriority;
		navAgent.Stop();
	}
}
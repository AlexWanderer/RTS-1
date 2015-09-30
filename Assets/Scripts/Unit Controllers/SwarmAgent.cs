using UnityEngine;
using System.Collections;

public class SwarmAgent : MonoBehaviour {
	public GameObject impactEffect;
	public int movingPriority = 45;
	int staticPriority = 40;
	public bool moving = false;
	float moveCommandTime = 0f;
	public bool falling = true;
	public float fallHeight = 100;
	float fallenDist = 0;
	public float fallVel = -5;
	NavMeshAgent navAgent;

	void Start () {
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.enabled = false;
		staticPriority = navAgent.avoidancePriority;
		transform.position += Vector3.up * fallHeight;
		fallenDist = 0f;
		moving = false;
		falling = true;
	}

	void Update () {
		if (moving) {
			if ((navAgent.velocity.magnitude <= 0.1f) && (Time.time > moveCommandTime + 0.5f)) {
				moving = false;
				navAgent.avoidancePriority = staticPriority;
				navAgent.Stop();
			}
		}

		if (falling) {
			if (fallenDist >= fallHeight)
				falling = false;
			else {
				transform.position += Vector3.up * fallVel * Time.deltaTime;
				fallenDist -= fallVel * Time.deltaTime;
            }
			//called after on the ground
			if (!falling) {
				transform.position += Vector3.up * (fallenDist - fallHeight);
				navAgent.enabled = true;
				Destroy(Instantiate(impactEffect, transform.position, impactEffect.transform.rotation), 2f);
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
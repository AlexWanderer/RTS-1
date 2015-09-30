using UnityEngine;
using System.Collections;

public class BeaconTimer : MonoBehaviour {
	public float totalTime;
	public float timeLeft;
	public Vector2 minMaxAlpha;
	GameObject timerGO;
	Material mat;

	void Start () {
		if (transform.GetChild(1).name != "Timer") {
			Debug.LogWarning("Beacon childern not set up! Disabling script.");
			this.enabled = false;
		}
		Setup(5f);
	}

	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft > 0.03f) {
			mat.SetFloat("_Cutoff", Mathf.Lerp(minMaxAlpha.x, minMaxAlpha.y, timeLeft / totalTime));
		} else {
			Remove();
		}
	}

	void Setup (float T) {
		totalTime = T;
		timeLeft = totalTime;
		timerGO = transform.GetChild(1).gameObject;
		timerGO.transform.position += Vector3.up * (float)(Random.value * 0.1);
		mat = timerGO.GetComponent<Renderer>().material;
		mat.SetFloat("_Cutoff", minMaxAlpha.y);
	}

	void Remove () {
		timerGO.SetActive(false);
		gameObject.SetActive(false);
	}
}

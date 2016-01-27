using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	[SerializeField] private float distance;
	[SerializeField] private float elevation;
	[SerializeField] private float lagSeconds;

	private GameObject player;
	private Queue<Quaternion> previousPlayerRotations;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		previousPlayerRotations = new Queue<Quaternion> ();
	}

	void Update () {
		previousPlayerRotations.Enqueue (player.transform.rotation);
	}

	void LateUpdate () {
		float x = Input.GetAxis ("Camera X");
		float y = Input.GetAxis ("Camera Y");

		if (lagSeconds < 0) {
			lagSeconds -= Time.deltaTime;
			transform.position = player.transform.position - transform.forward * distance + transform.up * elevation;
		} else {
			Quaternion newRotation = Quaternion.Lerp (transform.rotation, player.transform.rotation, Time.deltaTime / lagSeconds);
			SetOrbitRotation (newRotation);
		}
	}

	public void SetOrbitRotation(Quaternion newRotation) {
		transform.position = player.transform.position;
		transform.rotation = newRotation;
		transform.position = transform.position - transform.forward * distance + transform.up * elevation;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	[SerializeField] private float distance;
	[SerializeField] private float elevation;
	[SerializeField] private float lagSeconds;

	private Transform player;
	private Queue<Quaternion> previousPlayerRotations;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		previousPlayerRotations = new Queue<Quaternion> ();
	}

	void FixedUpdate () {
		float x = Input.GetAxis ("Camera X");
		float y = Input.GetAxis ("Camera Y");

		previousPlayerRotations.Enqueue (player.rotation);

		if (lagSeconds > 0) {
			lagSeconds -= Time.fixedDeltaTime;
			transform.position = player.position - transform.forward * distance + transform.up * elevation;
		} else {
			SetOrbitRotation (previousPlayerRotations.Dequeue());
		}
	}

	public void SetOrbitRotation(Quaternion newRotation) {
		transform.rotation = newRotation;
		transform.position = player.position - transform.forward * distance + transform.up * elevation;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	private enum CameraMode
	{
		MODE_FOLLOW,
		MODE_LOOKAT
	}

	[SerializeField] private float distance;
	[SerializeField] private float elevation;
	[SerializeField] private float lagSeconds;

	private Transform player;
	private Queue<Quaternion> previousPlayerRotations;
	private CameraMode mode;

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
		} else if (mode == CameraMode.MODE_FOLLOW) {
            Quaternion orbit = Mathf.Abs(x) > Mathf.Abs(y) ? Quaternion.AngleAxis(180f * x, player.transform.up) : Quaternion.AngleAxis(90f * y, player.transform.right);
			SetOrbitRotation (orbit * previousPlayerRotations.Dequeue());
		} else {
			previousPlayerRotations.Dequeue ();
		}
		transform.position = player.position - transform.forward * distance + transform.up * elevation;
	}

	public void SetOrbitRotation(Quaternion newRotation) {
		transform.rotation = newRotation;
	}

	public void LookAt(Transform tgt, Vector3 up) {
		mode = CameraMode.MODE_LOOKAT;
		transform.LookAt (tgt, up);
	}

	public void Follow () {
		mode = CameraMode.MODE_FOLLOW;
	}
}

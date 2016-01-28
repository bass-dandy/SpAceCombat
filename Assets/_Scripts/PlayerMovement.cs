using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float maxRollRate;
	[SerializeField] private float maxPitchRate;
	[SerializeField] private float maxAccelRate;
	[SerializeField] private float maxSpeed;
	[SerializeField] private float minSpeed;
	[SerializeField] private float idleSpeed;
	[SerializeField] private float maxYawRate;

	private float rollRate;
	private float pitchRate;
	private float speed;
	private float yawRate;


	void Start () {
		rollRate = pitchRate = 0.0f;
		speed = idleSpeed;
	}
	
	void FixedUpdate () {
		rollRate = DoRoll ();
		transform.Rotate (new Vector3 (0f, 0f, rollRate * Time.fixedDeltaTime));

		pitchRate = DoPitch ();
		transform.Rotate (new Vector3(pitchRate * Time.fixedDeltaTime, 0f, 0f));

		speed = DoAccel ();
		transform.position = transform.position + transform.forward * speed * Time.fixedDeltaTime;

		yawRate = DoYaw ();
		transform.Rotate (new Vector3(0f, yawRate * Time.fixedDeltaTime, 0f));
	}

	private float DoRoll() {
		float roll  = Input.GetAxis ("Roll");

		if (roll != 0) {
			rollRate = Mathf.Lerp (rollRate, -roll * maxRollRate, 0.1f);
		} else {
			rollRate = Mathf.Lerp (rollRate, 0.0f, 0.2f);
		}
		return rollRate;
		//Quaternion deltaRoll = Quaternion.Euler(new Vector3(0f, 0f, rollRate * Time.deltaTime));
	}

	private float DoPitch() {
		float pitch = Input.GetAxis ("Pitch");

		if (pitch != 0) {
			pitchRate = Mathf.Lerp (pitchRate, pitch * maxPitchRate, 0.1f);
		} else {
			pitchRate = Mathf.Lerp (pitchRate, 0.0f, 0.2f);
		}
		return pitchRate;
		//Quaternion deltaPitch = Quaternion.Euler(new Vector3(pitchRate * Time.deltaTime, 0f, 0f));
	}

	private float DoAccel() {
		float accel = Input.GetAxis ("Acceleration");

		if (accel > 0) {
			speed = Mathf.Lerp (speed, accel * maxSpeed, 0.02f);
		} else if (accel < 0) {
			speed = Mathf.Lerp (speed, minSpeed, Mathf.Abs(accel) * 0.02f);
		} else {
			speed = Mathf.Lerp (speed, idleSpeed, 0.001f);
		}
		return speed;
		//rb.MovePosition (transform.position + transform.forward * idleSpeed * Time.deltaTime);
	}

	private float DoYaw() {
		bool right = Input.GetButton ("Yaw Right");
		bool left  = Input.GetButton ("Yaw Left");

		if (left && !right) {
			yawRate = Mathf.Lerp (yawRate, -maxYawRate, 0.1f);
		} else if (right && !left) {
			yawRate = Mathf.Lerp (yawRate, maxYawRate, 0.1f);
		} else {
			yawRate = Mathf.Lerp (yawRate, 0f, 0.2f);
		}
		return yawRate;
	}
}

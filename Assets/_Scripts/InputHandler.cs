using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	[SerializeField] private CameraController mainCamera;
	[SerializeField] private float targetLookDelay;

	private PlayerMovement player;
    private Gun gun;
    private MissileLauncher missiles;
	private ParticipantManager participants;
	private float targetHold;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
        gun = player.GetComponentInChildren<Gun> ();
        missiles = player.GetComponentInChildren<MissileLauncher> ();
		participants = GameObject.FindGameObjectWithTag ("Participants").GetComponent<ParticipantManager> ();
		targetHold = targetLookDelay;
	}


	// Inputs unrelated to movement
	void Update () {
        // Target switching/looking
		if (Input.GetButton ("Target")) {
			if (targetHold > 0.0f)
				targetHold -= Time.deltaTime;
			else if (participants.CurrentTarget != null)
				mainCamera.LookAt (participants.CurrentTarget.transform, player.transform.up);
		} else {
			if (0.0f < targetHold && targetHold < targetLookDelay) {
				participants.ChangeTarget ();
			}
			targetHold = targetLookDelay;
			mainCamera.Follow ();
		}
        // Shoot gun
        if (Input.GetButton ("Gun")) {
            gun.Fire();
        }
        // Fire missiles
        if(Input.GetButtonDown("Missile")) {
            missiles.Fire();
        }
	}


	// Movement-related inputs
	void FixedUpdate() {
		float roll  = Input.GetAxis ("Roll");
		player.DoRoll (roll);

		float pitch = Input.GetAxis ("Pitch");
		player.DoPitch (pitch);

		float accel = Input.GetAxis ("Acceleration");
		player.DoAccel (accel);

		bool right = Input.GetButton ("Yaw Right");
		bool left  = Input.GetButton ("Yaw Left");
		player.DoYaw (left, right);
	}
}

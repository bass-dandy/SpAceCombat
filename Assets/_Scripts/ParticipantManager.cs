using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticipantManager : MonoBehaviour {

	private List<Participant> participants;
	private Participant currentTarget;

	public List<Participant> Participants {
		get { return participants; }
	}

	public Participant CurrentTarget {
		get { return currentTarget; }
	}

	void Start() {
		GameObject[] participantObjs = GameObject.FindGameObjectsWithTag ("Tgt");
		participants = new List<Participant> ();

		foreach (GameObject obj in participantObjs) {
			participants.Add (obj.GetComponent<Participant>());
		}
	}

	public void ChangeTarget() {
		foreach (Participant p in participants) {
			if (p.State == Participant.HudState.P_NORMAL) {
				if (currentTarget != null) {
					currentTarget.State = Participant.HudState.P_NORMAL;
				}
				currentTarget = p;
				p.State = Participant.HudState.P_TRACKED;
				return;
			}
		}
	}

}

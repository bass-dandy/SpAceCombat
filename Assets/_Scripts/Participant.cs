using UnityEngine;
using System.Collections;

public class Participant : MonoBehaviour {

	public enum HudState
	{
		P_ALLY,
		P_NORMAL,
		P_TRACKED,
		P_LOCKED
	}

	[SerializeField] private string callsign;
	[SerializeField] private HudState state;

	public string Callsign {
		get { return callsign; }
	}

	public HudState State {
		get { return state; }
		set { state = value; }
	}
}

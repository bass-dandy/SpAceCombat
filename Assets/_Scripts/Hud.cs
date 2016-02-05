using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour {

	[SerializeField] private Color color;
	[SerializeField] private Color lockedColor;
	[SerializeField] private Color allyColor;
	[SerializeField] private Material material;
	[SerializeField] private int minBoxWidth;
	[SerializeField] private int maxBoxWidth;
	[SerializeField] private int gunCrossWidth;
	[SerializeField] private float flickerDuration;
	[SerializeField] private int fontSize;
	[SerializeField] private int lockThreshold;

	private ParticipantManager participants;
	private Camera cam;
	private GUIStyle style;
	private GUIStyle lockedStyle;
	private GUIStyle allyStyle;
	private bool isFlickered;

	private GunCrossRenderer gunCross;

	void Start() {
		participants = GameObject.FindGameObjectWithTag("Participants").GetComponent<ParticipantManager>();
		cam = gameObject.GetComponent<Camera> ();

		// Set GUI styles
		style = new GUIStyle();
		lockedStyle = new GUIStyle ();
		allyStyle = new GUIStyle ();
		style.fontSize = lockedStyle.fontSize = allyStyle.fontSize = fontSize;
		style.clipping =  lockedStyle.clipping = allyStyle.clipping = TextClipping.Overflow;

		style.normal.textColor = color;
		lockedStyle.normal.textColor = lockedColor;
		allyStyle.normal.textColor = allyColor;

		InvokeRepeating ("Flicker", flickerDuration, flickerDuration);
		gunCross = new GunCrossRenderer (gunCrossWidth, cam, allyColor);
	}

	private void Flicker() {
		isFlickered = !isFlickered;
	}

	void OnGUI () {
		if (Event.current.type.Equals (EventType.Repaint)) {
			Draw ();
		}
	}

	private void Draw () {
		foreach (Participant p in participants.Participants) {
			Vector3 screenPos = cam.WorldToScreenPoint (p.transform.position);
			if (screenPos.z > 0) {
				DrawBox (screenPos, p);
			}
		}
	}

	private void DrawBox(Vector3 screenPos, Participant p) {
		float boxWidth = Mathf.Clamp (maxBoxWidth - screenPos.z / 5, minBoxWidth, maxBoxWidth);

		float xMin = screenPos.x - boxWidth / 2;
		float xMax = screenPos.x + boxWidth / 2;
		float yMin = screenPos.y - boxWidth / 2;
		float yMax = screenPos.y + boxWidth / 2;

		GUIStyle styleToUse = p.State == Participant.HudState.P_LOCKED ? lockedStyle : 
			p.State == Participant.HudState.P_ALLY ? allyStyle : style;

		// Draw box labels first since GUI class uses pixel coords (y-down while GL is y-up)
		GUI.Label (new Rect(xMax + 5f, Screen.height - yMax, 0, fontSize), p.Callsign.ToUpper(), styleToUse);

		// Draw distance to target if tracked or locked
		if (p.State == Participant.HudState.P_TRACKED || p.State == Participant.HudState.P_LOCKED) {
			float distance = Vector3.Distance (cam.transform.position, p.transform.position);
			GUI.Label (new Rect (xMax + 5f, Screen.height - yMax + fontSize, 0, fontSize), (int) distance + "m", styleToUse);

			if (distance < lockThreshold) {
				p.State = Participant.HudState.P_LOCKED;
				material.SetPass (0);
				gunCross.Draw (distance);
			} else {
				p.State = Participant.HudState.P_TRACKED;
			}
		}
		// Convert to NDC coordinates for GL calls
		xMin /= Screen.width;
		xMax /= Screen.width;
		yMin /= Screen.height;
		yMax /= Screen.height;

		if (p.State != Participant.HudState.P_TRACKED || !isFlickered) {

			material.SetPass (0);
			GL.PushMatrix ();
			GL.LoadOrtho ();
			GL.Begin (GL.LINES);

			// If target locked, set locked color and draw diamond
			if (p.State == Participant.HudState.P_LOCKED) {
				GL.Color (lockedColor);

				float xCenter = (xMin + xMax) / 2.0f;
				float yCenter = (yMin + yMax) / 2.0f;

				// top right
				GL.Vertex3 (xCenter, yMax, 0);
				GL.Vertex3 (xMax, yCenter, 0);

				// bottom right
				GL.Vertex3 (xMax, yCenter, 0);
				GL.Vertex3 (xCenter, yMin, 0);

				// bottom left
				GL.Vertex3 (xCenter, yMin, 0);
				GL.Vertex3 (xMin, yCenter, 0);

				// top left
				GL.Vertex3 (xMin, yCenter, 0);
				GL.Vertex3 (xCenter, yMax, 0);
			} else if (p.State == Participant.HudState.P_ALLY) {
				GL.Color (allyColor);

				GL.Vertex3 (xMin, yMin, 0);
				GL.Vertex3 ((xMin + xMax) / 2f, yMax, 0);

				GL.Vertex3 (xMax, yMin, 0);
				GL.Vertex3 ((xMin + xMax) / 2f, yMax, 0);
			} else {
				GL.Color (color);
			}
			// top
			GL.Vertex3 (xMin, yMax, 0);
			GL.Vertex3 (xMax, yMax, 0);

			// right
			GL.Vertex3 (xMax, yMax, 0);
			GL.Vertex3 (xMax, yMin, 0);

			// bottom
			GL.Vertex3 (xMax, yMin, 0);
			GL.Vertex3 (xMin, yMin, 0);

			// left
			GL.Vertex3 (xMin, yMin, 0);
			GL.Vertex3 (xMin, yMax, 0);

			GL.End ();
			GL.PopMatrix ();
		}
	}
}

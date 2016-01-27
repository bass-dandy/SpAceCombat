using UnityEngine;
using System.Collections;

public class TgtBoxRenderer : MonoBehaviour {

	[SerializeField] private Texture tgtBox;
	[SerializeField] private Material tgtBoxMaterial;
	[SerializeField] private int lineWidth;
	[SerializeField] private int minBoxWidth;
	[SerializeField] private int maxBoxWidth;

	private GameObject[] tgts;
	private Camera cam;

	void Start () {
		tgts = GameObject.FindGameObjectsWithTag ("Tgt");
		cam = gameObject.GetComponent<Camera> ();
	}
	
	void OnGUI () {
		if (Event.current.type.Equals (EventType.Repaint)) {
			foreach (GameObject tgt in tgts) {
				Vector3 screenPos = cam.WorldToScreenPoint (tgt.transform.position);
				if (screenPos.z > 0) {
					// Flip the returned y value to be consistent with actual screen coords
					screenPos.y = Screen.height - screenPos.y;
					DrawBox (screenPos);
				}
			}
		}
	}

	private void DrawBox(Vector3 screenPos) {
		float boxWidth = Mathf.Clamp (maxBoxWidth - screenPos.z / 5, minBoxWidth, maxBoxWidth);

		int xMin = (int) (screenPos.x - boxWidth / 2);
		int xMax = (int) (screenPos.x + boxWidth / 2);
		int yMin = (int) (screenPos.y - boxWidth / 2);
		int yMax = (int) (screenPos.y + boxWidth / 2);

		Rect leftRect  = new Rect (xMin, yMin, lineWidth, boxWidth);
		Rect rightRect = new Rect (xMax - lineWidth, yMin, lineWidth, boxWidth);
		Rect topRect   = new Rect (xMin, yMin, boxWidth, lineWidth);
		Rect botRect   = new Rect (xMin, yMax - lineWidth, boxWidth, lineWidth);

		Graphics.DrawTexture (leftRect, tgtBox, tgtBoxMaterial);
		Graphics.DrawTexture (rightRect, tgtBox, tgtBoxMaterial);
		Graphics.DrawTexture (topRect, tgtBox, tgtBoxMaterial);
		Graphics.DrawTexture (botRect, tgtBox, tgtBoxMaterial);
	}
}

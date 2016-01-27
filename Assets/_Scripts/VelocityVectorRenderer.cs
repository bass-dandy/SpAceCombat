using UnityEngine;
using System.Collections;

public class VelocityVectorRenderer : MonoBehaviour {

	[SerializeField] private Texture velocityVectorTexture;
	[SerializeField] private Material mat;
	[SerializeField] private float distance;
	[SerializeField] private float size;

	private Transform player;
	private Camera cam;
	private float offset;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		cam = gameObject.GetComponent<Camera> ();
		offset = size / 2.0f;
	}
	
	void OnGUI () {
		if (Event.current.type.Equals (EventType.Repaint)) {
			Vector3 screenPos = cam.WorldToScreenPoint (player.position + player.forward * distance);
			screenPos.y = Screen.height - screenPos.y;

			Rect vectorRect = new Rect (screenPos.x - offset, screenPos.y - offset, size, size);
			Graphics.DrawTexture (vectorRect, velocityVectorTexture, mat);
		}
	}
}

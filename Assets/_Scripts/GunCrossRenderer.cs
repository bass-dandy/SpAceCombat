using UnityEngine;
using System.Collections;

public class GunCrossRenderer {

	private Camera cam;
	private Transform player;
	private float offsetX;
	private float offsetY;
	private Color color;

	public GunCrossRenderer (int size, Camera cam, Color color) {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		this.cam = cam;
		this.color = color;
		offsetX = size / 2.0f / Screen.width;
		offsetY = size / 2.0f / Screen.height;
	}

	public void Draw (float distance) {
		Vector3 screenPos = cam.WorldToScreenPoint (player.position + player.forward * distance);

		// Convert to NDC coordinates
		screenPos.x /= Screen.width;
		screenPos.y /= Screen.height;

		GL.PushMatrix ();
		GL.LoadOrtho ();
		GL.Begin (GL.LINES);
		GL.Color (color);

		// horizontal line
		GL.Vertex3 (screenPos.x - offsetX, screenPos.y, 0);
		GL.Vertex3 (screenPos.x + offsetX, screenPos.y, 0);

		// vertical line
		GL.Vertex3 (screenPos.x, screenPos.y - offsetY, 0);
		GL.Vertex3 (screenPos.x, screenPos.y + offsetY, 0);

		GL.End ();
		GL.PopMatrix ();
	}
}

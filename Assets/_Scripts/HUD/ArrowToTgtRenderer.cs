using UnityEngine;
using System.Collections;

public class ArrowToTgtRenderer {

    private ParticipantManager participants;
    private Camera cam;
    private Color color;
    private Material material;

    // Center of the screen in NDC
    private Vector3 origin;

    public ArrowToTgtRenderer(Camera cam, Color color, Material material) {
        this.cam = cam;
        this.color = color;
        this.material = material;
        participants = GameObject.FindGameObjectWithTag("Participants").GetComponent<ParticipantManager>();
        origin = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
    }

    
    public void Draw() {
        if(participants.CurrentTarget != null) {
            Vector3 screenPos = cam.WorldToScreenPoint(participants.CurrentTarget.transform.position);

            // Only display arrow if target is off screen
            if(screenPos.x < 0 || screenPos.y < 0 || screenPos.z < 0 || screenPos.x > Screen.width || screenPos.y > Screen.height) {

                Vector3 direction = screenPos - origin;

                // Directions are flipped if target is behind camera
                if (direction.z < 0)
                    direction *= -1;

                direction.z = 0;
                direction.Normalize();

                // Vector perpendicular to direction. This is for the triangle base.
                Vector3 directionOrtho = new Vector3(-direction.y, direction.x, 0) * 5;
                Vector3 tip = origin + direction * 200;
                Vector3 bottom = tip - direction * 40;

                // Convert everything to NDC space
                directionOrtho.x /= Screen.width;
                directionOrtho.y /= Screen.height;

                tip.x /= Screen.width;
                tip.y /= Screen.height;

                bottom.x /= Screen.width;
                bottom.y /= Screen.height;

                // Draw the triangle (finally)
                material.SetPass(0);
                GL.PushMatrix();
                GL.LoadOrtho();
                GL.Begin(GL.TRIANGLES);
                GL.Color(color);

                GL.Vertex(tip);
                GL.Vertex(bottom + directionOrtho);
                GL.Vertex(bottom - directionOrtho);

                GL.End();
                GL.PopMatrix();
            }
        }

    }
}

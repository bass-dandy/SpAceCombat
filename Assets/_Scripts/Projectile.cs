using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private float ttl;
	
	void FixedUpdate () {
	    if (ttl > 0f) {
            ttl -= Time.fixedDeltaTime;
            transform.position = transform.position + transform.forward * speed;
        } else {
            Destroy (gameObject);
        }
	}
}

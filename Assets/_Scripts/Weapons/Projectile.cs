using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    [SerializeField] protected float speed;
    [SerializeField] protected float ttl;
	
	void FixedUpdate () {
        Move ();
	}

    protected void Move () {
        if (ttl > 0f) {
            ttl -= Time.fixedDeltaTime;
            transform.position = transform.position + transform.forward * speed;
        } else {
            DestroySelf();
        }
    }

    protected virtual void DestroySelf() {
        Destroy (gameObject);
    }
}

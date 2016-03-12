using UnityEngine;
using System.Collections;

public class HomingProjectile : Projectile {

    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float agility;

    private Transform tgt;
	
    void Start () {
        Participant tgtObj = GameObject.FindGameObjectWithTag("Participants").GetComponent<ParticipantManager>().CurrentTarget;
        if (tgtObj != null && tgtObj.State == Participant.HudState.P_LOCKED)
            tgt = tgtObj.transform;
    }

	void FixedUpdate () {
	    if (tgt != null) {
            Quaternion tgtRotation = Quaternion.LookRotation (tgt.position - transform.position, transform.up);
            transform.rotation = Quaternion.RotateTowards (transform.rotation, tgtRotation, agility);
        }
        Move ();
	}

    protected override void DestroySelf () {
        ParticleSystem[] trails = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem trail in trails) {
            trail.transform.SetParent(null);
            trail.Stop();
            Destroy(trail.gameObject, trail.duration + trail.startLifetime);
        }
        Destroy (gameObject);
        GameObject explosionInstance = Instantiate (explosion.gameObject, transform.position, transform.rotation) as GameObject;
        Destroy (explosionInstance, explosion.duration + explosion.startLifetime);
    }
}

using UnityEngine;
using System.Collections;

public class HomingProjectile : Projectile {

    [SerializeField] private float agility;

    private Transform tgt;
	
    void Start () {
        Participant tgtObj = GameObject.FindGameObjectWithTag("Participants").GetComponent<ParticipantManager>().CurrentTarget;
        if (tgtObj != null)
            tgt = tgtObj.transform;
    }

	void FixedUpdate () {
	    if (tgt != null) {
            Quaternion tgtRotation = Quaternion.LookRotation (tgt.position - transform.position);
            transform.rotation = Quaternion.RotateTowards (transform.rotation, tgtRotation, agility);
        }
        Move ();
	}
}

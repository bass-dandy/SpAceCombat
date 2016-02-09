using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float delay;

    private float elapsed;
    private bool alternator;

	void Start () {
        elapsed = delay;
        alternator = false;
	}
	
	public void Fire() {
        if (elapsed <= 0f) {
            elapsed = delay;
            Transform exitPoint = alternator ? left.transform : right.transform;
            Instantiate (bulletPrefab, exitPoint.position, transform.rotation);
            alternator = !alternator;
        } else {
            elapsed -= Time.deltaTime;
        }
    }
}

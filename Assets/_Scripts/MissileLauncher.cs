using UnityEngine;
using System.Collections;

public class MissileLauncher : MonoBehaviour {

    [SerializeField] private GameObject missilePrefab;

	public void Fire() {
        Instantiate (missilePrefab, transform.position, transform.rotation);
    }
}

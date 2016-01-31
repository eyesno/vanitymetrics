﻿using UnityEngine;

public class ProjectileLauncher : MonoBehaviour {
    [SerializeField] private GameObject _projectile;

    public void Fire(float force) {
		AudioSource fire = GetComponent<AudioSource> ();
		fire.Play ();

        var shot = (GameObject) Instantiate(_projectile, transform.position, transform.rotation);
        shot.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
    }
}

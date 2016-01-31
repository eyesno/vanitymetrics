﻿using UnityEngine;
using System.Collections;

public class BoatTarget : MonoBehaviour {

	protected BoatHandler boatHandler;
	protected float boatSpeed = 0;
	protected float boatDeathTimer = 0;
	protected float boatSinkTimer = 0;
	protected float boatSinkDuration = 20.0F;
	protected bool fireStarted = false;
	protected float particleStopTime = 0.2F;
	protected float boatInitSinkSpeed = 0;
	protected float sinkRate = 0;

	// Attached objects
	[SerializeField]
	protected float boatHeight = 0;
	[SerializeField]
	protected ParticleSystem fireSystem;
	[SerializeField]
	protected ParticleSystem wakeSystem;

	public void Init (BoatHandler handler, float angle, float speed) {

		boatHandler = handler;
		transform.localPosition = Vector3.zero;
		transform.localEulerAngles = new Vector3 (0, angle, 0);
		boatSpeed = speed;
		boatDeathTimer = 0;

		// wake system settings
		wakeSystem.startSpeed = boatSpeed / 2;
		wakeSystem.startLifetime = boatSpeed * 4;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 newPosition = transform.position;
		newPosition += transform.forward * (boatSpeed * Time.deltaTime);

		// decrement the boat death timer
//		if (boatDeathTimer > 0.0F) {
//			boatDeathTimer -= Time.deltaTime;
//
//			if (boatDeathTimer <= 0.0F) {
//				SpawnDemon ();
//			}
//		}

		// decrement the sink timer
		if (boatSinkTimer > 0.0F) {

			if (boatSinkTimer > (boatSinkDuration * 0.75F)) {
				boatSpeed -= boatInitSinkSpeed * (Time.deltaTime / boatSinkDuration);
			} else {
				sinkRate += (Time.deltaTime / boatSinkDuration) * (0.2F / boatSinkDuration);
				newPosition.y -= sinkRate;
			}

			boatSinkTimer -= Time.deltaTime;

			// wake system settings
			wakeSystem.startSpeed = boatSpeed / 2;
			wakeSystem.startLifetime = boatSpeed;

			if (fireStarted && boatSinkTimer <= (boatSinkDuration * particleStopTime)) {
				fireStarted = false;
				fireSystem.Stop ();
//				wakeSystem.Stop ();
			}

			if (boatSinkTimer <= 0) {
				Destroy (gameObject);
			}
		}

		transform.position = newPosition;
	}

	void OnTriggerEnter (Collider other) {
		
		// Did we collide with an arrow
		if (other.gameObject.layer == 9) {
			// start a fire
			fireStarted = true;
			fireSystem.Play ();

			// sink the ship
			Sink ();
		}

		// Did we collide with the boundary
		if (other.gameObject.layer == 10) {
			SpawnDemon ();
		}
	}

	void SpawnDemon () {

		// sink the ship
//		boatDeathTimer = 0;
		Sink ();

		// spawn a demon
		GameHandler.Instance.SpawnDemon (transform);
	}

	void Sink () {
		
		// stop the boat death timer so that a demon doesn't spawn
//		boatDeathTimer = 0;
		boatInitSinkSpeed = boatSpeed;
		boatSinkTimer = boatSinkDuration;

		// tell boat handler the boat is sinking
		boatHandler.BoatSinked ();
	}
}

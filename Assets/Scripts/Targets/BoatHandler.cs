﻿using UnityEngine;
using System.Collections;

public class BoatHandler : MonoBehaviour {

	[SerializeField]
	protected BoatTarget boatPrefab;
	[SerializeField]
	protected Transform boatParent;

	// wave variables
	protected int boatsToRelease = 0;
	protected Vector2 boatReleaseTimerRange;
	protected float boatReleaseTimer;
	protected float boatSpeed = 0;
	protected float lastAngle = 0;

	// Use this for initialization
	void Start () {
		StartWave (50, 2);
	}

	void Update () {

		if (boatsToRelease > 0) {

			// decrement boat release timer
			boatReleaseTimer -= Time.deltaTime;
			if (boatReleaseTimer <= 0.0F) {

				// choose an angle
				float angle = 0;
				do {
					angle = -10.0F * Random.Range (-2, 3);
				} while (angle == lastAngle);
				lastAngle = angle;

				// create a boat
				BoatTarget newBoat = (BoatTarget) Instantiate (boatPrefab);
				newBoat.transform.SetParent (boatParent);
				newBoat.Init (angle, boatSpeed);

				boatReleaseTimer = Random.Range (boatReleaseTimerRange.x, boatReleaseTimerRange.y);
			}
		}
	}
	
	public void StartWave (int boatCount, float speed) {

		boatsToRelease = boatCount;
		boatSpeed = speed;
		float intervalSpeed = 1 / speed;
		boatReleaseTimerRange = new Vector2 (intervalSpeed + 2.5F, (intervalSpeed * 10.5F) + 2.5F);

		boatReleaseTimer = 0;
	}
}

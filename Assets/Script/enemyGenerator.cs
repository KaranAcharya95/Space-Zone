using UnityEngine;
using System.Collections;


public class enemyGenerator : MonoBehaviour {

	public GameObject alien;

	private float timeBetweenEnemies = 1f;  // 0.2 = 5 shots per second
	private int secondsBeforeFirstEnemyAppears = 3; // Wait X seconds before the first enemy appears after the game starts

	private float timeLastAlien;


	void FixedUpdate () {

		if (Time.realtimeSinceStartup > secondsBeforeFirstEnemyAppears && Time.time >= timeLastAlien) {

		GameObject.Instantiate(alien);
		timeLastAlien = Time.time + timeBetweenEnemies;

		}
	}
}
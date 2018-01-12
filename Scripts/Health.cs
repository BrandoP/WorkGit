using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public float startingHealth = 100.0f;
	public float currentHealth;
	public Slider healthSlider;
	public Player player;

	public bool isDefeated;

	// Use this for initialization
	void Start () {
		currentHealth = startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//For the player to take damage based on a certain amount
	public void takeDamage(float amount){
		player.consecutiveHits = 0;

		//subtract amount from current health and then represent it in the health bar
		currentHealth = currentHealth - amount;
		healthSlider.value = currentHealth;
		if (player.GetComponent<AttackPlayer> ().poweredUp == true) {
			player.GetComponent<AttackPlayer> ().poweredUp = false;
			player.comboPowerUp ();
		}

		//if the player has no health, set defeated to true
		if (currentHealth <= 0) {
			Defeat ();
		}
	}

	void Defeat(){
		isDefeated = true;
	}
}

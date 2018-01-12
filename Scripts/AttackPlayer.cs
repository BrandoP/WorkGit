using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour {

	public float damage = 10.0f;
	public GameObject player;
	Health playerHealth;
	public bool poweredUp;

	// Use this for initialization
	void Start () {
		playerHealth = player.GetComponent<Health> ();
	}

	public void Attack(float newDamage){

		float curDamage = damage;
		curDamage = damage / newDamage;

		//Max damage is 10
		if (poweredUp == true) {
			curDamage = 20;
		}
		else{
			if(curDamage > 10) {
				curDamage = 10;
			}

			//Smallest damage is 1
			else if (curDamage <= 0) {
				curDamage = 1;
			}
		}

		Debug.Log ("Current damage" + curDamage);
		//While player still has health, they can still take damage
		if (playerHealth.currentHealth > 0) {
			poweredUp = false;
			playerHealth.takeDamage (curDamage);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChanges : MonoBehaviour {

	public static PlayerChanges instance;

	bool attackWhileDown;

	// Use this for initialization
	void Start () {
		instance = this;
		attackWhileDown = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void strike(GameObject attackingPlayer, GameObject damagingPlayer){
		attackingPlayer.SendMessage ("clickedStrike");
		attackingPlayer.SendMessage ("Attack", PlayerChoices.instance.timer);
		damagingPlayer.SendMessage ("clickedGrab");
	}

	public void counter(GameObject attackingPlayer, GameObject damagingPlayer){
		attackingPlayer.SendMessage ("clickedCounter");
		attackingPlayer.SendMessage ("Attack", PlayerChoices.instance.timer);
		damagingPlayer.SendMessage ("clickedStrike");
	}

	public void grab(GameObject attackingPlayer, GameObject damagingPlayer){
		attackingPlayer.SendMessage ("clickedGrab");
		attackingPlayer.SendMessage ("Attack", PlayerChoices.instance.timer);
		damagingPlayer.SendMessage ("clickedCounter");
	}

	public void groundStrike(GameObject attackingPlayer, GameObject damagingPlayer){

		if (attackWhileDown == true) {
			attackingPlayer.SendMessage ("clickedStrike");
			attackingPlayer.SendMessage ("Attack", PlayerChoices.instance.timer);
		}
	}

	public void groundCounter(GameObject attackingPlayer, GameObject damagingPlayer){

		if (attackWhileDown == true) {
			attackingPlayer.SendMessage ("clickedCounter");
			attackingPlayer.SendMessage ("Attack", PlayerChoices.instance.timer);
		}
	}

	public void groundSubmit(GameObject attackingPlayer, GameObject damagingPlayer){

		if (attackWhileDown == true) {
			attackingPlayer.SendMessage ("clickedGrab");
			attackingPlayer.SendMessage ("Attack", PlayerChoices.instance.timer);
		}
	}

	public void draw(GameObject player1, GameObject player2){
		player1.SendMessage ("takeDamage", 5.0f);
		player2.SendMessage ("takeDamage", 5.0f);
	}

	public void groundDraw(GameObject player1, GameObject player2){
		
	}

	public void setCanAttack(bool attack){
		attackWhileDown = attack;
	}
		
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	//Setting up the animations
	AttackPlayer ap;
	public Animator anim;

	public ParticleSystem powerOutline;

	int numOfStrikesDone = 0;
	int numOfGrabsDone = 0;
	int numOfCountersDone = 0;
	public int consecutiveHits= 0;

	//The states
	public bool onGround = false;
	public bool onEnemyPlayer;

	float timer;

	bool clickOne = false;

	public static Player instance;
	public Player player2;

	public GameManager gm;

	void Awake(){
		gm = FindObjectOfType<GameManager> ();
		instance = this;
		onGround = false;
	}

	// Use this for initialization
	void Start () {

		ap = GetComponent<AttackPlayer> ();
		powerOutline.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (clickOne == false) {
			clickOne = true;
		}
	}

	//This method adds up the choice of the players
	//theClickNum is the prompt clicked associated with a
	//certain number
	public void addUpClicks(int theClickNum){
		//if theClickNum is 1, add to numOfStrikesDone
		if (theClickNum == 1) {
			numOfStrikesDone += 1;
			numOfCountersDone = 0;
			numOfGrabsDone = 0;
		}
		//if theClickNum is 2, add to numOfCountersDone
		if (theClickNum == 2) {
			numOfCountersDone += 1;
			numOfStrikesDone = 0;
			numOfGrabsDone = 0;
		}
		//if theClickNum is 2, add to numOfGrabsDone
		if (theClickNum == 3) {
			numOfGrabsDone += 1;
			numOfCountersDone = 0;
			numOfStrikesDone = 0;
		}
	}

	//If the player has clicked strike
	public void clickedStrike(){

		consecutiveHits += 1;
		PlayerChoices.instance.promptsOff ();
		PlayerChoices.instance.choiceMovement();
		comboPowerUp ();
		fallToGround ();
	}

	//If the player has clicked counter
	public void clickedCounter(){

		consecutiveHits += 1;
		PlayerChoices.instance.promptsOff ();
		PlayerChoices.instance.choiceMovement();
		comboPowerUp ();
		fallToGround ();
	}

	//If the player has clicked grab
	public void clickedGrab(){

		if(player2.onGround){
			numOfGrabsDone = 0;
			anim.SetBool ("Submit", true);
			PlayerChoices.instance.promptsOff ();
			onEnemyPlayer = true;

		} else {
			consecutiveHits += 1;
			PlayerChoices.instance.promptsOff ();
			PlayerChoices.instance.choiceMovement();
			comboPowerUp ();
			fallToGround ();
		}
	}

	//If a action is done more than three times, cause player to fall to ground
	public void fallToGround(){

		if (numOfStrikesDone == 3 || numOfGrabsDone == 3 || numOfCountersDone == 3 || this.GetComponent<Health>().isDefeated ) {

			this.onGround = true;
			this.anim.SetBool ("FallDown", onGround);

		} else {
			this.onGround = false;
		}
	}

	//If a player has landed 3 consecutive hits, they are powered up
	public void comboPowerUp(){
		if(consecutiveHits >= 3){
			ap.poweredUp = true;
			powerOutline.gameObject.SetActive (true);
		}

		if(this.GetComponent<AttackPlayer> ().poweredUp == false){
			powerOutline.gameObject.SetActive (false);
		}
	}

	public void playerGetUp(){
	
			this.onGround = false;

			this.anim.SetBool ("GetUp", true);
			this.anim.SetBool ("FallDown", onGround);

			player2.onEnemyPlayer = false;
			player2.anim.SetBool ("Submit", false);
			PlayerChoices.instance.GetUp.SetActive (false);

			numOfStrikesDone = 0;
			numOfGrabsDone = 0;
			numOfCountersDone = 0;
	}
}

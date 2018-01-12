using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class PlayerChoices : MonoBehaviour {

	DecisionTree root;

	public GameObject Strike;
	public GameObject Counter;
	public GameObject Grab;
	public GameObject GetUp;
	public GameObject countDown;

	public GameManager gm;

	public Player player;
	public Player player2;

	private float randX;
	private float randY;
	public float timer = 0.0f;
	public int changeTime = 5;

	//number of times each action is done
	bool isActive = true;

	public static PlayerChoices instance;

	void Awake(){
		instance = this;
	}

	void Start(){

		BuildDecisionTree();

		Strike.SetActive(false);
		Counter.SetActive(false);
		Grab.SetActive(false);
		GetUp.SetActive (false);
	
		Invoke ("choiceMovement", 0.0f);
	}

	void Update(){

		if (isActive) {
			root.Search ();
		}

		if (!GetComponent<countDown> ().showCountdown) {
			timer += Time.deltaTime;
		}
	}

	/*****  Decisions  ******/

	bool isCountdownShowing(){
		return GetComponent<countDown> ().showCountdown;
	}

	bool timeOut(){
		if (timer > changeTime) {
			return true;
		} else {
			return false;
		}
	}

	bool playerDown(){
		return player.onGround;
	}

	bool enemyDown(){
		return player2.onGround;
	}

	bool isAnimationPlaying(){
		if (gm.GetComponent<PlayableDirector> ().state == PlayState.Playing) {
			return true;
		} else {
			return false;
		}
	}

	bool submitCountdown(){
		if (player.onEnemyPlayer || player2.onEnemyPlayer) {
			return true;
		} else {
			return false;
		}
	}

	bool isPlayerDefeated(){
		if (player.GetComponent<Health> ().currentHealth == 0 || player2.GetComponent<Health> ().currentHealth == 0) {
			return true;
		} else {
			return false;
		}
	}
		

	/*****  Actions  ******/
	//Moves prompts on screen randomly
	public void choiceMovement(){
		//while timer is greater than changeTime seconds have prompts displayed, when time is up move the prompts
		GetUp.SetActive (false);

		//randomly pick where Strike will show up and move on screen
		randX = Random.Range (-395.0f, 395.0f);
		randY = Random.Range (-100.0f, 50.0f);
		Strike.transform.localPosition = new Vector3 (randX, randY, 0);

		//randomly pick where Counter will show up and move on screen
		randX = Random.Range (-395.0f, 395.0f);
		randY = Random.Range (-100.0f, 50.0f);
		Counter.transform.localPosition = new Vector3 (randX, randY, 0);

		//randomly pick where Grab will show up and move on screen
		randX = Random.Range (-395.0f, 395.0f);
		randY = Random.Range (-100.0f, 50.0f);
		Grab.GetComponentInChildren<Text> ().text = "Grab";
		Grab.transform.localPosition = new Vector3 (randX, randY, 0);

		timer = 0;
	}

	//Turn off all attack prompts
	public void promptsOff(){
		Strike.SetActive (false);
		Grab.SetActive (false);
		Counter.SetActive (false);
	}

	//Turns on all attacking prompts, if someone has not won
	public void promptsOn(){
		if (gm.winScreenActive() == false) {
			Strike.SetActive (true);
			Grab.SetActive (true);
			Counter.SetActive (true);
		}
	}

	//Sets grab to submit if enemy is on ground
	bool enemyOnGround(){
		Grab.GetComponentInChildren<Text> ().text = "Submit";
		return true;
	}

	//Turns off prompts and turns on GetUp if player is onGround
	void playerOnGround(){
		promptsOff ();
		GetUp.SetActive (true);
	}

	//Begin the countdown if player or enemy is pinned
	void beginSubmitCountDown(){
		GetComponent<countDown> ().StartCoroutine ("finalCountDown");
	}


	void BuildDecisionTree()
	{
		/******  Decision Nodes  ******/

		DecisionTree isCountDown = new DecisionTree();
		isCountDown.SetDecision(isCountdownShowing);

		DecisionTree isEnemyOnGround = new DecisionTree ();
		isEnemyOnGround.SetDecision (enemyDown);

		DecisionTree isPlayerOnGround = new DecisionTree();
		isPlayerOnGround.SetDecision(playerDown);

		DecisionTree isSubmittingPlayer = new DecisionTree ();
		isSubmittingPlayer.SetDecision (submitCountdown);

		DecisionTree isSubmittingEnemy = new DecisionTree ();
		isSubmittingEnemy.SetDecision (submitCountdown);

		DecisionTree isTimeOut = new DecisionTree ();
		isTimeOut.SetDecision (timeOut);

		DecisionTree animationPlaying = new DecisionTree();
		animationPlaying.SetDecision(isAnimationPlaying);

		DecisionTree enemyOnGroundPrompts = new DecisionTree ();
		enemyOnGroundPrompts.SetDecision (enemyOnGround);

		DecisionTree playersHealthZero = new DecisionTree ();
		playersHealthZero.SetDecision (isPlayerDefeated);


		/******  Set up Actions  ******/

		DecisionTree turnOffPrompts = new DecisionTree ();
		turnOffPrompts.SetAction (promptsOff);

		DecisionTree turnOnPrompts = new DecisionTree ();
		turnOnPrompts.SetAction (promptsOn);

		DecisionTree movePrompts = new DecisionTree ();
		movePrompts.SetAction (choiceMovement);

		DecisionTree playerOnGroundPrompts = new DecisionTree ();
		playerOnGroundPrompts.SetAction (playerOnGround);

		DecisionTree submitCountDownOn = new DecisionTree ();
		submitCountDownOn.SetAction (beginSubmitCountDown);

		/******  Assemble Tree  ******/

		isCountDown.SetLeft (playersHealthZero);
		isCountDown.SetRight (turnOffPrompts);

		playersHealthZero.SetLeft (isPlayerOnGround);
		playersHealthZero.SetRight (submitCountDownOn);

		isEnemyOnGround.SetLeft (animationPlaying);
		isEnemyOnGround.SetRight (isSubmittingEnemy);

		isPlayerOnGround.SetLeft (isEnemyOnGround);
		isPlayerOnGround.SetRight (isSubmittingPlayer);

		isSubmittingPlayer.SetLeft (playerOnGroundPrompts);
		isSubmittingPlayer.SetRight (submitCountDownOn);

		isSubmittingEnemy.SetLeft (enemyOnGroundPrompts);
		isSubmittingEnemy.SetRight (submitCountDownOn);

		enemyOnGroundPrompts.SetRight (isTimeOut);

		animationPlaying.SetLeft (isTimeOut);
		animationPlaying.SetRight (turnOffPrompts);

		isTimeOut.SetLeft (turnOnPrompts);
		isTimeOut.SetRight (movePrompts);

		root = isCountDown;
	}
}

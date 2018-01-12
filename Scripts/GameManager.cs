using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	enum actions { Strike = 1, Counter = 2, Grab = 3, Submit = 4, GetUp = 5}

	public GameObject prefab;
	Canvas canvas;

	private int playerChoose = -1;
	private int botChoose = -2;

	private bool playersTurn = true;
	private bool canSubmit;
	private bool paused = false;
	private bool win = false;

	public GameObject player;
	public GameObject playerAI;
	public GameObject Pause;
	public GameObject Quit;

	public float timer = 0;


	// Use this for initialization
	void Start () {

		//prefab = cs.characterList [cs.getIndex ()];
		//Instantiate (prefab);

		canvas = GameObject.FindObjectOfType<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (canvas.GetComponent<countDown> ().getCountDown () == "Victory") {
			WinScreen ();
			PlayerChoices.instance.promptsOff ();
		} else {
			if (playersTurn && playerChoose == -1) {
				return;
			} else {
				if (botChoose != -2) {
				
					CheckWinner ();

					playerChoose = -1;
					botChoose = -2;
					playersTurn = true;

				}
			}
		}
	}

	void CheckWinner(){
		if (!player.GetComponent<Player> ().onGround && !playerAI.GetComponent<Player> ().onGround) {
			Debug.Log ("in check winner");
			if (playerChoose == botChoose) {
				//Draw
				PlayerChanges.instance.draw (player, playerAI);

			} else if (playerChoose == (int)actions.Strike && botChoose == (int)actions.Grab) {
				//Player wins strike
				PlayerChanges.instance.strike (player, playerAI);
				GetComponent<TimelineController> ().PlayFromTimelines (1);

			} else if ((playerChoose == (int)actions.Grab || playerChoose == 0) && botChoose == (int)actions.Strike) {
				//AI wins strike
				PlayerChanges.instance.strike (playerAI, player);
				GetComponent<TimelineController> ().PlayFromTimelines (3);

			} else if (playerChoose == (int)actions.Counter && botChoose == (int)actions.Strike) {
				//Players wins counter
				PlayerChanges.instance.counter (player, playerAI);
				GetComponent<TimelineController> ().PlayFromTimelines (2);

			} else if ((playerChoose == (int)actions.Strike || playerChoose == 0) && botChoose == (int)actions.Counter) {
				//AI wins counter
				PlayerChanges.instance.counter (playerAI, player);
				GetComponent<TimelineController> ().PlayFromTimelines (4);

			} else if (playerChoose == (int)actions.Grab && botChoose == (int)actions.Counter) {
				//Player wins grab
				PlayerChanges.instance.grab (player, playerAI);
				GetComponent<TimelineController> ().PlayFromTimelines (0);

			} else if ((playerChoose == (int)actions.Counter || playerChoose == 0) && botChoose == (int)actions.Grab) {
				//AI wins grab
				PlayerChanges.instance.grab (playerAI, player);
				GetComponent<TimelineController> ().PlayFromTimelines (5);
			}
		}

		//Choices if players are grounded
		else if (player.GetComponent<Player> ().onGround || playerAI.GetComponent<Player> ().onGround) {
			if (playerChoose == botChoose) {
				//Draw
				Debug.Log ("Draw");
				PlayerChanges.instance.groundDraw (player, playerAI);

			} else if (playerChoose == (int)actions.Grab && botChoose == 0) {
				//Player submit while AI is down
				PlayerChanges.instance.groundSubmit(player, playerAI);
			}
			else if (botChoose == (int)actions.Grab && playerChoose == 0) {
				//AI submit while player is down
				PlayerChanges.instance.groundSubmit(playerAI, player);
			}
			else if (playerChoose == (int)actions.Strike && botChoose == 0) {
				//Player kicks while AI is down
				PlayerChanges.instance.groundStrike(player, playerAI);
			}
			else if (botChoose == (int)actions.Strike && playerChoose == 0) {
				//AI kicks while player is down
				PlayerChanges.instance.groundStrike(playerAI, player);
			}
			else if (playerChoose == (int)actions.Counter && botChoose == 0) {
				//Player counters while AI is down
				PlayerChanges.instance.groundCounter(player, playerAI);
			}
			else if (botChoose == (int)actions.Counter && playerChoose == 0) {
				//AI counters while player is down
				PlayerChanges.instance.groundCounter(playerAI, player);
			}

			if (player.GetComponent<Player> ().onEnemyPlayer && botChoose == (int)actions.GetUp){
				Debug.Log ("On enemy player");
			}

			if (playerAI.GetComponent<Player> ().onEnemyPlayer && playerChoose == (int)actions.GetUp){
				Debug.Log ("Enemy on player");
			}
		}
	}

	public void PlayerChoose(int choose){

		playerChoose = choose;
		player.SendMessage ("addUpClicks", playerChoose);
		player.SendMessage ("fallToGround");

		playersTurn = false;// Now AI's Turn
	}

	public void SetBotChoose(int botChoice){
		botChoose = botChoice;
		playerAI.SendMessage ("addUpClicks", botChoose);
		playerAI.SendMessage ("fallToGround");
	}

	public int getPlayerChoice(){
		return playerChoose;
	}

	void pausePressed(){
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}

	void quitGame(){
		Application.Quit();
	}
		
	void OnGUI(){
		if (paused) {
			GUILayout.Label ("Paused Game");
		} else if(player.GetComponent<Health>().isDefeated && win || player.GetComponent<Player>().onGround && win) {
			GUILayout.Label (playerAI + "Wins!");
		}
		if (playerAI.GetComponent<Health>().currentHealth < player.GetComponent<Health>().currentHealth && win || playerAI.GetComponent<Player>().onGround && win) {
			GUILayout.Label (player + "Wins!");
		}
	}

	void WinScreen(){
		win = true;
	}

	public bool winScreenActive(){
		return win;
	}

	public void clickedGetUp(Slider slider){

		float weakness = player.GetComponent<Health> ().currentHealth;

		if (weakness == 0) {
			weakness = 2;
		}
		slider.value += weakness/2;

		if (slider.value == slider.maxValue) {

			player.GetComponent<Player> ().playerGetUp ();
			slider.value = slider.minValue;
			PlayerChoices.instance.timer = 0;

			//Determines that enemy can attack while other is down
			PlayerChanges.instance.SendMessage ("setCanAttack", false);
		} else {

			player.GetComponent<Player>().onGround = true;

			//Determines that enemy can attack while other is down
			PlayerChanges.instance.SendMessage ("setCanAttack", true);
	
		}
	}
}

using UnityEngine;
using System.Collections;

public class AIDecisions : MonoBehaviour {

	public Transform player;

	public GameManager gm;

	private Animator anim;

	bool isActive = true;

	DecisionTree root;

	void Awake()
	{
		anim = GetComponent<Animator> ();
	}

	void Start()
	{
		BuildDecisionTree();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}

	void Update()
	{
		if (isActive) {
			root.Search ();
		}
	}

	public void SetAnimator(Animator an)
	{
		anim = an;
		anim.applyRootMotion = false;
	}

	void SetActive()
	{
		isActive = true;
	}

	void SetInactive()
	{
		isActive = false;
	}

	/*****  Decisions  ******/

	bool CheckPlayerChoice()
	{
		//Debug.Log ("Checking if player made choice");
		if (gm.getPlayerChoice() != -1) {
			return true;
		} else {
			return false;
		}
	}

	bool CheckAIHealth(){
		if (GetComponent<Health> ().currentHealth == 0) {
			return false;
		} else {
			return true;
		}
	}

	bool CheckAIStatus()
	{
		//Debug.Log ("Checking if AI on ground, it is" + player.GetComponent<Player> ().onGround);
		return this.GetComponent<Player> ().onGround;
	}

	bool CheckPlayerStatus()
	{
		//Debug.Log ("Checking if player on ground, it is" + player.GetComponent<Player> ().onGround);
		return player.GetComponent<Player> ().onGround;
	}

	bool playerHealthStatus()
	{
		//Debug.Log ("checking player health");
		if (player.GetComponent<Health> ().currentHealth > 30) {
			return false;
		} else {
			return true;
		}
	}

	bool attackDecision()
	{
		int decision = Random.Range (0, 2);
		//Debug.Log ("getting AI attack decision " + decision);

		if (decision == 1) {
			return false;
		} else {
			return true;
		}
	}

	bool Attack()
	{
		int decision = Random.Range (0,2);
		//Debug.Log ("getting AI 2nd attack decision "+ decision);

		if (decision == 1) {
			return false;
		} else {
			return true;
		}
	}

	bool playerAttackTime()
	{
		if (PlayerChoices.instance.timer > PlayerChoices.instance.changeTime) {
			//Debug.Log ("Player did not attack ");
			gm.PlayerChoose (0);
			return true;
		} else {
			//Debug.Log ("Player must attack");
			return false;
		}
	}

	bool playerOnAI(){
		if (player.GetComponent<Player> ().onEnemyPlayer == true) {
			return true;
		} else {
			return false;
		}
	}

	/******  Actions  ******/
	void Idle()
	{
		return;
	}

	void GetUp()
	{
		Debug.Log ("Tried to get up");
		float weakness = player.GetComponent<Health> ().startingHealth - player.GetComponent<Health> ().currentHealth;
		float rand = Random.Range (0, 100);

		if (rand < weakness && PlayerChoices.instance.timer > PlayerChoices.instance.changeTime) {
			this.GetComponent<Player> ().playerGetUp ();
		} else {
			gm.SetBotChoose (0);
		}
	}

	void submit()
	{
		//Debug.Log ("Chose submit");
		gm.SetBotChoose (3);
	}

	void grab()
	{
		//Debug.Log ("Chose grab");
		gm.SetBotChoose (3);
	}

	void strike()
	{
		//Debug.Log ("Chose strike");
		gm.SetBotChoose (1);
	}

	void counter()
	{
		//Debug.Log ("Chose counter");
		gm.SetBotChoose (2);
	}


	/******  Build Decision Tree  ******/

	void BuildDecisionTree()
	{
		/******  Decision Nodes  ******/

		DecisionTree isAttackingNode = new DecisionTree();
		isAttackingNode.SetDecision(CheckPlayerChoice);

		DecisionTree AIOnGroundNode = new DecisionTree ();
		AIOnGroundNode.SetDecision (CheckAIStatus);

		DecisionTree playerOnGroundNode = new DecisionTree();
		playerOnGroundNode.SetDecision(CheckPlayerStatus);

		DecisionTree willSubmitNode = new DecisionTree ();
		willSubmitNode.SetDecision (playerHealthStatus);

		DecisionTree willAttackNode = new DecisionTree ();
		willAttackNode.SetDecision (attackDecision);

		DecisionTree actAttackNode = new DecisionTree();
		actAttackNode.SetDecision(Attack);

		DecisionTree didPlayerAttack = new DecisionTree ();
		didPlayerAttack.SetDecision (playerAttackTime);

		DecisionTree isPlayerOnAI = new DecisionTree ();
		isPlayerOnAI.SetDecision (playerOnAI);

		/******  Set up Actions  ******/

		DecisionTree actIdleNode = new DecisionTree();
		actIdleNode.SetAction(Idle);

		DecisionTree tryGetUp = new DecisionTree ();
		tryGetUp.SetAction (GetUp);

		DecisionTree Submit = new DecisionTree ();
		Submit.SetAction (submit);

		DecisionTree Grab = new DecisionTree ();
		Grab.SetAction (grab);

		DecisionTree Counter = new DecisionTree ();
		Counter.SetAction (counter);

		DecisionTree Strike = new DecisionTree ();
		Strike.SetAction (strike);

		/******  Assemble Tree  ******/

		isAttackingNode.SetLeft(didPlayerAttack);
		isAttackingNode.SetRight(AIOnGroundNode);

		didPlayerAttack.SetLeft (actIdleNode );
		didPlayerAttack.SetRight (AIOnGroundNode);

		AIOnGroundNode.SetRight (tryGetUp);
		AIOnGroundNode.SetLeft (playerOnGroundNode);

		playerOnGroundNode.SetRight (willSubmitNode);
		playerOnGroundNode.SetLeft (willAttackNode);

		willSubmitNode.SetRight (Submit);
		willSubmitNode.SetLeft (actAttackNode);

		willAttackNode.SetRight (Grab);
		willAttackNode.SetLeft (actAttackNode);

		actAttackNode.SetRight (Counter);
		actAttackNode.SetLeft (Strike);

		root = isAttackingNode;
	}
}


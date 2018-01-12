using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStuff : MonoBehaviour {

	Animator anim;
	int animStart = Animator.StringToHash("Small_Guy_1_animation");
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if(Input.GetKey(KeyCode.E) && stateInfo.fullPathHash != animStart){
			anim.SetTrigger(animStart);
		}
	}
}

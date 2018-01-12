using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectCharacter : MonoBehaviour {

	public GameObject spotlight;
	public Canvas canvas;
	CharacterSelection cs;
	bool selectedCharacter;

	// Use this for initialization
	void Start () {
		cs = FindObjectOfType<CharacterSelection> ();
		canvas.gameObject.SetActive (false);
		cs.setCharacterSelection (false);
	}
	
	// Update is called once per frame
	void Update () {

	} 
		
	void OnMouseDown(){
		if (!cs.getCharacterSelection()) {
			for (int i = 0; i < cs.characterList.Length; i++) {
				if (cs.characterList [i].gameObject.name == this.gameObject.name) {
					cs.setIndex (i);
					canvas.gameObject.SetActive (true);
					cs.setCharacterSelection (true);
					Debug.Log (i);
				}
			}

			GameObject[] lights = GameObject.FindGameObjectsWithTag ("SelectionLight");
			foreach (GameObject light in lights) {
				light.GetComponent<Light> ().enabled = false;
			}
			spotlight.GetComponent<Light> ().enabled = true;
		}
	}

	public void TurnOffSelectionPrompt(){
		canvas.gameObject.SetActive (false);
		cs.setCharacterSelection (false);
	}
}

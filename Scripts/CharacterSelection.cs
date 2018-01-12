using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	public GameObject[] characterList;
	int index;
	bool characterChosen;

	// Use this for initialization
	void Start () {
		index = PlayerPrefs.GetInt ("CharacterSelected");

		characterList = new GameObject[transform.childCount];

		//Fill the array with our models
		for (int i = 0; i < transform.childCount; i++) {
			characterList [i] = transform.GetChild (i).gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ConfirmCharacter(){
		PlayerPrefs.SetInt ("CharacterSelected", index);
		SceneManager.LoadScene ("MMA May82017");
	}

	public int getIndex(){
		return index;
	}

	public void setIndex(int newIndex){
		index = newIndex;
	}

	public bool getCharacterSelection(){
		return characterChosen;
	}

	public void setCharacterSelection(bool selected){
		characterChosen = selected;
	}
}

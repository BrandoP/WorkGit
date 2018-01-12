using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {


	public Button start;
	public Button quit;

	// Use this for initialization
	void Start () {
		start = start.GetComponent<Button> ();
		quit = quit.GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadLevel(){
		start.enabled = false;
		quit.enabled = false;
		SceneManager.LoadScene ("MMA Selection", LoadSceneMode.Single);
		Scene scene = SceneManager.GetSceneByName ("MMA Selection");
		SceneManager.SetActiveScene (scene);
	}

	public void quitLevel(){
		Application.Quit ();
	}
}

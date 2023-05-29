using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class logoAnimation : MonoBehaviour {
	public GameObject loader;
	public void loadMainMenu(){
		loader.SetActive (true);
		SceneManager.LoadScene ("MainMenu");

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (fadeinmanager ());	
	}
	IEnumerator fadeinmanager(){
		yield return new WaitForSeconds (1f);
		gameObject.SetActive (false);
	}
	

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateManager : MonoBehaviour {

	public bool isAvailable;
	void Start(){
		isAvailable = true;
	}

	public void shouldStartCountDown(){
		StartCoroutine (healer ());
	}

	IEnumerator healer(){
		yield return new WaitForSeconds (6f);
		isAvailable = true;
	}
}

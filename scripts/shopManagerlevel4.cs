using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopManagerlevel4 : MonoBehaviour {

	public static bool isILSRunway1On = false;
	public static bool isILSRunway2On = false;
	GameObject ils1;

	GameObject signal1;

	private bool isTimerOn;
	Color fade;
	float time;
	void Start(){
		ils1 = GameObject.FindGameObjectWithTag ("ILSRun1");
		signal1 = GameObject.FindGameObjectWithTag ("signal1");
		deactivator ();
		isTimerOn = false;
	}
	public void ilsPurchase(){
		if (!isTimerOn) {
			ils1.GetComponent<Renderer> ().sortingOrder = 0;

			signal1.GetComponent<Renderer> ().sortingOrder = 0;

			isILSRunway1On = true;
			isILSRunway2On = true;
			time = 15;
			isTimerOn = true;
		}

	}
	void Update(){
		if (isTimerOn) {
			time -= Time.deltaTime;
			if (time <= 0) {
				deactivator ();
				isTimerOn = false;
			}
		}	
	}
	void deactivator(){
		ils1.GetComponent<Renderer>().sortingOrder = -4;

		signal1.GetComponent<Renderer>().sortingOrder = -4;
	
		isILSRunway1On = false;
		isILSRunway2On = false;


	}
}

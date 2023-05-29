using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopManager : MonoBehaviour {
	public static bool isILSRunway1On = false;
	public static bool isILSRunway2On = false;
	private Image ILSButtonSprite;
	public Sprite ILSOnImage;
	public Sprite ILSOffImage;
	public GameObject ILSButton;
	GameObject ils1;
	GameObject ils2;
	private int noOfTimes;
	GameObject signal1;
	GameObject signal2;
	private bool isTimerOn;
	public GameObject notEnoughCoins;
	public GameObject ILS1Anim;
	public GameObject ILS2Anim;
	Color fade;
	float time;
	void Start(){
		noOfTimes = 0;
		ils1 = GameObject.FindGameObjectWithTag ("ILSRun1");
		ils2 = GameObject.FindGameObjectWithTag ("ILSRun2");
		ILSButtonSprite = ILSButton.GetComponent<Image>();
		//signal1 = GameObject.FindGameObjectWithTag ("signal1");
		//signal2 = GameObject.FindGameObjectWithTag ("signal2");
		deactivator ();
		isTimerOn = false;
	}
	public void ilsPurchase(){
		saveData.loadCoins ();
		if (!isTimerOn) {
			if (noOfTimes < 2 || saveData.c.getIsILSPurchased()) {
				noOfTimes++;
				//ils1.GetComponent<Renderer> ().sortingOrder = 0;
				//ils2.GetComponent<Renderer> ().sortingOrder = 0;
				//signal1.GetComponent<Renderer> ().sortingOrder = 0;
				//signal2.GetComponent<Renderer> ().sortingOrder = 0;
				isILSRunway1On = true;
				isILSRunway2On = true;
				ILS1Anim.SetActive(true);
				ILS2Anim.SetActive(true);
				ILSButtonSprite.sprite = ILSOnImage;
				saveData.loadCoins ();
				saveData.c.setCoins (saveData.c.getCoins () - 5);
				saveData.saveCoins ();
				time = 15;
				isTimerOn = true;
			} else {
				notEnoughCoins.GetComponent<Animator> ().SetTrigger ("nocoins");
			}
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
		//ils1.GetComponent<Renderer>().sortingOrder = -8;
		//ils2.GetComponent<Renderer>().sortingOrder = -8;
		//signal1.GetComponent<Renderer>().sortingOrder = -8;
		//signal2.GetComponent<Renderer>().sortingOrder = -8;
		isILSRunway1On = false;
		isILSRunway2On = false;
		ILS1Anim.SetActive(false);
		ILS2Anim.SetActive(false);
		ILSButtonSprite.sprite = ILSOffImage;

	}

}

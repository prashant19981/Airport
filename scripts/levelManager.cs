using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class levelManager : MonoBehaviour {
	public Text coins;
	public Sprite[] spritess = new Sprite[4];
	public Transform panel;
	private int index;
	public GameObject notEnoughCoinsPanel;
	private bool isPopUp;
	levelinfo le ;
	public GameObject watchADBUTTON;
	public GameObject fadeOut;
	public GameObject fadeIn;
	public GameObject buyThisLevel;
	public Image levelMoney;
	GameObject currentPopUp;
	 long currentTime ;
	long nextAdTime ;
	private bool canWatchAd;
	Color32 buttonColor;
	long current;
	long delay = 2;
     int levelToBeBought;
	public static int level2AD;
	public static int level3AD;
	public static int level4AD;
	public static int level5AD;
	// Use this for initialization
	void Start () {
		level2AD = 0;
		level3AD = 0;
		level4AD = 0;
		level5AD = 0;
		StartCoroutine (fadeInScene ());
		isPopUp = false;
		saveData.loadCoins ();
		saveData.loadlevelInfo ();

		for (int i = 0; i < 3; i++) {
			print (saveData.l [i].getStatus ());
			if (saveData.l[i].getStatus()) {
				panel.GetChild (i + 1).GetChild (0).gameObject.SetActive (false);
				panel.GetChild (i + 1).GetChild (1).gameObject.SetActive (false);
				panel.GetChild (i + 1).GetChild (2).gameObject.SetActive (false);
			
			} else {
				panel.GetChild (i + 1).GetChild (0).gameObject.SetActive (true);
				panel.GetChild (i + 1).GetChild (1).gameObject.SetActive (true);
				panel.GetChild (i + 1).GetChild (2).gameObject.SetActive (true);
			}
		
		}

	}
	void Update(){
		saveData.loadCoins ();
		coins.text = saveData.c.getCoins ().ToString ();
		if (PlayerPrefs.GetInt ("inter", 0) >= 1) {
			saveData.loadCoins ();
			canWatchAd = false;
			fadeButtonColor ();
			currentTime = DateTime.Now.Ticks;
			nextAdTime = saveData.c.getTimer ();
			if (nextAdTime - currentTime < 0) {
				canWatchAd = true;
				PlayerPrefs.SetInt ("inter", 0);
				normalizeColor ();
			}
		} else {
			normalizeColor ();
			canWatchAd = true;
		}
		if (Input.GetKey (KeyCode.Escape) && !isPopUp) {
			StartCoroutine (loadMainMenu ());
		}
		if (level2AD >= 1) {
			level2AD = 0;
			StartCoroutine (loadScene (3));
		} else if (level3AD >= 1) {
			level3AD = 0;
			StartCoroutine (loadScene (4));
		} else if (level4AD >= 2) {
			level4AD = 0;
			StartCoroutine (loadScene (5));
		} else if (level5AD >= 2) {
			level5AD = 0;
			StartCoroutine (loadScene (1));
		}
	}
	public void openLevel1(){
		if (!isPopUp) {
			StartCoroutine (loadScene (1));
		}
	}
	public void openLevel2(){
		if (!isPopUp) {
			saveData.loadCoins ();
			if (saveData.l [0].getStatus ()) {
				StartCoroutine (loadScene (2));
		  
			} else if (saveData.c.getCoins () >= 500) {
				levelToBeBought = 0;
				buyThisLevel.SetActive (true);
				levelMoney.sprite = spritess [0];
				currentPopUp = buyThisLevel;
				openPopUp ();

			} else if (saveData.c.getCoins () < 500) {
				notEnoughCoinsPanel.SetActive (true);
				currentPopUp = notEnoughCoinsPanel;
				openPopUp ();
			}
		}
		
	
	}
	public void openLevel3(){
		saveData.loadCoins ();
		if (saveData.l[1].getStatus()) {
			StartCoroutine (loadScene (3));
		} else if (saveData.c.getCoins () >= 1000) {
			levelToBeBought = 1;
			buyThisLevel.SetActive (true);
			levelMoney.sprite = spritess [1];
			currentPopUp = buyThisLevel;
			openPopUp ();
		
			//display buy option
		} else if(saveData.c.getCoins() <1000) {
			notEnoughCoinsPanel.SetActive (true);
			currentPopUp = notEnoughCoinsPanel;
			openPopUp ();
		}



	}
	public void openLevel4(){
		saveData.loadCoins ();
		if (saveData.l[2].getStatus()) {
			StartCoroutine (loadScene (4));
		} else if (saveData.c.getCoins () >= 2000) {
			levelToBeBought = 2;

			buyThisLevel.SetActive (true);
			levelMoney.sprite = spritess [2];
			currentPopUp = buyThisLevel;
			openPopUp ();
			//display buy option
		} else if(saveData.c.getCoins() <2000) {
			notEnoughCoinsPanel.SetActive (true);
			currentPopUp = notEnoughCoinsPanel;
			openPopUp ();
		}



	}

	public void openLevel5(){
		saveData.loadCoins ();
		if (saveData.l[3].getStatus()) {
			StartCoroutine (loadScene (1));

		} else if (saveData.c.getCoins () >= 4000) {
			levelToBeBought = 3;
			buyThisLevel.SetActive (true);
			levelMoney.sprite = spritess [3];
			currentPopUp = buyThisLevel;
			openPopUp ();

			//display buy option
		} else if(saveData.c.getCoins() <4000) {
			notEnoughCoinsPanel.SetActive (true);
			currentPopUp = notEnoughCoinsPanel;
			openPopUp ();
		}



	}
	IEnumerator loadMainMenu(){
		fadeOut.SetActive (true);
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene ("MainMenu");

	}

	IEnumerator loadScene(int index){
		fadeOut.SetActive (true);
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene (""+index);

	}
	IEnumerator fadeInScene(){
		yield return new WaitForSeconds (1f);
		fadeIn.SetActive (false);
	}
	public void closePopUp(){
		currentPopUp.GetComponent<Animator> ().speed = -1f;
		currentPopUp.GetComponent<Animator> ().SetTrigger ("closeShop");
		isPopUp = false;
	}
	void openPopUp(){
		currentPopUp.GetComponent<Animator> ().speed = 1f;
		currentPopUp.GetComponent<Animator> ().SetTrigger ("openShop");
		isPopUp = true;
	
	}
	public void levelBought(){
		closePopUp ();
		saveData.loadCoins ();
		saveData.loadlevelInfo ();
		if (levelToBeBought == 0) {
			print ("HEY");
			saveData.c.setCoins (saveData.c.getCoins () - 500);
			saveData.l [0].setStatus (true);
			saveData.saveLevelInfo ();
		}
		else if(levelToBeBought == 1){
			print ("HEY");
			saveData.c.setCoins (saveData.c.getCoins () - 1000);
			saveData.l [1].setStatus (true);
		}
		else if(levelToBeBought == 2){
			print ("HEY");
			saveData.c.setCoins (saveData.c.getCoins () - 2000);
			saveData.l [2].setStatus (true);
		}
		else if(levelToBeBought == 3){
			print ("HEY");
			saveData.c.setCoins (saveData.c.getCoins () - 4000);
			saveData.l [3].setStatus (true);
		}

		saveData.saveCoins ();
    	saveData.saveLevelInfo ();
		coins.text = saveData.c.getCoins ().ToString ();
		panel.GetChild (levelToBeBought+ 1).GetChild (0).gameObject.SetActive (false);
		panel.GetChild (levelToBeBought + 1).GetChild (1).gameObject.SetActive (false);
		panel.GetChild (levelToBeBought + 1).GetChild (2).gameObject.SetActive (false);
	}
	public void watchAd(){
		if (canWatchAd) {
			PlayerPrefs.SetInt ("inter", PlayerPrefs.GetInt ("inter", 0) + 1);
			adsController.instance.caseIs = 0;
			adsController.instance.ShowAd();
			closePopUp ();

		}
		if (PlayerPrefs.GetInt ("inter", 0) >= 1) {
			current = DateTime.Now.AddMinutes(delay).Ticks;
			saveData.loadCoins ();
			saveData.c.setTimer (current);
			saveData.saveCoins ();
			canWatchAd = false;
			fadeButtonColor ();
		}
	}

	void normalizeColor(){
		buttonColor = new Color32 (255, 255, 255, 255);
		watchADBUTTON.GetComponent<Image> ().color = buttonColor;
	}
	void fadeButtonColor(){
		buttonColor = new Color32 (148, 117, 117, 255);
		watchADBUTTON.GetComponent<Image> ().color = buttonColor;
	}
	public void buy5000(){
		IAPManager.instance.Buy5000 ();
	}
	public void buy10000(){
		IAPManager.instance.Buy10000 ();
	}
	public void allLevels(){
		IAPManager.instance.BuyAllLevels ();
	}
	public void watchAdLevel2(){
		adsController.instance.caseIs = 2;
		adsController.instance.ShowAd ();
	}
	public void watchAdLevel3(){
		adsController.instance.caseIs = 3;
		adsController.instance.ShowAd ();
	}
	public void watchAdLevel4(){
		adsController.instance.caseIs = 4;
		adsController.instance.ShowAd ();
	}
	public void watchAdLevel5(){
		adsController.instance.caseIs = 5;
		adsController.instance.ShowAd ();
	}
	public void backButton(){
		StartCoroutine(loadMainMenu());
	}
}

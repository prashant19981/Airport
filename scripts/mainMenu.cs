using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class mainMenu : MonoBehaviour {
	public Sprite soundOn;
	public Sprite soundOff;
	public Sprite musicOn;
	public Sprite musicOff;
	Image sound;
	Image music;
	public GameObject sprite;
	public GameObject sprite2;
	public GameObject shop;
	public GameObject privacyPanel;
	private bool isPopUpOpen;
	public GameObject fadeOut;
	public GameObject watchADBUTTON;
	TimeSpan timespan;
	long current ;
	long  delay = 2;
	long nextAdTime;
	long currentTime;
	levelinfo le ;
	private bool canWatchAd;
	Color32 buttonColor;
	void Start(){
		//REMOVE THIS
		//saveData.loadCoins();
		//saveData.c.setCoins (100000000);
		//saveData.saveCoins ();
		//StartCoroutine (showStartShop ());
		canWatchAd = true;
		sound = sprite.GetComponent<Image> ();
		music = sprite2.GetComponent<Image> ();
		isPopUpOpen = false;
		if (PlayerPrefs.GetInt ("First", 0) == 0 ) {
			PlayerPrefs.SetInt ("First", 1);

			saveData.loadCoins ();
			saveData.c.setCoins (0);
			
			saveData.c.setAdsPurchase (false);
			for (int i = 0; i < 4; i++) {
				le = new levelinfo (false);
				saveData.l.Add (le);
				saveData.l [i] = le;
			}
			saveData.saveCoins ();
			saveData.saveLevelInfo ();

		}
		if (PlayerPrefs.GetInt ("4crash", 0) == 0) {
			PlayerPrefs.SetInt ("4crash", 1);
			saveData.loadCoins ();
			saveData.c.setIs4crashPurchased (false);
			saveData.c.setIsILSPurchased (false);
			saveData.saveCoins ();
		}
		if (PlayerPrefs.GetInt ("sound", 0) == 1) {
			sound.sprite = soundOn;

		} else {
			sound.sprite = soundOff;

		}

		if (PlayerPrefs.GetInt ("sound", 0) == 0) {
			GetComponent<AudioSource> ().Stop ();

		} else {
			GetComponent<AudioSource> ().Play ();
		}
		if (PlayerPrefs.GetInt ("music", 1) == 1) {
			music.sprite = musicOn;

		} else {
			music.sprite = musicOff;

		}

	}
	void Update(){
		print (nextAdTime - currentTime);
		if (PlayerPrefs.GetInt ("inter", 0) >= 1) {
			saveData.loadCoins ();
			fadeButtonColor ();
			canWatchAd = false;
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
		if (Input.GetKey (KeyCode.Escape) && !isPopUpOpen) {
			Application.Quit ();
		}

	}
	public void sounds(){
		//print("soundsss");
		if (PlayerPrefs.GetInt ("sound", 0) == 0) {
			sound.sprite = soundOn;
			GetComponent<AudioSource> ().Play ();
			PlayerPrefs.SetInt ("sound", 1);
		} else {
			sound.sprite = soundOff;
			GetComponent<AudioSource> ().Stop ();
			PlayerPrefs.SetInt ("sound", 0);
		}
	
	}
	public void musics(){
		if (PlayerPrefs.GetInt ("music", 1) == 0) {
			music.sprite = musicOn;
			PlayerPrefs.SetInt ("music", 1);
		} else {
			music.sprite = musicOff;
			PlayerPrefs.SetInt ("music", 0);
		}

	}
	public void shopMenu(){
		shop.SetActive (true);
		isPopUpOpen = true;

	}
	public void closeShop(){
//		shop.GetComponent<Animator> ().speed = -1f;
//		shop.GetComponent<Animator> ().Play ("nm");
//		shop.GetComponent<Animator> ().speed = 1f;
		//shop.GetComponent<Animator>().SetTrigger("noshopping");
		shop.SetActive (false);

		isPopUpOpen = false;
	}
	public void playGame(){
		if (!isPopUpOpen) {
			StartCoroutine (loadScene ());


		}

	}
	public void openPrivacyPanel(){
		privacyPanel.SetActive (true);
		isPopUpOpen = true;
	}
	public void closePrivacyPanel(){
		//privacyPanel.GetComponent<Animator> ().speed = -1f;
		//privacyPanel.GetComponent<Animator> ().Play ("nm");
		//privacyPanel.GetComponent<Animator> ().speed = 1f;
		privacyPanel.SetActive (false);
		isPopUpOpen = false;
	}
	IEnumerator loadScene(){
		fadeOut.SetActive (true);
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene ("LevelPage");
	
	}
	public void watchAd(){
		if (canWatchAd) {
			PlayerPrefs.SetInt ("inter", PlayerPrefs.GetInt ("inter", 0) + 1);
			adsController.instance.caseIs = 0;
			adsController.instance.ShowAd();
			closeShop ();

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
	public void buyNoAds(){
		IAPManager.instance.Buynoads ();
	}
	public void Crash4(){
		IAPManager.instance.Buy4crash ();
	}
	public void unlimitedILS(){
		IAPManager.instance.BuyUnlimitedILS ();
	}
	public void allLevels(){
		IAPManager.instance.BuyAllLevels ();
	}
	public void rateUs(){
		Application.OpenURL ("market://details?id=com.psrsgames.airport");
	}
	public void leaderBoardopener(){
		leaderboard.instances.openLeaderBoard ();
	}
	public void viewPrivacyPolicy(){
		Application.OpenURL ("https://sites.google.com/view/psrsgamesprivacypolicy/home");
	}
	IEnumerator showStartShop(){
		yield return new WaitForSeconds (1f);
		shop.SetActive (true);
		//shop.GetComponent<Animator> ().SetTrigger ("shopping");
		isPopUpOpen = true;
	}
}

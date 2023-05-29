 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game : MonoBehaviour {
	private const string leaderBoardField = "";
	private const string leaderBoardJetcity = "";
	private const string leaderBoardNightisland = "";
	private const string leaderBoardCarrier = "";
	private const string leaderBoardDesertStorm = "";
	private string leaderBoardstring = null;
	public Sprite fastForwardSpeed;
	public Sprite normalSpeed;
	Image fastForwardButton;
	public GameObject ffButton;
	public List<GameObject> planes1; 
	public List<GameObject> planes2;
	public List<GameObject> planes3;
	public GameObject finalGameOver;
	List<GameObject> instantiatePoints = new List<GameObject> ();
	GameObject score;
	public GameObject gameOverPanel;
	public static bool isRewardVideoComplete;
	Text count;
	private bool already;
	private bool alreadygameover;
	Text coins;
	float counter;
	public GameObject fadeOut;
	List<GameObject> totalPlanes1;
	List<GameObject> totalPlanes2;
	List<GameObject> totalPlanes3;
//	public GameObject explosion1;
//	public GameObject explosion2;
	private int previous;
	private GameObject flag;
	private bool flagControlEnable;
	private float screenHeight;
	private float screenWidth;
	int index;
	public BoxCollider2D border;
	int noOfPlanes=1;
	string levelIndex;
	private bool isPaused;
	private bool exp;
	public GameObject pausePanel;
	public static bool playerOut;
	private bool interstitialAds;
	private bool isAdShownAlready;
	public GameObject bestScore;
	public Text bestScores;
	public Text bestScore1;
	public Text score1;
	private int crashCounter;
	int counterVar=1;
	private bool fastForwardCheck;
	private Vector3 temp;
	void Start(){
		saveData.loadCoins ();
		crashCounter = 0;
		//Time.timeScale = 3f;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		alreadygameover = false;
		levelDeterminer ();
		already = false;
		isAdShownAlready = false;
		interstitialAds = false;
		isRewardVideoComplete = false;
		isPaused = false;
		Time.timeScale = 1f;
		fastForwardCheck = false;
		playerOut = false;
		counter = 10f;
		coins = GameObject.FindGameObjectWithTag ("coins").GetComponent<Text> ();
		PlayerPrefs.SetInt ("count", 0);
		score = GameObject.FindGameObjectWithTag ("count");
		count = score.GetComponent<Text> ();
		flagControlEnable = false;
		exp = false;
		previous = -1;
		flag = GameObject.FindGameObjectWithTag ("Flag");
		GameObject obj = GameObject.FindGameObjectWithTag ("instantiatepoint");
		for (int i = 0; i < obj.transform.childCount; i++) {
			instantiatePoints.Add(obj.transform.GetChild (i).gameObject);
			//call points adjuster
			instantiatePointAdjuster(obj.transform.GetChild(i));
		
		}
		totalPlanes1 = new List<GameObject> ();
		totalPlanes2 = new List<GameObject> ();
		totalPlanes3 = new List<GameObject> ();
		borderAdjuster();
		spawner ();
		fastForwardButton = ffButton.GetComponent<Image>();
		StartCoroutine (flagControl ());
		if (PlayerPrefs.GetInt ("sound", 0) == 0) {
			GetComponent<AudioSource> ().Stop ();

		} else {
			GetComponent<AudioSource> ().Play ();
		}

	}
//	void Start(){
//		
//	}
	IEnumerator gameManager(){
		for (int i = 0; i < noOfPlanes; i++) {
			int val = (int)UnityEngine.Random.Range (0f, 10f);
			if (val > 6) {
				counterVar = 1;
			} else if (val > 3) {
				counterVar = 2;
			} else {
				counterVar = 3; 
			}
			if (counterVar == 1) {
				if (totalPlanes1 != null) {
					//for (int i = 0; i < noOfPlanes; i++) {
					index = UnityEngine.Random.Range (0, totalPlanes1.Count);
					if (index != previous) {
						if (!totalPlanes1 [index].GetComponent<insidePlane> ().getIsActive ()) {
							int x = (int)UnityEngine.Random.Range (0, 8);
							if (instantiatePoints [x].GetComponent<instantiateManager> ().isAvailable) {
								instantiatePoints [x].GetComponent<instantiateManager> ().isAvailable = false;
								instantiatePoints [x].GetComponent<instantiateManager> ().shouldStartCountDown ();
								totalPlanes1 [index].transform.position = instantiatePoints [x].transform.position;
								totalPlanes1 [index].transform.rotation = instantiatePoints [x].transform.rotation; 
								totalPlanes1 [index].SetActive (true);
								totalPlanes1 [index].GetComponent<insidePlane> ().setIsActive (true);
								//totalPlanes.RemoveAt (index);
								previous = index;
								yield return new WaitForSeconds (counter);
								if (counter > 4f) {
									counter -= 0.2f;
								}
							}
						}
					}

					//}

				}//counterVar = 2;
			} else if (counterVar == 2) {
				if (totalPlanes2 != null) {
					//for (int i = 0; i < noOfPlanes; i++) {
					index = UnityEngine.Random.Range (0, totalPlanes2.Count);
					if (index != previous) {
						if (!totalPlanes2 [index].GetComponent<insidePlane> ().getIsActive ()) {
							int x = (int)UnityEngine.Random.Range (0, 8);
							if (instantiatePoints [x].GetComponent<instantiateManager> ().isAvailable) {
								instantiatePoints [x].GetComponent<instantiateManager> ().isAvailable = false;
								instantiatePoints [x].GetComponent<instantiateManager> ().shouldStartCountDown ();
								totalPlanes2 [index].transform.position = instantiatePoints [x].transform.position;
								totalPlanes2 [index].transform.rotation = instantiatePoints [x].transform.rotation; 
								totalPlanes2 [index].SetActive (true);
								totalPlanes2 [index].GetComponent<insidePlane> ().setIsActive (true);
								//totalPlanes.RemoveAt (index);
								previous = index;
								yield return new WaitForSeconds (counter);
								if (counter > 4f) {
									counter -= 0.2f;
								}
							}
						}
					}

					//}
				}
				//counterVar = 3;
			} else if (counterVar == 3) {
				if (totalPlanes3 != null) {
					//for (int i = 0; i < noOfPlanes; i++) {
					index = UnityEngine.Random.Range (0, totalPlanes3.Count);
					if (index != previous) {
						if (!totalPlanes3 [index].GetComponent<insidePlane> ().getIsActive ()) {
							int x = (int)UnityEngine.Random.Range (0, 8);
							if (instantiatePoints [x].GetComponent<instantiateManager> ().isAvailable) {
								instantiatePoints [x].GetComponent<instantiateManager> ().isAvailable = false;
								instantiatePoints [x].GetComponent<instantiateManager> ().shouldStartCountDown ();
								totalPlanes3 [index].transform.position = instantiatePoints [x].transform.position;
								totalPlanes3 [index].transform.rotation = instantiatePoints [x].transform.rotation; 
								totalPlanes3 [index].SetActive (true);
								totalPlanes3 [index].GetComponent<insidePlane> ().setIsActive (true);
								//totalPlanes.RemoveAt (index);
								previous = index;
								yield return new WaitForSeconds (counter);
								if (counter > 4f) {
									counter -= 0.2f;
								}
							}
						}
					}

					//}
				}
				//counterVar = 1;
			}
		}
			yield return new WaitForSeconds (1f);
		if (PlayerPrefs.GetInt ("count", 0) > 3) {
		   
			noOfPlanes = 1;
		} else if (PlayerPrefs.GetInt ("count", 0) > 7) {
			noOfPlanes = 3;
		} else if (PlayerPrefs.GetInt ("count", 0) > 14) {
			noOfPlanes = 4;
		} 
			StartCoroutine (gameManager ());

//		else {
//			spawner ();
//		}
	}
	void Update(){
		print ("Crash Counter " + saveData.c.getIs4CrashPurchased()); 
		if (flagControlEnable) {
			float random = UnityEngine.Random.Range (0, 180f);
			//flag.transform.rotation = Quaternion.Lerp (flag.transform.rotation, Quaternion.Euler (0, 0, random), Time.deltaTime * 3f);
		
		
		}
		count.text = PlayerPrefs.GetInt ("count", 0).ToString();
		saveData.loadCoins ();
		coins.text = saveData.c.getCoins ().ToString ();
		if (playerOut && !alreadygameover) {
			alreadygameover = true;
			if (saveData.c.getIs4CrashPurchased () && crashCounter < 4) {
				playerOut = false;
				crashCounter++;
				StartCoroutine (counterCrash ());

			} else {
				gameOver ();
				if (isRewardVideoComplete) {
					playerOut = false;
					isRewardVideoComplete = false;
					continueAfterAds ();
					isAdShownAlready = true;
				}
			}
		}
		print ("Level is" + levelIndex);
		bestScores.text = PlayerPrefs.GetInt ("count", 0).ToString ();

	}
	public void explodeFun(Vector3 pos1,Vector3 pos2){
		if (!exp) {
			exp = true;
//			explosion1.transform.position = pos1;
//			explosion2.transform.position = pos2;
		}
	}
	IEnumerator flagControl(){
		flagControlEnable = true;
		yield return new WaitForSeconds (1f);
		flagControlEnable = false;
		yield return new WaitForSeconds (5f);
		StartCoroutine (flagControl ());
	}
	void spawner(){
		for (int j = 0; j < 2; j++) {
			for (int i = 0; i < planes1.Count; i++) {
				index = UnityEngine.Random.Range (0, 8);
				print (index);

				GameObject	obj = Instantiate (planes1 [i], instantiatePoints [index].transform.position, instantiatePoints [index].transform.rotation);
				obj.SetActive (false);
				
				totalPlanes1.Add (obj);

			}
			for (int i = 0; i < planes2.Count; i++) {
				index = UnityEngine.Random.Range (0, 8);
				print (index);
				GameObject	obj = Instantiate (planes2 [i], instantiatePoints [index].transform.position, instantiatePoints [index].transform.rotation);
				obj.SetActive (false);

				totalPlanes2.Add (obj);

			}
			for (int i = 0; i < planes3.Count; i++) {
				index = UnityEngine.Random.Range (0, 8);
				print (index);
				GameObject	obj = Instantiate (planes3 [i], instantiatePoints [index].transform.position, instantiatePoints [index].transform.rotation);
				obj.SetActive (false);

				totalPlanes3.Add (obj);

			}
		}
		StartCoroutine (gameManager ());
	}
	public void coinUpdater(){
		saveData.loadCoins ();
		saveData.c.setCoins (saveData.c.getCoins () + 5);
		saveData.saveCoins ();
	}
	void gameOver(){
		print ("GameOver");
		saveData.loadCoins ();

			if (!interstitialAds && !saveData.c.getAdsPurchase ()) {
				interstitialAds = true;
				PlayerPrefs.SetInt ("ads", PlayerPrefs.GetInt ("ads", 0) + 1);
				if (PlayerPrefs.GetInt ("ads", 0) >= 2) {
					PlayerPrefs.SetInt ("ads", 0);
					adsController.instance.showInterStetial ();
				}
			}
			if (!isAdShownAlready) {
				gameOverPanel.SetActive (true);
				Time.timeScale = 0.1f;
				gameOverPanel.GetComponent<Animator> ().SetTrigger ("gameover");
				Time.timeScale = 0f;
			} else if (isAdShownAlready && !already) {
				if (PlayerPrefs.GetInt ("count", 0) <= PlayerPrefs.GetInt (levelIndex, 0)) {
					already = true;
					finalGameOver.SetActive (true);
					bestScore1.text = "Best:" + PlayerPrefs.GetInt (levelIndex, 0).ToString ();
					score1.text = "Score:" + PlayerPrefs.GetInt ("count", 0).ToString ();
					finalGameOver.GetComponent<Animator> ().SetTrigger ("gameover");
					Time.timeScale = 0;
				} else {
					leaderboard.instances.reportScore (PlayerPrefs.GetInt ("count", 0), leaderBoardstring);
					already = true;
					PlayerPrefs.SetInt (levelIndex, PlayerPrefs.GetInt ("count", 0));
					bestScore.SetActive (true);
					bestScore.GetComponent<Animator> ().SetTrigger ("gameover");
					print ("KKKJJJ");

					Time.timeScale = 0;
				}
				
			}
		}
	
	public void skip(){
		Time.timeScale = 1f;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
	public void newSkip(){
		already = true;
		isAdShownAlready = true;
		gameOverPanel.SetActive (false);
		if (PlayerPrefs.GetInt ("count", 0) <= PlayerPrefs.GetInt (levelIndex, 0)) {
			
			finalGameOver.SetActive (true);
			bestScore1.text = "Best:" + PlayerPrefs.GetInt (levelIndex, 0).ToString();
			score1.text = "Score:" + PlayerPrefs.GetInt ("count", 0).ToString();
			finalGameOver.GetComponent<Animator> ().SetTrigger ("gameover");
			Time.timeScale = 0;
		}
		else {
			leaderboard.instances.reportScore (PlayerPrefs.GetInt ("count", 0), leaderBoardstring);
			PlayerPrefs.SetInt (levelIndex, PlayerPrefs.GetInt ("count", 0));
			bestScore.SetActive (true);
			bestScore.GetComponent<Animator> ().SetTrigger ("gameover");

			Time.timeScale = 0;
		}
	
	}
	public void pauseGame(){
		isPaused = true;
		pausePanel.GetComponent<Animator> ().speed = 1f;
		pausePanel.SetActive (true);
		pausePanel.GetComponent<Animator> ().SetTrigger ("openShop");
		Time.timeScale = 0;
	}
	public void watchAd(){
		adsController.instance.caseIs = 1;
		adsController.instance.ShowAd();
	}
	public void continuePlay(){
		Time.timeScale = 1f;
		pausePanel.GetComponent<Animator> ().speed = -1f;
		pausePanel.GetComponent<Animator> ().SetTrigger ("closeShop");


	}
	void continueAfterAds(){
		Time.timeScale = 1f;
		gameOverPanel.SetActive (false);
	}
	public void mainMenuButton(){
		Time.timeScale = 1f;
		StartCoroutine (loadScene ());
	}
	public void fastForward()
    {
		if (!fastForwardCheck)
        {
			Time.timeScale = 4f;
			fastForwardCheck = true;
			fastForwardButton.sprite = normalSpeed;
        }
		else {
			Time.timeScale = 1f;
			fastForwardCheck = false;
			fastForwardButton.sprite = fastForwardSpeed;

		}

    }
	IEnumerator loadScene(){
		fadeOut.SetActive (true);
		Time.timeScale = 1f;
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene ("LevelPage");

	}
IEnumerator counterCrash(){
	yield return new WaitForSeconds(2f);
		alreadygameover = false;

}
	void levelDeterminer(){
		if (SceneManager.GetActiveScene ().name == "1") {
			levelIndex = "1";
			leaderBoardstring = leaderBoardField;
		} else if (SceneManager.GetActiveScene ().name == "2") {
			levelIndex = "2";
			leaderBoardstring = leaderBoardJetcity;
		}
		else if (SceneManager.GetActiveScene ().name == "3") {
			levelIndex = "3";
			leaderBoardstring = leaderBoardNightisland;
		}
		else if (SceneManager.GetActiveScene ().name == "4") {
			levelIndex = "4";
			leaderBoardstring = leaderBoardCarrier;
		}
		else if (SceneManager.GetActiveScene ().name == "5") {
			levelIndex = "5";
			leaderBoardstring = leaderBoardDesertStorm;
		}

	}
	void instantiatePointAdjuster(Transform point)
    {//2.18f
		float offset = 1.3f;
		Vector3 worldPos;
		Vector3 originPos;
		Vector3 pos = Camera.main.WorldToScreenPoint(point.position);
		originPos = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.position.z));
		worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, Camera.main.transform.position.z));
		if (pos.x > screenWidth && pos.y < screenHeight && pos.y > 0)
		{
			temp = new Vector3(worldPos.x + offset, point.position.y, point.position.z);
			point.position = temp;
		}
		else if (pos.x < screenWidth && pos.y > screenHeight && pos.x > 0)
		{
			temp = new Vector3(point.position.x, worldPos.y + offset, point.position.z);
			point.position = temp;
		}
		else if (pos.x < 0 && pos.y < screenHeight && pos.y > 0)
		{
			temp = new Vector3(originPos.x - offset, point.position.y, point.position.z);
			point.position = temp;
			
		}
		else if (pos.x < screenWidth && pos.y < 0 && pos.x > 0)
		{
			temp = new Vector3(point.position.x, originPos.y - offset, point.position.z);
			point.position = temp;
		}
		else if (pos.x > screenWidth && pos.y > screenHeight)
		{
			temp = new Vector3(worldPos.x + offset, worldPos.y + offset, point.position.z);
			point.position = temp;
		}
		else if (pos.x < 0 && pos.y > screenHeight)
		{
			temp = new Vector3(originPos.x - offset, worldPos.y + offset, point.position.z);
			point.position = temp;

		}
		else if (pos.x < 0 && pos.y < 0 )
		{
			temp = new Vector3(originPos.x - offset, originPos.y - offset, point.position.z);
			point.position = temp;
		}
		else if (pos.x > screenWidth && pos.y < 0)
		{
			temp = new Vector3(worldPos.x + offset, originPos.y - offset, point.position.z);
			point.position = temp;
		}
		
	


	}

	void borderAdjuster()
    {
		Vector3 worldPos;
		Vector3 originPos;
		originPos = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.position.z));
		worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, Camera.main.transform.position.z));
		float x = worldPos.x - originPos.x;
		x = Mathf.Abs(x+3);
		float y = worldPos.y - originPos.y;
		y = Mathf.Abs(y+3);
		border.size = new Vector2(x, y);
	
    }

}

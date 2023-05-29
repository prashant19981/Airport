using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class adsController : MonoBehaviour {
	public int caseIs = 0;
	public static adsController instance;
	private const string app_id = "";
	public static bool canShowInterstetial;
	public bool canShowRewardVideo;
	private bool count;
	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}

	}
	void Start () {

	}

	public void loadAds(){
		if (Advertisement.isSupported) {
			Advertisement.Initialize (app_id,false);
		}
	}

	public void ShowAd()
	{
		ShowOptions options = new ShowOptions ();
		options.resultCallback = AdCallbackhandler;

		if (Advertisement.IsReady ("rewardedVideo"))
			Advertisement.Show ("rewardedVideo", options);
		loadAds ();
	}

	void AdCallbackhandler (ShowResult result)
	{
		switch(result)
		{
		case ShowResult.Finished:
			Debug.Log ("Ad Finished. Rewarding player...");
			//gameController.adsWatched = true;
			if (caseIs == 0) {
				saveData.loadCoins ();
				saveData.c.setCoins (saveData.c.getCoins () + 100);
				saveData.saveCoins ();
			} else if (caseIs == 1) {
			
				game.isRewardVideoComplete = true;
			
			} else if (caseIs == 2) {
				levelManager.level2AD++;
			}
			else if (caseIs == 3) {
				levelManager.level3AD++;
			}
			else if (caseIs == 4) {
				levelManager.level4AD++;
			}
			else if (caseIs == 5) {
				levelManager.level5AD++;
			}
			break;
		case ShowResult.Skipped:
			Debug.Log ("Ad skipped. Son, I am dissapointed in you");
			break;
		case ShowResult.Failed:
			Debug.Log("I swear this has never happened to me before");
			break;
		}
	}
	public void showInterStetial(){
		if (Advertisement.IsReady (null))
			Advertisement.Show (null,null);
		loadAds ();
	}
}

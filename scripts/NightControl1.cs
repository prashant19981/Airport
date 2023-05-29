using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NightControl1 : MonoBehaviour {
	Vector3 mouse;
	Vector2 mousePos;
	List<Vector2> lightPoints = new List<Vector2>();
	//	Vector2[] navPoints = new Vector2[12];
	List<Vector2> runway1nav= new List<Vector2>();
	List<Vector2> runway2nav = new List<Vector2> ();
	List<Vector2> ilsRunway2 = new List<Vector2> ();
	List<Vector2> ilsRunway1 = new List<Vector2> ();
	public GameObject prefabPath;
	//public LineRenderer line;
	private Vector3 instantiatePoint = new Vector3 (22.24f,1.71f,0);
	private float speed = 0.25f;//.22f
	private float rotationSpeed=2f;
	private float accuracy = 0.5f;
	float delta=0.01f;
	List<GameObject> pathIndicators;
	List<Vector2> points;
	private float angle;
	private float anglePath;
	private float screenHeight;
	private float screenWidth;
	private float proximity=0.5f;
	private Vector2 differenceAngle;
	private Vector2 fromAngle;
	private Vector2 toAngle;
	private Vector2 fromAnglePath;
	private Vector2 toAnglePath;
	private Vector2 differenceAnglePath;
	private Quaternion targetAnglePath;
	Quaternion targetAngle;
	private int index1=0;
	private int index2 = 0;
	private bool canControl;
	private bool shadow;
	List<GameObject> instantiatePoints = new List<GameObject> ();
	private bool isSelected;
	private bool isAlreadyCalled = false;
	public bool isOnScreen;
	private bool shouldStop;
	public bool hasLanded;
	private bool takeTaxiControl;
	private bool locked1;
	private bool locked2;
	private bool portSelectMode;
	private bool isRunway2active;
	private bool isILSRunway1;
	private bool isILSRunway2;
	private bool isAudioPlayed;
	private bool trigger;//new
	private bool isReadyToPark = false;
	private bool canLock1;
	private bool canLock2;
	private GameObject runwayIndicator;
	Vector3 scale;
	bool lightsOn;
	Vector3 startShadowPos;
	[SerializeField]List<Vector2> parkingSpots = new List<Vector2> ();
	game g = new game();
	GameObject park;
	private bool takeOff = false;
	GameObject ils2;
	GameObject ils1;
	void Awake(){
		scale = transform.localScale;
		startShadowPos = transform.GetChild (1).localPosition;
		print ("done");
	}
	void Starter(){
		//Time.timeScale = 3f;
		speed = 0.25f;
		isAlreadyCalled = false;
		isAudioPlayed = false;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		isILSRunway1 = false;
		isILSRunway2 = false;
		shouldStop = false;
		canControl = false;
		hasLanded = false;
		locked1 = false;
		locked2 = false;
		trigger = false;
		takeTaxiControl = false;
		portSelectMode = false;
		shadow = false;
		isSelected = false;
		isOnScreen = false;
		shadow = false;
		lightsOn = false;
	}
	void Start(){
		transform.GetChild(4).gameObject.SetActive(false);
		lockDeterminer();
		Starter ();
		pathIndicators = new List<GameObject> ();
		for (int i = 0; i < 80; i++) {
			GameObject obj = Instantiate (prefabPath,instantiatePoint,transform.rotation);
			pathIndicators.Add (obj);

		}
		GameObject lights = GameObject.FindGameObjectWithTag ("Point");
		for (int i = 0; i < lights.transform.childCount; i++) {
			lightPoints.Add (new Vector2 (lights.transform.GetChild (i).transform.position.x, lights.transform.GetChild (i).transform.position.y));

		}
		//if (shopManager.isILSRunway2On) {
		ils2 = GameObject.FindGameObjectWithTag ("ILSRun2");
		for (int i = 0; i < ils2.transform.childCount; i++) {
			ilsRunway2.Add (new Vector2 (ils2.transform.GetChild (i).transform.position.x, ils2.transform.GetChild (i).transform.position.y));
		}
		//}
		//if (shopManager.isILSRunway1On) {
		ils1 = GameObject.FindGameObjectWithTag ("ILSRun1");
		for (int i = 0; i < ils1.transform.childCount; i++) {
			ilsRunway1.Add (new Vector2 (ils1.transform.GetChild (i).transform.position.x, ils1.transform.GetChild (i).transform.position.y)); 
			//	}
		}
		GameObject nav = GameObject.FindGameObjectWithTag("taxiWay");
		for(int i=0;i<nav.transform.childCount;i++){
			//navPoints [i] = new Vector2(nav.transform.GetChild (i).position.x,nav.transform.GetChild (i).position.y);
			runway1nav.Add(new Vector2(nav.transform.GetChild (i).position.x,nav.transform.GetChild (i).position.y));
		}
		GameObject objs = GameObject.FindGameObjectWithTag ("instantiatepoint");
		for (int i = 0; i < objs.transform.childCount; i++) {
			instantiatePoints.Add(objs.transform.GetChild (i).gameObject);

		}

		GameObject run = GameObject.FindGameObjectWithTag ("Runway2");
		for (int i = 0; i < run.transform.childCount; i++) {
			runway2nav.Add (new Vector2 (run.transform.GetChild (i).transform.position.x, run.transform.GetChild (i).transform.position.y));
		}
		//		park = GameObject.FindGameObjectWithTag ("parking");
		//		for (int i = 0; i < park.transform.childCount; i++) {
		//			parkingSpots.Add (new Vector2 (park.transform.GetChild (i).position.x, park.transform.GetChild (i).position.y));
		//		
		//		}

		//add more points to the runway.
		
		transform.GetChild(5).gameObject.SetActive(true);
	}
	void Update(){
		Vector3 position;
		Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
		//print (screenHeight);
		if (Input.GetMouseButtonDown (0) && !takeTaxiControl && Input.touchCount < 2) {
			mouse = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f);
			mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (mouse).x, Camera.main.ScreenToWorldPoint (mouse).y);
			if (Vector2.Distance (mousePos, new Vector2 (transform.position.x, transform.position.y)) < 1f) {
				isSelected = true;
				isAlreadyCalled = false;
				locked1 = false;
				locked2 = false;

			}
		}
		if (!shouldStop) {
			transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));
		}
		if (Input.GetMouseButton (0) && !takeTaxiControl && Input.touchCount < 2) {


			mouse = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f);
			mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (mouse).x, Camera.main.ScreenToWorldPoint (mouse).y);
			if (Vector2.Distance (mousePos, new Vector2 (transform.position.x, transform.position.y)) < 0.5f && !canControl && isSelected) {
				canControl = true;
				colorNormalizer ();
				try{
					points.Clear ();
					points = new List<Vector2>();
					for(int i=0;i<=index1;i++){
						pathIndicators[i].transform.position = instantiatePoint;
					}
					index1 = 0;
					index2 = 0;
				}
				catch (Exception e){
				}

			}
			if (canControl) {

				pathMaker (mousePos);
				runwayIndicator.transform.GetChild(0).gameObject.SetActive(true);
			}
		}
		if(Input.GetMouseButtonUp(0) && !takeTaxiControl && Input.touchCount < 2){
			canControl = false;
			if(!locked1 && !locked2)
				colorChanger ();
			isSelected = false;
			runwayIndicator.transform.GetChild(0).gameObject.SetActive(false);
		}
		if (points != null && points.Count!=0) {
			fromAngle = new Vector2 (transform.position.x, transform.position.y);
			toAngle = new Vector2 (points [0].x, points [0].y);
			differenceAngle = toAngle - fromAngle;
			angle = Mathf.Atan2 (differenceAngle.y, differenceAngle.x) * Mathf.Rad2Deg;
			angle -= 90f;
			targetAngle = Quaternion.Euler (new Vector3 (0, 0, angle));
			transform.rotation = Quaternion.Slerp (transform.rotation, targetAngle, rotationSpeed * Time.deltaTime);
		}


		pathFollower ();

		if (shadow) {
			shadowAnimation ();
		}

		if (!isOnScreen) {
			targetArrow ();
			gameObject.GetComponent<BoxCollider2D> ().isTrigger = true;

		} else {
			transform.GetChild (3).gameObject.SetActive (false);
			gameObject.GetComponent<BoxCollider2D> ().isTrigger = false;
		}
		if (trigger) {
			this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		}
		/*if (locked1 || locked2) {
			for (int i = 0; i < lightPoints.Count; i++) {
				if (Vector2.Distance (new Vector2 (transform.position.x, transform.position.y), lightPoints [i]) < 0.5f && !lightsOn) {
					lightsOn = true;
					transform.GetChild (4).gameObject.SetActive (true);
					transform.GetChild (5).gameObject.SetActive (true);
				}

			}
		} else {
			transform.GetChild (4).gameObject.SetActive (false);
		}*/


	}



	void pathMaker(Vector2 point){

		try{
			if ((points == null || points.Count ==0) &&  (Vector2.Distance (point, new Vector2(transform.position.x,transform.position.y)) > delta)) {
				points = new List<Vector2> ();
				points.Add (point);
				pathIndicators[index1].transform.position = point;
				fromAnglePath = new Vector2 (transform.position.x, transform.position.y);
				toAnglePath = new Vector2 (points [0].x, points [0].y);
				differenceAnglePath = toAnglePath - fromAnglePath;
				anglePath = Mathf.Atan2 (differenceAnglePath.y, differenceAnglePath.x) * Mathf.Rad2Deg;
				//anglePath -= 90f;
				targetAnglePath = Quaternion.Euler (new Vector3 (0, 0, anglePath));
				pathIndicators[index1].transform.rotation = targetAnglePath;
				index1++;
			} else 
				if ( Vector2.Distance (point, points [points.Count -1]) > 0.25f && index1!=pathIndicators.Count-2) {
					points.Add (point);
					print(points.Count);
					pathIndicators[index1].transform.position = point;
					fromAnglePath = new Vector2 (points[points.Count-1].x, points[points.Count-1].y);
					toAnglePath = new Vector2 (points [points.Count-2].x, points [points.Count-2].y);
					differenceAnglePath = toAnglePath - fromAnglePath;
					anglePath = Mathf.Atan2 (differenceAnglePath.y, differenceAnglePath.x) * Mathf.Rad2Deg;
					//anglePath -= 90f;
					targetAnglePath = Quaternion.Euler (new Vector3 (0, 0, anglePath));
					pathIndicators[index1].transform.rotation = targetAnglePath;
					index1++;
					//					if(index1 == pathIndicators.Count)
					//						index1 = 0;
				}
				else if(index1==pathIndicators.Count-2){
					canControl = false;
					colorChanger();

				}
			if(Vector2.Distance(point,runway1nav[0])<0.2f && !isAlreadyCalled && canLock1){
				isAlreadyCalled = true;
				canControl = false;
				//colorChanger();
				lockedTargetColor();
				locked1 = true;
				Runway1Nav();
				//				for (int i=0; i<12;i++){
				//					points.Add(navPoints[i]);
				//
				//				} 
			}
			if(Vector2.Distance(point,runway2nav[0])<0.2f && !isAlreadyCalled && canLock2){
				isAlreadyCalled = true;
				canControl = false;
				lockedTargetColor();
				locked2 = true;
				//				for (int i=0; i<runway2nav.Count;i++){
				//					points.Add(runway2nav[i]);
				//
				//				} 	
				Runway2Nav();
			}
			if(shopManager.isILSRunway1On && canLock1)
			{
				for(int i=0;i<ilsRunway1.Count;i++){
					if(Vector2.Distance(point,ilsRunway1[i])<0.2f || isILSRunway1 )
					{
						isILSRunway1 = true;
						points.Add(ilsRunway1[i]);
						pathIndicators[index1].transform.position = ilsRunway1[i];
						pathIndicators[index1].transform.rotation = ils1.transform.rotation;
						index1++;
					}
				}
			}

			//some ILS condition for check
			if(shopManager.isILSRunway2On && canLock2)
			{
				for(int i=0;i<ilsRunway2.Count;i++){
					if(Vector2.Distance(point,ilsRunway2[i])<0.2f || isILSRunway2){
						isILSRunway2 = true;
						points.Add(ilsRunway2[i]);
						pathIndicators[index1].transform.position = ilsRunway2[i];
						pathIndicators[index1].transform.rotation = ils2.transform.rotation;
						index1++;
					}

				}
			}
			if(isILSRunway2){
				isILSRunway2 = false;
				canControl = false;
				lockedTargetColor();
				locked2 = true;
				Runway2Nav();
			}
			if(isILSRunway1){
				isILSRunway1 = false;
				canControl = false;
				lockedTargetColor();
				locked1 = true;
				Runway1Nav();
			}

		}
		catch(Exception e){
		}

	}
	void pathFollower(){
		try {
			if( Vector2.Distance(new Vector2(transform.position.x,transform.position.y), new Vector2(pathIndicators[index2].transform.position.x,pathIndicators[index2].transform.position.y)) < proximity){

				pathIndicators[index2].transform.position = instantiatePoint;
				index2++;
				if(index2 == pathIndicators.Count)
					index2 = 0;
			}
			if (Vector2.Distance(new Vector2(transform.position.x,transform.position.y),points[0])<accuracy) {
				points.RemoveAt (0);

			}
			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),runway1nav[1]) < 0.2f) && locked1){
				//this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
				trigger = true;
				gameObject.GetComponent<Animator>().SetTrigger("shrink");

				shadow = true;
				hasLanded = true;
				takeTaxiControl = true;
				if(PlayerPrefs.GetInt("music",1) == 1){
				if(!isAudioPlayed){
					isAudioPlayed = true;
					gameObject.GetComponent<AudioSource>().Play();
				}
				}
			}
			if( (Vector2.Distance(new Vector2(transform.position.x,transform.position.y),runway2nav[2]) < 0.2f) && locked2){
				//this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
				gameObject.GetComponent<Animator>().SetTrigger("shrink");
				trigger = true;
				shadow = true;
				hasLanded = true;
				takeTaxiControl = true;
				if(PlayerPrefs.GetInt("music",1) == 1){
				if(!isAudioPlayed){
					isAudioPlayed = true;
					gameObject.GetComponent<AudioSource>().Play();
				}
				}
			}
			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),runway1nav[7]) < 0.2f)&&hasLanded ){
				speed = 0.2f;
				rotationSpeed = 3f;
				accuracy = 0.2f;

			}
			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),runway2nav[7]) < 0.2f)&&hasLanded ){
				speed = 0.2f;
				rotationSpeed = 3f;
				accuracy = 0.2f;

			}

			//			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),navPoints[9]) < 0.3f) && hasLanded ){
			//				shouldStop = true;
			//			}

			if(points.Count==0  && hasLanded ){//removed speed check

				PlayerPrefs.SetInt("count",PlayerPrefs.GetInt("count",0)+1);
				g.coinUpdater();

				//shouldStop = true;
				takeTaxiControl = true;
				//this.gameObject.SetActive(false);
				//groundControl.taxiControl = true;
				respawner();
				//speed = 0f;
				//isReadyToPark = true;
				//				locked = false;
				//				
				//respawner();
			}


		} 
		catch (Exception e){}
	}
	void colorChanger(){
		foreach (GameObject obj in pathIndicators) {
			obj.GetComponent<SpriteRenderer> ().color = new Color32 (173, 173, 173, 255);
			obj.transform.localScale = new Vector3 (0.01653977f, 0.03373132f, 1f);

		}
		//red for red plane and so on

	}
	void colorNormalizer(){
		foreach (GameObject obj in pathIndicators) {
			obj.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
			obj.transform.localScale = new Vector3 (0.01653977f, 0.03373132f, 1f);

		}

	}
	void lockedTargetColor(){
		foreach (GameObject obj in pathIndicators) {
			obj.GetComponent<SpriteRenderer> ().color = new Color32 (0, 255, 44, 255);
			obj.transform.localScale = new Vector3 (0.01653977f,0.03373132f, 1f);

		}
	}
	void OnTriggerEnter2D(Collider2D other){
		try{
			if (isOnScreen) {
				if (other.tag == "Airplane" || other.tag == "jet" || other.tag == "Helicopter" ) {
					//if (!hasLanded && !other.gameObject.GetComponent<carrierControl> ().hasLanded || !hasLanded && !other.gameObject.GetComponent<HelicopterControl> ().hasLanded || !hasLanded && !other.gameObject.GetComponent<Control> ().hasLanded ) {
					if(other.gameObject.GetComponent<carrierControl>() != null){
						if(!hasLanded && !other.gameObject.GetComponent<carrierControl>().hasLanded)
						{
							transform.GetChild (0).gameObject.SetActive (true);
							transform.GetChild (2).gameObject.SetActive (true);
						}
					}
					else if(other.gameObject.GetComponent<Control>() != null){
						if(!hasLanded && !other.gameObject.GetComponent<Control>().hasLanded){
							transform.GetChild (0).gameObject.SetActive (true);
							transform.GetChild (2).gameObject.SetActive (true);
						}
					}
					else if(other.gameObject.GetComponent<HelicopterControl>() != null){
						if(!hasLanded && !other.gameObject.GetComponent<HelicopterControl>().hasLanded){
							transform.GetChild (0).gameObject.SetActive (true);
							transform.GetChild (2).gameObject.SetActive (true);	
						}
					}
					else if (other.gameObject.GetComponent<NightControl1>()!=null){
						if(!hasLanded && !other.gameObject.GetComponent<NightControl1>().hasLanded){
							transform.GetChild (0).gameObject.SetActive (true);
							transform.GetChild (2).gameObject.SetActive (true);
						}
					}
				}
				//	}

			}
		}
		catch(Exception e){
		}
	}
	void OnTriggerExit2D(Collider2D other){
		try{
			if (isOnScreen) {
				if (other.tag == "Airplane" || other.tag == "jet" || other.tag == "Helicopter" ) {
					//if (!hasLanded && !other.gameObject.GetComponent<carrierControl> ().hasLanded || !hasLanded && !other.gameObject.GetComponent<HelicopterControl> ().hasLanded || !hasLanded && !other.gameObject.GetComponent<Control> ().hasLanded ) {
					if(other.gameObject.GetComponent<carrierControl>() != null){
						if(!hasLanded && !other.gameObject.GetComponent<carrierControl>().hasLanded)
						{
							transform.GetChild (0).gameObject.SetActive (false);
							transform.GetChild (2).gameObject.SetActive (false);
						}
					}
					else if(other.gameObject.GetComponent<Control>() != null){
						transform.GetChild (0).gameObject.SetActive (false);
						transform.GetChild (2).gameObject.SetActive (false);
					}
					else if(other.gameObject.GetComponent<HelicopterControl>() != null){
						transform.GetChild (0).gameObject.SetActive (false);
						transform.GetChild (2).gameObject.SetActive (false);	
					}
					else if (other.gameObject.GetComponent<NightControl1>()!=null){
						transform.GetChild (0).gameObject.SetActive (false);
						transform.GetChild (2).gameObject.SetActive (false);
					}
				}
				//	}

			}
		}
		catch(Exception e){
		}
	}
	void OnCollisionEnter2D(Collision2D other){
		try{

			if (isOnScreen) {
				if(other.gameObject.GetComponent<carrierControl>() != null){
					if(!hasLanded && !other.gameObject.GetComponent<carrierControl>().hasLanded)
					{
						GameObject obj	= GameObject.FindGameObjectWithTag ("explode");
						obj.GetComponent<Animator> ().SetTrigger ("explode");
						if(PlayerPrefs.GetInt("music",1) == 1){
							obj.GetComponent<AudioSource> ().Play ();
						}
						obj.transform.position = this.transform.position;
						for (int i = index2; i <= index1; i++) {
							pathIndicators [i].transform.position = instantiatePoint;
						}
						game.playerOut = true;
						//Time.timeScale = 0;
						this.gameObject.SetActive (false);
					}
				}
				else if(other.gameObject.GetComponent<Control>() != null){
					if(!hasLanded && !other.gameObject.GetComponent<Control>().hasLanded){
						GameObject obj	= GameObject.FindGameObjectWithTag ("explode");
						obj.GetComponent<Animator> ().SetTrigger ("explode");
						if(PlayerPrefs.GetInt("music",1) == 1){
							obj.GetComponent<AudioSource> ().Play ();
						}
						obj.transform.position = this.transform.position;
						for (int i = index2; i <= index1; i++) {
							pathIndicators [i].transform.position = instantiatePoint;
						}
						game.playerOut = true;
						//Time.timeScale = 0;
						this.gameObject.SetActive (false);;
					}
				}
				else if(other.gameObject.GetComponent<HelicopterControl>() != null){
					if(!hasLanded && !other.gameObject.GetComponent<HelicopterControl>().hasLanded){
						GameObject obj	= GameObject.FindGameObjectWithTag ("explode");
						obj.GetComponent<Animator> ().SetTrigger ("explode");
						if(PlayerPrefs.GetInt("music",1) == 1){
							obj.GetComponent<AudioSource> ().Play ();
						}
						obj.transform.position = this.transform.position;
						for (int i = index2; i <= index1; i++) {
							pathIndicators [i].transform.position = instantiatePoint;
						}
						game.playerOut = true;
						//Time.timeScale = 0;
						this.gameObject.SetActive (false);
					}
				}
				else if (other.gameObject.GetComponent<NightControl1>()!=null){
					if(!hasLanded && !other.gameObject.GetComponent<NightControl1>().hasLanded){
						GameObject obj	= GameObject.FindGameObjectWithTag ("explode");
						obj.GetComponent<Animator> ().SetTrigger ("explode");
						if(PlayerPrefs.GetInt("music",1) == 1){
							obj.GetComponent<AudioSource> ().Play ();
						}
						obj.transform.position = this.transform.position;
						for (int i = index2; i <= index1; i++) {
							pathIndicators [i].transform.position = instantiatePoint;
						}
						game.playerOut = true;
						//Time.timeScale = 0;
						this.gameObject.SetActive (false);
					}
				}
			}
		}
		catch (Exception e){
		}
	}

	void lockDeterminer()
	{
		if (SceneManager.GetActiveScene().name == "1")
		{
			if (this.tag == "Airplane")
			{
				canLock1 = false;
				canLock2 = true;
				runwayIndicator = GameObject.FindGameObjectWithTag("runway2Sign");

			}
			else if (this.tag == "jet")
			{
				canLock1 = true;
				canLock2 = false;
				runwayIndicator = GameObject.FindGameObjectWithTag("runway1Sign");
				//runwayIndicator.SetActive(false);// BE CAREULL CAUSING UNCONTROLLABLE SPIN
			}
		}
		else if (SceneManager.GetActiveScene().name == "2")
		{
			if (this.tag == "Airplane")
			{
				canLock1 = false;
				canLock2 = true;
				runwayIndicator = GameObject.FindGameObjectWithTag("runway2Sign");

			}
			else if (this.tag == "jet")
			{
				canLock1 = true;
				canLock2 = false;
				runwayIndicator = GameObject.FindGameObjectWithTag("runway1Sign");
				//runwayIndicator.SetActive(false);// BE CAREULL CAUSING UNCONTROLLABLE SPIN
			}

		}
		else if (SceneManager.GetActiveScene().name == "3")
		{
			if (this.tag == "Airplane")
			{
				canLock1 = false;
				canLock2 = true;
				runwayIndicator = GameObject.FindGameObjectWithTag("runway2Sign");

			}
			else if (this.tag == "jet")
			{
				canLock1 = true;
				canLock2 = false;
				runwayIndicator = GameObject.FindGameObjectWithTag("runway1Sign");
				//runwayIndicator.SetActive(false);// BE CAREULL CAUSING UNCONTROLLABLE SPIN
			}
		}
		else if (SceneManager.GetActiveScene().name == "4")
		{
			if (this.tag == "Airplane")
			{
				canLock1 = false;
				canLock2 = true;
				runwayIndicator = GameObject.FindGameObjectWithTag("runway2Sign");

			}
			else if (this.tag == "jet")
			{
				canLock1 = true;
				canLock2 = false;
				runwayIndicator = GameObject.FindGameObjectWithTag("runway1Sign");
				//runwayIndicator.SetActive(false);// BE CAREULL CAUSING UNCONTROLLABLE SPIN
			}
		}
		else if (SceneManager.GetActiveScene().name == "5")
		{

		}
	}

	void shadowAnimation(){
		transform.GetChild (1).position = Vector3.Slerp (transform.GetChild(1).position, transform.position, 1f*Time.deltaTime);

	}
	void OnBecameVisible(){
		isOnScreen = true;
		if (this.tag == "jet")
			speed = 0.42f;
		else
			speed = 0.25f;

		StartCoroutine(headlightsOn());
	}
	void OnBecameInvisible(){
		isOnScreen = false;
		speed = 0.25f;
	}
	void Runway2Nav(){

		for (int i = 0; i < runway2nav.Count; i++) {
			points.Add (runway2nav [i]);
		}
	}
	void Runway1Nav(){
		for (int i = 0; i < runway1nav.Count; i++) {
			points.Add (runway1nav [i]);
		}
	}
	void targetArrow(){

		GameObject pointer = transform.GetChild(3).gameObject;
		pointer.SetActive(true);
		Vector3 position;
		Vector3 worldPos;
		Vector3 originPos;
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		originPos = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.position.z));
		worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, Camera.main.transform.position.z));
		if (pos.x > screenWidth && pos.y < screenHeight && pos.y > 0)
		{
			pointer.transform.rotation = Quaternion.Euler(0, 0, 90f);
			position = new Vector3(worldPos.x - pointer.GetComponent<SpriteRenderer>().bounds.size.x / 2, transform.position.y, -2f);
			pointer.transform.position = position;
			//6.599999f
		}
		else if (pos.x < screenWidth && pos.y > screenHeight && pos.x > 0)
		{
			pointer.transform.rotation = Quaternion.Euler(0, 0, 180f);
			position = new Vector3(transform.position.x, worldPos.y - pointer.GetComponent<SpriteRenderer>().bounds.size.y / 2, -2f);
			pointer.transform.position = position;
			//3.600001f
		}
		else if (pos.x < 0 && pos.y < screenHeight && pos.y > 0)
		{
			pointer.transform.rotation = Quaternion.Euler(0, 0, -90f);
			position = new Vector3(originPos.x + pointer.GetComponent<SpriteRenderer>().bounds.size.x / 2, transform.position.y, -2f);
			pointer.transform.position = position;
			//-6.68f
		}
		else if (pos.x < screenWidth && pos.y < 0 && pos.x > 0)
		{
			pointer.transform.rotation = Quaternion.Euler(0, 0, 0);
			position = new Vector3(transform.position.x, originPos.y + pointer.GetComponent<SpriteRenderer>().bounds.size.y / 2, -2f);
			pointer.transform.position = position;
			//-3.73f
		}
		else if (pos.x > screenWidth && pos.y > screenHeight)
		{
			pointer.transform.rotation = Quaternion.Euler(0, 0, 135f);
			position = new Vector3(worldPos.x - pointer.GetComponent<SpriteRenderer>().bounds.size.x / 2, worldPos.y - pointer.GetComponent<SpriteRenderer>().bounds.size.y / 2, -2f);
			pointer.transform.position = position;
			// 6.77f,3.74f
		}
		else if (pos.x < 0 && pos.y > screenHeight)
		{
			pointer.transform.rotation = Quaternion.Euler(0, 0, -135f);
			position = new Vector3(originPos.x + pointer.GetComponent<SpriteRenderer>().bounds.size.x / 2, worldPos.y - pointer.GetComponent<SpriteRenderer>().bounds.size.y / 2, -2f);
			pointer.transform.position = position;

		}
		else if (pos.x < 0 && pos.y < 0)
		{
			pointer.transform.rotation = Quaternion.Euler(0, 0, -45f);
			position = new Vector3(originPos.x + pointer.GetComponent<SpriteRenderer>().bounds.size.x / 2, originPos.y + pointer.GetComponent<SpriteRenderer>().bounds.size.y / 2, -2f);
			pointer.transform.position = position;
		}
		else if (pos.x > screenWidth && pos.y < 0)
		{
			pointer.transform.rotation = Quaternion.Euler(0, 0, 45f);
			position = new Vector3(worldPos.x - pointer.GetComponent<SpriteRenderer>().bounds.size.x / 2, originPos.y + pointer.GetComponent<SpriteRenderer>().bounds.size.y / 2, -2f);
			pointer.transform.position = position;
		}





	}
	void respawner(){
		gameObject.GetComponent<insidePlane>().setIsActive(false);
		int index = UnityEngine.Random.Range (0, instantiatePoints.Count);
		transform.position = instantiatePoints [index].transform.position;
		transform.rotation = instantiatePoints [index].transform.rotation;
		transform.localScale = scale;
		transform.GetChild (1).localPosition = startShadowPos;
		transform.GetChild (0).gameObject.SetActive (false);
		transform.GetChild (2).gameObject.SetActive (false);
		Starter ();
		this.gameObject.SetActive (false);

	}
	IEnumerator headlightsOn()
    {
		yield return new WaitForSeconds(2f);
		transform.GetChild(4).gameObject.SetActive(true);
    }

}

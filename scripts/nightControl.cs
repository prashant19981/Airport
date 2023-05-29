using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class nightControl : MonoBehaviour {
	Vector3 mouse;
	Vector2 mousePos;
	//	Vector2[] navPoints = new Vector2[12];
	List<Vector2> lightPoints = new List<Vector2>();
	List<Vector2> runway1nav= new List<Vector2>();
	List<Vector2> runway2nav = new List<Vector2> ();
	List<Vector2> ilsRunway2 = new List<Vector2> ();
	List<Vector2> ilsRunway1 = new List<Vector2> ();
	public GameObject prefabPath;
	//public LineRenderer line;
	private Vector3 instantiatePoint = new Vector3 (22.24f,1.71f,0);
	private float speed = 0.5f;//0.47f
	private float rotationSpeed=2f;
	private float accuracy = 0.5f;
	float delta=0.01f;
	List<GameObject> pathIndicators;
	List<Vector2> points;
	private float angle;
	private float screenHeight;
	private float screenWidth;
	private float proximity=0.6f;
	private Vector2 differenceAngle;
	private Vector2 fromAngle;
	private Vector2 toAngle;
	Quaternion targetAngle;
	private int index1=0;
	private int index2 = 0;
	private bool canControl;
	private bool shadow;
	private bool isSelected;
	public bool isOnScreen;
	private bool shouldStop;
	public bool hasLanded;
	private bool takeTaxiControl;
	private bool locked;
	private bool portSelectMode;
	private bool isRunway2active;
	private bool isILSRunway1;
	private bool isILSRunway2;
	private bool isAudioPlayed;
	private bool lightsOn;
	private bool trigger;
	game g = new game();
	void OnEnable(){



	}

	void Start(){
		trigger = false;
		lightsOn = false;
		isAudioPlayed = false;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		isILSRunway1 = false;
		isILSRunway2 = false;
		shouldStop = false;
		canControl = false;
		hasLanded = false;
		locked = false;
		takeTaxiControl = false;
		portSelectMode = false;
		pathIndicators = new List<GameObject> ();
		for (int i = 0; i < 80; i++) {
			GameObject obj = Instantiate (prefabPath,instantiatePoint,transform.rotation);
			pathIndicators.Add (obj);
			shadow = false;
		}
		GameObject lights = GameObject.FindGameObjectWithTag ("Point");
		for (int i = 0; i < lights.transform.childCount; i++) {
			lightPoints.Add (new Vector2 (lights.transform.GetChild (i).transform.position.x, lights.transform.GetChild (i).transform.position.y));
		
		}
		//if (shopManager.isILSRunway2On) {
		GameObject ils2 = GameObject.FindGameObjectWithTag ("ILSRun2");
		for (int i = 0; i < ils2.transform.childCount; i++) {
			ilsRunway2.Add (new Vector2 (ils2.transform.GetChild (i).transform.position.x, ils2.transform.GetChild (i).transform.position.y));
		}
		//}
		//if (shopManager.isILSRunway1On) {
		GameObject ils1 = GameObject.FindGameObjectWithTag ("ILSRun1");
		for (int i = 0; i < ils1.transform.childCount; i++) {
			ilsRunway1.Add (new Vector2 (ils1.transform.GetChild (i).transform.position.x, ils1.transform.GetChild (i).transform.position.y)); 
			//	}
		}
		GameObject nav = GameObject.FindGameObjectWithTag("taxiWay");
		for(int i=0;i<nav.transform.childCount;i++){
			//navPoints [i] = new Vector2(nav.transform.GetChild (i).position.x,nav.transform.GetChild (i).position.y);
			runway1nav.Add(new Vector2(nav.transform.GetChild (i).position.x,nav.transform.GetChild (i).position.y));
		}
		isSelected = false;
		isOnScreen = false;
		GameObject run = GameObject.FindGameObjectWithTag ("Runway2");
		for (int i = 0; i < run.transform.childCount; i++) {
			runway2nav.Add (new Vector2 (run.transform.GetChild (i).transform.position.x, run.transform.GetChild (i).transform.position.y));
		}

		//add more points to the runway.
	}
	void Update(){
		Vector3 position;
		Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
		print (screenHeight);
		if (Input.GetMouseButtonDown (0) && !takeTaxiControl) {
			mouse = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f);
			mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (mouse).x, Camera.main.ScreenToWorldPoint (mouse).y);
			if (Vector2.Distance (mousePos, new Vector2 (transform.position.x, transform.position.y)) < 1f) {
				isSelected = true;
				locked = false;

			}
		}
		if (!shouldStop) {
			transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));
		}
		if (Input.GetMouseButton (0) && !takeTaxiControl) {


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
			}
		}
		if(Input.GetMouseButtonUp(0) && !takeTaxiControl){
			canControl = false;
			if(!locked)
				colorChanger ();
			isSelected = false;
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
		if (locked) {
			for (int i = 0; i < lightPoints.Count; i++) {
				if (Vector2.Distance (new Vector2 (transform.position.x, transform.position.y), lightPoints [i]) < 0.5f && !lightsOn) {
					lightsOn = true;
					transform.GetChild (4).gameObject.SetActive (true);
					transform.GetChild (5).gameObject.SetActive (true);
				}
	     	  
			}
		} else {
			transform.GetChild (4).gameObject.SetActive (false);
		}
		if (trigger) {
			this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		}
	}



	void pathMaker(Vector2 point){

		try{
			if ((points == null || points.Count ==0) &&  (Vector2.Distance (point, new Vector2(transform.position.x,transform.position.y)) > delta)) {
				points = new List<Vector2> ();
				points.Add (point);
				pathIndicators[index1].transform.position = point;
				pathIndicators[index1].transform.rotation = targetAngle;
				index1++;
			} else 
				if ( Vector2.Distance (point, points [points.Count -1]) > 0.25f && index1!=pathIndicators.Count-2) {
					points.Add (point);
					pathIndicators[index1].transform.position = point;
					pathIndicators[index1].transform.rotation = targetAngle;
					index1++;
					//					if(index1 == pathIndicators.Count)
					//						index1 = 0;
				}
				else if(index1==pathIndicators.Count-2){
					canControl = false;
					colorChanger();

				}
			if(Vector2.Distance(point,runway1nav[0])<0.2f){
				canControl = false;
				//colorChanger();
				lockedTargetColor();
				locked = true;
				Runway1Nav();
				//				for (int i=0; i<12;i++){
				//					points.Add(navPoints[i]);
				//
				//				} 
			}
			if(Vector2.Distance(point,runway2nav[0])<0.2f){
				canControl = false;
				lockedTargetColor();
				locked = true;
				//				for (int i=0; i<runway2nav.Count;i++){
				//					points.Add(runway2nav[i]);
				//
				//				} 	
				Runway2Nav();
			}
			if(shopManager.isILSRunway1On){
			for(int i=0;i<ilsRunway1.Count;i++){
				if(Vector2.Distance(point,ilsRunway1[i])<0.2f || isILSRunway1){
					isILSRunway1 = true;
					points.Add(ilsRunway1[i]);
						pathIndicators[index1].transform.position = ilsRunway1[i];
						index1++;
				}
					}
			}

			//some ILS condition for check
				if(shopManager.isILSRunway2On){
			for(int i=0;i<ilsRunway2.Count;i++){
				if(Vector2.Distance(point,ilsRunway2[i])<0.2f || isILSRunway2){
					isILSRunway2 = true;
					points.Add(ilsRunway2[i]);
						pathIndicators[index1].transform.position = ilsRunway2[i];
						index1++;
				}

				}
			}
			if(isILSRunway2){
				isILSRunway2 = false;
				canControl = false;
				lockedTargetColor();
				locked = true;
				Runway2Nav();
			}
			if(isILSRunway1){
				isILSRunway1 = false;
				canControl = false;
				lockedTargetColor();
				locked = true;
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
			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),runway1nav[1]) < 0.2f) && locked){
				gameObject.GetComponent<Animator>().SetTrigger("shrink");
				//this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
				trigger = true;
				shadow = true;
				hasLanded = true;
				takeTaxiControl = true;

				if(!isAudioPlayed){
					isAudioPlayed = true;
					gameObject.GetComponent<AudioSource>().Play();
				}
			}
			if( (Vector2.Distance(new Vector2(transform.position.x,transform.position.y),runway2nav[2]) < 0.2f) && locked){
				gameObject.GetComponent<Animator>().SetTrigger("shrink");
				//this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
				shadow = true;
				hasLanded = true;
				takeTaxiControl = true;
				trigger = true;
				if(!isAudioPlayed){
					isAudioPlayed = true;
					gameObject.GetComponent<AudioSource>().Play();
				}
			}
			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),runway1nav[7]) < 0.2f)&&hasLanded ){
				speed = 0.5f;
				rotationSpeed = 3f;
				accuracy = 0.2f;

			}
			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),runway2nav[7]) < 0.2f)&&hasLanded ){
				speed = 0.5f;
				rotationSpeed = 3f;
				accuracy = 0.2f;

			}

			//			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),navPoints[9]) < 0.3f) && hasLanded ){
			//				shouldStop = true;
			//			}
			if(points.Count==0 && hasLanded && speed == 0.5f){
				g.coinUpdater();
				PlayerPrefs.SetInt("count",PlayerPrefs.GetInt("count",0)+1);
				shouldStop = true;
				takeTaxiControl = true;
				this.gameObject.SetActive(false);
			}

		} 
		catch (Exception e){}
	}
	void colorChanger(){
		foreach (GameObject obj in pathIndicators) {
			obj.GetComponent<SpriteRenderer> ().color = new Color32 (173, 173, 173, 255);
			obj.transform.localScale = new Vector3 (0.04f, 0.04f, 1f);

		}
		//red for red plane and so on

	}
	void colorNormalizer(){
		foreach (GameObject obj in pathIndicators) {
			obj.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
			obj.transform.localScale = new Vector3 (0.04f, 0.04f, 1f);

		}

	}
	void lockedTargetColor(){
		foreach (GameObject obj in pathIndicators) {
			obj.GetComponent<SpriteRenderer> ().color = new Color32 (0, 255, 44, 255);
			obj.transform.localScale = new Vector3 (0.04f,0.04f, 1f);

		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Airplane") {
			if (!hasLanded && !other.GetComponent<nightControl> ().hasLanded) {
				if (isOnScreen) {
					transform.GetChild (0).gameObject.SetActive (true);
					transform.GetChild (2).gameObject.SetActive (true);
				}
			}
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if (isOnScreen && other.tag == "Airplane") {
			transform.GetChild (0).gameObject.SetActive (false);
			transform.GetChild (2).gameObject.SetActive (false);
		}
	}
	void OnCollisionEnter2D(Collision2D other){
		//		transform.GetChild (0).gameObject.SetActive (false);
		//		transform.GetChild (2).gameObject.SetActive (false);
		//		g.explodeFun (transform.position, other.transform.position);
		if (!hasLanded && !other.gameObject.GetComponent<nightControl> ().hasLanded) {
			if (isOnScreen) {
				GameObject obj	= GameObject.FindGameObjectWithTag ("explode");
				obj.GetComponent<Animator> ().SetTrigger ("explode");
				obj.GetComponent<AudioSource> ().Play ();
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
	void shadowAnimation(){
		transform.GetChild (1).position = Vector3.Slerp (transform.GetChild(1).position, transform.position, 1f*Time.deltaTime);

	}
	void OnBecameVisible(){
		isOnScreen = true;
	}
	void OnBecameInvisible(){
		isOnScreen = false;

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

		GameObject pointer =  transform.GetChild (3).gameObject;
		pointer.SetActive (true);
		Vector3 position;
		Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);

		if (pos.x > screenWidth && pos.y < screenHeight && pos.y > 0 ) {
			pointer.transform.rotation = Quaternion.Euler (0, 0, 0);
			position = new Vector3 (6.599999f, transform.position.y, 0);
			pointer.transform.position = position;
		} else if (pos.x < screenWidth && pos.y > screenHeight && pos.x >0) {
			pointer.transform.rotation = Quaternion.Euler (0, 0, 90f);
			position = new Vector3 (transform.position.x, 3.600001f, 0);
			pointer.transform.position = position;

		} else if (pos.x < 0 && pos.y < screenHeight && pos.y > 0) {
			pointer.transform.rotation = Quaternion.Euler (0, 0, 180f);
			position = new Vector3 (-6.68f, transform.position.y, 0);
			pointer.transform.position = position;

		} else if (pos.x < screenWidth && pos.y < 0 && pos.x > 0) {
			pointer.transform.rotation = Quaternion.Euler (0, 0, 270f);
			position = new Vector3 (transform.position.x, -3.73f, 0);
			pointer.transform.position = position;
		} else if (pos.x > screenWidth && pos.y > screenHeight) {
			pointer.transform.rotation = Quaternion.Euler (0, 0, 45f);
			position = new Vector3 (6.77f, 3.74f, 0);
			pointer.transform.position = position;
		} else if (pos.x < 0 && pos.y > screenHeight) {
			pointer.transform.rotation = Quaternion.Euler (0, 0, 135f);
			position = new Vector3 (-6.78f, 3.67f, 0);
			pointer.transform.position = position;

		} else if (pos.x < 0 && pos.y < 0) {
			pointer.transform.rotation = Quaternion.Euler (0, 0, 225f);
			position = new Vector3 (-6.78f, -3.68f, 0);
			pointer.transform.position = position;
		} else if (pos.x > screenWidth && pos.y < 0) {
			pointer.transform.rotation = Quaternion.Euler (0, 0, 315f);
			position = new Vector3 (6.84f, -3.68f, 0);
			pointer.transform.position = position;
		}


	}

}

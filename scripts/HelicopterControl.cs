using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class HelicopterControl : MonoBehaviour {

	Vector3 mouse;
	Vector2 mousePos;
	Vector2 helipadLoc = new Vector2 ();
	Vector2 helipadLoc2 = new Vector2();
	public GameObject prefabPath;
	//public LineRenderer line;
	private Vector3 instantiatePoint = new Vector3 (22.24f,1.71f,0);
	private float speed = 0.25f;
	private float rotationSpeed=2f;
	private float accuracy = 0.5f;
	float delta=0.01f;
	List<GameObject> pathIndicators;
	List<GameObject> instantiatePoints = new List<GameObject> ();
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
	private bool trigger;
	Vector3 scale;
	private Vector2 fromAnglePath;
	private Vector2 toAnglePath;
	private Vector2 differenceAnglePath;
	private Quaternion targetAnglePath;
	private bool isAlreadyCalled;
	Vector3 startShadowPos;
	float anglePath;

	game g = new game();
	void Awake(){
		scale = transform.localScale;
		startShadowPos = transform.GetChild (1).localPosition;
		print ("done");
	}
	void Starter(){
		speed = 0.25f;
//		if (this.tag == "jet")
//			speed = 0.35f;//.32f
//		else
//			speed = 0.25f;
		isAudioPlayed = false;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		isAlreadyCalled = false;
		isILSRunway1 = false;
		isILSRunway2 = false;
		shouldStop = false;
		canControl = false;
		hasLanded = false;
	
		locked = false;
		trigger = false;
		takeTaxiControl = false;
		portSelectMode = false;
		shadow = false;
		isSelected = false;
		isOnScreen = false;
		shadow = false;

	}
	void Start(){
		Starter ();
		pathIndicators = new List<GameObject> ();
		for (int i = 0; i < 80; i++) {
			GameObject obj = Instantiate (prefabPath,instantiatePoint,transform.rotation);
			pathIndicators.Add (obj);
			shadow = false;
		}
		GameObject heli = GameObject.FindGameObjectWithTag("Helipad");
		helipadLoc = new Vector2 (heli.transform.position.x, heli.transform.position.y);
		/*if (SceneManager.GetActiveScene ().name == "4") {
		
			GameObject heli2 = GameObject.FindGameObjectWithTag ("Helipad2");
			helipadLoc2 = new Vector2 (heli2.transform.position.x, heli2.transform.position.y);
		
		}*/

		GameObject objs = GameObject.FindGameObjectWithTag ("instantiatepoint");
		for (int i = 0; i < objs.transform.childCount; i++) {
			instantiatePoints.Add(objs.transform.GetChild (i).gameObject);

		}
		isSelected = false;
		isOnScreen = false;

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
				fromAnglePath = new Vector2 (transform.position.x, transform.position.y);
				toAnglePath = new Vector2 (points [0].x, points [0].y);
				differenceAnglePath = toAnglePath - fromAnglePath;
				anglePath = Mathf.Atan2 (differenceAnglePath.y, differenceAnglePath.x) * Mathf.Rad2Deg;
				targetAnglePath = Quaternion.Euler (new Vector3 (0, 0, anglePath));
				pathIndicators[index1].transform.rotation = targetAnglePath;

				index1++;
			} else 
				if ( Vector2.Distance (point, points [points.Count -1]) > 0.25f && index1!=pathIndicators.Count-2) {
					points.Add (point);
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
			
			if(Vector2.Distance (point, helipadLoc )< 0.2f){
				points.Add(helipadLoc);
				canControl = false;
				lockedTargetColor();
				locked = true;
			}
			/*if (SceneManager.GetActiveScene ().name == "4") {
			if(Vector2.Distance (point, helipadLoc2 )< 0.2f){
				points.Add(helipadLoc2);
				canControl = false;
				lockedTargetColor();
				locked = true;
			}
			}*/

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
			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),helipadLoc) < 0.6f) && locked){

				speed = 0.3f;

				shadow = true;
				hasLanded = true;
				takeTaxiControl = true;
				rotationSpeed = 5f;
				accuracy = 0.1f;
				gameObject.GetComponent<Animator>().SetTrigger("shrink");
				//this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
				trigger = true;
				if(PlayerPrefs.GetInt("music",1) == 1){
				if(!isAudioPlayed){
					isAudioPlayed = true;
					gameObject.GetComponent<AudioSource>().Play();
				}
				}
			}
			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),helipadLoc) < 0.1f) && locked){
				speed = 0.1f;
			//	PlayerPrefs.SetInt("count",PlayerPrefs.GetInt("count",0)+1);
			//	g.coinUpdater();
				//this.gameObject.SetActive(false);
			}
//			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),helipadLoc) < 0.1f) && locked){
//				//speed = 0f;
//			}
			if (SceneManager.GetActiveScene ().name == "4") {
			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),helipadLoc2) < 0.6f) && locked){

				speed = 0.3f;

				shadow = true;
				hasLanded = true;
				takeTaxiControl = true;
				rotationSpeed = 5f;
				accuracy = 0.1f;
				gameObject.GetComponent<Animator>().SetTrigger("shrink");
				//this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
				trigger = true;
				if(PlayerPrefs.GetInt("music",1) == 1){
				if(!isAudioPlayed){
					isAudioPlayed = true;
					gameObject.GetComponent<AudioSource>().Play();
				}
				}
			}
			}
			if (SceneManager.GetActiveScene ().name == "4") {
			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),helipadLoc2) < 0.1f) && locked){
				//speed = 0.1f;
				//	PlayerPrefs.SetInt("count",PlayerPrefs.GetInt("count",0)+1);
				//	g.coinUpdater();
				//this.gameObject.SetActive(false);
			}
			}
			//if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),helipadLoc2) < 0.1f) && locked){
			//	speed = 0f;
			//}

			//			if((Vector2.Distance(new Vector2(transform.position.x,transform.position.y),navPoints[9]) < 0.3f) && hasLanded ){
			//				shouldStop = true;
			//			}
			if(points.Count==0 && hasLanded && !isAlreadyCalled){
				isAlreadyCalled = true;
				g.coinUpdater();
				PlayerPrefs.SetInt("count",PlayerPrefs.GetInt("count",0)+1);
				shouldStop = true;
				takeTaxiControl = true;
				//this.gameObject.SetActive(false);
				respawner();
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
	void shadowAnimation(){
		transform.GetChild (1).position = Vector3.Slerp (transform.GetChild(1).position, transform.position, 1f*Time.deltaTime);

	}
	void OnBecameVisible(){
		isOnScreen = true;

	}
	void OnBecameInvisible(){
		isOnScreen = false;

	}

	void targetArrow(){

		/*GameObject pointer =  transform.GetChild (3).gameObject;
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
		}*/
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
	


}

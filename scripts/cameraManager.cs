using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour {



	public SpriteRenderer reference;

	void Start () {
		float screenRatio = (float)Screen.width / (float)Screen.height;
		float targetRatio = reference.bounds.size.x / reference.bounds.size.y;
		print("Width Reference "+ reference.bounds.size.x);
		if(screenRatio >= targetRatio){
			Camera.main.orthographicSize = reference.bounds.size.y / 2;
		}else{
			float differenceInSize = targetRatio / screenRatio;
			//Camera.main.orthographicSize = reference.bounds.size.y / 2 * differenceInSize;
			Camera.main.orthographicSize = reference.bounds.size.y / 2;
		}
	}
}

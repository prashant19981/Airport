using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carrier : MonoBehaviour {
	Vector3 startPos;
	void Start () {
		startPos = transform.position;
	}

	void Update () {
		
			StartCoroutine (shipMover ());

	}
	IEnumerator shipMover(){
		float x = Random.Range (startPos.x, startPos.x-0.3f);
		float y = Random.Range (startPos.y, startPos.y + 0.3f);
		transform.position = Vector3.Lerp (transform.position, new Vector3 (x, y, transform.position.z), 1f * Time.deltaTime);
		yield return  new WaitForSeconds(1f);
		transform.position = Vector3.Lerp (transform.position, new Vector3 (startPos.x,startPos.y,transform.position.z), 1f * Time.deltaTime);

	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stormManager : MonoBehaviour {

	Color alpha;
	float value;
	SpriteRenderer sprite;
	private bool changeValue;
	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		alpha = sprite.color;
		changeValue = false;
		StartCoroutine (stormManagers ());
	}

	IEnumerator stormManagers(){
		value = Random.Range (0, 0.95f);
		yield return new WaitForSeconds (5f);
		StartCoroutine (stormManagers ());
	}
	void Update(){
		
		alpha.a = Mathf.Lerp (alpha.a, value, 1f*Time.deltaTime);
			sprite.color = alpha;




	}
}

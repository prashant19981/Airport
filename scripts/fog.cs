using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fog : MonoBehaviour {
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
		////if (PlayerPrefs.GetInt ("count", 0) >= 2)
			changeValue = true;

	    value = Random.Range (0.3f, 0.75f);
		yield return new WaitForSeconds (5f);
		StartCoroutine (stormManagers ());
	}
	void Update(){
		if (changeValue) {
			alpha.a = Mathf.Lerp (alpha.a, value, 1f * Time.deltaTime);
			sprite.color = alpha;
		}



	}
}

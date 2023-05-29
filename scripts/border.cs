using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class border : MonoBehaviour {

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Airplane"|| other.tag == "jet" || other.tag == "Helicopter") {
			other.transform.Rotate (0, 0, 180);
		}
	}
}

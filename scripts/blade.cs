using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blade : MonoBehaviour {


	void Update () {
		transform.Rotate (Vector3.forward*1000f*Time.deltaTime);	
	}
}

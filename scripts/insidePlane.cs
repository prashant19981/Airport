using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class insidePlane : MonoBehaviour {

	private bool isActive = false;

	public void setIsActive(bool val){
		this.isActive = val;
	}
	public bool getIsActive(){

		return this.isActive;
	}

}

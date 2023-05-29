using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class levelinfo  {
	private bool isUnlocked;
	public levelinfo(bool value){
		isUnlocked = value;
	}
	public bool getStatus(){
		return this.isUnlocked;
	}
	public void setStatus(bool val){
		this.isUnlocked = val;
	}
}

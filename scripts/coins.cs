using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class coins  {
	private int coin;
	private long timer;
	private bool isAdsPurchased;
	private bool is4crashPurchased;
	private bool isILSPurchased;
	public void setCoins(int val){
		this.coin = val;
	}
	public int getCoins(){
		return coin;
	}
	public void setTimer(long time){
		this.timer = time;
	}
	public long getTimer(){
		return this.timer;
	}
	public void setAdsPurchase(bool value){
		this.isAdsPurchased = value;
	}
	public bool getAdsPurchase(){
		return this.isAdsPurchased;
	}
	public void setIs4crashPurchased(bool value){
		this.isAdsPurchased = value;
	}
	public bool getIs4CrashPurchased(){
		return this.isAdsPurchased;
	}
	public void setIsILSPurchased(bool value){
		this.isILSPurchased = value;
	}
	public bool getIsILSPurchased(){
	   return this.isILSPurchased;
	}
}

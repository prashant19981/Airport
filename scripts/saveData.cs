using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class saveData : MonoBehaviour {

	public static coins c = new coins();
	public static List<levelinfo> l = new List<levelinfo>();
	//it's static so we can call it from anywhere


	public static void saveCoins(){
		string destination = Application.persistentDataPath + "/ad.aw";
		FileStream filestream;
		if (File.Exists (destination)) {
			filestream = File.OpenWrite (destination);
		} else {
			filestream = File.Create (destination);

		}
		BinaryFormatter b = new BinaryFormatter ();
		b.Serialize (filestream, c);
		filestream.Close ();

	}
	public static void loadCoins(){
		string destination = Application.persistentDataPath +"/ad.aw";
		FileStream filestream;
		if (File.Exists (destination)) {
			BinaryFormatter b = new BinaryFormatter ();
			filestream = File.OpenRead (destination);
			saveData.c = (coins)b.Deserialize (filestream);
			filestream.Close ();
		} else {
			Debug.Log ("FILE DOES NOT EXIST");
		}
	}

	public static void saveLevelInfo(){
		string destination = Application.persistentDataPath + "/ak.aw";
		FileStream files;
		if (File.Exists (destination)) {
			files = File.OpenWrite (destination);
		} else {
			files = File.Create (destination);

		}
		BinaryFormatter br = new BinaryFormatter ();
		br.Serialize (files,l);
		files.Close ();

	}
	public static void loadlevelInfo(){
		string destination = Application.persistentDataPath +"/ak.aw";
		FileStream files;
		if (File.Exists (destination)) {
			BinaryFormatter br = new BinaryFormatter ();
			files = File.OpenRead (destination);
			saveData.l = (List<levelinfo>)br.Deserialize (files);
			files.Close ();

		} else {
			Debug.Log ("FILE DOES NOT EXIST");
		}
	}
	public static void deleteCoins(){
	 string destination = Application.persistentDataPath + "/ad.aw";
		if (File.Exists (destination)) {
			File.Delete (destination);
		}
	}
	//	public static void Edit(int index,int size,bool value){
	//	
	//		SaveLoad.Load ();
	//		saveInstances [index].setDs(value);
	//		if (File.Exists (Application.persistentDataPath + "/savedGames.gd")) {
	//		
	//			BinaryFormatter bf = new BinaryFormatter();
	//			FileStream file = File.Create (Application.persistentDataPath + "/gs.gd"); 
	//			bf.Serialize(file, SaveLoad.saveInstances);
	//			file.Close();
	//		
	//		
	//		}


}

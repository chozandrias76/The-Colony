using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using SQLite;

public class StoredPlayerPrefs
{
	public class PlayerPref
	{
		[PrimaryKey]
		public int ID;
		
		[Indexed]
		public int playerChoice;
		public string gender;
		public string age;
		public string playerName;
		public string facialHair;
		public string hair;
		public Color FacialHairColor;
		public Color HairColor;
		public string underwear;
		public BaseJobs[] jobPrefrences = new BaseJobs[3];
		bool beAngatonist;
		bool beAlien;
		bool beRevHead;
		bool beHead;
		bool beCaptain;
		bool beAi;
		bool randJob;
		bool randAppearance;
	}
	//TODO: Collect preferences and save them when the player does
	//TODO: Think about saving prefrences on player computer instead of server
	
	private int _ownerID;

	private int _playerChoice = 0;

	public int playerChoice {
		get {
			return _playerChoice;
		}
		set {
			if (0 <= value && value >= 2) {
				_playerChoice = value;
			}
		}
	}
	
	public StoredPlayerPrefs()
	{
		//var db = new SQLiteConnection (Application.dataPath + "The Colony Server.db");
		//TODO: Figure out if this creates the array of tables
	}
	public void GetPlayerPrefs ()
	{
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update ()
	{

	}
}

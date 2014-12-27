#region License

// // StationData.cs
// //  
// //  Author:
// //        <colin.p.swensonh@gmail.com>
// // 
// //  Copyright (c) 2013 swensonhcp
// // 
// //  This program is free software: you can redistribute it and/or modify
// //  it under the terms of the GNU General Public License as published by
// //  the Free Software Foundation, either version 3 of the License, or
// //  (at your option) any later version.
// // 
// //  This program is distributed in the hope that it will be useful,
// //  but WITHOUT ANY WARRANTY; without even the implied warranty of
// //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //  GNU General Public License for more details.
// // 
// //  You should have received a copy of the GNU General Public License
// //  along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using UnityEngine;

public class StationData
{
	public static StationData Singleton;
	public static string stationStatusCurrent;
	private readonly string[] stationStatus =
    {
        "Station under attack by Syndacates",
        "Clowns have invaded the station",
        "Station self-destruct activated",
        "Shuttle has left the station",
        "Shuttle has arrived at the station",
        "Aliens have attacked the station",
        "There is a revolution on the station"
    };
	private int _timeLeft;
	private float currentTime = Time.time;
	private float shuttleCalled;

	public static int stationBudget
	{
		get { return QMData.sectionBudget; }
		set { }
	}

	public static int stationExpenses
	{
		get { return QMData.sectionExpenses; }
		set { }
	}

	public static int currentStatus { get; set; }

	public string CurrentStatus
	{
		get { return stationStatusCurrent; }
		set { }
	}

	public static int TimeLeft { get; set; }

	private void Awake ()
	{
 var db = new SQLiteConnection (Application.dataPath + "The Colony Server.db");
		db.CreateTable<PlayerManifest> ();
		db.CreateTable<JobRoleManifest> ();
		db.CreateTable<MedicalManifest> ();
		db.CreateTable<SecurityManifest> ();
		db.CreateTable<StoredPlayerPrefs.PlayerPref>();
	}

	public static void AddPlayer (PlayerCharacter pc, IDBadge pcIDBadge)
	{
		//var db = new SQLiteConnection (Application.dataPath + "The Colony Server.db");
		//TODO: Look up command for adding to table
	}

	public static IEnumerable<JobRoleManifest> QueryForJob (SQLiteConnection db, string jobName)
	{
		return
            db.Query<JobRoleManifest> (
                "SELECT Name,Job,HireDate,CurrentLocation,Notes FROM playermanifest GROUP BY Name HAVING Job like '?';",
                jobName);
	}

	public static IEnumerable<JobRoleManifest> QueryForHop (SQLiteConnection db)
	{
		return
			db.Query<JobRoleManifest> (
				"Select Name,Job,HireDate,CurrentLocation,Notes FROM playermanifest GROUP BY Job;");
	}

	public static IEnumerable<PlayerManifest> QueryForAll (SQLiteConnection db)
	{
		return
			db.Query<PlayerManifest> (
				"Select Name,Job,HireDate,HealthStatus,CurrentLocation,Notes FROM playermanifest GROUP BY Job;");
	}

	public static IEnumerable<SecurityManifest> QueryForSecurity (SQLiteConnection db)
	{
		return
			db.Query<SecurityManifest> (
				"Select Name,Job,CurrentLocation,WantedLevel,Notes FROM playermanifest GROUP BY Job;");
	}

	public static IEnumerable<MedicalManifest> QueryForMedical (SQLiteConnection db)
	{
		return
			db.Query<MedicalManifest> (
				"Select Name,HealthStatus,CurrentLocation,Notes FROM playermanifest GROUP BY Job;");
	}

	public static IEnumerable<string> StringsFromJobManifest (IEnumerable<JobRoleManifest> manifests)
	{
		foreach (JobRoleManifest manifest in manifests)
		{
			yield return String.Format("{0} {1} {2} {3}",
				manifest.PlayerName,
				manifest.PlayerJob,
				manifest.CurrentLocation,
				manifest.Notes);
		}
	}

	public static IEnumerable<string> StringsFromMedicalRecords (IEnumerable<MedicalManifest> manifests)
	{
		foreach (MedicalManifest manifest in manifests)
		{
			yield return String.Format("{0} {1} {2} {3}",
				manifest.PlayerName,
				manifest.HealthStatus,
				manifest.CurrentLocation,
				manifest.Notes);
		}
	}

	public static IEnumerable<string> StringsFromSecurityRecords (IEnumerable<SecurityManifest> manifests)
	{
		foreach (SecurityManifest manifest in manifests)
		{
			yield return String.Format("{0} {1} {2} {3}",
				manifest.PlayerName,
				manifest.PlayerJob,
				manifest.CurrentLocation,
				manifest.Notes);
		}
	}

	public static IEnumerable<string> StringsFromPlayerManifest (IEnumerable<PlayerManifest> manifests)
	{
		foreach (PlayerManifest manifest in manifests)
		{
			yield return String.Format("{0} {1} {2} {3} {4} {5} {6}",
				manifest.PlayerName,
				manifest.PlayerJob,
				System.Convert.ToString(manifest.SecurityStatus),
				manifest.HealthStatus,
				manifest.WantedLevel,
				manifest.CurrentLocation,
				manifest.Notes);
		}

	}

	private static List<string> SQLGetManifest (string headRole)
	{
		//string outString;
		var outStrings = new List<string> ();
		var sqlLiteConnection = new SQLiteConnection (Application.dataPath + "The Colony Server.db");
		if (headRole == "Security Head")
		{
			IEnumerable<string> securityWantedLevelStrings = StringsFromSecurityRecords(QueryForSecurity (sqlLiteConnection));
			outStrings = securityWantedLevelStrings.ToList ();
		}
		else if (headRole == "HoP")
		{
			IEnumerable<string> hopStrings = StringsFromJobManifest (QueryForHop (sqlLiteConnection));
			outStrings = hopStrings.ToList ();
		}
		else if (headRole == "All")
		{
			IEnumerable<string> allPlayerStrings = StringsFromPlayerManifest (QueryForAll (sqlLiteConnection));
			outStrings = allPlayerStrings.ToList ();
		}
		else
		{
			IEnumerable<string> jobRoleManifestStrings = StringsFromJobManifest (QueryForJob (sqlLiteConnection, headRole));

			outStrings = jobRoleManifestStrings.ToList ();
		}
		return outStrings;
	}

	private void Start ()
	{
	}

	private void Update ()
	{
		currentTime = Time.time;
		if (shuttleCalled < currentTime && shuttleCalled != 0)
		{
			_timeLeft = (int)currentTime - (int)shuttleCalled;
			if (_timeLeft >= 3000.0f)
			{
				currentTime = Time.time;
				shuttleCalled = currentTime + 3000.0f;
				currentStatus = 4;
				stationStatusCurrent = stationStatus [currentStatus];
			}
		}
		else if (shuttleCalled > currentTime)
		{
			_timeLeft = (int)currentTime - (int)shuttleCalled;
			if (_timeLeft > 0)
			{
				shuttleCalled = 0;
				currentStatus = 3;
				stationStatusCurrent = stationStatus [currentStatus];
			}
		}
	}

	public class JobRoleManifest
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Indexed]
		public string PlayerName { get; set; }
		public string PlayerJob { get; set; }
		public DateTime HireDate { get; set; }
		public string CurrentLocation { get; set; }
		public string Notes { get; set; }

		public IEnumerable<string> GetEnumerator ()
		{
			yield return PlayerName;
			yield return PlayerJob;
			//yield return HireDate;
			yield return CurrentLocation;
			yield return Notes;
		}
	}

	public class MedicalManifest
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Indexed]
		public string PlayerName { get; set; }

		public string HealthStatus { get; set; }

		public string CurrentLocation { get; set; }

		public string Notes { get; set; }

		public IEnumerable<string> GetEnumerator ()
		{
			yield return PlayerName;
			yield return HealthStatus;
			yield return CurrentLocation;
			yield return Notes;
		}
	}

	public class SecurityManifest
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Indexed]
		public string PlayerName { get; set; }
		public string PlayerJob { get; set; }
		public int SecurityStatus { get; set; }
		public string CurrentLocation { get; set; }
		public string WantedLevel { get; set; }
		public string Notes { get; set; }

		public IEnumerable<string> GetEnumerator ()
		{
			yield return PlayerName;
			yield return PlayerJob;
            yield return System.Convert.ToString(SecurityStatus);
			yield return CurrentLocation;
			yield return WantedLevel;
			yield return Notes;
		}
	}

	public class PlayerManifest
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Indexed]
		public string PlayerName { get; set; }
		public string PlayerJob { get; set; }
		public DateTime HireDate { get; set; }
		public int SecurityStatus { get; set; }
		public string HealthStatus { get; set; }
		public string CurrentLocation { get; set; }
		public string WantedLevel { get; set; }
		public string Notes { get; set; }

		public IEnumerable<string> GetEnumerator ()
		{
			yield return PlayerName;
			yield return PlayerJob;
			//yield return HireDate;
			yield return HealthStatus;
			yield return CurrentLocation;
			yield return WantedLevel;
			yield return Notes;
		}
	}

	public class RestrictedData01
	{
		public static List<string> GetData (int securityStatus)
		{
			var dataStrings = new List<string> ();
			var manifestData = new List<string> ();
			switch (securityStatus)
			{
				case 19:
					manifestData = SQLGetManifest ("Cargo Technician");
					dataStrings = manifestData;

					break;
				case 20:
					manifestData = SQLGetManifest ("Miner");
					dataStrings = manifestData;

                    //data.Add("");//MiningHead = 20
					break;
				case 21:
					manifestData = SQLGetManifest ("Station Engineer");
					//manifestData += SQLGetManifest ("Atmospheric Technician");
					dataStrings = manifestData;
                    //data.Add("");//EngineeringHead = 21
					break;
				case 22:
					manifestData = SQLGetManifest ("Medical Doctor");
					dataStrings = manifestData;
                    //data.Add("");//MedicalHead = 22
					break;
				case 23:
					manifestData = SQLGetManifest ("Scientist");
					//manifestData += SQLGetManifest ("Geneticist");
					//manifestData += SQLGetManifest ("Roboticist");
					dataStrings = manifestData;
                    //data.Add("");//ScienceHead = 23
					break;
				case 24:
					manifestData = SQLGetManifest ("Security Officer");
					//manifestData += SQLGetManifest ("Warden");
					//manifestData += SQLGetManifest ("Lawyer");
					dataStrings = manifestData;
                    //data.Add("");//SecurityHead = 24
					break;
				default:
                    //data.Add("You do not have access to this data.");
                    //return data;
					dataStrings.Add ("You do not have access to this data.");
					break;
			}

			return dataStrings;
		}
	}

	public class RestrictedData02
	{
		public static List<string> GetData (int securityStatus)
		{
			var dataStrings = new List<string> ();
			var manifestData = new List<string> ();
			switch (securityStatus)
			{
				case 25:
					manifestData = SQLGetManifest ("HoP");
					dataStrings = manifestData;
                    //data.Add("");//HOP
					break;
				case 26:
					manifestData = SQLGetManifest ("All");
					dataStrings = manifestData;
                    //data.Add("");//AI
					break;
				default:
                    //data.Add("You do not have access to this data.");
					dataStrings.Add ("You do not have access to this data.");

					break;
			}
			return dataStrings;
		}
	}

	public class RestrictedData03
	{
		public static List<string> GetData (int securityStatus)
		{
			var dataStrings = new List<string> ();
			var manifestData = new List<string> ();
			switch (securityStatus)
			{
				case 27:
					manifestData = SQLGetManifest ("All");
					dataStrings = manifestData;
                    //data.Add("");//Captain
					break;
				case 28:
					manifestData = SQLGetManifest ("All");
					dataStrings = manifestData;
                    //data.Add("");//Admin
					break;
				default:
					dataStrings.Add ("You do not have access to this data.");
                    //data.Add("You do not have access to this data.");
					break;
			}
			return dataStrings;
		}
	}
}
#region License

// // QMData.cs
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
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class QMData
{
	public static int sectionBudget;
	public static int sectionExpenses;
	public static List<QMRequisitionForms> possibleForms = new List<QMRequisitionForms> ();
	public static List<QMRequisitionForms> formsToProcess = new List<QMRequisitionForms> ();
	public static List<QMRequisitionForms> formsApproved = new List<QMRequisitionForms> ();
	public static List<QMRequisitionForms> formsDenied = new List<QMRequisitionForms> ();
	public static List<Quote> quotes = new List<Quote> ();

	public static int _timeLeft;
	public static int currentStatusIndex;
	public static float currentTime = Time.time;
	public static bool shuttleCalled = false;
	public static float shuttleCalledTime;
	public static string shuttleStatusCurrent;

	public static int TimeLeft
	{
		get { return _timeLeft; }
		set { }
	}
	
	private void Start ()
	{
		
	}
	
	void Update ()
	{
		currentTime = Time.time;
		if (shuttleCalledTime < currentTime && shuttleCalledTime != 0)//Shuttle Arrived
		{
			_timeLeft = 0;
			shuttleCalledTime = 0.0f;
			currentStatusIndex = 1;
			shuttleCalled = false;
			//shuttleStatusCurrent = shuttleStatus[currentStatusIndex];
			foreach (Quote quote in quotes)
			{
				if (quote.statusIndex == 2)
				{
					quote.statusIndex++;
				}
			}
		}
		else if (shuttleCalledTime > currentTime)//Shuttle is away from station
		{
			if (shuttleCalled)//Shuttle returning to station
			{
				_timeLeft = (int)currentTime - (int)shuttleCalledTime;
				if (_timeLeft > 0)
				{
					//shuttleCalledTime = 0;
					currentStatusIndex = 0;
					//shuttleStatusCurrent = shuttleStatus [currentStatusIndex];
				}
			}
			else if (!shuttleCalled)//Shuttle sitting at hub
			{
				shuttleCalledTime = currentTime + 3000.0f;
				currentStatusIndex = 2;
				foreach (Quote quote in quotes)
				{
					if (quote.statusIndex == 0)
					{
						quote.statusIndex = 1;
					}

				}
				//shuttleStatusCurrent = shuttleStatus [currentStatusIndex];
			}
		}
	}
	
	public class Quote
	{
		public Quote (string who, string whatSection, QMRequisitionForms form)
		{
			orderedBy = who;
			section = whatSection;
			thisForm = form;
		}

		QMRequisitionForms thisForm;
		public string authorizedBy;
		public string orderedBy, section;
		public string[] orderStatus = 
		{
			"In Queue",
			"Out for Pickup",
			"Out for Delivery",
			"Ready for Pickup",
			"Being Delivered",
			"Delivered and Completed"
		};
		public int statusIndex;

		public override string ToString ()
		{
			return System.String.Format ("{0} ordered {1} from {2} section/n The status of this order is: {3}/n", orderedBy, thisForm.ToString (), section, orderStatus [statusIndex]);
		}

//		static readonly string[] shuttleStatus =
//	    {
//	        "Shuttle is in transit",
//	        "Shuttle is at the loading dock",
//			"Shuttle is waiting at the hub"
//	    };
		
	}

}
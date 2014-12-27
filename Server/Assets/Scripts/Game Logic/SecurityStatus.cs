#region License

// // SecurityStatus.cs
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

public class SecurityStatus
{
	//<summary>
	//This class handles a system for setting clearances based on your job role. 
	//Currently set so that roles have more than one clearance level as part of their job.
	//</summary>
    public enum GeneralSecurityClearance
    {
        Tir0 = 0,
        Tir1,
        Tir2,
        Tir3,
    };

    public enum JobSecurityClearance
    {
        #region Tir0

        None = 0,
        Bar = 1,
        Kitchen = 2,
        GreenHouse = 3,
        CargoSpace = 4,
        Janitorial = 5,
        Library = 6,
        MiningOutpost = 7,
        LawOffices = 8,
        ReligiousArea = 9,
        Engineering = 10,
        Atmos = 11,
        Medical = 12,
        Chemistry = 13,
        Science = 14,
        Robotics = 15,
        Security = 16,
        Detective = 17,
        Warden = 18,

        #endregion

        #region Tir1

        CargoHead = 19,
        MiningHead = 20,
        EngineeringHead = 21,
        MecialHead = 22,
        ScienceHead = 23,
        SecurityHead = 24,

        #endregion

        #region Tir2

        HoP = 25,
        AI = 26,

        #endregion

        #region Tir3

        Captain = 27,

        #endregion

        Unlimited = 28
    };


    //public static int[][] jobSecurityClearance = new int[31][];
	public int[] idClearance = new int[2];

    /*
    {
        //corgi 0
        //assistant,1
    
        //bartender,2
        //chef,3
        //botanist,4
        //cT,5
        //janitor,6
        //librarian,7
        //miner,8
        //lawyer,9
        //chaplain,10
        //sE,11
        //aT,12
        //mD,13
        //chemist,14
        //geneticist,15
        //virologist,16
        //scientist,17
        //roboticist,18
        //sO,19
        //detective,20
        //warden,21

        //qM,22
        //cMD,23
        //cE,24
        //cMO,25
        //rD,26
        //hoS,27

        //hoP,28
        //aI29
        //captain,30
    Unlimited31
    };*/

//    public static int[] GetJobClearance(string jobName)
//    {
//        // convert string to enum, invalid cast will throw an exception
//        int _jobSecurityClearance =
//            (int) Enum.Parse(typeof (JobSecurityClearance), jobName);
//
//        // convert an enum to an int
//        int generalClearance;
//		if (jobClearance <= 18) {
//			generalClerance = 0;
//		} else if (jobClearance > 18 && jobClearance <= 24) {
//			generalClerance = 1;
//		} else if (jobClearance > 24 && jobClearance <= 26) {
//			generalClerance = 2;
//		} else if (jobClearance = 27) {
//			generalClerance = 3;
//		} else if (jobClearance = 28) {
//			generalClerance = 4;
//		} else {
//			generalClerance = 0;
//		}
//		int[] returnValue = new int[2];
//		returnValue [0] = _jobSecurityClearance;
//		returnValue [1] = generalClearance;
//		return returnValue;
//    }

    public static bool OnCheckClearance(int[] objectClerances, int[] objectClearanceRequirements)
    {
        //int highestClearance;
        return true;
    }

    // Use this for initialization
    private void Start()
    {
		
		
    }


    // Update is called once per frame
    private void Update()
    {
    }
}
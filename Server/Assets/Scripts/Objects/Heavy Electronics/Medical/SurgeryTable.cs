#region License

// // SurgeryTable.cs
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
using uLink;
using UnityEngine;

public class SurgeryTable : HeavyElectronic
{
    public bool attached = false;
    public bool computerUsing = false;

    private string consoleString;
    public bool manual = true;
    public bool modeHalted = false;
    public bool patientUsing = false;
    public bool playerUsing = false;
    public PlayerCharacter patient;
    // Use this for initialization
    private void Start()
    {
        powerRequirement = 1.0f;
        consoleString =
            String.Format("Welcome to the Surgery Table interface. What operation would you like to run:/n");
        string[] menuModes = Enum.GetNames(typeof (AutomaticModes));
        for (int i = 0; i < menuModes.Length; i++)
        {
            consoleString += String.Format("{0}){1}/n", i, menuModes[i]);
        }
    }

    public class SurgeryOperations
    {
        private PlayerCharacter patientPlayerCharacter;
        public SurgeryOperations(PlayerCharacter pc)
        {
            patientPlayerCharacter = pc;
        }
        public void SawHead()
        {
            patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int) CharacterVitals.DamageLocations.Head][(int) CharacterVitals.DamageTypes.Sharp] += 0.2f;
        }

        public void AugmentGenitals()
        {
            
        }

        public void AugmentFace()
        {
            
        }

        public void FixEye()
        {
            patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int)CharacterVitals.DamageLocations.Eyes][(int)CharacterVitals.DamageTypes.Sharp] += 0.1f;
            patientPlayerCharacter.playerAttributes.RemoveDisease((int)BaseDisease.diseases.Myopia);
        }

        public void SewOperation(int mode)
        {
            switch (mode)
            {
                case 0:
                    patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int)CharacterVitals.DamageLocations.Eyes][(int)CharacterVitals.DamageTypes.Sharp] -= 0.1f;
                    break;
                case 1:
				patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int) CharacterVitals.DamageLocations.Head][(int) CharacterVitals.DamageTypes.Sharp] -= 0.4f;
				patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int) CharacterVitals.DamageLocations.Head][(int) CharacterVitals.DamageTypes.Brute] -= 0.2f;
                    break;
                case 2:
				patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int) CharacterVitals.DamageLocations.Chest][(int) CharacterVitals.DamageTypes.Sharp] -= 0.2f;
				patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int) CharacterVitals.DamageLocations.Chest][(int) CharacterVitals.DamageTypes.Brute] -= 0.2f;
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
            }
        }

        public void CutOperation(int mode)
        {
            switch (mode)
            {
                case 0:
                    patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int) CharacterVitals.DamageLocations.Eyes][(int) CharacterVitals.DamageTypes.Sharp] += 0.1f;
                    break;
                case 1:
				 patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int) CharacterVitals.DamageLocations.Head][(int) CharacterVitals.DamageTypes.Sharp] += 0.2f;
                    break;
                case 2:
					patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int) CharacterVitals.DamageLocations.Chest][(int) CharacterVitals.DamageTypes.Sharp] += 0.3f;
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
            }
        }

        public void RetractOperation(int mode)
        {
            switch (mode)
            {
                case 0:
                    patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int)CharacterVitals.DamageLocations.Eyes][(int)CharacterVitals.DamageTypes.Brute] += 0.1f;
                    break;
                case 1:
				patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int) CharacterVitals.DamageLocations.Head][(int) CharacterVitals.DamageTypes.Brute] += 0.2f;
                    break;
                case 2:
				patientPlayerCharacter.playerAttributes.ourVitals.damageValues[
                        (int) CharacterVitals.DamageLocations.Chest][(int) CharacterVitals.DamageTypes.Brute] += 0.3f;
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
            }
        }
		public Item implant;
        public void RemovePartOperation(int mode)
        {
            switch (mode)
            {
                case 0:
                    break;
                case 1:
				if(patientPlayerCharacter.playerAttributes.brainAttached)
				{
					
				}
                    break;
                case 2:
				if(patientPlayerCharacter.playerAttributes.chestImplant != null)
				{
					implant = patientPlayerCharacter.playerAttributes.chestImplant;
					patientPlayerCharacter.playerAttributes.chestImplant = null;
				}
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
            }
        }

        public void AddpartOperation(int mode)
        {
            switch (mode)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
            }
        }

        public void ComputerDoOperation(int automaticMode)
        {
            switch (automaticMode)
            {
                case 0://Eye Surgery
                    CutOperation(automaticMode);
                    RetractOperation(automaticMode);
                    FixEye();
                    SewOperation(automaticMode);
                    break;
                case 1://brain surgery
                    SawHead();
                    CutOperation(automaticMode);
                    RetractOperation(automaticMode);
                    RemovePartOperation(automaticMode);
                    SewOperation(automaticMode);
                    break;
                case 2://implant removal
                    CutOperation(automaticMode);
                    RetractOperation(automaticMode);
                    RemovePartOperation(automaticMode);
                    SewOperation(automaticMode);
                    break;
                case 3://appendix removal
                    CutOperation(automaticMode);
                    RetractOperation(automaticMode);
                    RemovePartOperation(automaticMode);
                    SewOperation(automaticMode);
                    break;
                case 4://parasite removal
                    CutOperation(automaticMode);
                    RetractOperation(automaticMode);
                    RemovePartOperation(automaticMode);
                    SewOperation(automaticMode);
                    break;
                case 5://lipo
                    CutOperation(automaticMode);
                    RetractOperation(automaticMode);
                    RemovePartOperation(automaticMode);
                    SewOperation(automaticMode);
                    break;
                case 6://plastic surgery
                    CutOperation(automaticMode);
                    RetractOperation(automaticMode);
                    AugmentFace();
                    SewOperation(automaticMode);
                    break;
                case 7://sex change
                    CutOperation(automaticMode);
                    RetractOperation(automaticMode);
                    AugmentGenitals();
                    SewOperation(automaticMode);
                    break;
                case 8://cavity implant
                    CutOperation(automaticMode);
                    RetractOperation(automaticMode);
                    //Halt mode
                    SewOperation(automaticMode);
                    break;
                default:
                    break;
            }
        }

    }
    // Update is called once per frame
    private void Update()
    {
    }

    public override void Main()
    {
    }

    private int surgeryMode = -1;
    private void OnManualSurgery(PlayerCharacter pc)
    {
        if (attached)
        {
            if (computerUsing)
            {
                if (modeHalted)
                {
                    //Player or computer can use
                }
            }
        }
    }

    private void OnComputerSurgery()
    {
        if (attached)
        {
            if (computerUsing)
            {
                if (modeHalted)
                {
                    //Computer can use
                    //switch(Input.inputString)
                }
                if (!modeHalted)
                {
                    SurgeryOperations runSurgeryOperations = new SurgeryOperations(patient);
                    runSurgeryOperations.ComputerDoOperation(surgeryMode);
                }
            }
        }
    }

    public override void OnPlayerUsingThis(PlayerCharacter pc)
    {
        
        if (manual)
        {
            OnManualSurgery(pc);
        }

        else if (!manual)
        {
            OnComputerSurgery();
        }
    }

//    public override void OnUsing()
//    {
//    }

    private enum AutomaticModes
    {
        EyeSurgery,
        BrainSurgery,
        ImplantRemoval,
        Appendectomy,
        ParasiteRemoval,
        Lipoplasty,
        FacialReconstruction,
        GenderRearrangement,
        CavityImplant
    }

//	public void AttachToTerminal(MedicalTerminal theTerminal)
//	{
//	}
}
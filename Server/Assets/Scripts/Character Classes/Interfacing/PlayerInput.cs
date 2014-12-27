#region License

// // PlayerInput.cs
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

using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static bool ePress;
    private static GameObject attachedTo;
    private CharacterMotor motor;

    // Use this for initialization
    private void Start()
    {
    }

    private void Awake()
    {
        attachedTo = gameObject;
        ePress = false;
        motor = GetComponent<CharacterMotor>();
    }

    // Update is called once per frame
    private void Update()
    {
        var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (directionVector != Vector3.zero)
        {
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            float directionLength = directionVector.magnitude;
            directionVector = directionVector/directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength = directionLength*directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector = directionVector*directionLength;
        }

        // Apply the direction to the CharacterMotor
        motor.inputMoveDirection = transform.rotation*directionVector;
        motor.inputJump = Input.GetButton("Jump");

        if (Input.GetKeyDown(KeyCode.E))
        {
            //DisableMotor();
			var rayHit = new RaycastHit();
            if (Physics.Raycast(transform.position, transform.forward, out rayHit, ~0))
            {
				if(GetComponent<PlayerCharacter>().playerInventory.thisHandItems[this.gameObject.GetComponent<PlayerCharacter>().HandSelected] == null)
				{
					try
					{
						if(rayHit.collider.GetComponent<HandItems>() == null)//If it is not equipable, use with open hand
						{
							rayHit.collider.GetComponent<Item>().OnPlayerUsingThis(this.GetComponent<PlayerCharacter>());
						}
						
						if(rayHit.collider.GetComponent<HandItems>() != null)//If it is equipable, pick up item
						{
							rayHit.collider.GetComponent<HandItems>().OnPickedUp(GetComponent<PlayerCharacter>());
						}
					}
					catch
					{
					}
				}
				else
				{
					GetComponent<PlayerCharacter>().playerInventory.thisHandItems[this.gameObject.GetComponent<PlayerCharacter>().HandSelected].OnPlayerUsingThis(GetComponent<PlayerCharacter>());
					//By Default, if player has item in hand, use it refrence to the player
				}
			}
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //EnableMotor();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var rayHit = new RaycastHit();
            if (Physics.Raycast(transform.position, transform.forward, out rayHit, ~0))
            {
                try
                {
                    HandItems rHO;
                    if (gameObject.GetComponent<PlayerCharacter>().playerInventory.thisHandItems[0] == null)
                    {
//                        try
//                        {
//                            rayHit.collider.gameObject.GetComponent<Item>().OnUsing(GetComponent<PlayerCharacter>());
//                        }
//                        catch
//                        {
//                            rayHit.collider.gameObject.GetComponent<Item>().OnUsing();
//
//                        }
                        Debug.Log(rayHit.collider.gameObject.GetType().ToString());
                    }
                    else
                    {
//                        gameObject.GetComponent<PlayerCharacter>().playerInventory.thisHandItems[0] = rHO;
//						try
//						{
//							rHO.OnPlayerUsingThis(GetComponent<PlayerCharacter>());
//						}
//						catch
//						{
//							rHO.OnUsing();
//						}
                    }
                }
                catch
                {
                    Debug.LogWarning("No object to use");
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            var rayHit = new RaycastHit();
            if (Physics.Raycast(transform.position, transform.forward, out rayHit, ~0))
            {
                try
                {
                    HandItems lHO;
                    if (gameObject.GetComponent<PlayerCharacter>().playerInventory.thisHandItems[1] == null)
                    {
//                        try
//                        {
//                            rayHit.collider.gameObject.GetComponent<Item>().OnUsing(GetComponent<PlayerCharacter>());
//                        }
//                        catch
//                        {
//                            rayHit.collider.gameObject.GetComponent<Item>().OnUsing();
//
//                        }
                        Debug.Log(rayHit.collider.gameObject.GetType().ToString());
                    }
                    else
                    {
//                        gameObject.GetComponent<PlayerCharacter>().playerInventory.thisHandItems[1] = lHO;
//						try
//						{
//							lHO.OnPlayerUsingThis(GetComponent<PlayerCharacter>());
//						}
//						catch
//						{
//							lHO.OnUsing();
//						}
                    }
                }
                catch
                {
                    Debug.LogWarning("No object to use");
                }
            }
		}
		
		if(Input.GetKeyDown(KeyCode.Q))
		{
			if (gameObject.GetComponent<PlayerCharacter>().playerInventory.thisHandItems[GetComponent<PlayerCharacter>().HandSelected] != null)
                    {
				gameObject.GetComponent<PlayerCharacter>().playerInventory.thisHandItems[GetComponent<PlayerCharacter>().HandSelected].OnPlayerUsingThis();
			}
		}
    }

    public static void DisableMotor()
    {
        attachedTo.GetComponent<CharacterMotor>().movement.maxGroundAcceleration = 0.0f;
        attachedTo.GetComponent<CharacterMotor>().movement.maxAirAcceleration = 0.0f;
        attachedTo.GetComponent<CharacterMotor>().jumping.enabled = false;
        ePress = true;
        ConsoleScreen.currentlyUsing = true;
    }

    public static void EnableMotor()
    {
        attachedTo.GetComponent<CharacterMotor>().movement.maxGroundAcceleration = 30.0f;
        attachedTo.GetComponent<CharacterMotor>().movement.maxAirAcceleration = 20.0f;
        attachedTo.GetComponent<CharacterMotor>().jumping.enabled = true;
        ePress = false;
        ConsoleScreen.currentlyUsing = false;
    }
}
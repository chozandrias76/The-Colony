#region License

// // FPSInputController.cs
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

// Require a character controller to be attached to the same game object
[RequireComponent(typeof (CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]
public class FPSInputController : MonoBehaviour
{
    private CharacterMotor motor;

    // Use this for initialization
    private void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Get the input vector from kayboard or analog stick
        var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (directionVector != Vector3.zero)
        {
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            float directionLength = directionVector.magnitude;
            directionVector = directionVector/directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1.0f, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength = directionLength*directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector = directionVector*directionLength;
        }

        // Apply the direction to the CharacterMotor
        motor.inputMoveDirection = transform.rotation*directionVector;
        motor.inputJump = Input.GetButton("Jump");
    }
}
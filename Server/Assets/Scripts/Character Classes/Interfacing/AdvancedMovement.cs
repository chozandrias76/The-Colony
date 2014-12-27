#region License

// // AdvancedMovement.cs
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

[RequireComponent(typeof (CharacterController))]
public class AdvancedMovement : MonoBehaviour
{
    public enum Forward
    {
        back = -1,
        none = 0,
        forward = 1
    }

    public enum State
    {
        Idle,
        Init,
        Setup,
        Run
    }

    public enum Turn
    {
        left = -1,
        none = 0,
        right = 1
    }

    private CollisionFlags _collisionFlags; //the collisionFlags we have from the last frame.
    private CharacterController _controller; //our cached CharacterController

    private Forward _forward;
    private bool _isSwimming;
    private bool _jump;
    private Vector3 _moveDirection; //This is the direction our character is moving
    private Transform _myTransform; //our cached transform
    private bool _run;

    private State _state;
    private Turn _strafe;
    private Turn _turn;
    public float airTime = 0; //how long have we been in the air since the last time we touched the ground
    public string fallAnimName;
    public float fallTime = .5f; //the length of time we have to be falling before the system knows its a fall
    public float gravity = 20; //the setting for gravity
    public string idleAnimName;
    public string jumpAnimName;
    public float jumpHeight = 8; //the height we move when we are jumping
    public float jumpTime = 1.5f;
    public float rotateSpeed = 250; //the speed our character turns at
    public string runAnimName;
    public float runMultiplier = 2; //how fast the player runs compared to the walk speed
    public string strafeAnimName;
    public float strafeSpeed = 2.5f; //the speed our character strafes at
    public string swimForwardAnimName;
    public string walkAnimName;
    public float walkSpeed = 2; //the speed our character walks at

    //Called before the script starts
    public void Awake()
    {
        _myTransform = transform; //cache our transform
        _controller = GetComponent<CharacterController>(); //cache our charactercontroller

        _state = State.Init;
    }

    // Use this for initialization
    private IEnumerator Start()
    {
        while (true)
        {
            switch (_state)
            {
                case State.Init:
                    Init();
                    break;
                case State.Setup:
                    SetUp();
                    break;
                case State.Run:
                    ActionPicker();
                    break;
            }

            yield return null;
        }
    }

    private void Init()
    {
        if (!GetComponent<CharacterController>()) return;
        if (!GetComponent<Animation>()) return;

        _state = State.Setup;
    }

    private void SetUp()
    {
        _moveDirection = Vector3.zero; //zero our the vector3 we will use for moving our player
        animation.Stop(); //stop any animations that might be set to play automatically
        animation.wrapMode = WrapMode.Loop; //set all animations to loop by default

        if (jumpAnimName != "")
        {
            animation[jumpAnimName].layer = -1; //move jump to a higher layer
            animation[jumpAnimName].wrapMode = WrapMode.Once; //set jump to only play once
        }

        animation.Play(idleAnimName); //start the idle animation when the script starts


        _turn = Turn.none;
        _forward = Forward.none;
        _strafe = Turn.none;
        _run = true;
        _jump = false;
        _isSwimming = false;

        _state = State.Run;
    }

    private void ActionPicker()
    {
        //allow the player to turn left and right
        _myTransform.Rotate(0, (int) _turn*Time.deltaTime*rotateSpeed, 0);


        //if we are on the ground, let us move
        if (_controller.isGrounded || _isSwimming)
        {
            //reset the air timer if we are on the ground
            airTime = 0;

            //get the user input if we should be moving forward or sideways
            //we will calculate a new vector3 for where the player needs to be
            _moveDirection = new Vector3((int) _strafe, 0, (int) _forward);
            _moveDirection = _myTransform.TransformDirection(_moveDirection).normalized;
            _moveDirection *= walkSpeed;

            if (_forward != Forward.none)
            {
                //if user is pressing forward
                if (_isSwimming)
                {
                    Swim();
                }
                else
                {
                    if (_run)
                    {
                        //and pressing the run key
                        _moveDirection *= runMultiplier; //move user at run speed
                        Run(); //play run animation
                    }
                    else
                    {
                        Walk(); //play walk animation
                    }
                }
            }
            else if (_strafe != Turn.none)
            {
                //if user is pressing strafe
                Strafe(); //play strafe animation
            }
            else
            {
                if (_isSwimming)
                {
                }
                else
                {
                    Idle(); //play idle animation
                }
            }

            if (_jump)
            {
                //if the user presses the jump key
                if (airTime < jumpTime)
                {
                    //if we have not already been in the air to long
                    _moveDirection.y += jumpHeight; //move them upwards
                    Jump(); //play jump animation
                    _jump = false;
                }
            }
        }
        else
        {
            //if we have a collisionFlag and it is CollideBelow
            if ((_collisionFlags & CollisionFlags.CollidedBelow) == 0)
            {
                airTime += Time.deltaTime; //increase the airTime

                if (airTime > fallTime)
                {
                    //if we have been in the air to long
                    Fall(); //play the fall animation
                }
            }
        }

        if (!_isSwimming)
            _moveDirection.y -= gravity*Time.deltaTime; //apply gravity

        //move the character and store any new CollisionFlags we get
        _collisionFlags = _controller.Move(_moveDirection*Time.deltaTime);
    }

    public void MoveMeForward(Forward z)
    {
        _forward = z;
    }

    public void ToggleRun()
    {
        _run = !_run;
    }

    public void RotateMe(Turn y)
    {
        _turn = y;
    }

    public void Strafe(Turn x)
    {
        _strafe = x;
    }

    public void JumpUp()
    {
        _jump = true;
    }

    public void IsSwimming(bool swim)
    {
        _isSwimming = swim;
    }


/**
 * Below is a list of all of the animations that every character in the game can perform along with any parameters needed for them to work right
**/
    //Idle animation
    public void Idle()
    {
        if (idleAnimName == "")
            return;

        animation.CrossFade(idleAnimName);
    }

    //walk animation
    public void Walk()
    {
        if (walkAnimName == "")
            return;

        animation.CrossFade(walkAnimName);
    }

    //run animation
    public void Run()
    {
        if (runAnimName == "")
            return;

        animation[runAnimName].speed = 1.5f;
        animation.CrossFade(runAnimName);
    }

    //strafe animation
    public void Strafe()
    {
        if (strafeAnimName == "")
            return;

        animation.CrossFade(strafeAnimName);
    }

    //jump animation
    public void Jump()
    {
        if (jumpAnimName == "")
            return;

        animation.CrossFade(jumpAnimName);
    }

    //fall animation
    public void Fall()
    {
        if (fallAnimName == "")
            return;

        animation.CrossFade(fallAnimName);
    }

    public void Swim()
    {
        if (swimForwardAnimName == "")
            return;

        animation.CrossFade(swimForwardAnimName);
    }
}
#region License

// // MouseLook.cs
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

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation
/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
/// -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
/// -> A CharacterMotor and a CharacterController component will be automatically added.
/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
/// -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float maximumX = 360F;

    public float maximumY = 60F;
    public float minimumX = -360F;
    public float minimumY = -60F;

    private float rotationY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    private void Update()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X")*sensitivityX;

            rotationY += Input.GetAxis("Mouse Y")*sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X")*sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y")*sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    private void Awake()
    {
        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
    }
}
#region License

// // DragMouseOrbit.cs
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

[AddComponentMenu("Camera-Control/Mouse drag Orbit with zoom")]
public class DragMouseOrbit : MonoBehaviour
{
    public float distance = 5.0f;
    public float distanceMax = 15f;
    public float distanceMin = .5f;
    //private float onLoadRotation;

    private float rotationXAxis;
    private float rotationYAxis;
    public float smoothTime = 2f;
    public Transform target;

    private float velocityX;
    private float velocityY;
    public float xSpeed = 120.0f;
    public float yMaxLimit = 80f;
    public float yMinLimit = -20f;
    public float ySpeed = 120.0f;


    // Use this for initialization
    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;
        //onLoadRotation = target.rotation.y - transform.rotation.y;
        //transform.rotation = new Quaternion (onLoadRotation, onLoadRotation, onLoadRotation, target.rotation.w);
        Quaternion.Slerp(transform.rotation, target.rotation, 1.0f);
        // Make the rigid body not change rotation
        if (rigidbody)
        {
            rigidbody.freezeRotation = true;
        }
    }

    private void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(2))
            {
                velocityX += xSpeed*Input.GetAxis("Mouse X")*distance*0.01f;
                //velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
                velocityY = 0.0f;
            }

            rotationYAxis += velocityX;
            rotationXAxis -= velocityY;

            rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);

            // Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
            Quaternion rotation = toRotation;

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);

            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                //distance -= hit.distance;
                distance = 3.2f;
            }
            var negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation*negDistance + target.position;
            position = position + new Vector3(0, 2, 0);

            transform.rotation = rotation;
            transform.position = position;

            velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime*smoothTime);
            velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime*smoothTime);
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
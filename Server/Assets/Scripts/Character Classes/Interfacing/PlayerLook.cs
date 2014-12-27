#region License

// // PlayerLook.cs
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

public class PlayerLook : MonoBehaviour
{
    // Position and Deltas
    public Camera cam;
    public Vector3 deltaCast;
    public Vector3 forward;
    public float forwardDot;
    public Vector3 mouseScreenDelta;
    public Vector2 mouseVector;
    public Vector3 mouseWorldDelta;
    private RaycastHit myhit;
    public Ray myray = new Ray();


    // External Game Objects
    public Transform playerTransform;
    public Vector3 right;
    public float rightDot;
    public Vector3 screenMousePosition;
    public float speed = 4.0f;
    public Vector3 worldMousePosition;

    private void Start()
    {
    }

    private void Update()
    {
        // Grab the screen position of Mouse Cursor
        screenMousePosition = Input.mousePosition;

        // Calculate the MouseScreenDelta
        mouseScreenDelta.x = (screenMousePosition.x - ((float) Screen.width/2))/((float) Screen.width/2);
        mouseScreenDelta.y = (screenMousePosition.y - ((float) Screen.height/2))/((float) Screen.height/2);

        //MouseScreenDelta = MouseScreenDelta.x + MouseScreenDelta.x;

        // Feed Camera v (Height) into ScreenMousePosition
        screenMousePosition.z = cam.transform.position.y;

        // Convert Screen Coords to World Coords
        worldMousePosition = cam.ScreenToWorldPoint(screenMousePosition);

        // Calculate delta offset based on Player Position
        mouseWorldDelta.x = worldMousePosition.x + playerTransform.position.x;
        mouseWorldDelta.z = worldMousePosition.z - playerTransform.position.z;

        //playerTransform.transform.Rotate(Vector3.right, Time.deltaTime); 

        //playerTransform.transform.LookAt(Camera.main.ScreenToWorldPoint(Vector3(mousePos.x, mousePos.y, zDistance)));

        myray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(myray, out myhit, 1000.0f))
        {
            //print(myhit.collider.transform.position);
            deltaCast = (myhit.point - playerTransform.position);
            deltaCast.Normalize();
            forward = playerTransform.transform.TransformDirection(Vector3.down);
            forwardDot = Vector3.Dot(deltaCast, forward);
            //print(DeltaCast);
            right = playerTransform.transform.TransformDirection(Vector3.right);
            rightDot = Vector3.Dot(deltaCast, right);
            playerTransform.transform.Rotate(Vector3.forward*rightDot*20.0f);
            /*do 
			{playerTransform.transform.Translate((deltaCast.x), deltaCast.y , 0);
			}
		while(Input.GetMouseButtonUp(0));
		*/
        }
    }
}
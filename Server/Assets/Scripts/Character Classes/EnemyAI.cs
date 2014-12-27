#region License

// // EnemyAI.cs
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

public class EnemyAI : MonoBehaviour
{
    public int maxDistance;
    public int moveSpeed;

    private Transform myTransform;
    public int rotationSpeed;
    public Transform target;

    private void Awake()
    {
        myTransform = transform;
    }

    // Use this for initialization
    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");

        target = go.transform;

        maxDistance = 2;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.DrawLine(target.position, myTransform.position, Color.yellow);

        //Look at target
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
            Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed*Time.deltaTime);

        if (Vector3.Distance(target.position, myTransform.position) > maxDistance)
        {
            //Move towards target
            myTransform.position += myTransform.forward*moveSpeed*Time.deltaTime;
        }
    }
}
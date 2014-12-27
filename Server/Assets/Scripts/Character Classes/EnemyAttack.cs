#region License

// // EnemyAttack.cs
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

public class EnemyAttack : MonoBehaviour
{
    public float attackTimer;
    public float coolDown;
    public GameObject target;

    // Use this for initialization
    private void Start()
    {
        attackTimer = 0;
        coolDown = 2.0f;
    }


    // Update is called once per frame
    private void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (attackTimer < 0)
        {
            attackTimer = 0;
        }


        if (attackTimer == 0)
        {
            Attack();
            attackTimer = coolDown;
        }
    }

    private void Attack()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        Vector3 dir = (target.transform.position - transform.position).normalized;

        float direction = Vector3.Dot(dir, transform.forward);

        Debug.Log(distance);

        if (distance < 2.5f)
        {
            if (direction > 0)
            {
                var eh = (PlayerHealth) target.GetComponent("PlayerHealth");
                eh.AddjustCurrentHealth(-10);
            }
        }
    }
}
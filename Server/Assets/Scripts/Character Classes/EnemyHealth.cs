#region License

// // EnemyHealth.cs
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

public class EnemyHealth : MonoBehaviour
{
    public int curHealth = 100;

    public float healthBarLength;
    public int maxHealth = 100;
    // Use this for initialization
    private void Start()
    {
        healthBarLength = Screen.width/2;
    }

    // Update is called once per frame
    private void Update()
    {
        AddjustCurrentHealth(0);
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 40, healthBarLength, 20), curHealth + "/" + maxHealth);
    }

    public void AddjustCurrentHealth(int adj)
    {
        curHealth += adj;

        if (curHealth < 0)
            curHealth = 0;

        if (curHealth > maxHealth)
            curHealth = maxHealth;

        if (maxHealth < 1)
            maxHealth = 1;

        healthBarLength = (Screen.width/2)*(curHealth/(float) maxHealth);
    }
}
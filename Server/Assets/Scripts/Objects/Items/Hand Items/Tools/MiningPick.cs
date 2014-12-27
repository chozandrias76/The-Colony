#region License

// // MiningPick.cs
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

public class MiningPick : HandItems
{
    private void Awake()
    {
        itemSize = (int) ObjectSizes.bulky;
        itemUseDistance = (int) UsableDistances.melee;
        usableOn = new[] {"Rock", "Ground", "Earth"};
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public override void OnPlayerUsingThis()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out rayHit, 500.0f,
            ~0))
        {
            if (rayHit.collider.gameObject.ToString() == "Rock" || rayHit.collider.gameObject.ToString() == "Ground" ||
                rayHit.collider.gameObject.ToString() == "Earth")
            {
                float discoverValue = 0.0f;
                if (rayHit.collider.gameObject.ToString() == "Rock")
                {
                    discoverValue = Random.Range(0.5f, 1.0f);
                }
                if (rayHit.collider.gameObject.ToString() == "Ground")
                {
                    discoverValue = Random.Range(0.2f, 1.0f);
                }
                if (rayHit.collider.gameObject.ToString() == "Earth")
                {
                    discoverValue = Random.Range(0.2f, 1.0f);
                }
                if (discoverValue > 0.9f && discoverValue < 1.0f)
                {
                }
                else if (discoverValue > 0.8f && discoverValue <= 0.9f)
                {
                }
                else if (discoverValue > 0.7f && discoverValue <= 0.8f)
                {
                }
                else if (discoverValue > 0.6f && discoverValue <= 0.7f)
                {
                }
                else if (discoverValue > 0.5f && discoverValue <= 0.6f)
                {
                    GameObject ore = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    ore.transform.position = transform.position;
                    ore.AddComponent<IronOre>();
                }
                else if (discoverValue <= 0.5f)
                {
                }
            }
        }
    }
}
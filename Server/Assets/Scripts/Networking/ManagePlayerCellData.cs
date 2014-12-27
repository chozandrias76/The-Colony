#region License

// // CreatePlayerCellData.cs
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
using BitStream = uLink.BitStream;
using MonoBehaviour = uLink.MonoBehaviour;
using NetworkMessageInfo = uLink.NetworkMessageInfo;
using NetworkPlayer = uLink.NetworkPlayer;
using RPCMode = uLink.RPCMode;

public class ManagePlayerCellData : MonoBehaviour
{
    public NetworkPlayer Player = new NetworkPlayer();
    public float oxygen;
    public static CellData playerData = new CellData();
    private int tick;

    private void Start()
    {
        oxygen = playerData.oxy;
    }

    private void Update()
    {
        //playerData = GameMap.cellsContents[GameMap.CellKey(transform.position)];
       playerData = CellDataTracker.Singleton.GetCell(transform.position);
		oxygen = playerData.oxy;
        tick++;
        if (tick%10 == 0)
        {
            //playerData = GameMap.cellsContents[GameMap.CellKey(transform.position)];
            networkView.RPC((string) "SendData", (RPCMode) RPCMode.Owner, playerData.oxy);
        }
    }

    [RPC]
    private void SendData(float data)
    {
        //GameMap.cellsContents[GameMap.CellKey(gameObject.transform.position)].oxy = data;
    }

}
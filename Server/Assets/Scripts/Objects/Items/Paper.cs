#region License

// // Paper.cs
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

public class Paper : HandItems
{
    private const int lineCountMax = 10;
    private readonly string[][] paperLines = new string[10][];
    private int lineCount;
    private int pageCount;

    private void Start()
    {
        itemSize = (int) ObjectSizes.small;
        itemUseDistance = (int) UsableDistances.melee;
    }

    public new string ToString()
    {
        return string.Format("Paper");
    }

//    public override void OnUsing()
//    {
//    }

    public void OnUsingPen()
    {
        bool usingItem = true;
        while (usingItem)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    usingItem = false;
                }
                else
                {
                    paperLines[pageCount][lineCount] += Input.inputString;
                    if (paperLines[pageCount][lineCount].Length == 32)
                    {
                        int spaceLast = paperLines[pageCount][lineCount].LastIndexOf(" ", 0);
                        if (lineCount == lineCountMax)
                        {
                            paperLines[pageCount + 1][lineCount] = paperLines[pageCount][lineCount].Substring(
                                spaceLast, paperLines[pageCount][lineCount].Length);
                            paperLines[pageCount][lineCount] = paperLines[pageCount][lineCount].Substring(0,
                                spaceLast - 1);
                            pageCount++;
                        }
                        else if (lineCount < lineCountMax)
                        {
                            paperLines[pageCount][lineCount + 1] = paperLines[pageCount][lineCount].Substring(
                                spaceLast, paperLines[pageCount][lineCount].Length);
                            paperLines[pageCount][lineCount] = paperLines[pageCount][lineCount].Substring(0,
                                spaceLast - 1);
                            lineCount++;
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
    }
}
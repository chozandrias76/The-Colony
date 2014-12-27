#region License

// // BlankPaper.cs
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

public class BlankPaper : HandItems
{
    private const int maxLines = 10;
    private int currentLines = 0;
    private int pages = 0;
    public string paperAuthor;
    public string[] paperText = new string[256];
    public string paperTopic;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public override void OnPlayerUsingThis()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (char c in Input.inputString)
                {
                    paperText[pages] += c;
                    if ((paperText[pages].Length%64 == 0) && (currentLines != (maxLines - 1)))
                    {
                        int eolIndex = paperText[pages].LastIndexOf(" ", currentLines*64);
                        paperText[pages].Insert(eolIndex, "/n");
                    }
                    else if ((paperText.Length%64 == 0) && (currentLines == (maxLines - 1)))
                    {
                    }
                }
            }
        }
    }
}
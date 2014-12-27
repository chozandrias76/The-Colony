#region License

// // JSONFiles.cs
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

using System.IO;
using JsonFx.Json;
using UnityEngine;

public class JSONGameMap
{
    public static string saveDirectory = Application.dataPath;
    private static readonly string filePath = Application.dataPath + "/WorldMap.lvl";

    public static void Save(PopulateWorldObjects.SaveData data)
    {
        //If the directory does not yet exist, create it
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        //Write the serialized JSON string of the save data to the specified filePath
        File.WriteAllText(filePath, JsonWriter.Serialize(data));
    }

    public static PopulateWorldObjects.SaveData Load()
    {
        //If the directory doesn't exist, we can't load the file...
        if (!Directory.Exists(saveDirectory)) return null;

        if (!File.Exists(filePath)) return null;
        //Return the contents of the save file, translated into SaveData
        return JsonReader.Deserialize<PopulateWorldObjects.SaveData>(File.ReadAllText(filePath));
    }
}
#region License

// // MetalMesh.cs
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

public class MetalMesh : Item
{
    public RecipeStructure metalMeshRecipeStruct = new RecipeStructure();

    private void Awake()
    {
        var _metalSheet = new MetalSheet();
        metalMeshRecipeStruct.requiredMats.Add(_metalSheet);
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}
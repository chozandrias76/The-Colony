#region License

// // int3.cs
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

public struct int3
{
    public int x;
    public int y;
    public int z;

    public int3(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public static int3 operator +(int3 a, int3 b)
    {
        return new int3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static int3 operator -(int3 a, int3 b)
    {
        return new int3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static int3 operator *(int3 a, int3 b)
    {
        return new int3(a.x*b.x, a.y*b.y, a.z*b.z);
    }

    public static int3 operator *(int3 a, int b)
    {
        return new int3(a.x*b, a.y*b, a.z*b);
    }

    public static int3 operator /(int3 a, int3 b)
    {
        return new int3(a.x/b.x, a.y/b.y, a.z/b.z);
    }

    public static int3 operator /(int3 a, int b)
    {
        return new int3(a.x/b, a.y/b, a.z/b);
    }

    public static bool operator ==(int3 a, int3 b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator !=(int3 a, int3 b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object o)
    {
        if (o is int3)
        {
            var i = (int3) o;
            return this == i;
        }
        return base.Equals(o);
    }

    public static implicit operator Vector3(int3 a)
    {
        return new Vector3(a.x, a.y, a.z);
    }

    public override string ToString()
    {
        return "(" + x + "," + y + "," + z + ")";
    }
}
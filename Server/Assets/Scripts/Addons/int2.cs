#region License

// // int2.cs
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

public struct int2
{
    public int x;
    public int y;

    public int2(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public static int2 operator +(int2 a, int2 b)
    {
        return new int2(a.x + b.x, a.y + b.y);
    }

    public static int2 operator -(int2 a, int2 b)
    {
        return new int2(a.x - b.x, a.y - b.y);
    }

    public static int2 operator *(int2 a, int2 b)
    {
        return new int2(a.x*b.x, a.y*b.y);
    }

    public static int2 operator *(int2 a, int b)
    {
        return new int2(a.x*b, a.y*b);
    }

    public static int2 operator /(int2 a, int2 b)
    {
        return new int2(a.x/b.x, a.y/b.y);
    }

    public static int2 operator /(int2 a, int b)
    {
        return new int2(a.x/b, a.y/b);
    }

    public static bool operator ==(int2 a, int2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(int2 a, int2 b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object o)
    {
        if (o is int2)
        {
            var i = (int2) o;
            return this == i;
        }
        return base.Equals(o);
    }

    public static implicit operator Vector2(int2 a)
    {
        return new Vector2(a.x, a.y);
    }

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }
}
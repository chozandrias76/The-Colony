
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
        return new int3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static int3 operator *(int3 a, int b)
    {
        return new int3(a.x * b, a.y * b, a.z * b);
    }
    public static int3 operator /(int3 a, int3 b)
    {
        return new int3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
    public static int3 operator /(int3 a, int b)
    {
        return new int3(a.x / b, a.y / b, a.z / b);
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
            var i = (int3)o;
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

using UnityEngine;
public struct int2 {
        public int x;
        public int y;
        public int2 (int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public static int2 operator + (int2 a, int2 b)
        {
            return new int2(a.x + b.x, a.y + b.y);
        }
        public static int2 operator - (int2 a, int2 b)
        {
            return new int2(a.x - b.x, a.y - b.y);
        }
        public static int2 operator * (int2 a, int2 b)
        {
            return new int2(a.x * b.x, a.y * b.y);
        }
        public static int2 operator * (int2 a, int b)
        {
            return new int2(a.x * b, a.y * b);
        }
        public static int2 operator / (int2 a, int2 b)
        {
            return new int2(a.x / b.x, a.y / b.y);
        }
        public static int2 operator / (int2 a, int b)
        {
            return new int2(a.x / b, a.y / b);
        }
        public static bool operator == (int2 a, int2 b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator != (int2 a, int2 b)
        {
            return !(a == b);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals (object o)
        {
            if(o is int2)
            {
                var i = (int2)o;
                return this == i;
            }
            return base.Equals (o);
        }
	
        public static implicit operator Vector2 (int2 a)
        {
            return new Vector2(a.x, a.y);
        }
        public override string ToString ()
        {
            return "("+x+","+y+")";
        }
    }
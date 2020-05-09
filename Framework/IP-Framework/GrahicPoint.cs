using System;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace IP_Framework
{
    class Point
    {
        public double x { get; set; }
        public double y { get; set; }

        public Point()
        {
            x = 0.0f;
            y = 0.0f;
        }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator >(Point A, Point B)
        {
            if (A.x != B.x)
            {
                return A.y > B.y;
            }
            return A.x > B.x;
        }

        public static bool operator <(Point A, Point B)
        {
            if (A.x != B.x)
            {
                return A.y < B.y;
            }
            return A.x < B.x;
        }
    }
}
using System;
using System.Collections.Generic;

namespace IP_Framework
{
    class ConvexHaul
    {
        private static double EPSILON = 1e-8;

        private static double R = 6376;

        public static double Distance(Point p1, Point p2)
        {
            return R * Math.Acos(Math.Sin(p1.y) * Math.Sin(p2.y) +
                                 Math.Cos(p1.y) * Math.Cos(p2.y) * Math.Cos(p1.x - p2.x));
            //return Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
        }

        private static List<List<Point>> DivideRegions(List<Point> points, double acceptedSize = 10)
        {
            List<List<int>> adjecent = new List<List<int>>();
            for (int i = 0; i < points.Count; i++)
            {
                adjecent.Add(new List<int>());
            }

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    if (Distance(points[i], points[j]) <= acceptedSize)
                    {
                        adjecent[i].Add(j);
                        adjecent[j].Add(i);
                    }
                }
            }

            List<bool> visited = new List<bool>();
            for (int i = 0; i < points.Count; i++)
            {
                visited.Add(false);
            }


            List<List<Point>> regions = new List<List<Point>>();
            for (int i = 0; i < points.Count; i++)
            {
                if (visited[i])
                {
                    continue;
                }

                visited[i] = true;

                List<Point> region = new List<Point>();
                Stack<int> recursive = new Stack<int>();
                recursive.Push(i);
                region.Add(points[i]);

                while (recursive.Count > 0)
                {
                    int node = recursive.Pop();

                    foreach (int neighbour in adjecent[node])
                    {
                        if (visited[neighbour])
                        {
                            continue;
                        }

                        visited[neighbour] = true;
                        recursive.Push(neighbour);
                        region.Add(points[neighbour]);
                    }
                }

                if (region.Count > 0)
                {
                    regions.Add(region);
                }
            }

            return regions;
        }

        private static double Aria(Point A, Point B, Point C)
        {
            return A.x * B.y + B.x * C.y + C.x * A.y - C.x * B.y - B.x * A.y - A.x * C.y;
        }

        private static int Compare(Point A, Point B, Point C)
        {
            double delta = Aria(A, B, C);
            if (delta > EPSILON)
                return 1;

            if (delta < -EPSILON)
                return -1;

            return 0;
        }

        private static List<Point> GetConvexHull(List<Point> region)
        {
            if (region.Count < 3)
                return region;

            Point leftBottom = region[0];
            int index = 0;

            for (int i = 1; i < region.Count; i++)
            {
                if (leftBottom > region[i])
                {
                    index = i;
                    leftBottom = region[i];
                }
            };

            List<Point> sortedRegion = new List<Point>();
            for (int i = 0; i < region.Count; i++)
            {
                if (i != index)
                {
                    sortedRegion.Add(region[i]);
                }
            }

            sortedRegion.Sort(
                (A, B) => (Compare(A, B, leftBottom))
            );

            List<Point> hull = new List<Point>();

            for (int counter = 0; counter < region.Count; counter++)
            {
                hull.Add(new Point(0, 0));
            }

            hull[0] = leftBottom;
            hull[1] = sortedRegion[0];
            int top = 1;
            for (int i = 1; i < sortedRegion.Count; i++)
            {
                while (top >= 1 && Compare(hull[top - 1], hull[top], sortedRegion[i]) < 1)
                {
                    top--;
                }
                top++;
                hull[top] = sortedRegion[i];
            }

            List<Point> result = new List<Point>();
            for (int i = 0; i <= top; i++)
            {
                result.Add(hull[i]);
            }

            return result;
        }

        public static List<List<Point>> CalculateHull(List<Point> points, double acceptedSize)
        {
            List<List<Point>> regions = DivideRegions(points, acceptedSize);

            List<List<Point>> hulls = new List<List<Point>>();
            foreach (List<Point> region in regions)
            {
                hulls.Add(GetConvexHull(region));
            }

            return hulls;
        }
    }
}

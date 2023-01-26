using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class HelperMethods
    {
        public static Enums.PointInPolygon PointInTriangle(Point p, Point a, Point b, Point c)
        {
            if (a.Equals(b) && b.Equals(c))
            {
                if (p.Equals(a) || p.Equals(b) || p.Equals(c))
                    return Enums.PointInPolygon.OnEdge;
                else
                    return Enums.PointInPolygon.Outside;
            }

            Line ab = new Line(a, b);
            Line bc = new Line(b, c);
            Line ca = new Line(c, a);

            if (GetVector(ab).Equals(Point.Identity)) return (PointOnSegment(p, ca.Start, ca.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (GetVector(bc).Equals(Point.Identity)) return (PointOnSegment(p, ca.Start, ca.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (GetVector(ca).Equals(Point.Identity)) return (PointOnSegment(p, ab.Start, ab.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;

            if (CheckTurn(ab, p) == Enums.TurnType.Colinear)
                return PointOnSegment(p, a, b)? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (CheckTurn(bc, p) == Enums.TurnType.Colinear && PointOnSegment(p, b, c))
                return PointOnSegment(p, b, c) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (CheckTurn(ca, p) == Enums.TurnType.Colinear && PointOnSegment(p, c, a))
                return PointOnSegment(p, a, c) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;

            if (CheckTurn(ab, p) == CheckTurn(bc, p) && CheckTurn(bc, p) == CheckTurn(ca, p))
                return Enums.PointInPolygon.Inside;
            return Enums.PointInPolygon.Outside;
        }
        public static Enums.TurnType CheckTurn(Point vector1, Point vector2)
        {
            double result = CrossProduct(vector1, vector2);
            if (result < 0) return Enums.TurnType.Right;
            else if (result > 0) return Enums.TurnType.Left;
            else return Enums.TurnType.Colinear;
        }
        public static double CrossProduct(Point a, Point b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
        public static bool PointOnRay(Point p, Point a, Point b)
        {
            if (a.Equals(b)) return true;
            if (a.Equals(p)) return true;
            var q = a.Vector(p).Normalize();
            var w = a.Vector(b).Normalize();
            return q.Equals(w);
        }
        public static bool PointOnSegment(Point p, Point a, Point b)
        {
            if (a.Equals(b))
                return p.Equals(a);

            if (b.X == a.X)
                return p.X == a.X && (p.Y >= Math.Min(a.Y, b.Y) && p.Y <= Math.Max(a.Y, b.Y));
            if (b.Y == a.Y)
                return p.Y == a.Y && (p.X >= Math.Min(a.X, b.X) && p.X <= Math.Max(a.X, b.X));
            double tx = (p.X - a.X) / (b.X - a.X);
            double ty = (p.Y - a.Y) / (b.Y - a.Y);

            return (Math.Abs(tx - ty) <= Constants.Epsilon && tx <= 1 && tx >= 0);
        }
        /// <summary>
        /// Get turn type from cross product between two vectors (l.start -> l.end) and (l.end -> p)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Enums.TurnType CheckTurn(Line l, Point p)
        {
            Point a = l.Start.Vector(l.End);
            Point b = l.End.Vector(p);
            return HelperMethods.CheckTurn(a, b);
        }
        public static Point GetVector(Line l)
        {
            return l.Start.Vector(l.End);
        }
        public static bool isEqual(Point p1, Point p2) => ((p2.X == p1.X) && (p2.Y == p1.Y));

        public static double distance(Point p1, Point p2) => Math.Sqrt(Math.Pow(p1.X - p2.X,2) + Math.Pow(p1.Y - p2.Y, 2));

        public static List<Point> isPointInConvex(List<Point> convex, Point point) {
            List<Point> result = new List<Point>();
            convex.Add(convex[0]);
            convex.Add(convex[1]);

            for (int i = 1;i < convex.Count - 1;i++) {
                if (isEqual(convex[i], point)) { result.Add(convex[i]);continue; }

                Enums.TurnType turn1 = CheckTurn(new Line(convex[i - 1], convex[i]), point);
                Enums.TurnType turn2 = CheckTurn(new Line(convex[i], convex[i + 1]), point);

                if (turn1 == Enums.TurnType.Left && turn2 == Enums.TurnType.Left)
                    result.Add(convex[i]);
                else if (turn1 == Enums.TurnType.Left && turn2 == Enums.TurnType.Right)
                {
                    result.Add(convex[i]);
                    result.Add(point);
                }
                else if (turn1 == Enums.TurnType.Right && turn2 == Enums.TurnType.Left)
                    result.Add(convex[i]);
                
                else if (turn1 == Enums.TurnType.Colinear) {
                    if (turn2 == Enums.TurnType.Right)
                        result.Add(point);
                 
                    else if (turn2 == Enums.TurnType.Left)
                        result.Add(convex[i]);
                }
                else if (turn2 == Enums.TurnType.Colinear)
                {
                    if (turn1 == Enums.TurnType.Right)
                    {
                        result.Add(point);
                    }

                    else if (turn1 == Enums.TurnType.Left)
                        result.Add(convex[i]);
                }

            }

            return result;          
        }
        private static List<Point> Merge(List<Point> list1, List<Point> list2, Enums.Priority sortPriority)
        {
            List<Point> tmp = new List<Point>();
            int N = list1.Count + list2.Count;
            for (int i = 0; i < N; i++)
            {
                if (list1.Count == 0)
                {
                    tmp.Add(list2[0]);
                    list2.RemoveAt(0);
                    continue;
                }
                if (list2.Count == 0)
                {
                    tmp.Add(list1[0]);
                    list1.RemoveAt(0);
                    continue;
                }
                if (sortPriority == Enums.Priority.Yaxis)
                {
                    if (list1[0].Y < list2[0].Y)
                    {
                        tmp.Add(list1[0]);
                        list1.RemoveAt(0);
                    }
                    else
                    {
                        tmp.Add(list2[0]);
                        list2.RemoveAt(0);
                    }
                }
                else
                {
                    if (list1[0].X < list2[0].X)
                    {
                        tmp.Add(list1[0]);
                        list1.RemoveAt(0);
                    }
                    else
                    {
                        tmp.Add(list2[0]);
                        list2.RemoveAt(0);
                    }
                }
            }

            return tmp;
        }
        public static List<Point> MergeSort(List<Point> list, Enums.Priority sortPriority)
        {
            if (list.Count == 1)
                return list;

            int cunt = list.Count % 2 == 0 ? list.Count / 2 : list.Count / 2 + 1;
            return Merge(MergeSort(list.GetRange(0, list.Count / 2).ToList(), sortPriority)
                , MergeSort(list.GetRange(list.Count / 2, cunt).ToList(), sortPriority), sortPriority);

        }

        public static Dictionary<string, Point> getBounds(List<Point> points) {
            Dictionary<string, Point> dict = new Dictionary<string, Point>
            {
                ["ML"] = points[0],
                ["MR"] = points[0],
                ["MU"] = points[0],
                ["MD"] = points[0]
            };
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].X > dict["MR"].X)
                    dict["MR"] = points[i];
                if(points[i].X < dict["ML"].X)
                    dict["ML"] = points[i];
                if (points[i].Y > dict["MU"].Y)
                    dict["MU"] = points[i];
                if (points[i].Y < dict["MD"].Y)
                    dict["MD"] = points[i];
            }
            return dict;
        }

        public static int CheckRotation(List<Point> res) {
            int count = 0;
            for (int i = 0; i < res.Count; i++)
            {
                if (CheckTurn(new Line(res[0], res[1]), res[2]) == Enums.TurnType.Left)
                    count++;
                else if (CheckTurn(new Line(res[0], res[1]), res[2]) == Enums.TurnType.Right) 
                    count--;
                res.Add(res[0]);
                res.RemoveAt(0);
            }
            return count;
        }

    }
}

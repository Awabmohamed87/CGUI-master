using CGUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        private List<Point> Merge(List<Point>list1, List<Point> list2, Point pivot) {
             int N = list1.Count +  list2.Count;
             List<Point> lst = new List<Point>();
            for (int i = 0; i < N; i++)
            {
                if (list1.Count == 0)
                {
                    lst.Add(list2[0]);
                    list2.RemoveAt(0);
                    continue;
                }
                if (list2.Count == 0)
                {
                    lst.Add(list1[0]);
                    list1.RemoveAt(0);
                    continue;
                }
                double angle1 = Math.Atan2(list1[0].Y - pivot.Y, list1[0].X - pivot.X) * 180 / Math.PI;
                double angle2 = Math.Atan2(list2[0].Y - pivot.Y, list2[0].X - pivot.X) * 180 / Math.PI;
                if (angle1 < angle2)
                {
                    lst.Add(list1[0]);
                    list1.RemoveAt(0);
                }
                else if (angle1 == angle2)
                {
                    if (HelperMethods.distance(pivot, list1[0]) < HelperMethods.distance(pivot, list2[0])) {
                        lst.Add(list1[0]);
                        list1.RemoveAt(0);
                    }
                    else {
                        lst.Add(list2[0]);
                        list2.RemoveAt(0);
                    }
                }
                else {
                    lst.Add(list2[0]);
                    list2.RemoveAt(0);
                }
            }
            return lst;
        }
        private List<Point> sortToPivotPoint(List<Point> list, Point pivot) {
            if(list.Count == 1) return list;

            int cunt = list.Count % 2 == 0 ? list.Count / 2 : list.Count / 2 + 1;
            return Merge(sortToPivotPoint(list.GetRange(0, list.Count / 2).ToList(), pivot)
                , sortToPivotPoint(list.GetRange(list.Count / 2, cunt).ToList(), pivot), pivot);
        }
        private List<Point> applyGrahamScan(List<Point> pnts, List<Point> outPoints,int index) {
            if (HelperMethods.isEqual(pnts[index], outPoints[0]))
                return outPoints;
            else
            {
                if (HelperMethods.CheckTurn(new Line(outPoints[outPoints.Count - 1], pnts[index]), outPoints[outPoints.Count - 2]) ==
                    Enums.TurnType.Left)
                {
                    outPoints.Add(pnts[index]);
                    return applyGrahamScan(pnts, outPoints, index + 1);
                }
                else
                {
                    outPoints.RemoveAt(outPoints.Count - 1);
                    return applyGrahamScan(pnts, outPoints, index);
                }
            }
            
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1) {
                outPoints = points;
                return;
            }
            Point pivotPoint = HelperMethods.getBounds(points)["MD"];
            outPoints.Add(pivotPoint);
            points.Remove(pivotPoint);

            List<Point> pnts = sortToPivotPoint(points, pivotPoint);
            pnts.Add(pivotPoint);
            outPoints.Add(pnts[0]);
            int index = 1;
            while (!HelperMethods.isEqual(pnts[index], outPoints[0])) {
                if (outPoints.Count >= 2)
                {
                    if (HelperMethods.CheckTurn(new Line(outPoints[outPoints.Count - 1], pnts[index]), outPoints[outPoints.Count - 2]) ==
                           Enums.TurnType.Left)
                    {
                        outPoints.Add(pnts[index++]);
                    }
                    else
                    {
                        outPoints.RemoveAt(outPoints.Count - 1);
                    }
                }

                else
                    outPoints.Add(pnts[index++]);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}

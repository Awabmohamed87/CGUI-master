using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Random random = new Random();
            
            if (points.Count == 1) {
                outPoints = points;
                return;
            }
            List<Point> pnts = HelperMethods.MergeSort(points, Enums.Priority.Yaxis);

            outPoints.Add(pnts[pnts.Count - 1]);
            
            Point endPoint = new Point(0, 0);
            while (!HelperMethods.isEqual(endPoint, pnts[pnts.Count - 1]))
            {

                Point maxPoint = points[random.Next(0, points.Count)];
                
                for (int i = 0; i < points.Count; i++)
                {

                    if (HelperMethods.CheckTurn(new Line(outPoints[outPoints.Count - 1], points[i]), maxPoint) == Enums.TurnType.Right)
                    {
                        maxPoint = points[i];
                    }
                    else if (HelperMethods.CheckTurn(new Line(outPoints[outPoints.Count - 1], points[i]), maxPoint) == Enums.TurnType.Colinear)
                    {
                        if (HelperMethods.distance(outPoints[outPoints.Count - 1], points[i]) > HelperMethods.distance(outPoints[outPoints.Count - 1], maxPoint))
                            maxPoint = points[i];
                    }

                }

                endPoint.X = maxPoint.X; endPoint.Y = maxPoint.Y;
                outPoints.Add(maxPoint);

            }
            outPoints.RemoveAt(outPoints.Count - 1);
            
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}

using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1 || points.Count == 2) {
                outPoints = points;
                return;
            }

            outPoints.Add(points[0]);
            outPoints.Add(points[1]);
            outPoints.Add(points[2]);
            
            points.RemoveAt(2);
            points.RemoveAt(1);
            points.RemoveAt(0);

            int direcrion = HelperMethods.CheckRotation(outPoints);
            if(direcrion < 0)
                outPoints.Reverse();

            for (int i = 0; i < points.Count; i++) {
                outPoints = HelperMethods.isPointInConvex(outPoints, points[i]);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}

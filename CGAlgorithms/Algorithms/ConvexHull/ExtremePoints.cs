using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            bool is_lieInTriangle;
            for (int i = 0; i < points.Count; i++)
            {
                is_lieInTriangle = false;
                for (int j=0; j< points.Count; j++)
                {
                    for (int k = 0; k<points.Count; k++)
                    {
                        for(int h = 0; h<points.Count; h++)
                        {
                            if (j!= i && k != i && h != i)
                            {
                                if (HelperMethods.PointInTriangle(points[i], points[j], points[k], points[h]) == Enums.PointInPolygon.Inside
                                    || HelperMethods.PointInTriangle(points[i], points[j], points[k], points[h]) == Enums.PointInPolygon.OnEdge)
                                {
                                    is_lieInTriangle = true;
                                    break;
                                }
                            }
                        }
                        if(is_lieInTriangle)
                            break;
                    }
                    if (is_lieInTriangle)
                        break;
                }
                if (is_lieInTriangle)
                {
                    points.Remove(points[i]);
                    i--;
                }
            }
            outPoints =points;
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}

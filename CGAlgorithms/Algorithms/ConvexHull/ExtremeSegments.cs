using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count < 3)
            {
                outPoints = points;
                return;
            }

            List<Point> out_extreme_points = new List<Point>();
            List<Point> onEdge_extreme_points = new List<Point>();

            Line temp_extreme_segmant;
            bool rightFlag;
            bool leftFlag;

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    temp_extreme_segmant = new Line(points[i], points[j]);
                    rightFlag = false;
                    leftFlag = false;
                    for (int k = 0; k < points.Count; k++)
                    {
                        
                        if (k != i && k != j && i != j)
                        {
                            if (HelperMethods.PointOnSegment(points[i], points[k], points[j])
                                && !points[i].Equals(points[k]) && !points[i].Equals(points[j]))
                            {
                                onEdge_extreme_points.Add(points[i]);
                            }
                            if (HelperMethods.CheckTurn(temp_extreme_segmant, points[k]) == Enums.TurnType.Right)
                            {
                                rightFlag = true;
                            }
                            else if (HelperMethods.CheckTurn(temp_extreme_segmant, points[k]) == Enums.TurnType.Left)
                            {
                                leftFlag = true;
                            }
                        }
                    }

                    if (!rightFlag && leftFlag || rightFlag && !leftFlag )
                    {
                        if (!out_extreme_points.Contains(points[i]))
                        {
                            out_extreme_points.Add(points[i]);
                        }
                        break;
                    }
                }
            }

            // Remove the points which is on the edges between extreme points
            foreach (Point p in onEdge_extreme_points)
            {
                out_extreme_points.Remove(p);
            }

            //foreach (Point p in out_extreme_points)
            //{
            //    Console.WriteLine(p.X + " " + p.Y);
            //}

            outPoints = out_extreme_points;

        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}

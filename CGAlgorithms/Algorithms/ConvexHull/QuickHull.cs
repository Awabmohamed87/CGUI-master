using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Region {
        public Point start, end;
        public List<Point> points;
        public List<double> distances;
        public Region(Point start, Point end) {
            this.start = start;
            this.end = end;
            points = new List<Point>();
            distances = new List<double>();
        }
        public List<Point> GetPoints() => new List<Point> { start, points[distances.IndexOf(distances.Min())], end };
        public List<Point> toList() => new List<Point>{start, end};
    }
    public class QuickHull : Algorithm
    {
    private List<Region> MakeRegions(List<Point> currentConvex) {
        List<Region> regions = new List<Region>();
            for (int i = 0;i<currentConvex.Count;i++) {
                if(i == currentConvex.Count - 1)
                {
                    if (!HelperMethods.isEqual(currentConvex[i], currentConvex[0]))
                        regions.Add(new Region(currentConvex[i], currentConvex[0]));
                    continue;
                }

                if (!HelperMethods.isEqual(currentConvex[i], currentConvex[i + 1]))
                    regions.Add(new Region(currentConvex[i], currentConvex[i + 1]));
            }

            return regions;
        } 
    private List<Region> ClassifyToRegions(List<Region> regions, List<Point> points) {
            for (int i = 0; i < points.Count; i++)
            {
            int leftTurnsCount = 0;
            int regionIndex = 0;
            double minDistance = 0;
            for (int j = 0; j < regions.Count; j++)
            {
                if (HelperMethods.CheckTurn(new Line(regions[j].start, regions[j].end), points[i])
                        != Enums.TurnType.Right) leftTurnsCount++;
                else {
                    double distance = HelperMethods.CrossProduct(regions[j].start.Vector(regions[j].end),
                        regions[j].end.Vector(points[i]));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                         regionIndex = j;
                    }
                }
            }
            if(leftTurnsCount < regions.Count) 
            {
                regions[regionIndex].points.Add(points[i]);
                regions[regionIndex].distances.Add(minDistance);
            }

        }
       return regions.ToList();
    }

    List<Point> results = new List<Point>();

    private void Quick_Hull(List<Point> currentConvex, List<Point> points) {
        List<Region> initialRegions = MakeRegions(currentConvex);
        List<Region> newRegions = ClassifyToRegions(initialRegions, points);
        for (int i = 0; i< newRegions.Count;i++) {
            if (newRegions[i].points.Count > 0)
                Quick_Hull(newRegions[i].GetPoints(), newRegions[i].points);
            else 
                Combine(newRegions[i].toList());
            }
    }
    public void Combine(List<Point> lst) {
        
        bool isFound1st = false, isFound2nd = false; ;
        for (int i = 0;i<results.Count;i++) {
            if (HelperMethods.isEqual(results[i], lst[0]))
            {
                isFound1st = true;
            }
            if(HelperMethods.isEqual(results[i], lst[1]))
            {
                isFound2nd= true;
            }
        }
        if(!isFound1st)
            results.Add(lst[0]);
        if (!isFound2nd)
            results.Add(lst[1]);
            
    }
    public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
    {
        if (points.Count == 1)
        {
            outPoints = points;
            return;
        }
        Dictionary<string, Point> dict = HelperMethods.getBounds(points);
        List<Point> currentConvex = new List<Point> {
        dict["MU"], dict["ML"], dict["MD"], dict["MR"]
        };
        points.Remove(dict["MU"]);
        points.Remove(dict["ML"]);
        points.Remove(dict["MD"]);
        points.Remove(dict["MR"]);

        Quick_Hull(currentConvex, points);
        outPoints = results;

    }

    public override string ToString()
    {
        return "Convex Hull - Quick Hull";
    }
    }
}

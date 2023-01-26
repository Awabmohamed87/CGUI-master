using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        private List<Point> Merge(List<Point>leftList, List<Point>rightList) {
           
            int rightMostIndex = 0;
            int leftMostIndex = 0;

            for (int i = 1; i < leftList.Count; i++)
            {
                if (leftList[i].X > leftList[rightMostIndex].X)
                    rightMostIndex = i;
                else if (leftList[i].X == leftList[rightMostIndex].X)
                {
                    if (leftList[i].Y > leftList[rightMostIndex].Y)
                        rightMostIndex = i;
                }
            }

            // leftMostIndex  leftmost point of rightList 
            for (int i = 1; i < rightList.Count; i++)
            {
                if (rightList[i].X < rightList[leftMostIndex].X)
                    leftMostIndex = i;
                else if (rightList[i].X == rightList[leftMostIndex].X)
                {
                    if (rightList[i].Y < rightList[leftMostIndex].Y)
                        leftMostIndex = i;
                }

            }

            //  upper tangent 
            int uppera = rightMostIndex, upperb = leftMostIndex;
            bool isFound = false;
            while (!isFound)
            {
                isFound = true;
                if (HelperMethods.CheckTurn(new Line(rightList[upperb], leftList[uppera]),
                           leftList[(uppera + 1) % leftList.Count]) == Enums.TurnType.Colinear)
                    uppera = (uppera + 1) % leftList.Count;

                int i = upperb > 0 ? upperb - 1 : rightList.Count - 1;
                if (HelperMethods.CheckTurn(new Line(leftList[uppera], rightList[upperb])
                    , rightList[i]) == Enums.TurnType.Colinear)
                {
                    upperb = upperb > 0 ? upperb - 1 : rightList.Count - 1;
                    i = upperb > 0 ? upperb - 1 : rightList.Count - 1;
                }

                while (HelperMethods.CheckTurn(new Line(rightList[upperb], leftList[uppera]),
                    leftList[(uppera + 1) % leftList.Count]) == Enums.TurnType.Right)
                    {
                        uppera = (uppera + 1) % leftList.Count;
                        isFound = false;
                    }
                
                while (HelperMethods.CheckTurn(new Line(leftList[uppera], rightList[upperb])
                    ,rightList[i]) == Enums.TurnType.Left)
                {
                    upperb = upperb > 0 ? upperb - 1 : rightList.Count - 1;
                    i = upperb > 0 ? upperb - 1 : rightList.Count - 1;
                    isFound = false;
                }
            }

            //lower tangent 
            int lowera = rightMostIndex, lowerb = leftMostIndex;
            isFound = false;
            while (!isFound)
            {
                isFound = true;
                int i = lowera > 0 ? lowera - 1 : leftList.Count - 1;
                if (HelperMethods.CheckTurn(new Line(rightList[lowerb], leftList[lowera]),
                        leftList[i]) == Enums.TurnType.Colinear)
                {
                    lowera = lowera > 0 ? lowera - 1 : leftList.Count - 1;
                    i = lowera > 0 ? lowera - 1 : leftList.Count - 1;

                }

                if (HelperMethods.CheckTurn(new Line(leftList[lowera], rightList[lowerb]),
                    rightList[(lowerb + 1) % rightList.Count]) == Enums.TurnType.Colinear)
                    lowerb = (lowerb + 1) % rightList.Count;

                while (HelperMethods.CheckTurn(new Line(rightList[lowerb], leftList[lowera]),
                        leftList[i]) == Enums.TurnType.Left)
                {
                    lowera = lowera > 0 ? lowera - 1 : leftList.Count - 1;
                    i = lowera > 0 ? lowera - 1 : leftList.Count - 1;
                    isFound = false;
                }

                while (HelperMethods.CheckTurn(new Line(leftList[lowera], rightList[lowerb]),
                    rightList[(lowerb + 1) % rightList.Count]) == Enums.TurnType.Right)
                {
                    lowerb = (lowerb + 1) % rightList.Count;
                    isFound = false;
                }

            }

            List<Point> result = new List<Point>();

            int j = uppera;
            while (j != lowera)
            {
                result.Add(leftList[j]);
                j = j < leftList.Count - 1? j + 1: 0;
            }
            result.Add(leftList[lowera]);

            j = lowerb;
            while (j != upperb)
            {
                result.Add(rightList[j]);
                j = j < rightList.Count - 1 ? j + 1 : 0;
            }
            result.Add(rightList[upperb]);

            return result;
        }

        private List<Point> DivideNdConquer(List<Point> lst) {
            if (lst.Count == 1) return lst;
            
        int end = lst.Count % 2 == 0? lst.Count / 2: lst.Count / 2 + 1;
            return Merge(DivideNdConquer(lst.GetRange(0, lst.Count / 2)),
                DivideNdConquer(lst.GetRange(lst.Count / 2, end)));
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1) {
                outPoints = points;
                return;
            }
            List<Point> pnts = points.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            List<Point> newPoints = DivideNdConquer(pnts);

            for (int i = 0; i < newPoints.Count; i++)
                if (!outPoints.Contains(newPoints[i]))
                {
                    outPoints.Add(newPoints[i]);
                }
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}

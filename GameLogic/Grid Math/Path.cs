using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic.Grid_Math
{
    public static class Path
    {

        public static List<Hexagon> GetPath(Map context, int startX, int startY,
                                                         int endX, int endY)
        {
            Hexagon start = context.GetHexagon(startX, startY);
            Hexagon end = context.GetHexagon(endX, endY);
            List<Hexagon> path = GetPath(context, start, end);
            return path;
        }


        public static List<Hexagon> GetPath(Map context, Hexagon start, Hexagon end)
        {
            // Memory hog, but could be faster
            int size = context.Size.Width * context.Size.Height;
            bool[] closedMap = new bool[size];
            int[] gScoreMap = new int[size];
            int[] fScoreMap = new int[size];
            uint[] comeFromMap = new uint[size];

            // TODO(Cristian): More appropiate data structure?
            List<Hexagon> openSet = new List<Hexagon>();
            openSet.Add(start);

            Hexagon current = null;
            Hexagon[] neighbours = new Hexagon[6];
            int distance = 1; // TODO(Cristian): Un hardcode this
            while (openSet.Count > 0)
            {
                // TODO(Cristian): Make this lookup faster!!!!
                current = GetMinimumFScore(context, openSet, fScoreMap);

                openSet.Remove(current);
                int currentIndex = context.Size.Width * current.Y + current.X;
                closedMap[currentIndex] = true;


                Hexagon.GetHexagonNeighbours(context, current, ref neighbours);
                foreach (Hexagon neighbour in neighbours)
                {
                    int neighbourIndex = context.Size.Width * neighbour.Y + neighbour.X;
                    if(closedMap[neighbourIndex]) { continue; }

                    int score = gScoreMap[neighbourIndex] + distance;
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else if (score >= gScoreMap[neighbourIndex])
                    {
                        continue;
                    }

                    comeFromMap[neighbourIndex] = current.Key;
                    gScoreMap[neighbourIndex] = score;
                    fScoreMap[neighbourIndex] = score + HeuristicScore(neighbour, end);
                }
            }

            throw new System.NotImplementedException();
        }

        public static int HeuristicScore(Hexagon start, Hexagon end)
        {
            int result = Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.X);
            return result;
        }

        public static Hexagon GetMinimumFScore(Map context, List<Hexagon> openSet, int[] fScoreMap)
        {
            int min = int.MaxValue;

            Hexagon result = null;
            foreach (Hexagon hexagon in openSet)
            {
                // TODO(Cristian): Cache this index??
                int fScore = fScoreMap[context.Size.Width * hexagon.Y + hexagon.X];
                if (fScore < min)
                {
                    result = hexagon;
                }
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic.Grid_Math
{
    public static class Path
    {

        public static bool GetPath(Map context, List<Hexagon> resultList,
                                   int startX, int startY, int endX, int endY)
        {
            Hexagon start = context.GetHexagon(startX, startY);
            Hexagon end = context.GetHexagon(endX, endY);
            bool result = GetPath(context, resultList, start, end);
            return result;
        }

        public static bool GetPath(Map context, List<Hexagon> resultList, Hexagon start, Hexagon goal)
        {
            // Memory hog, but could be faster
            int size = context.Size.Width * context.Size.Height;
            bool[] closedMap = new bool[size];
            int[] realCostMap = new int[size];          // g()
            int[] tentativeScoreMap = new int[size];    // g() + h()
            Hexagon[] parentMap = new Hexagon[size];

            // TODO(Cristian): More appropiate data structure?
            List<Hexagon> openSet = new List<Hexagon>();

            // We initialize initial set
            int initialIndex = Hexagon.GenerateIndex(context, start);
            realCostMap[initialIndex] = 0;      // Un-necessary, but formal
            tentativeScoreMap[initialIndex] = HeuristicScore(start, goal);
            parentMap[initialIndex] = null;   // Invalid value
            openSet.Add(start);

            Hexagon current = null;
            Hexagon[] neighbours = new Hexagon[6];
            while (openSet.Count > 0)
            {
                // We pop the node that we think is closer to finding the goal
                // TODO(Cristian): Make this lookup faster!!!!
                current = GetMinimumTentativeScore(context, openSet, tentativeScoreMap);
                openSet.Remove(current);
                // We mark this node as closed
                int currentIndex = Hexagon.GenerateIndex(context, current);
                closedMap[currentIndex] = true;

                // If we found the goal, we're done. Just return the path
                if (current == goal)
                {
                    ReconstructPath(context, resultList, start, goal, parentMap);
                    return true;
                }

                // We now look for all the new nodes we can explore
                Hexagon.GetHexagonNeighbours(context, current, ref neighbours);
                foreach (Hexagon neighbour in neighbours)
                {
                    if (neighbour == null) { continue; }

                    // If the node is already visited, we ignore it
                    int neighbourIndex = Hexagon.GenerateIndex(context, neighbour);
                    if(closedMap[neighbourIndex]) { continue; }

                    // We store the cost to get to this node
                    int costToNeighbour = realCostMap[currentIndex] + 
                                          StepDistance(context, current, neighbour);

                    // If it's new, we add it the open list
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    // If the cost to get to this node from this route is bigger
                    // than the one we already have, we don't care about it
                    else if (costToNeighbour >= realCostMap[neighbourIndex])
                    {
                        continue;
                    }

                    // We update the costs of this node
                    parentMap[neighbourIndex] = current;
                    realCostMap[neighbourIndex] = costToNeighbour;
                    tentativeScoreMap[neighbourIndex] = costToNeighbour + HeuristicScore(neighbour, goal);
                }
            }

            // No path was found
            return false;
        }

        /// <summary>
        /// Calculates the cost from moving from one hexagon to another
        /// Requisite: The hexagons are neighbours
        /// </summary>
        /// <param name="context"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private static int StepDistance(Map context, Hexagon from, Hexagon to)
        {
            int absX = Math.Abs(to.X - from.X);
            int absY = Math.Abs(to.Y - from.Y);
            if ((absX > 1) || (absY > 1))
            {
                string message = "Hexagons are not neighbours";
                throw new System.InvalidProgramException(message);
            }

            // Eventually this could be more complex
            return 1;
        }

        private static void ReconstructPath(Map context, List<Hexagon> resultList, 
                                            Hexagon start, Hexagon goal, 
                                            Hexagon[] parentMap)
        {
            // We need to unwind the stack
            List<Hexagon> list = new List<Hexagon>();
            list.Add(goal);
            int goalIndex = Hexagon.GenerateIndex(context, goal);
            Hexagon parent = parentMap[goalIndex];
            while (parent != start)
            {
                list.Add(parent);
                // We look for the parent in the map
                int index = Hexagon.GenerateIndex(context, parent);
                parent = parentMap[index];
            }

            // As we stop when we found the start, we add the start
            list.Add(start);

            // We now need to reverse the list
            for (int i = list.Count - 1; i >= 0; --i)
            {
                resultList.Add(list[i]);
            }
        }


        public static int HeuristicScore(Hexagon start, Hexagon end)
        {
            // This heuristic overestimates a little in many cases (off by one mainly).
            // This will make the path-finder a little more aggresive towards the goal,
            // which I think is good actually
            int result = Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y);
            return result;
        }

        public static Hexagon GetMinimumTentativeScore(Map context, List<Hexagon> openSet, 
                                                       int[] tentativeScoreMap)
        {
            int min = int.MaxValue;
            Hexagon result = null;
            foreach (Hexagon hexagon in openSet)
            {
                // TODO(Cristian): Cache this index??
                int index = Hexagon.GenerateIndex(context, hexagon);
                int fScore = tentativeScoreMap[index];
                if (fScore <= min)
                {
                    min = fScore;
                    result = hexagon;
                }
            }

            return result;
        }
    }
}

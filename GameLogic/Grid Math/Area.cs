﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameLogic.Utils;

namespace GameLogic.Grid_Math
{
    public static class Area
    {
        public static List<Hexagon> EntityMovementRange(Map context, Entity entity)
        {
            // We first check that the entity is effectively in the map
            // We can never be too sure, can we? :) 
            Entity checkEntity = context.GetEntity(entity.Hexagon);
            if (checkEntity == null)
            {
                string message = String.Format("[CONTEXT: {0}] Entity not in map table provided!",
                                               MethodBase.GetCurrentMethod().ToString());
                throw new System.InvalidProgramException(message);
            }
            if (checkEntity != entity)
            {
                string message = String.Format("[CONTEXT: {0}] Mismatch between entity and stored entity.",
                                               MethodBase.GetCurrentMethod().ToString());
                throw new System.InvalidProgramException(message);
            }

            // Now that the entity is valid, we can generate the area
            HashSet<uint> checkedNodes = new HashSet<uint>();
            List<Hexagon> resultList = new List<Hexagon>();

            ListBFS(context, entity.Hexagon, entity.Stats.MovementRange, 
                    resultList, checkedNodes);

            return resultList;
        }

        /// <summary>
        /// Returns a BFS (whole area) from a center
        /// </summary>
        /// <param name="context">Map where the hexagons resides</param>
        /// <param name="center">The center of the area to search</param>
        /// <param name="level">"Radius of the area: 0-Center only, 1+: that distance from center</param>
        /// <param name="resultList">Where the results will be stored</param>
        /// <param name="checkedNodes">Hash set to store the checked nodes</param>
        public static void ListBFS(Map context, Hexagon center, int level,
                                   List<Hexagon> resultList, HashSet<uint> checkedNodes)
        {
            if (level < 0)
            {
                string message = String.Format("[CONTEXT: {0}] Negative level passed to BFS",
                                               MethodBase.GetCurrentMethod().ToString());
                throw new System.InvalidProgramException(message);
            }

            if (center == null)
            {
                string message = String.Format("[CONTEXT: {0}] null hexagon passed as center to BFS",
                                               MethodBase.GetCurrentMethod().Name);
                throw new System.InvalidProgramException(message);
            }

            // We see if the center is in the list
            if (!checkedNodes.Contains(center.Key))
            {
                checkedNodes.Add(center.Key);
                resultList.Add(center);
            }

            if (level == 0) { return; }

            // We check the neighbours, sadly we must allocate
            Hexagon[] neighbours = new Hexagon[6];
            Hexagon.GetHexagonNeighbours(context, center, ref neighbours);
            foreach (Hexagon hexagon in neighbours)
            {
                if (hexagon == null) { continue; }
                if (!checkedNodes.Contains(hexagon.Key))
                {
                    checkedNodes.Add(hexagon.Key);
                    resultList.Add(hexagon);
                }
            }

            // We already checked the self and one node around
            if (level == 1) { return; }

            foreach(Hexagon hexagon in neighbours)
            {
                if (hexagon == null) { continue; }
                ListBFS(context, hexagon, level - 1, resultList, checkedNodes);
            }
        }


    }
}

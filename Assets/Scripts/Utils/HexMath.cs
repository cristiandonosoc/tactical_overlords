using UnityEngine;

namespace Assets.Scripts.Utils
{
    internal static class HexCoordsUtils
    {
        internal static Vector3 HexToWorld(float hexSide, float hexHalfHeight,
                                        Vector3 hexCoords)
        {
            Vector3 worldCoords = Vector3.zero;
            worldCoords.x = (3 * hexSide * hexCoords.x) / 2;
            worldCoords.y = (hexHalfHeight * hexCoords.x ) + (2 * hexHalfHeight * hexCoords.y);
            return worldCoords;
        }

        internal static Vector3 WorldToHex(float hexSide, float hexHalfHeight,
                                           Vector3 worldCoords)
        {
            Vector3 hexCoords = Vector3.zero;
            hexCoords.x = (2 / (3 * hexSide)) * worldCoords.x;
            hexCoords.y = (-1 / (3 * hexSide)) * worldCoords.x + 
                          (1 / (2 * hexHalfHeight)) * worldCoords.y;
            return hexCoords;
        }

        internal static Vector3 RoundHex(float hexSide, float hexHalfHeight, 
                                      Vector3 hexCoords)
        {
            // We map into cube coordinates
            hexCoords.z = -hexCoords.x - hexCoords.y;

            Vector3 roundedCoords = Vector3.zero;
            roundedCoords.x = Mathf.Round(hexCoords.x);
            roundedCoords.y = Mathf.Round(hexCoords.y);
            roundedCoords.z = Mathf.Round(hexCoords.z);

            Vector3 diff = Vector3.zero;
            diff.x = Mathf.Abs(roundedCoords.x - hexCoords.x);
            diff.y = Mathf.Abs(roundedCoords.y - hexCoords.y);
            diff.z = Mathf.Abs(roundedCoords.z - hexCoords.z);

            if ((diff.x > diff.y) && (diff.x > diff.z))
            {
                roundedCoords.x = -roundedCoords.y - roundedCoords.z;
            }
            else if (diff.y > diff.z)
            {
                roundedCoords.y = -roundedCoords.x - roundedCoords.z;
            }
            else
            {
                roundedCoords.z = -roundedCoords.x - roundedCoords.y;
            }

            return roundedCoords;
        }
    }
}

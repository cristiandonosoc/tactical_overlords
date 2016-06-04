using UnityEngine;

namespace Assets.Scripts.Utils
{
    internal static class HexCoordsUtils
    {
        internal static Vector3 HexToWorld(HexWorld world, Vector3 hexCoords)
        {
            Vector3 worldCoords = Vector3.zero;
            worldCoords.x = (3 * world.HexSide * hexCoords.x) / 2;
            worldCoords.y = (world.HexHalfHeight * hexCoords.x ) + 
                            (2 * world.HexHalfHeight * hexCoords.y);
            return worldCoords;
        }

        internal static Vector3 WorldToHex(HexWorld world, Vector3 worldCoords)
        {
            Vector3 hexCoords = Vector3.zero;
            hexCoords.x = (2 / (3 * world.HexSide)) * worldCoords.x;
            hexCoords.y = (-1 / (3 * world.HexSide)) * worldCoords.x + 
                          (1 / (2 * world.HexHalfHeight)) * worldCoords.y;
            return hexCoords;
        }

        internal static Vector3 RoundHex(Vector3 hexCoords)
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

        internal static Vector3 GetHexRoundedWorldPosition(HexWorld context, Vector3 worldPos)
        {
            Vector3 hexCoords = WorldToHex(context, worldPos);
            Vector3 rounded = HexCoordsUtils.RoundHex(hexCoords);
            return rounded;
        }
    }
}

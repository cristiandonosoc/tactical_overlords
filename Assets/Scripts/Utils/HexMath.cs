using UnityEngine;

namespace Assets.Scripts.Utils
{
    internal static class HexCoords
    {
        internal static void HexToWorld(float hexSide, float hexHalfHeight,
                        Vector3 hexCoords, ref Vector3 worldCoords)
        {
            worldCoords.x = (3 * hexSide * hexCoords.x) / 2;
            worldCoords.y = (hexHalfHeight * hexCoords.x ) + (2 * hexHalfHeight * hexCoords.y);
        }

        internal static void WorldToHex(float hexSide, float hexHalfHeight,
                                        Vector3 worldCoords, ref Vector3 hexCoords)
        {
            hexCoords.x = 2 / (3 * hexSide);
            hexCoords.y = (-1 / (3 * hexSide)) + (1 / (2 * hexHalfHeight));
        }

        internal static void RoundHex(float hexSide, float hexHalfHeight, 
                                      Vector3 hexCoords, ref Vector3 roundedCoords)
        {
            if ((hexCoords.x < 0) || (hexCoords.y < 0))
            {
                // TODO(Cristian): Support negative hex coords
                throw new System.Exception("NO NEGATIVE HEX COORDS SUPPORTED");
            }

            float scaledX = hexCoords.x / ((3 * hexSide) / 2);
            float scaledY = hexCoords.y / (2 * hexHalfHeight);

            float fX = Mathf.Floor(scaledX);
            float fY = Mathf.Floor(scaledY);
            float restX = scaledX - fX;
            float restY = scaledY - fY;

            if (restX > 0.5)
            {
                roundedCoords.x = fX + 1;
                roundedCoords.y = fY;
            }
            else
            {
                if (restY > 0.5)
                {
                    if (restX > (restY / 2))
                    {
                        roundedCoords.x = fX + 1;
                        roundedCoords.y = fY;
                    }
                    else
                    {
                        roundedCoords.x = fX;
                        roundedCoords.y = fY + 1;
                    }
                }
                else
                {
                    if (restX > (0.5 - (restY / 2)))
                    {
                        roundedCoords.x = fX + 1;
                        roundedCoords.y = fY;
                    }
                    else
                    {
                        roundedCoords.x = fX;
                        roundedCoords.y = fY;
                    }
                }
            }
        }
    }
}

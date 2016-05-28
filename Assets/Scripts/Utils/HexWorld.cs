namespace Assets.Scripts.Utils
{
    internal class HexWorld
    {
        #region INTERFACE 

        internal float HexSide { get; set; }
        internal float HexHalfHeight { get; set; }

        #endregion

        internal HexWorld(float hexSide)
        {
            HexSide = hexSide;
            HexHalfHeight = (UnityEngine.Mathf.Sqrt(3) * hexSide) / 2;
        }
    }
}

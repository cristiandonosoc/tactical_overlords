using UnityEngine;
using System.Collections;

using Assets.Scripts.Utils;

public class GridGeneration : MonoBehaviour
{

    public Transform HexagonPrefab;

    public int GridWidth;
    public int GridHeight;

    float _hexSide;
    float _hexHalfHeight;

	// Use this for initialization
	void Start () {
        SpriteRenderer spriteRenderer = HexagonPrefab.GetComponent<SpriteRenderer>();

        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Vector3 hexagonExtents = spriteBounds.extents;

        _hexSide = hexagonExtents.x;
        _hexHalfHeight = (Mathf.Sqrt(3) * _hexSide) / 2;


        Vector3 hexCoords = Vector3.zero;
        Vector3 hexPosition = Vector3.zero;
        // We instantiate the prefabs
        for (int hY = 0; hY < GridHeight; ++hY)
        {
            for (int hX = 0; hX < GridWidth; ++hX)
            {
                hexCoords.x = hX;
                hexCoords.y = hY;
                HexCoords.HexToWorld(_hexSide, _hexHalfHeight,
                                   hexCoords, ref hexPosition);
                Transform t = (Transform)Instantiate(HexagonPrefab,
                                                     hexPosition,
                                                     Quaternion.identity);
            }
        }
	
	}
	
	// Update is called once per frame
	void Update () {
        // We get mouse position
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rounded = Vector3.zero;
        HexCoords.RoundHex(_hexSide, _hexHalfHeight, pos, ref rounded);

        Debug.Log(string.Format("POS: {0}, ROUNDED: {1}", pos, rounded));
	}
}

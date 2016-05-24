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

    Transform[] _grid;

	// Use this for initialization
	void Start () {
        _grid = new Transform[GridWidth * GridHeight];


        SpriteRenderer spriteRenderer = HexagonPrefab.GetComponent<SpriteRenderer>();

        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Vector3 hexagonExtents = spriteBounds.extents;

        _hexSide = hexagonExtents.x;
        _hexHalfHeight = (Mathf.Sqrt(3) * _hexSide) / 2;


        Vector3 hexCoords = Vector3.zero;
        // We instantiate the prefabs
        for (int hY = 0; hY < GridHeight; ++hY)
        {
            for (int hX = 0; hX < GridWidth; ++hX)
            {
                hexCoords.x = hX;
                hexCoords.y = hY;
                Vector3 hexPosition = HexCoordsUtils.HexToWorld(_hexSide, _hexHalfHeight, hexCoords);
                Transform t = (Transform)Instantiate(HexagonPrefab,
                                                     hexPosition,
                                                     Quaternion.identity);

                // We store it in the array
                _grid[GridWidth * hY + hX] = t;
            }
        }
	
	}

    Transform _currentHex = null;
	
	// Update is called once per frame
	void Update () {
        // We get mouse position
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 hexCoords = HexCoordsUtils.WorldToHex(_hexSide, _hexHalfHeight, pos);
        Vector3 rounded = Vector3.zero;

        rounded = HexCoordsUtils.RoundHex(_hexSide, _hexHalfHeight, hexCoords);

        int x = (int)rounded.x;
        int y = (int)rounded.y;
        Transform newHex = null;
        if ((x >= 0) && (x < GridWidth) && 
            (y >= 0) && (y < GridHeight))
        {
            newHex = _grid[GridWidth * y + x];
            SpriteRenderer spriteRenderer = newHex.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
        }

        if (_currentHex != newHex)
        {
            if (_currentHex != null)
            {
                SpriteRenderer currentRenderer = _currentHex.GetComponent<SpriteRenderer>();
                currentRenderer.color = Color.white;
            }
            _currentHex = newHex;
        }

        // We clear the previous
        Debug.Log(string.Format("POS: {0}, HEX: {1}, ROUNDED: {2}", pos, hexCoords, rounded));

    }
}

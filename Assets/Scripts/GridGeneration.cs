using UnityEngine;
using System.Collections;

using Assets.Scripts.Utils;

using GameLogic;

public class GridGeneration : MonoBehaviour
{
    #region EDITOR

    public Transform HexagonPrefab;
    public Transform CharacterPrefab;

    public uint GridWidth;
    public uint GridHeight;

    #endregion

    float _hexSide;
    float _hexHalfHeight;

    HexWorld _world;

    Transform[] _grid;

    GameObject _mainObject;

    Map _map;

	// Use this for initialization
	void Start () {
        _map = new Map(GridWidth, GridHeight, 2);

        // The associated prefabs with the grid
        _grid = new Transform[GridWidth * GridHeight];
        SpriteRenderer spriteRenderer = HexagonPrefab.GetComponent<SpriteRenderer>();

        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Vector3 hexagonExtents = spriteBounds.extents;

        _world = new HexWorld(hexagonExtents.x);

        _mainObject = new GameObject("Grid");
        Transform mainTransform = _mainObject.transform;

        Vector3 hexCoords = Vector3.zero;
        // We instantiate the prefabs
        for (int hY = 0; hY < GridHeight; ++hY)
        {
            for (int hX = 0; hX < GridWidth; ++hX)
            {
                if (_map.GetHexagon(hX, hY) == null) { continue; }

                hexCoords.x = hX;
                hexCoords.y = hY;
                Vector3 hexPosition = HexCoordsUtils.HexToWorld(_world, hexCoords);
                Transform t = (Transform)Instantiate(HexagonPrefab,
                                                     hexPosition,
                                                     Quaternion.identity);
                t.parent = mainTransform;

                // We store it in the array
                _grid[GridWidth * hY + hX] = t;
            }
        }
	
	}

    Transform _currentHex = null;
    Transform _character;

    enum MouseButtons
    {
        LEFT = 0,
        RIGHT = 1,
        MIDDLE = 2
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        HighlightHex(worldPos);

        if (Input.GetMouseButtonUp((int)MouseButtons.LEFT))
        {
            if (_character == null)
            {

                Vector3 charPos = HexCoordsUtils.RoundHex(HexCoordsUtils.WorldToHex(_world, worldPos));
                _character = (Transform)Instantiate(CharacterPrefab, 
                                                    charPos,
                                         			Quaternion.identity);
            }
        }

    }

    void HighlightHex(Vector3 worldPos)
    {
        // We get mouse position
        Vector3 hexCoords = HexCoordsUtils.WorldToHex(_world, worldPos);
        Vector3 rounded = Vector3.zero;

        rounded = HexCoordsUtils.RoundHex(hexCoords);

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
    }
}

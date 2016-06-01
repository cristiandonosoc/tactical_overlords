using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using GameLogic;

public class GridGeneration : MonoBehaviour
{
    #region EDITOR

    public Transform WorldPrefab;
    public Transform HexagonPrefab;
    public Transform CharacterPrefab;

    public uint GridWidth;
    public uint GridHeight;

    #endregion

    #region FIELDS 

    Map _map;
    internal Map Map { get { return _map; } }
    // This is the object that will hold the grid (is kind of a world instance)
    // TODO(Cristian): Should this object be in the scene instead of generated?
    GameObject _gridObject;
    GameObject _entitiesObject;

    // The actual grid of prefabs
    HexagonScript[] _grid;

    // HexWorld  -> Unity coords transformation information
    HexWorld _world;

    #endregion

    // Use this for initialization
    void Start () {
        // We start the game instance
        _map = new Map(GridWidth, GridHeight, 2);

        // We obtain the hex -> world transformation
        SpriteRenderer spriteRenderer = HexagonPrefab.GetComponent<SpriteRenderer>();

        Rect spriteRect = spriteRenderer.sprite.rect;
        _world = new HexWorld(spriteRect.width / 2);

        // We instance the main object
        _gridObject = new GameObject("Grid");
        _gridObject.transform.parent = WorldPrefab;
        Transform mainTransform = _gridObject.transform;
        _grid = new HexagonScript[GridWidth * GridHeight];

        // We instantiate the prefabs
        Vector3 hexCoords = Vector3.zero;
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
                                                     HexagonPrefab.localRotation);
                HexagonScript hexagonScript = t.GetComponent<HexagonScript>();
                t.parent = mainTransform;
                t.gameObject.name = string.Format("{0}_{1}", hX, hY);

                // We store it in the array
                _grid[GridWidth * hY + hX] = hexagonScript;
            }
        }

        // We instantiate the characters
        _entitiesObject = new GameObject("Entities");
        _entitiesObject.transform.parent = WorldPrefab;
        List<Entity> entities = _map.GetEntities();
        // We recycle hexCoords
        hexCoords = Vector3.zero;
        foreach(Entity entity in entities)
        {
            hexCoords.x = entity.Hexagon.X;
            hexCoords.y = entity.Hexagon.Y;
            Vector3 entityPos = HexCoordsUtils.HexToWorld(_world, hexCoords);
            entityPos.z = -1f;
            Transform t = (Transform)Instantiate(CharacterPrefab,
                                                 entityPos,
                                                 Quaternion.identity);
            t.parent = _entitiesObject.transform;
            t.gameObject.name = entity.Name;
            EntityScript entityScriptInstance = t.GetComponent<EntityScript>();
            entityScriptInstance.Entity = entity;
        }
	}


    enum MouseButtons
    {
        LEFT = 0,
        RIGHT = 1,
        MIDDLE = 2
    }
	

    HexagonScript _currentHex = null;

	// Update is called once per frame
	void Update ()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        HighlightHex(worldPos);
    }

    void HighlightHex(Vector3 worldPos)
    {
        // We get mouse position
        Vector3 hexCoords = HexCoordsUtils.WorldToHex(_world, worldPos);
        Vector3 rounded = Vector3.zero;

        rounded = HexCoordsUtils.RoundHex(hexCoords);

        int x = (int)rounded.x;
        int y = (int)rounded.y;
        HexagonScript newHex = null;
        if ((x >= 0) && (x < GridWidth) && 
            (y >= 0) && (y < GridHeight))
        {
            newHex = _grid[GridWidth * y + x];
        }

        if (_currentHex != newHex)
        {
            if (newHex != null)
            {
                newHex.ChangeColor(Color.red);
            }
            if (_currentHex != null)
            {
                _currentHex.RevertColor();
            }
            _currentHex = newHex;
        }
    }

    internal void ClearGrid()
    {
        for (int y = 0; y < GridHeight; ++y)
        {
            for (int x = 0; x < GridWidth; ++x)
            {
                HexagonScript hex = _grid[GridWidth * y + x];
                hex.ChangeColor(Color.white);
            }
        }
    }

    internal void PaintHexagon(int x, int y, Color color)
    {
        HexagonScript hex = _grid[GridWidth * y + x];
        hex.ChangeColor(color, color);
    }

}

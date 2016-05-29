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
    Transform[] _grid;

    // HexWorld  -> Unity coords transformation information
    HexWorld _world;

    #endregion

    // Use this for initialization
    void Start () {
        // We start the game instance
        _map = new Map(GridWidth, GridHeight, 2);

        // We obtain the hex -> world transformation
        SpriteRenderer spriteRenderer = HexagonPrefab.GetComponent<SpriteRenderer>();
        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Vector3 hexagonExtents = spriteBounds.extents;
        _world = new HexWorld(hexagonExtents.x);

        // We instance the main object
        _gridObject = new GameObject("Grid");
        _gridObject.transform.parent = WorldPrefab;
        Transform mainTransform = _gridObject.transform;
        _grid = new Transform[GridWidth * GridHeight];

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
                                                     Quaternion.identity);
                t.parent = mainTransform;
                t.gameObject.name = string.Format("{0}_{1}", hX, hY);

                // We store it in the array
                _grid[GridWidth * hY + hX] = t;
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
	

    Transform _currentHex = null;

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

    internal void ClearGrid()
    {
        for (int y = 0; y < GridHeight; ++y)
        {
            for (int x = 0; x < GridWidth; ++x)
            {
                Transform t = _grid[GridWidth * y + x];
                SpriteRenderer spriteRenderer = t.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.white;
            }
        }
    }

    internal void PaintHexagon(int x, int y, Color color)
    {
        Transform t = _grid[GridWidth * y + x];
        SpriteRenderer spriteRenderer = t.GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }

}

using UnityEngine;
using GameLogic;
using System.Collections.Generic;
using Assets.Scripts.Utils;

public class SceneManagerScript : MonoBehaviour
{
    #region SINGLETON

    // NOTE(Cristian): The SceneManagerScript runs *before* any other script.
    // This means that this way of getting the instance should always work
    private static SceneManagerScript _instance;
    internal static SceneManagerScript GetInstance()
    {
        if (_instance == null)
        {
            // TODO(Cristian): Create instance here? Diagnose?
            string message = "No Scene Manager Found! (Is correct order set in scene?)";
            throw new System.InvalidProgramException(message);
        }

        return _instance;
    }

    #endregion

    #region EDITOR

    public Transform HexagonPrefab;
    public Transform CharacterPrefab;

    public uint GridWidth;
    public uint GridHeight;

    #endregion

    internal Map Map { get; private set; }
    internal HexWorld HexWorld { get; private set; }
    internal Transform WorldObject { get; private set; }

    private GridManagementScript _gridManager;
    private EntitiesManagementScript _entitiesManager;

    #region SETUP

    // Use this for initialization
    void Start()
    {
        // We set the static instance
        _instance = this;

        // We get the world
        // TODO(Cristian): See the best way to unhardcode this
        WorldObject = GameObject.Find("World").transform;

        // We get the other components
        _gridManager = (GridManagementScript)FindObjectOfType(typeof(GridManagementScript));
        _entitiesManager = (EntitiesManagementScript)FindObjectOfType(typeof(EntitiesManagementScript));

        GenerateMapData();
    }

    private void GenerateMapData()
    {
        Map = new Map(GridWidth, GridHeight, 2);
        GenerateHexWorld();
        GridGeneration.GenerateEntities(this);
    }

    private void GenerateHexWorld()
    {
        // We obtain the hex -> world transformation
        // This will be used throughout the world transformation,
        // so SceneManager, as a global singleton, will hold a reference to it.
        Transform hexagonPrefab = HexagonPrefab;
        SpriteRenderer spriteRenderer = hexagonPrefab.GetComponent<SpriteRenderer>();
        Rect spriteRect = spriteRenderer.sprite.rect;
        HexWorld = new HexWorld(spriteRect.width / 2);
    }

    #endregion

    // TODO(Cristian): Should we receive an EntityScript and manage all in unity-speak?
    internal Entity SelectedEntity { get; private set; }
    internal void SetSelectedEntity(Entity entity)
    {
        if (SelectedEntity == entity) { return; }

        SelectedEntity = entity;

        _gridManager.ClearGrid();

        List<Hexagon> area = GameLogic.Grid_Math.Area.EntityMovementRange(Map, SelectedEntity);

        foreach(Hexagon hexagon in area)
        {
            _gridManager.PaintHexagon(hexagon.X, hexagon.Y, Color.green);
        }
    }
}
using UnityEngine;
using GameLogic;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using System;
using System.Reflection;

public partial class SceneManagerScript : MonoBehaviour
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
        SetupStateMachine();
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

    public void EntityClick(EntityScript entityScript)
    {
        // TODO(Cristian): Eventually we shouldn't use all this reflection,
        // but for now it's very convenient
        // IMPORTANT(Cristian): Methods apparently have to be public in order to be found
        string methodName = string.Format("{0}_EntityClick", _stateMachine.State.ToString());
        MethodInfo method = this.GetType().GetMethod(methodName);
        if (method == null)
        {
            string msg = string.Format("State entity method {0} doesn't exist", methodName);
            throw new InvalidProgramException(msg);
        }
        object[] args = new object[1];
        args[0] = entityScript;
        method.Invoke(this, args);
    }

    internal void HexagonClick(HexagonScript hexagonScript)
    {
        // TODO(Cristian): Eventually we shouldn't use all this reflection,
        // but for now it's very convenient
        // IMPORTANT(Cristian): Methods apparently have to be public in order to be found
        string methodName = string.Format("{0}_HexagonClick", _stateMachine.State.ToString());
        MethodInfo method = this.GetType().GetMethod(methodName);
        if (method == null)
        {
            string msg = string.Format("State entity method {0} doesn't exist", methodName);
            throw new InvalidProgramException(msg);
        }
        object[] args = new object[1];
        args[0] = hexagonScript;
        method.Invoke(this, args);
    }
}
using UnityEngine;
using GameLogic;
using System.Reflection;
using System.Collections.Generic;

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
            string message = "No Scene Manager Found!";
            throw new System.InvalidProgramException(message);
        }

        return _instance;
    }

    #endregion

    // Use this for initialization
    void Start()
    {
        // We set the static instance
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO(Cristian): Should we receive an EntityScript and manage all in unity-speak?
    private Entity _selectedEntity;
    internal void SetSelectedEntity(Entity entity)
    {
        if (_selectedEntity == entity) { return; }

        _selectedEntity = entity;

        GridGeneration gridManager = GetComponent<GridGeneration>();
        gridManager.ClearGrid();

        List<Hexagon> area = GameLogic.Grid_Math.Area.EntityMovementRange(gridManager.Map, _selectedEntity);

        foreach(Hexagon hexagon in area)
        {
            gridManager.PaintHexagon(hexagon.X, hexagon.Y, Color.green);
        }
    }

    // GUI TEST
    void OnGUI()
    {
        float marginRatio = 0.01f;
        float widthRatio = 0.25f;
        float heightRatio = 0.5f;

        Rect guiRect = new Rect(marginRatio * Screen.width, marginRatio * Screen.height,
                                widthRatio * Screen.width, heightRatio * Screen.height);
        GUI.Box(guiRect, "Debug Window");

        if (_selectedEntity == null) { return; }

        DisplayEntityInfo(_selectedEntity, guiRect);
    }

    void DisplayEntityInfo(Entity entity, Rect guiRect)
    {
        // We show the name
        int yIndex = 0;
        GenerateKeyValueLabel(guiRect, new Rect(10, 25 + yIndex * 15, 0, 0),
                              "Name", entity.Name);
        ++yIndex;

        // We show all the entities
        Entity.EntityStats stats = entity.Stats;
        PropertyInfo[] properties = stats.GetType().GetProperties();
        foreach(PropertyInfo property in properties)
        {
            GenerateKeyValueLabel(guiRect, new Rect(10, 25 + yIndex * 15, 0, 0),
                                  property.Name, property.GetValue(stats, null).ToString());
            ++yIndex;
        }
    }

    void GenerateKeyValueLabel(Rect origin, Rect offset, string key, string value)
    {
        Rect pos = new Rect(origin.x + offset.x,
                            origin.y + offset.y,
                            origin.width + offset.width,
                            origin.height + offset.height);

        GUI.Label(pos, key);
        pos.x += 100;
        GUI.Label(pos, value);
    }

}
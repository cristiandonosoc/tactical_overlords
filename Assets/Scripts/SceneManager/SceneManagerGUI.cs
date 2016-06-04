using UnityEngine;
using System.Collections;
using GameLogic;
using System.Reflection;

[RequireComponent(typeof(SceneManagerScript))]
public class SceneManagerGUI : MonoBehaviour
{
    SceneManagerScript _sceneManager;

	// Use this for initialization
	void Start ()
    {
        _sceneManager = SceneManagerScript.GetInstance();
	}
	
    // gui test
    void OnGUI()
    {
        float marginRatio = 0.01f;
        float widthRatio = 0.25f;
        float heightRatio = 0.5f;

        Rect guiRect = new Rect(marginRatio * Screen.width, marginRatio * Screen.height,
                                widthRatio * Screen.width, heightRatio * Screen.height);
        GUI.Box(guiRect, "Debug Window");

        if (_sceneManager.SelectedEntityScript == null) { return; }

        DisplayEntityInfo(_sceneManager.SelectedEntityScript.Entity, guiRect);
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

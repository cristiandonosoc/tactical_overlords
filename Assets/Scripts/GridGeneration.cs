using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using GameLogic;

internal static class GridGeneration
{
    // Use this for initialization
    internal static void GenerateGrid(SceneManagerScript sceneManager) {
        Map map = sceneManager.Map;

        // We start the game instance
        uint gridWidth = sceneManager.GridWidth;
        uint gridHeight = sceneManager.GridHeight;

        // We instance the main object
        Transform worldObject = sceneManager.WorldObject;
        GridManagementScript gridManager = worldObject.GetComponentInChildren<GridManagementScript>();

        Transform mainTransform = gridManager.transform;
        HexagonScript[] grid = new HexagonScript[sceneManager.GridWidth * sceneManager.GridHeight];
        // We set the grid to its manager
        gridManager.SetGrid(grid);

        Transform hexagonPrefab = sceneManager.HexagonPrefab;

        // We instantiate the prefabs
        Vector3 hexCoords = Vector3.zero;
        Hexagon hexagon;
        for (int hY = 0; hY < gridHeight; ++hY)
        {
            for (int hX = 0; hX < gridWidth; ++hX)
            {
                hexagon = map.GetHexagon(hX, hY);
                if (hexagon == null) { continue; }

                hexCoords.x = hX;
                hexCoords.y = hY;
                Vector3 hexPosition = HexCoordsUtils.HexToWorld(sceneManager.HexWorld, hexCoords);
                Transform t = (Transform)MonoBehaviour.Instantiate(hexagonPrefab,
                                                                   hexPosition,
                                                                   hexagonPrefab.localRotation);
                HexagonScript hexagonScript = t.GetComponent<HexagonScript>();
                hexagonScript.Hexagon = hexagon;
                t.parent = mainTransform;
                t.gameObject.name = string.Format("{0}_{1}", hX, hY);

                // We store it in the array
                grid[gridWidth * hY + hX] = hexagonScript;
            }
        }
	}

    internal static void GenerateEntities(SceneManagerScript sceneManager)
    {
        // We instantiate the characters
        Transform worldObject = sceneManager.WorldObject;
        EntitiesManagementScript entitiesObject = worldObject.GetComponentInChildren<EntitiesManagementScript>();
        List<Entity> entities = sceneManager.Map.GetEntities();

        Transform characterPrefab = sceneManager.CharacterPrefab;

        Vector3 hexCoords = Vector3.zero;
        foreach(Entity entity in entities)
        {
            hexCoords.x = entity.Hexagon.X;
            hexCoords.y = entity.Hexagon.Y;
            Vector3 entityPos = HexCoordsUtils.HexToWorld(sceneManager.HexWorld, hexCoords);
            entityPos.z = -1f;
            Transform t = (Transform)MonoBehaviour.Instantiate(characterPrefab, 
                                                               entityPos,
                                                               Quaternion.identity);
            t.parent = entitiesObject.transform;
            t.gameObject.name = entity.Name;
            EntityScript entityScriptInstance = t.GetComponent<EntityScript>();
            entityScriptInstance.Entity = entity;
        }
    }

}

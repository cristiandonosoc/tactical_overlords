using Assets.Scripts.Utils;
using GameLogic;
using System.Collections.Generic;
using UnityEngine;

internal class GridManagementScript : MonoBehaviour
{

    private uint _gridWidth;
    private uint _gridHeight;

    private HexagonScript[] _grid;
    internal void SetGrid(HexagonScript[] grid)
    {
        _grid = grid;
    }

    private bool _validGridSetup = false;

    SceneManagerScript _sceneManager;
    void Start()
    {
        _sceneManager = SceneManagerScript.GetInstance();
        GenerateGrid();

        if (!_validGridSetup)
        {
            throw new System.InvalidProgramException("Wrong Grid Setup!");
        }
    }

    private void GenerateGrid()
    {
        GridGeneration.GenerateGrid(_sceneManager);
        _gridWidth = _sceneManager.GridWidth;
        _gridHeight = _sceneManager.GridHeight;

        // Eventually this check should be *much* better
        if ((_gridWidth > 0) && (_gridHeight > 0))
        {
            _validGridSetup = true;
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
        if (!_validGridSetup) { return; }
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        HighlightHex(worldPos);
    }

    void HighlightHex(Vector3 worldPos)
    {
        // We get mouse position
        Vector3 hexCoords = HexCoordsUtils.WorldToHex(_sceneManager.HexWorld, worldPos);
        Vector3 rounded = HexCoordsUtils.RoundHex(hexCoords);

        int x = (int)rounded.x;
        int y = (int)rounded.y;
        HexagonScript newHex = null;
        if ((x >= 0) && (x < _gridWidth) && 
            (y >= 0) && (y < _gridHeight))
        {
            newHex = _grid[_gridWidth * y + x];
        }

        if (_currentHex != newHex)
        {
            _currentHex = newHex;
            ClearGrid();
            if (newHex != null)
            {
                List<Hexagon> path = new List<Hexagon>();
                GameLogic.Grid_Math.Path.GetPath(_sceneManager.Map, path, 0, 0, x, y);

                foreach (Hexagon hexagon in path)
                {
                    PaintHexagon(hexagon.X, hexagon.Y, Color.red);
                }
            }
        }
    }

    internal void ClearGrid()
    {
        for (int y = 0; y < _gridHeight; ++y)
        {
            for (int x = 0; x < _gridWidth; ++x)
            {
                HexagonScript hex = _grid[_gridWidth * y + x];
                hex.ChangeColor(Color.white);
            }
        }
    }

    internal void PaintHexagon(int x, int y, Color color)
    {
        HexagonScript hex = _grid[_gridWidth * y + x];
        hex.ChangeColor(color, color);
    }


}

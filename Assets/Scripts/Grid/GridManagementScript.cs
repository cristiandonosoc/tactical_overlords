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

    internal void PaintList(List<Hexagon> path, Color color)
    {
        foreach (Hexagon hexagon in path)
        {
            PaintHexagon(hexagon, color);
        }
    }

    internal void ClearGrid()
    {
        for (int y = 0; y < _gridHeight; ++y)
        {
            for (int x = 0; x < _gridWidth; ++x)
            {
                HexagonScript hex = _grid[_gridWidth * y + x];
                hex.ChangeColor(Color.white, Color.white);
            }
        }
    }

    internal void PaintHexagon(int x, int y, Color color)
    {
        if ((x < 0) || (x >= _gridWidth) ||
            (y < 0) || (y >= _gridHeight))
        {
            return;
        }
        HexagonScript hex = _grid[_gridWidth * y + x];
        hex.ChangeColor(color, color);
    }

    internal void PaintHexagon(Hexagon hexagon, Color color)
    {
        PaintHexagon(hexagon.X, hexagon.Y, color);
    }


}
